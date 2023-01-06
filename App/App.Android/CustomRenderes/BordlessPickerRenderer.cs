using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using App.Droid;
using App.Renderes;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Color = Android.Graphics.Color;
using Picker = Xamarin.Forms.Picker;

[assembly: ExportRenderer(typeof(Picker), typeof(BordlessPickerRenderer))]
namespace App.Droid
{
    public class BordlessPickerRenderer : PickerRenderer
    {
        private Context context;
        BorderlessPicker picker = null;

        public BordlessPickerRenderer(Context context) : base(context)
        {
            this.context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control == null || e.NewElement == null) return;

            picker = Element as BorderlessPicker;
            UpdatePickerPlaceholder();
            if (picker.SelectedIndex <= -1)
            {
                UpdatePickerPlaceholder();
            }


            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
            else
                Control.Background.SetColorFilter(Color.Transparent, PorterDuff.Mode.SrcAtop);
        }


        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (picker != null)
            {
                if (e.PropertyName.Equals(BorderlessPicker.PlaceholderProperty.PropertyName))
                {
                    UpdatePickerPlaceholder();
                }
            }
        }

        void UpdatePickerPlaceholder()
        {
            if (picker == null)
                picker = Element as BorderlessPicker;
            if (picker.Placeholder != null)
                Control.Hint = picker.Placeholder;
        }

    }
}