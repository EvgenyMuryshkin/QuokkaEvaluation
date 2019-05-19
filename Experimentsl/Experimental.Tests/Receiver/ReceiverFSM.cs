namespace QuokkaTests.Experimental
{
    /// <summary>
    /// FSM States
    /// </summary>
    public enum ReceiverFSM
    {
        Idle,
        Receiving,
        WaitingForAck
    }
}
