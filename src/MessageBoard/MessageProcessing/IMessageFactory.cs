using MessageBoard.Messages;

namespace MessageBoard.MessageProcessing
{
    public interface IMessageFactory
    {
        /// <summary>
        /// Creates an IMessage from an IRawMessage
        /// </summary>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        IMessage Create(IRawMessage rawMessage);
    }
}
