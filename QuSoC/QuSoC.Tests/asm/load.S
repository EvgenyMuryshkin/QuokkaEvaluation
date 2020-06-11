addi x1, x0 ,0
li x3, 0x100

# load byte 0 and add to x1
lbu x4, 0(x3)
add x1, x1, x4

# load byte 1 and add to x1
lbu x4, 1(x3)
add x1, x1, x4

# load byte 2 and add to x1
lbu x4, 2(x3)
add x1, x1, x4

# load byte 3 and add to x1
lbu x4, 3(x3)
add x1, x1, x4

# load low halfwork and add to x1
lhu x5, 0(x3)
add x1, x1, x5

# load upper halfwork and add to x1
lhu x5, 2(x3)
add x1, x1, x5

# load whole word and add to x1
lw x6, 0(x3)
add x1, x1, x6

# store result in next work
sw x1, 4(x3)

# infinite loop is the end of simulation
loop:
	j loop