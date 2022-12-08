using System;
using System.Collections.Generic;
using System.Text;

namespace App.Tools
{
    using Xamarin.Forms;

    public class BorderlessPicker : Picker
    {
        public BorderlessPicker() : base()
        {
        }

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        propertyName: nameof(Placeholder),
        returnType: typeof(string),
        declaringType: typeof(string),
        defaultValue: string.Empty);

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }
    }
}
