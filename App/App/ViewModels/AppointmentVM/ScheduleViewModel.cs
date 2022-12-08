using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace App.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        private readonly ReceptionDeskData data;

        public event PropertyChangedEventHandler PropertyChanged;


        public DateTime StartDate { get { return ReceptionDeskData.BaseDate; } }
        public IReadOnlyList<ScheduleAppointment> Appointments { get => data.Appointments; }
        public IReadOnlyList<AppointmentLabelType> AppointmentsLabel { get => data.Labels; }



        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public ScheduleViewModel()
        {
            data = new ReceptionDeskData();
        }


    }
}
