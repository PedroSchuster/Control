using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using App.Enums;
using App.Models;
using SQLite;
using System.IO;
using System.Threading.Tasks;
using static SQLite.SQLite3;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace App.Services
{
    public class AppointmentService
    {
        private readonly SQLiteAsyncConnection _database;

        public AppointmentService()
        {
            try
            {
                _database = App.DataBase;

            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
            }
        }


        public Task<List<Appointment>> ToListAsync()
        {
            try
            {
                return _database.Table<Appointment>().OrderBy(x => x.Date).ToListAsync();
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }

        }

        public async Task<List<Appointment>> FilterSearchAsync(string patient, DateTime? startDate, DateTime? finalDate, Status? status, int? year, int? month, TimeSpan? duration, bool orderByAscending)
        {
            try
            {
                List<Appointment> filterSearch = await ToListAsync();

                if (patient != null)
                {
                    filterSearch = filterSearch.Where(x => !String.IsNullOrEmpty(x.Patient) && x.Patient.ToLower().Contains(patient.ToLower())).ToList();
                }

                if (startDate != null && duration == null)
                {
                    filterSearch = filterSearch.Where(x => x.Date >= startDate).ToList();
                }

                if (finalDate != null)
                {
                    filterSearch = filterSearch.Where(x => x.Date <= finalDate).ToList();
                }

                if (duration != null)
                {
                    filterSearch = filterSearch.Where(x =>
                    startDate.Value.Add(duration.Value) > x.Date && startDate.Value < x.Date.Add(x.Duration)).ToList();
                }

                if (status != null)
                {
                    filterSearch = filterSearch.Where(x => x.PaymentStatus == status).ToList();
                }

                if (year != null)
                {
                    filterSearch = filterSearch.Where(x => x.Date.Year == year).ToList();
                }

                if (month != null)
                {
                    filterSearch = filterSearch.Where(x => x.Date.Month == month).ToList();
                }

                if (!orderByAscending)
                {
                    return filterSearch.OrderByDescending(x => x.Date).ToList();

                }
                return filterSearch;

            }

            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }


        public Task<int> InsertAsync(Appointment appointment)
        {
            try
            {
                ScheduleAppointment schAppointment = new ScheduleAppointment();
                schAppointment.Id = appointment.Id;
                schAppointment.StartTime = appointment.Date;
                schAppointment.EndTime = appointment.Date.Add(appointment.Duration);
                schAppointment.Subject = appointment.Patient + " \n" + schAppointment.StartTime.ToString("HH:mm") + " até " + schAppointment.EndTime.ToString("HH:mm");

                _database.InsertAsync(schAppointment);
                return _database.InsertAsync(appointment);
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

        public Task<int> UpdateAsync(Appointment appointment)
        {
            try
            {
                return _database.UpdateAsync(appointment);

            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

        public Task<int> DeleteAsync(Appointment appointment)
        {
            try
            {
                return _database.DeleteAsync(appointment);
            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }


        public Task<List<ScheduleAppointment>> ToListScheduleAppointmentAsync()
        {
            try
            {
                return _database.Table<ScheduleAppointment>().ToListAsync();

            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

    }
}