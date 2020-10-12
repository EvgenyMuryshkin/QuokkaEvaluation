#include "Counter.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Counter
{
	void Firmware::EntryPoint()
	{
		uint32_t counter = 0;
		while(true)
		{
			(counter++);
			Counter_SOC_Counter = counter;
		}
	}
}
