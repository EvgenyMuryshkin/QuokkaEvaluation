using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuokkaIntegrationTests
{
    public class QuokkaPort : IDisposable
    {
        protected SerialPort mPort = null;
        public TimeSpan DefaultTimeout = TimeSpan.FromSeconds(1);

        public QuokkaPort(string portName = "COM3", int baud = 9600)
        {
            mPort = new SerialPort();
            mPort.PortName = portName;
            mPort.BaudRate = baud;
            mPort.Parity = Parity.None;
            mPort.DataBits = 8;
            mPort.StopBits = StopBits.One;
            mPort.Handshake = Handshake.None;

            OnBeforeOpen();

            mPort.Open();
        }

        protected virtual void OnBeforeOpen()
        {

        }

        public SerialPort Port { get { return mPort; } }

        public void Write(byte p)
        {
            Port.Write(new byte[] { p }, 0, 1);
        }

        public void Write(byte[] p)
        {
            Port.Write(p, 0, p.Length);
        }

        void TimeoutAction(Action p, TimeSpan timeout, string hint, Func<string> extraFactory = null)
        {
            var task = Task.Factory.StartNew(p);
            if (!task.Wait(timeout))
                throw new TimeoutException((hint ?? "") + ((null != extraFactory) ? extraFactory() : ""));
        }

        public string ReadLine(TimeSpan timeout, string hint = "")
        {
            List<byte> data = new List<byte>();
            try
            {
                do
                {
                    var b = Read(1, null, timeout);
                    data.Add(b[0]);
                }
                while (data.Last() != '\n');

                var line = Encoding.ASCII.GetString(data.ToArray());
                return line;
            }
            catch(TimeoutException ex)
            {
                throw new TimeoutException( string.Format("Read so far: {0}, {1}", Encoding.ASCII.GetString(data.ToArray()), hint), ex);
            }
        }
        
        public byte[] ReadAll()
        {
            var count = mPort.BytesToRead;
            return Read(count, null, DefaultTimeout, "");
        }

        public byte[] Read(int nCount, bool? expectedEmpty, TimeSpan timeout, string hint = "")
        {
            byte[] buff = new byte[nCount];

            TimeoutAction(
                () =>
                {
                    int nOffset = 0;
                    while (nCount > 0)
                    {
                        int bytesToRead = Math.Min(nCount, Port.BytesToRead);

                        mPort.Read(buff, nOffset, bytesToRead);

                        nOffset += bytesToRead;
                        nCount -= bytesToRead;
                    }

                    if (null != expectedEmpty)
                    {
                        if (expectedEmpty.Value)
                            EnsureEmpty();
                        else
                            EnsureNotEmpty();
                    }
                },
                timeout,
                hint,
                () =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Current buffer:");
                    foreach(var b in buff)
                    {
                        sb.AppendLine(string.Format("{0}", b));
                    }

                    return sb.ToString();
                }
                );

            return buff;
        }

        public byte ReadOne(TimeSpan timeout, string hint = "")
        {
            return Read(1, null, timeout, hint)[0];
        }

        public byte ReadOneExact(TimeSpan timeout, string hint = "")
        {
            return Read(1, true, timeout, hint)[0];
        }

        public void EnsureEmpty()
        {
            if (0 != mPort.BytesToRead)
            {
                var buffer = new byte[mPort.BytesToRead];
                mPort.Read(buffer, 0, mPort.BytesToRead);
                var content = buffer.Select(c => c.ToString()).Aggregate(new StringBuilder(), (a, s) => { a.AppendFormat("{0},", s); return a; }, a => a.ToString());

                throw new Exception("Receiving port buffer is not empty: " + content);
            }
        }

        public void EnsureNotEmpty()
        {
            if (0 == mPort.BytesToRead)
                throw new Exception("Receiving port buffer is empty");
        }

        public void Flush()
        {
            while( 0 != mPort.BytesToWrite )
            {
                Thread.SpinWait(1);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (mPort != null)
                    mPort.Dispose();

                mPort = null;
            }
        }

        public void Write(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            Write(bytes);
            Flush();
        }

        public void WriteUInt64(ulong data)
        {
            for (int i = 0; i < 8; i++)
            {
                byte t = (byte)data;
                Write(t);
                data = data >> 8;
            }
        }

        public void ReadUInt64(out ulong result, TimeSpan timeout)
        {
            ulong res = 0;
            for (int i = 0; i < 8; i++)
            {
                var data = Read(1, null, timeout)[0];
                res = ((ulong)data << 56) | (res >> 8);
            }

            result = res;
        }

        public uint ReadUInt32(TimeSpan timeout)
        {
            uint res = 0;
            for (int i = 0; i < 4; i++)
            {
                var data = Read(1, null, timeout)[0];
                res = ((uint)data << 24) | (res >> 8);
            }

            return res;
        }

        public void WriteFloat(float value)
        {
            Write(TestConverters.ToByteArray(value));
        }

        public float ReadFloat(TimeSpan timeout)
        {
            var actualBytes = Read(4, null, timeout);
            return TestConverters.FloatFromByteArray(actualBytes);
        }

        public long WaitForData(TimeSpan timeout)
        {
            var sw = new Stopwatch();
            sw.Start();

            var ev = new ManualResetEvent(mPort.BytesToRead != 0);

            mPort.DataReceived += (e, d) =>
            {
                ev.Set();
            };

            if (!ev.WaitOne(timeout))
                throw new TimeoutException();

            return sw.ElapsedMilliseconds;
        }
    }
}
