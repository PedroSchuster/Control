using App.Models;
using App.ViewModels;
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
    public partial class AppointmentDetailsPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public AppointmentDetailsPopup(Appointment appointment)
        {
            BindingContext = new AppointmentViewModel(appointment);
            InitializeComponent();
        }

        
    }
}