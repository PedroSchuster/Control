using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Enums;
using App.Models;
using App.Services;
using App.ViewModels;
using App.ViewModels.TabsVM;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppointmentListViewDetail : ContentPage
    {


        public AppointmentListViewDetail()
        {
            InitializeComponent();

            BindingContext = new AppointmentListViewModel();


            appointmentListView.ItemSelected += (s, e) =>
            {
                if (e.SelectedItem as Appointment != null)
                {
                    Action goToPage = async () =>
                    {
                        await PopupNavigation.Instance.PushAsync(new AppointmentDetailsPopup(e.SelectedItem as Appointment));
                        appointmentListView.SelectedItem = null;
                    };
                    goToPage.Invoke();
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var b = (BindingContext as AppointmentListViewModel);

            Task.Run(async () =>
            {
                await b.Load();
                b.LoadItems();
            });

            MessagingCenter.Subscribe<Appointment>(this, "Reload", (p) =>
            {
                Task.Run(async () =>
                {
                    await b.Load();
                    b.AppointmentsFilteredList = new ObservableCollection<Appointment>();
                    b.LoadItems();

                });
            });

            MessagingCenter.Subscribe<CalendarViewModel>(this, "UpdateDates", (e) =>
            {
                var bind = this.BindingContext as AppointmentListViewModel;
                bind.StartDate = e.StartDate;
                bind.EndDate = e.EndDate;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            (BindingContext as AppointmentListViewModel).ResetFilters();
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            FilterPicker.Focus();
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            StatusPicker.Focus();
        }

        private void appointmentListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var b = (BindingContext as AppointmentListViewModel);

            if (b.AppointmentsFilteredList.Count == 0)
                return;

            var ordererAppointmentList = b.OrderByAscending ? b.AppointmentsFilteredList.ToList() : b.AppointmentsFilteredList.OrderByDescending(x => x.Date).ToList();

            if ((e.Item as Appointment).Id == b.AppointmentsFilteredList[b.AppointmentsFilteredList.Count - 1].Id && b.AppointmentsFilteredList.Count >= 10 
                && b.appointments.Count > b.AppointmentsFilteredList.Count)
            {
                b.LoadItems();
            }
        }



    }

}