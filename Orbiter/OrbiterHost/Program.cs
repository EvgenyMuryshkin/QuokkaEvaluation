using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Native;

namespace OrbiterHost
{
	class Program
	{
		static void Main(string[] args)
		{
			Task.Factory.StartNew(async () =>
			{
				var sim = new InputSimulator();
				for( int i = 0; i < 10; i++ )
				{
					await Task.Delay(TimeSpan.FromSeconds(1));
					sim.Keyboard.KeyPress(VirtualKeyCode.VK_A);
				}

				sim.Keyboard.KeyPress(VirtualKeyCode.RETURN);
			});

			var text = Console.ReadLine();

			Console.WriteLine(text);
		}
	}
}
