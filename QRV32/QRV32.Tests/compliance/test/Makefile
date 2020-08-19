files = C-ADD.S
arch = rv32imc

elf: $(files)
	riscv32-unknown-elf-gcc -march=$(arch) -nostartfiles -Wl,-Bstatic,-T,sections.lds,--strip-debug,-Map=firmware.map,--cref  -ffreestanding -nostdlib -o firmware.elf $(files)

bin: elf
	riscv32-unknown-elf-objcopy -O binary firmware.elf /dev/stdout > firmware.bin




