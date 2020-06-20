li x10, 0x1
li x11, 0x100
li x12, 0x80000000
csrrs  zero, mie, x10
csrrs  x14, mie, x11
csrrs  x15, mie, x12
csrrs  x16, mie, zero