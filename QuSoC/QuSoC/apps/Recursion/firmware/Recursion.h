#ifndef Recursion_H
#define Recursion_H
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Recursion
{
	class Firmware
	{
		private: static void RecursiveCounter(uint32_t counter);
		public: static void EntryPoint();
	};
}
#endif
