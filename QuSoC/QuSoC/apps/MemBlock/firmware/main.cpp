#include <stdint.h>
#include <stdbool.h>

#include "soc.h"
#include "MemBlock.h"

// main is called from start.S assembly file
void main() {
	MemBlock::Firmware::EntryPoint();
}
