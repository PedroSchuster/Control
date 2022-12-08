using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using App.Enums;
using SQLite;

namespace App.Models
{
    public class Appointment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Duration { get; set; }

        public Status PaymentStatus { get; set; }

        public string Patient { get; set; }

    }
}
