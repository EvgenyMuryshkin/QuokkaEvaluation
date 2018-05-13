using Android.Media;
using BHDto;
using BHMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.Content;

namespace BHMobile
{
	public partial class MainPage : ContentPage
	{
        public IObservable<ReportDTO> DtoStream;
        Subject<bool> _notificationsStream = new Subject<bool>();

        IDisposable _subscription;

        public BumperHeroViewModel VM = new BumperHeroViewModel();
        Ringtone _alertRingtonne = RingtoneManager.GetRingtone(
            Android.App.Application.Context,
            RingtoneManager.GetDefaultUri(RingtoneType.Notification));

        public MainPage()
		{
            BindingContext = VM;

            InitializeComponent();
		}

        public void Initialize()
        {
            _notificationsStream
                .DistinctUntilChanged()
                .Subscribe(ntf =>
                {
                    if (ntf)
                    {
                        _alertRingtonne.Play();
                    }
                });

            _subscription = DtoStream.Subscribe(dto =>
            {
                VM.Distance = dto.Ave;

                // forward notifications flag to next stream
                _notificationsStream.OnNext(dto.Gone == 0 ? false : true);

                VM.Status = dto.Prx.ToString();

                switch (dto.Prx)
                {
                    case ProximityState.Measuring:
                        VM.Color = Color.FromRgb(45, 214, 236);
                        break;
                    case ProximityState.Alert:
                        VM.Color = Color.FromRgb(244, 150, 138);
                        break;
                    case ProximityState.Warning:
                        VM.Color = Color.FromRgb(254, 215, 151);
                        break;
                    case ProximityState.Safe:
                        VM.Color = Color.FromRgb(132, 224, 168);
                        break;
                }
            });
        }
	}
}
