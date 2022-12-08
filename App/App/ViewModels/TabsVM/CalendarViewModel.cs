using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace App.ViewModels.TabsVM
{
    public class CalendarViewModel : INotifyPropertyChanged
    {
        private int taps = 0;

        private DateTime? _startDate = null;
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }
        
        private DateTime? _endDate = null;
        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }

        public ICommand OnTapCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CalendarViewModel()
        {
            OnTapCommand = new Command( async() =>
            {
                if (taps >= 1)
                {
                    if (StartDate != null && EndDate != null)
                    {
                        MessagingCenter.Send(this, "UpdateDates");
                        await PopupNavigation.Instance.PopAsync(true);
                        taps = 0;
                    }
                }
                else
                {
                    taps += 1;
                }

            });
        }


        public async void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }
    }

}
