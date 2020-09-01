#include <stdint.h>
#include <stdbool.h>

#include "soc.h"
#include "Fibonacci.h"

// main is called from start.S assembly file
void main() {
	Fibonacci::Firmware::EntryPoint();
}
