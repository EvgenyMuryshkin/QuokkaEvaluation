using Quokka.RTL;

namespace QRV32.CPU
{
    public class RegistersModuleInput
    {
        public bool Read;
        public RTLBitArray RS1Addr = new RTLBitArray().Resized(5);
        public RTLBitArray RS2Addr = new RTLBitArray().Resized(5);
        public RTLBitArray RD = new RTLBitArray().Resized(5);
        public bool WE;
        public uint WriteData;
    }

    public interface IRegistersModuleState
    {
        uint[] x { get; }
    }

    public abstract class RegistersModuleState : IRegistersModuleState
    {
        public uint[] x { get; set; } = new uint[32];
    }

    public interface IRegistersModule<TState> : IRTLSynchronousModule<RegistersModuleInput, TState>
        where TState : IRegistersModuleState
    {
        bool Ready { get; }
        RTLBitArray RS1 { get; }
        RTLBitArray RS2 { get; }
    }

    public abstract class RegistersModule<TState> : RTLSynchronousModule<RegistersModuleInput, TState>, IRegistersModule<TState>
        where TState : class, IRegistersModuleState, new()
    {
        public abstract bool Ready { get; }
        public abstract RTLBitArray RS1 { get; }
        public abstract RTLBitArray RS2 { get; }
    }
}
