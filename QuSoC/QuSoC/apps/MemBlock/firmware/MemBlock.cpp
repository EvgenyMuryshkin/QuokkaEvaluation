#include "MemBlock.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace MemBlock
{
	void Firmware::EntryPoint()
	{
		MemBlock_SOC_MemBlock[1023] = 42;
		for(uint8_t i = 0; (i < 10); (i++))
		{
			MemBlock_SOC_MemBlock[i] = i;
		}
		for(uint8_t i = 0; (i < 10); (i++))
		{
			MemBlock_SOC_MemBlock[1023] += MemBlock_SOC_MemBlock[i];
		}
	}
}
