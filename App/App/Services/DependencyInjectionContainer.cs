using System;
using System.Collections.Generic;
using System.Text;
using App.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace App.Services
{
    public static class DependencyInjectionContainer 
    {

        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            try
            {
                services.AddSingleton<PatientService>();

                services.AddSingleton<AppointmentService>();

                services.AddSingleton<GenerateChartService>();

                return services;
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

        
    }
}
