#ifndef Fibonacci_H
#define Fibonacci_H
#include <stdint.h>
#include <stdbool.h>
#include "soc.h"
namespace Fibonacci
{
	class Firmware
	{
		private: static uint32_t Fib(uint32_t value);
		public: static void EntryPoint();
	};
}
#endif
