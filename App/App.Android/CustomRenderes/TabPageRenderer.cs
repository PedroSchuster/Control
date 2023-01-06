using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Core.Content;
using AndroidX.ViewPager.Widget;
using App.Droid;
using App.Renderes;
using Google.Android.Material.Tabs;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Android.Graphics.Color;

[assembly: ExportRenderer(typeof(CustomTabbedPage), typeof(TabPageRenderer))]
namespace App.Droid
{
    public class TabPageRenderer : TabbedRenderer
    {

        private bool _isConfigured = false;
        private ViewPager _pager;
        private TabLayout _layout;

        public TabPageRenderer(Context context) : base(context) { }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            _pager = (ViewPager)ViewGroup.GetChildAt(0);
            _layout = (TabLayout)ViewGroup.GetChildAt(1);

            var control = (CustomTabbedPage)sender;
            Android.Graphics.Color selectedColor = new Color();
            Android.Graphics.Color unselectedColor = new Color();


            for (int i = 0; i < _layout.TabCount; i++)
            {
                var tab = _layout.GetTabAt(i);
                var icon = tab.Icon;
                if (icon != null)
                {
                    var color = tab.IsSelected ? selectedColor : unselectedColor;
                    icon.SetColorFilter(color, PorterDuff.Mode.SrcIn);
                }
            }
        }

    }
}
