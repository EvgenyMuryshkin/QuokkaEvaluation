namespace QRV32.CPU
{
    public enum HaltCode : byte
    {
        None,
        BranchTypeCode,
        CSRWriteFault,
        RetTypeCode,
        IRQTypeCode,
        SysTypeCode,
        OPCode,
        OPIMMCode,
        SystemCode,
        OpTypeCode,
        NoTrapHandler,
        NoMIE
    }
}
