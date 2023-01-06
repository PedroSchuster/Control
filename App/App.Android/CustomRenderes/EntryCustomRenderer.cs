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
using App.Droid;
using App.Tools;
using EntryCustomRendererAndroid.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(EntryCustomRenderer))]

namespace EntryCustomRendererAndroid.Droid
{

    public class EntryCustomRenderer : EntryRenderer
    {
        public EntryCustomRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if(Control != null)
            {
                if (Control.InputType == Android.Text.InputTypes.ClassText)
                {
                    Control.InputType = Android.Text.InputTypes.TextFlagNoSuggestions;
                }
                Control.SetPadding(0,0,0,0);
            }
        }
    }
}