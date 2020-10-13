using MessageBoard.Messages;

namespace MessageBoard.MessageProcessing
{
    public class RawMessageValidator : IRawMessageValidator
    {
        public bool Validate(IRawMessage rawMessage) => 
            !string.IsNullOrWhiteSpace(rawMessage.Payload);
    }
}
