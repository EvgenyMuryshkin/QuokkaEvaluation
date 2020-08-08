addi x1, x0, 10
addi x2, x0, 20

li x3, 0x100
li x4, 0xBAD0C0DE
li x5, 0xC0DE0BAD
li x6, 0xC00DC0DE

addi x7, x4, 0

sb x7, 0x8(x3)

srli x7, x7, 8
sb x7, 0x9(x3)

addi x7, x5, 0

srli x7, x7, 16
sb x7, 0xA(x3)

srli x7, x7, 8
sb x7, 0xB(x3)

addi x7, x4, 0
sh x7, 0x4(x3)

addi x7, x5, 0
srli x7, x7, 16
sh x7, 0x6(x3)

sw x6, 0(x3)

# infinite loop is the end of simulation
loop:
	j loop