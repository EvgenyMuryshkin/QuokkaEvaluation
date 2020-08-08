addi x1, x0, 0x100
li x3, 16

li x2, 0xBADC0FFE
sh x2, 0(x1);

srl x2, x2, x3
sh x2, 2(x1);