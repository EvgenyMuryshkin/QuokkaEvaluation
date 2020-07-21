#include "SOCCounter.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace SOCCounter
{
	void Firmware::EntryPoint()
	{
		SOCCounter_SOC_Counter = 100;
	}
}
