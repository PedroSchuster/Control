using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Tools;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App.Views.Tabs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageLayout : Shell
    {
        public TabbedPageLayout()
        {
            InitializeComponent();
            tab.CurrentItem = home;
            chartTab.ContentTemplate = new DataTemplate(() =>
            {
                return App.ChartV;
            });
        }
    }
}