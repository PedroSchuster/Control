using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using static Java.Util.ResourceBundle;
using Xamarin.Forms.Platform.Android;
using EntryCustomRendererAndroid.Droid;
using Xamarin.Forms;
using DatePicker = Xamarin.Forms.DatePicker;
using App.Tools;
using App.Droid;

[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerCustomRenderAndroid))]

namespace App.Droid
{
    public class DatePickerCustomRenderAndroid : DatePickerRenderer
    {
        public DatePickerCustomRenderAndroid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetPadding(0, 0, 0, 0);
            }
        }
    }
}