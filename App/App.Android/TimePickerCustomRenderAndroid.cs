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
using App.Tools;
using App.Droid;
using TimePicker = Xamarin.Forms.TimePicker;

[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerCustomRenderAndroid))]

namespace App.Droid
{
    public class TimePickerCustomRenderAndroid : TimePickerRenderer
    {
        public TimePickerCustomRenderAndroid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.SetPadding(0, 0, 0, 0);
            }
        }
    }
}