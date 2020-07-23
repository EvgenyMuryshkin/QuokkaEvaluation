li x9, -1
li x10, 0x1
li x11, 0x100
li x12, 0x80000000
csrrs  zero, mie, x9
csrrc  x14, mie, x10
csrrc  x15, mie, x11
csrrc  x16, mie, x12
csrrc  x17, mie, zero