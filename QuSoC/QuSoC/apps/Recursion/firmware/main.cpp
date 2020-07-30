#include <stdint.h>
#include <stdbool.h>

#include "soc.h"
#include "Recursion.h"

// main is called from start.S assembly file
void main() {
	Recursion::Firmware::EntryPoint();
}
