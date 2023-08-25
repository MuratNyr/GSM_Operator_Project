using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GSM_Operator_Project.Data;
using GSM_Operator_Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GSM_Operator_Project.Services
{
    public class FaturalandirmaService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public FaturalandirmaService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var today = DateTime.Today;

                    var tarifeler = await dbContext.MusteriTarifeleri
                        .Where(mt => mt.BaslangicTarihi <= today)
                        .ToListAsync(stoppingToken);

                    foreach (var tarife in tarifeler)
                    {
                        var baslangicTarihi = tarife.BaslangicTarihi;
                        var sonFaturaDonemi = tarife.BaslangicTarihi.AddMonths(1);
                        var simdikiDonem = DateTime.Today;

                        while (sonFaturaDonemi <= simdikiDonem)
                        {
                            var donemStr = sonFaturaDonemi.ToString("yyyy-MM");

                            var existingFatura = await dbContext.Faturalar
                                .FirstOrDefaultAsync(f => f.MusteriTarifeID == tarife.MusteriTarifeID && f.Donem == donemStr, stoppingToken);

                            if (existingFatura == null)
                            {
                                var tarifeUcreti = await dbContext.Tarifeler
                                    .Where(t => t.TarifeID == tarife.TarifeID)
                                    .Select(t => t.TarifeUcreti)
                                    .FirstOrDefaultAsync(stoppingToken);

                                var newFatura = new Fatura
                                {
                                    MusteriTarifeID = tarife.MusteriTarifeID,
                                    Donem = donemStr,
                                    Ucret = tarifeUcreti
                                };

                                dbContext.Faturalar.Add(newFatura);
                                await dbContext.SaveChangesAsync(stoppingToken);
                            }

                            sonFaturaDonemi = sonFaturaDonemi.AddMonths(1);
                        }

                    }
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }


    }
}
