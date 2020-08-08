#include "Arrays.h"
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Arrays
{
	int8_t Firmware::S8Buff[16] = {0};
	int16_t Firmware::S16Buff[16] = {0};
	uint32_t Firmware::U32Buff[16] = {0};
	void Firmware::Fill(uint32_t size)
	{
		uint32_t offset = (3 + ((size >> 1)));
		for(uint32_t i = 0; (i < size); (i++))
		{
			uint32_t value = (i - offset);
			S8Buff[i] = (int8_t)value;
			S16Buff[i] = (int16_t)value;
			U32Buff[i] = value;
		}
	}
	void Firmware::Sum(uint32_t size)
	{
		uint32_t result = 42;
		for(uint32_t i = 0; (i < size); (i++))
		{
			result += (uint32_t)S8Buff[i];
			result += (uint32_t)S16Buff[i];
			result += U32Buff[i];
		}
		Arrays_SOC_Counter = result;
	}
	void Firmware::EntryPoint()
	{
		uint32_t size = 6;
		Fill(size);
		Sum(size);
	}
}
