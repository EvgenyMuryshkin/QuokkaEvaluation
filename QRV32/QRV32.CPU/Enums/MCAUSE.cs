namespace QRV32.CPU
{
    public enum MCAUSE : uint
    {
        // Traps
        InstAddrMisalign = 0U,
        Breakpoint = 0x3U,
        LoadAddrMisalign = 0x4U,
        StoreAddrMisalign = 0x6U,
        MECall = 0xBU,
        // Software interrupts
        USoftIRQ = 0x80000000,
        SSoftIRQ = 0x80000001,
        MSoftIRQ = 0x80000003,
        // Timer interrupts
        UTimerIRQ = 0x80000004,
        STimerIRQ = 0x80000005,
        MTimerIRQ = 0x80000007,
        // External interrupts
        UExternalIRQ = 0x80000008,
        SExternalIRQ = 0x80000009,
        MExternalIRQ = 0x8000000B,
    }
}
