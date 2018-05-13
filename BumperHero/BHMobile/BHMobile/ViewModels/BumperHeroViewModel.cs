using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace BHMobile.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BumperHeroViewModel : BaseViewModel
    {
        private string _Status;
        public string Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                OnPropertyChanged();
            }
        }

        private List<String> _EndPoints;
        public List<String> EndPoints
        {
            get { return _EndPoints; }
            set
            {
                _EndPoints = value;
                OnPropertyChanged();
            }
        }

        private int _Distance;
        public int Distance
        {
            get { return _Distance; }
            set
            {
                _Distance = value;
                OnPropertyChanged();
            }
        }

        private Color _Color;
        public Color Color
        {
            get { return _Color; }
            set
            {
                _Color = value;
                OnPropertyChanged();
            }
        }
    }
}
