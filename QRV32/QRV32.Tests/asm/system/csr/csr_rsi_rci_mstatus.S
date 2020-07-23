li x2, 1
csrrsi  zero, mstatus, 0x8
csrrw x3, mstatus, zero
csrrci zero, mstatus, 0x8
csrrw x2, mstatus, zero
