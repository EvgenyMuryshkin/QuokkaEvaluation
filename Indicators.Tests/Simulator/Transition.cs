namespace Quokka.Simulator
{
    public class Transition<T>
    {
        public Transition(uint iteration, T from, T to)
        {
            Iteration = iteration;
            From = from;
            To = to;
        }

        public uint Iteration { get; set; }
        public T From { get; set; }
        public T To { get; set; }
    }
}
