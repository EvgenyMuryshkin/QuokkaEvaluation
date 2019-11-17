namespace QuokkaTests.Experimental
{
    /// <summary>
    /// FSM States
    /// </summary>
    public enum TransmitterFSM
    {
        Idle,
        Transmitting,
        WaitingForAck
    }
}
