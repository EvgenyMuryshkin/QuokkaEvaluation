addi x1, x0, 0x100
li x3, 8

li x2, 0xBADC0FFE
sb x2, 0(x1);

srl x2, x2, x3
sb x2, 1(x1);

srl x2, x2, x3
sb x2, 2(x1);

srl x2, x2, x3
sb x2, 3(x1);