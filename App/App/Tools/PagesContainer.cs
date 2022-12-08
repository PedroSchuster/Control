using System;
using System.Collections.Generic;
using System.Text;
using App.Views.AppointmentViews;
using App.Views;

namespace App.Tools
{
    public static class PagesContainer
    {
        private static AppointmentListViewDetail _appListV;
        public static AppointmentListViewDetail AppListV { get { return _appListV; } }

        private static RegisterAppointmentView _regAppV;
        public static RegisterAppointmentView RegAppV { get { return _regAppV; } }

        private static ScheduleView _schV;
        public static ScheduleView SchV { get { return _schV; } }

        private static PatientListViewDetail _patientListV;
        public static PatientListViewDetail PatientListV { get { return _patientListV; } }

        private static RegisterPatientView _regPatientV;
        public static RegisterPatientView RegPatientV { get { return _regPatientV; } }

        private static ChartView _chartV;
        public static ChartView ChartV { get { return _chartV; } }


        public static void Init()
        {
            _appListV = new AppointmentListViewDetail();

            _regAppV = new RegisterAppointmentView();

            _schV = new ScheduleView();

            _patientListV = new PatientListViewDetail();

            _regPatientV = new RegisterPatientView();

            _chartV = new ChartView();
        }


    }
}
