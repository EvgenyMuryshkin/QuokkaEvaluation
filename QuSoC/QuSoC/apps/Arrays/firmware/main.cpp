#include <stdint.h>
#include <stdbool.h>

#include "soc.h"
#include "Arrays.h"

// main is called from start.S assembly file
void main() {
	Arrays::Firmware::EntryPoint();
}
