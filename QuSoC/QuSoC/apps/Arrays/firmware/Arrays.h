#ifndef Arrays_H
#define Arrays_H
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Arrays
{
	class Firmware
	{
		private: static int8_t S8Buff[];
		private: static int16_t S16Buff[];
		private: static uint32_t U32Buff[];
		private: static void Fill(uint32_t size);
		private: static void Sum(uint32_t size);
		public: static void EntryPoint();
	};
}
#endif
