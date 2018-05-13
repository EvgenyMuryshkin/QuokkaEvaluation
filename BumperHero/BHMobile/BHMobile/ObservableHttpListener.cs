using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BHMobile
{
    class ObservableHttpListener<T> : IObservable<T>, IDisposable
    {
        HttpListener _listener = new HttpListener();
        Subject<T> _dtoStream = new Subject<T>();
        ManualResetEvent[] _events = new[] { new ManualResetEvent(false), new ManualResetEvent(false) };

        public IEnumerable<String> EndPoints => Dns.GetHostAddresses(Dns.GetHostName()).Select(a => $"http://{a}:8080/");

        public ObservableHttpListener()
        {
            foreach (var address in EndPoints)
            {
                _listener.Prefixes.Add(address);
            }

            Task.Factory.StartNew(ListenTask);
        }

        private async void ListenTask()
        {
            while(WaitHandle.WaitAny(_events) != 0)
            {
                while (_listener.IsListening)
                {
                    try
                    {
                        var ctx = await _listener.GetContextAsync();

                        try
                        {
                            if (ctx != null)
                            {
                                using (var reader = new StreamReader(ctx.Request.InputStream))
                                {
                                    var content = reader.ReadToEnd();
                                    if (!string.IsNullOrWhiteSpace(content))
                                    {
                                        var dtoList = JsonConvert.DeserializeObject<List<T>>(content);
                                        dtoList.ForEach(dto => _dtoStream.OnNext(dto));
                                    }
                                }

                                HttpListenerResponse response = ctx.Response;
                                using (var writer = new StreamWriter(response.OutputStream))
                                {
                                    await writer.WriteLineAsync("Hello");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            HttpListenerResponse response = ctx.Response;
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            using (var writer = new StreamWriter(response.OutputStream))
                            {
                                await writer.WriteLineAsync(ex.ToString());
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // cannot get context
                    }
                }
            }
        }

        public void Start()
        {
            _events[1].Set();
            _listener.Start();
        }

        public void Stop()
        {
            _events[1].Reset();
            _listener.Stop();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return _dtoStream.Subscribe(observer);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _events[0].Set();
                    Stop();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
