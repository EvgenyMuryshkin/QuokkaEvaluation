#include "SOCCounter.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace SOCCounter
{
	void Firmware::EntryPoint()
	{
		uint32_t counter = 0;
		while(true)
		{
			(counter++);
			SOCCounter_SOC_Counter = counter;
		}
	}
}
