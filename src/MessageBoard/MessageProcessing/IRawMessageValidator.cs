using MessageBoard.Messages;

namespace MessageBoard.MessageProcessing
{
    public interface IRawMessageValidator
    {
        /// <summary>
        /// Validates the IRawMessage
        /// </summary>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        bool Validate(IRawMessage rawMessage);
    }
}
