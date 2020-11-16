using Quokka.RTL;

namespace RTL.Modules
{
    public class YandexTestModuleInputs
    {
        public bool inReady;
        public byte inData0;
        public byte inData1;
        public byte inData2;
        public byte inData3;
        public byte inData4;
        public byte inData5;
    }

    public struct Stage0
    {
        public bool ready;
        public byte max01;
        public byte max23;
        public byte max45;
        public byte min01;
        public byte min23;
        public byte min45;
        public ushort sum01;
        public ushort sum23;
        public ushort sum45;
    }

    public struct Stage1
    {
        public bool ready;
        public byte max0123;
        public byte max45;
        public byte min0123;
        public byte min45;
        public ushort sum0123;
        public ushort sum45;
    }

    public struct Stage2
    {
        public bool ready;
        public byte max;
        public byte min;
        public ushort sum;
    }

    public struct Stage3
    {
        public bool ready;
        public ushort max_min;
        public ushort sum;
    }

    public struct Stage4
    {
        public bool ready;
        public ushort sum_ave;
    }

    public class YandexTestModuleState
    {
        public Stage0 stage0 = new Stage0();
        public Stage1 stage1 = new Stage1();
        public Stage2 stage2 = new Stage2();
        public Stage3 stage3 = new Stage3();
        public Stage4 stage4 = new Stage4();
    }

    public class YandexTestModule : RTLSynchronousModule<YandexTestModuleInputs, YandexTestModuleState>
    {
        byte Max(byte lhs, byte rhs)
        {
            return lhs > rhs ? lhs : rhs;
        }

        byte Min(byte lhs, byte rhs)
        {
            return lhs < rhs ? lhs : rhs;
        }

        Stage0 nextStage0 => new Stage0()
        { 
            ready = Inputs.inReady,
            sum01 = (ushort)(Inputs.inData0 + Inputs.inData1),
            sum23 = (ushort)(Inputs.inData2 + Inputs.inData3),
            sum45 = (ushort)(Inputs.inData4 + Inputs.inData5),
            max01 = Max(Inputs.inData0, Inputs.inData1),
            max23 = Max(Inputs.inData2, Inputs.inData3),
            max45 = Max(Inputs.inData4, Inputs.inData5),
            min01 = Min(Inputs.inData0, Inputs.inData1),
            min23 = Min(Inputs.inData2, Inputs.inData3),
            min45 = Min(Inputs.inData4, Inputs.inData5),
        };

        Stage1 nextStage1 => new Stage1()
        {
            ready = State.stage0.ready,
            max0123 = Max(State.stage0.max01, State.stage0.max23), 
            max45 = State.stage0.max45,
            min0123 = Min(State.stage0.min01, State.stage0.min23),
            min45 = State.stage0.min45,
            sum0123 = (ushort)(State.stage0.sum01 + State.stage0.sum23),
            sum45 = State.stage0.sum45,
        };

        Stage2 nextStage2 => new Stage2()
        {
            ready = State.stage1.ready,
            max = Max(State.stage1.max0123, State.stage1.max45),
            min = Min(State.stage1.min0123, State.stage1.min45),
            sum = (ushort)(State.stage1.sum0123 + State.stage1.sum45)
        };

        Stage3 nextStage3 => new Stage3()
        {
            ready = State.stage2.ready,
            max_min = (ushort)(State.stage2.min + State.stage2.max),
            sum = State.stage2.sum,
        };

        Stage4 nextStage4 => new Stage4()
        { 
            ready = State.stage3.ready,
            sum_ave = (ushort)(State.stage3.sum - State.stage3.max_min)
        };

        RTLBitArray sumAveBits => State.stage4.sum_ave;

        public bool outReady => State.stage4.ready;
        public byte outResult => (sumAveBits >> 2) + (sumAveBits[1] ? 1 : 0); // rounding

        protected override void OnStage()
        {
            NextState.stage0 = nextStage0;
            NextState.stage1 = nextStage1;
            NextState.stage2 = nextStage2;
            NextState.stage3 = nextStage3;
            NextState.stage4 = nextStage4;
        }
    }
}

