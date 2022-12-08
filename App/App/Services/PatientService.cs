using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Models;
using SQLite;
using System.Linq;
using System.IO;
using DevExpress.XamarinForms.Scheduler.Internal;
using Xamarin.Forms;

namespace App.Services
{
    public class PatientService
    {
        private readonly SQLiteAsyncConnection _database;

        public PatientService()
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

        public Task<List<Patient>> ToListAsync()
        {
            try
            {
                return _database.Table<Patient>().ToListAsync();

            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

        public async Task<List<Patient>> FilterSearchAsync(string name, string cpf, string email, string phoneNumber)
        {
            try
            {
                List<Patient> filterSearch = await _database.Table<Patient>().ToListAsync();

                if (!String.IsNullOrEmpty(name))
                {
                    filterSearch = filterSearch.Where(x => x.Name.ToLower().StartsWith(name)).ToList();
                }
                if (!String.IsNullOrEmpty(cpf))
                {
                    filterSearch = filterSearch.Where(x => x.Cpf.StartsWith(cpf)).ToList();
                }
                if (!String.IsNullOrEmpty(email))
                {
                    filterSearch = filterSearch.Where(x => x.Email != null && x.Email.ToLower().StartsWith(email.ToLower())).ToList();
                }
                if (!String.IsNullOrEmpty(phoneNumber))
                {
                    filterSearch = filterSearch.Where(x => x.PhoneNumber != null && x.PhoneNumber.StartsWith(phoneNumber)).ToList();
                }

                return filterSearch;
            }

            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }



        public Task<int> InsertAsync(Patient patient)
        {
            try
            {
                return _database.InsertAsync(patient);

            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

        public Task<int> UpdateAsync(Patient patient)
        {
            try
            {
                return _database.UpdateAsync(patient);

            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }

        public Task<int> DeleteAsync(Patient patient)
        {
            try
            {
                return _database.DeleteAsync(patient);

            }
            catch (Exception)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Não foi possivel concluir essa operação", "Fechar");
                return null;
            }
        }



    }
}
