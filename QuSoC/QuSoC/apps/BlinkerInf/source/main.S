li x1, 0x80000000 # mem blink storage
addi x3, x0, 0 # loop iterator
li x20, 2000000 # iterations in blink toggle

blink:
lw x31, 0(x1)
addi x31, x31, 1
sw x31, 0(x1)

addi x5, x0, 0 # iterators
counter:
addi x5, x5, 1
beq x5, x20, blink
j counter


# infinite loop is the end of simulation
loop:
	j loop