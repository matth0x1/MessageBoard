namespace MessageBoard.Messages
{
    public interface IRawMessage
    {
        /// <summary>
        /// Message payload content.
        /// </summary>
        string Payload { get; set; }
    }
}
