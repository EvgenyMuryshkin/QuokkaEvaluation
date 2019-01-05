using Drivers;
using Newtonsoft.Json;
using OrbiterDTO;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace OrbiterHost
{
	class Program
	{
		static IDisposable ReadPort(
			string portName, 
			Subject<byte> byteStream)
		{
			var port = new SerialPort();
			var result = new CancellationDisposable();

			Task.Factory.StartNew(() =>
			{
				try
				{
					port.PortName = portName;
					port.BaudRate = 115200;// 9600;
					port.Parity = Parity.None;
					port.DataBits = 8;
					port.StopBits = StopBits.Two;
					port.Handshake = Handshake.None;

					port.Open();

					while (!result.IsDisposed)
					{
						byte data = (byte)port.ReadByte();

						byteStream.OnNext(data);
					}
				}
				catch (Exception)
				{
					if (!result.IsDisposed)
						throw;
				}
			})
			.ContinueWith((task) =>
			{
				Console.WriteLine(task.Exception.InnerException);
				Environment.Exit(1);
			}, TaskContinuationOptions.OnlyOnFaulted);

			return result;
		}

		static IDisposable ReadString(
			Subject<byte> byteStream, 
			Subject<string> stringStream) 
		{
			List<byte> buff = new List<byte>();

			return byteStream
				.ObserveOn(Scheduler.Default)
				.Subscribe(b =>
				{
					buff.Add(b);
					if (b == '\n') {
						var stringValue = Encoding.UTF8.GetString(buff.ToArray());
						stringStream.OnNext(stringValue);
						buff.Clear();
					}
				});
		}

		static IDisposable DeserializeDTO(
			Subject<string> stringStream, 
			Subject<KeysDTO> dtoStream)
		{
			return stringStream
				.ObserveOn(Scheduler.Default)
				.Subscribe(str =>
				{
					try
					{
						var dtos = JsonConvert.DeserializeObject<KeysDTO[]>(str);
						if (dtos.Any())
						{
							dtoStream.OnNext(dtos.Last());
						}
					}
					catch(Exception)
					{ 
					}
				});
		}

		static IDisposable DispatchKeys(
			Subject<KeysDTO> dtoStream)
		{
			var sim = new InputSimulator();

			return dtoStream
				.Buffer(TimeSpan.FromMilliseconds(50))
				.Subscribe(e =>
				{
					if (e.Any()) 
					{
						var dto = e.Last();

						var keyLookup = new Dictionary<KeypadKeyCode, VirtualKeyCode>()
						{
							{ KeypadKeyCode.D0, VirtualKeyCode.NUMPAD0 },
							{ KeypadKeyCode.D1, VirtualKeyCode.NUMPAD1 },
							{ KeypadKeyCode.D2, VirtualKeyCode.NUMPAD2 },
							{ KeypadKeyCode.D3, VirtualKeyCode.NUMPAD3 },
							{ KeypadKeyCode.D4, VirtualKeyCode.NUMPAD4 },
							{ KeypadKeyCode.D5, VirtualKeyCode.NUMPAD5 },
							{ KeypadKeyCode.D6, VirtualKeyCode.NUMPAD6 },
							{ KeypadKeyCode.D7, VirtualKeyCode.NUMPAD7 },
							{ KeypadKeyCode.D8, VirtualKeyCode.NUMPAD8 },
							{ KeypadKeyCode.D9, VirtualKeyCode.NUMPAD9 },
						};

						var keyCode = dto.KeyCode;

						// check joystick
						if (dto.X < 5000 )
						{
							keyCode = KeypadKeyCode.D2;
						}

						if (dto.X > 60000)
						{
							keyCode = KeypadKeyCode.D8;
						}


						if (dto.Y > 60000 )
						{
							keyCode = KeypadKeyCode.D4;
						}

						if (dto.Y < 5000)
						{
							keyCode = KeypadKeyCode.D6;
						}

						if (keyLookup.TryGetValue(keyCode, out VirtualKeyCode virtualkeyCode))
						{
							sim.Keyboard.KeyPress(virtualkeyCode);
						}
					}
				});
		}

		static void SimulatedInput(Subject<byte> byteStream, string input) 
		{
			Task.Factory.StartNew(async () =>
			{
				int idx = 0;
				var lookup = new Dictionary<char, KeypadKeyCode>()
				{
					{ '0', KeypadKeyCode.D0 },
					{ '1', KeypadKeyCode.D1 },
					{ '2', KeypadKeyCode.D2 },
					{ '3', KeypadKeyCode.D3 },
					{ '4', KeypadKeyCode.D4 },
					{ '5', KeypadKeyCode.D5 },
					{ '6', KeypadKeyCode.D6 },
					{ '7', KeypadKeyCode.D7 },
					{ '8', KeypadKeyCode.D8 },
					{ '9', KeypadKeyCode.D9 },
				};

				while (true)
				{
					await Task.Delay(TimeSpan.FromMilliseconds(100));

					var dto = new KeysDTO()
					{
						KeyCode = KeypadKeyCode.D6
					};

					if (lookup.TryGetValue(input[idx % input.Length], out KeypadKeyCode keyCode))
					{
						dto.KeyCode = keyCode;
					}
					
					var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new List<KeysDTO>() { dto }));

					foreach (var b in bytes)
						byteStream.OnNext(b);

					byteStream.OnNext(10);

					idx++;
				}
			});
		}

		static void Main(string[] args)
		{
			using (var disposable = new CompositeDisposable())
			{
				Subject<byte> byteStream = new Subject<byte>();
				Subject<string> stringStream = new Subject<string>();
				Subject<KeysDTO> dtoStream = new Subject<KeysDTO>();

				var port = args.FirstOrDefault(a => a.ToLower().StartsWith("com")) ?? "COM5";
				var isSim = args.Any(a => a.ToLower() == "-s");
				var simInput = args.FirstOrDefault(a => a.ToLower().StartsWith("-i")) ?? "-i12345";

				if (isSim)
				{
					SimulatedInput(byteStream, simInput.Substring(2));
				}
				else
				{
					disposable.Add(ReadPort(port, byteStream));
				}

				disposable.Add(ReadString(byteStream, stringStream));
				disposable.Add(DeserializeDTO(stringStream, dtoStream));
				disposable.Add(DispatchKeys(dtoStream));

				Console.WriteLine($"Press ENTER to exit");
				Console.ReadLine();
			}
		}
	}
}
