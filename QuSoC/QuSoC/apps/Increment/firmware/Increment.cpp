#include "Increment.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Increment
{
	void Firmware::EntryPoint()
	{
		uint32_t counter = 0;
		while(true)
		{
			(counter++);
			Increment_SOC_Counter = counter;
		}
	}
}
