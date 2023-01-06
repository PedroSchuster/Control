using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Services;
using App.Services;
using System.Collections.ObjectModel;
using App.ViewModels;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientListViewDetail : ContentPage
    {


        public PatientListViewDetail()
        {
            InitializeComponent();

            BindingContext = new PatientListViewModel();


            patientListView.ItemSelected += (s, e) =>
            {
                
                if (e.SelectedItem as Patient != null)
                {
                    Action popup = new Action(async () =>
                    {
                        await PopupNavigation.Instance.PushAsync(new PatientDetailsPopup(e.SelectedItem as Patient));
                        patientListView.SelectedItem = null;
                    });

                    popup.Invoke();
                }
            };
        }

        public PatientListViewDetail(bool fromRegPage)
        {
            InitializeComponent();

            BindingContext = new PatientListViewModel();

            patientListView.ItemSelected += (s, e) =>
            {
                if (e.SelectedItem as Patient != null)
                {
                    Action selectPatient = new Action(async () =>
                    {
                        MessagingCenter.Send(e.SelectedItem as Patient, "SelectedPatient");
                        await Application.Current.MainPage.Navigation.PopAsync(true);
                    });

                    selectPatient.Invoke();
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var b = BindingContext as PatientListViewModel;

            Task.Run(async () =>
            {
                await b.Load();
                b.LoadItems();

            });

            MessagingCenter.Subscribe<Patient>(this, "Reload", (p) =>
            {
                Task.Run(async () =>
                {
                    await b.Load();
                    b.PatientsFilteredList = new ObservableCollection<Patient>();
                    b.LoadItems();
                });
            });

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var b = BindingContext as PatientListViewModel;
            b.Filter = null;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            FilterPicker.Focus();
        }

        private void patientListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var b = (BindingContext as PatientListViewModel);

            if (b.PatientsFilteredList.Count == 0)
                return;

            if ((e.Item as Patient).Id == b.PatientsFilteredList[b.PatientsFilteredList.Count - 1].Id && b.PatientsFilteredList.Count >= 10
                && b.patients.Count > b.PatientsFilteredList.Count)
            {
                b.LoadItems();
            }
        }
    }
}