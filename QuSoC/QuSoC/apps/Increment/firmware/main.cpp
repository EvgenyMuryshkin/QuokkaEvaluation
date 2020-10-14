#include <stdint.h>
#include <stdbool.h>

#include "soc.h"
#include "Increment.h"

// main is called from start.S assembly file
void main() {
	Increment::Firmware::EntryPoint();
}
