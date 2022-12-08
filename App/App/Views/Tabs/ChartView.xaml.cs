using App.Services;
using App.ViewModels;
using App.ViewModels.TabsVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartView : ContentPage
    {
        public ChartView()
        {
            InitializeComponent();
            BindingContext = new ChartViewModel();

        }

        private void FilterButton_Clicked(object sender, EventArgs e)
        {
            FilterPicker.Focus();
        }
        private void MonthButton_Clicked(object sender, EventArgs e)
        {
            MonthPicker.Focus();
        }
        private void YearButton_Clicked(object sender, EventArgs e)
        {
            YearPicker.Focus();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = new ChartViewModel();

            var b = BindingContext as ChartViewModel;


            MessagingCenter.Subscribe<CalendarViewModel>(this, "UpdateDates", (e) =>
            {
                var bind = this.BindingContext as ChartViewModel;
                bind.StartDate = e.StartDate;
                bind.EndDate = e.EndDate;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var b = BindingContext as ChartViewModel;

            b.SelectedFilter = "Mês";
        }

    }
}