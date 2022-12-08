using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using App.ViewModels.TabsVM;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Hub : ContentPage 
    { 
        private Timer t;

        public Hub()
        {
            InitializeComponent();
            BindingContext = new HubViewModel();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = new HubViewModel();
            var b = BindingContext as HubViewModel;

            t = new Timer((o) => {
                Device.BeginInvokeOnMainThread(() => b.ChangeAppointmentTextValue());

            }, null, 0, 2000);

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            t.Cancel();
        }


    }
}