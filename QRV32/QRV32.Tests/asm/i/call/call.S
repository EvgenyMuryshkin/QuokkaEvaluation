li x11, 10
li x14, 40
call proc
j end

proc:
li x11, 20
li x13, 30
ret

li x14, 0xBAD

end:
nop