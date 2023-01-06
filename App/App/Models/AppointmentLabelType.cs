using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Xamarin.Forms;

namespace App.Models
{
    public class AppointmentLabelType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Caption { get; set; }
        [Ignore]
        public Color Color { get; set; }
    }
}
