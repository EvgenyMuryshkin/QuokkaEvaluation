files = I-CSRRW-01.S
arch = rv32i

elf: $(files)
	riscv32-unknown-elf-gcc -march=$(arch) -nostartfiles -Wl,-Bstatic,-T,sections.lds,--strip-debug,-Map=firmware.map,--cref  -ffreestanding -nostdlib -o firmware.elf $(files)

bin: elf
	riscv32-unknown-elf-objcopy -O binary firmware.elf /dev/stdout > firmware.bin




