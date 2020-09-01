namespace Fibonacci
{
    public class SOC
    {
        public static SOC Instance { get; set; } = new SOC();

        public virtual uint Counter { get; set; }
    }

    public static class Firmware
    {
        static uint Fib(uint value)
        {
            switch (value)
            {
                case 0: return 0;
                case 1: return 1;
                default: return Fib(value - 1) + Fib(value - 2);
            }
        }

        public static void EntryPoint()
        {
            SOC.Instance.Counter = Fib(SOC.Instance.Counter);
        }
    }
}
