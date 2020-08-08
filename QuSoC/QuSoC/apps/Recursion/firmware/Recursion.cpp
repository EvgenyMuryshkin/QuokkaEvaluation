#include "Recursion.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Recursion
{
	void Firmware::RecursiveCounter(uint32_t counter)
	{
		Recursion_SOC_Counter += counter;
		if ((counter < 10))
		{
			RecursiveCounter((counter + 1));
		}
	}
	void Firmware::EntryPoint()
	{
		RecursiveCounter(0);
	}
}
