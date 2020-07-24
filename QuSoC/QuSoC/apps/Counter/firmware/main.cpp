#include <stdint.h>
#include <stdbool.h>

#include "soc.h"
#include "SOCCounter.h"

// main is called from start.S assembly file
void main() {
	SOCCounter::Firmware::EntryPoint();
}
