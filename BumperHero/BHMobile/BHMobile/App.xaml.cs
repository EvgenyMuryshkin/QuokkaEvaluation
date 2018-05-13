using BHDto;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace BHMobile
{
	public partial class App : Application
	{
        ObservableHttpListener<ReportDTO> _listener = new ObservableHttpListener<ReportDTO>();

        public App ()
		{
			InitializeComponent();

            var main = new MainPage()
            {
                DtoStream = _listener
            };
            main.VM.EndPoints = _listener.EndPoints.ToList();
            MainPage = main;

            main.Initialize();
        }

        /*
        private void MockTask()
        {
            Observable.Timer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1))
                .Subscribe(count =>
                {
                    var dto = new ReportDTO()
                    {
                        Ave = (ushort)count,
                        Gone = (ushort)(count % 5 == 1 ? 1 : 0),
                        Prx = (ProximityState)(count % 4)
                    };

                    _dtoStream.OnNext(dto);
                });
        }
        */

        protected override void OnStart ()
		{
            // Handle when your app starts
            _listener.Start();
        }

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            _listener.Stop();

        }

		protected override void OnResume ()
		{
            // Handle when your app resumes
            _listener.Start();
        }
	}
}
