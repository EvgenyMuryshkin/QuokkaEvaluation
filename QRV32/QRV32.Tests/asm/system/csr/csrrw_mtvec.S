li x1, 0xF0000004
csrrw  zero, mtvec, x1
csrrs  x3, mtvec, zero
csrr  x4, mtvec