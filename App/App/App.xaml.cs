using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using App.Views;
using App.Views.Tabs;
using App.Views.AppointmentViews;
using App.Models;
using SQLite;
using System.Collections.ObjectModel;
using App.ViewModels;
using App.ViewModels.TabsVM;
using System.Threading.Tasks;

namespace App
{
    public partial class App : Application
    {
        private static SQLiteAsyncConnection _database;
        public static SQLiteAsyncConnection DataBase
        {
            get
            {
                if(_database == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "control.db3");
                    _database = new SQLiteAsyncConnection(dbPath);
                }
                return _database;
            }

        }

        private static RegisterAppointmentView _regAppV;
        public static RegisterAppointmentView RegAppV { get { return _regAppV; } }

        private static ScheduleView _schV;
        public static ScheduleView SchV { get { return _schV; } }

        private static RegisterPatientView _regPatientV;
        public static RegisterPatientView RegPatientV { get { return _regPatientV; } }

        private static ChartView _chartV;
        public static ChartView ChartV { get { return _chartV; } }



        public App()
        {
            DevExpress.XamarinForms.Scheduler.Initializer.Init();

            DataBase.CreateTableAsync<Patient>().Wait();
            DataBase.CreateTableAsync<Appointment>().Wait();
            DataBase.CreateTableAsync<ScheduleAppointment>().Wait();

            InitializeComponent();

            Startup.Init();

            MainPage = new TabbedPageLayout();
        }

        protected override void OnStart()
        {
            Task.Run(() =>
            {
                _regAppV = new RegisterAppointmentView();

                _regPatientV = new RegisterPatientView();

                _schV = new ScheduleView();

                _chartV = new ChartView();
            });
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private void InitPages()
        {
            
        }

    }
}
