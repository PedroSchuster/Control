using App.Enums;
using App.Models;
using Microcharts;
using Microcharts.Forms;
using Microsoft.Extensions.DependencyInjection;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App.Services
{
    public class GenerateChartService
    {
        public GenerateChartService() { }

        public Task<PieChart> GenrateChartAsync(int? month, int? year, DateTime? startDate, DateTime? finalDate)
        {
            try
            {
                ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>();

                if (month != null)
                {
                    Task.Run(async () =>
                    {
                        appointments = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, null, null, null, null, month, null, true));

                    }).Wait();
                }
                else if (year != null)
                {
                    Task.Run(async () =>
                    {
                        appointments = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, null, null, null, year, null, null, true));
                    }).Wait();
                }
                else if (startDate != null && finalDate != null)
                {
                    Task.Run(async () =>
                    {
                        appointments = new ObservableCollection<Appointment>(await Startup.ServiceProvider.GetService<AppointmentService>().FilterSearchAsync(null, startDate, finalDate, null, null, null, null, true));
                    }).Wait();
                }


                List<ChartEntry> entries = new List<ChartEntry>();

                ChartEntry chartEntryNotPaid = new ChartEntry(appointments.Where(x => x.PaymentStatus == Status.NotPaid).Sum(x => x.Price))
                {
                    Color = SKColor.Parse("#e8473c")
                };


                ChartEntry chartEntryPaid = new ChartEntry(appointments.Where(x => x.PaymentStatus == Status.Paid).Sum(x => x.Price))
                {
                    Color = SKColor.Parse("#2cbb77")
                };

                entries.Add(chartEntryNotPaid);
                entries.Add(chartEntryPaid);

                return Task.FromResult(new PieChart() { Entries = entries, IsAnimated = true, BackgroundColor = SKColor.Empty, LabelMode = LabelMode.None });
            }


            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

    }
}
