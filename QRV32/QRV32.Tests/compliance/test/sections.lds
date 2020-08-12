MEMORY
{
    RAM (xrw)       : ORIGIN = 0x00000000, LENGTH = 0x008000 /* 32 KB */
}

SECTIONS {
    .text :
    {
        . = ALIGN(4);
        *(.text)           /* .text sections (code) */
        . = ALIGN(4);
        _etext = .;        /* define a global symbol at end of code */
        _sidata = _etext;  /* This is used by the startup in order to initialize the .data secion */
    } >RAM

    .data :
    {
        . = ALIGN(4);
        *(.data)           /* .data sections */
        . = ALIGN(4);
        _edata = .;        /* define a global symbol at data end; used by startup code in order to initialise the .data section in RAM */
    } >RAM
}
