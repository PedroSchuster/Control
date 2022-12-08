using App.Models;
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
    public partial class CalendarPopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        public CalendarPopup()
        {
            InitializeComponent();
            BindingContext = new CalendarViewModel();

        }


    }
}