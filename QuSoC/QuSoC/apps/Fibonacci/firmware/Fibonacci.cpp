#include "Fibonacci.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Fibonacci
{
	uint32_t Firmware::Fib(uint32_t value)
	{
		switch (value)
		{
			case 0:
			{
				return 0;
			}
			case 1:
			{
				return 1;
			}
			default:
			{
				return (Fib((value - 1)) + Fib((value - 2)));
			}
		}
	}
	void Firmware::EntryPoint()
	{
		Fibonacci_SOC_Counter = Fib(Fibonacci_SOC_Counter);
	}
}
