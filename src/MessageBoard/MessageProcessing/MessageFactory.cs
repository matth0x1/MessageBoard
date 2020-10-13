using System;
using MessageBoard.Messages;

namespace MessageBoard.MessageProcessing
{
    public class MessageFactory : IMessageFactory
    {
        /// <summary>
        /// Creates and populates a new Message from and IRawMessage
        /// </summary>
        /// <param name="rawMessage">Received message</param>
        /// <returns></returns>
        public IMessage Create(IRawMessage rawMessage) => 
            new Message()
            {
                Payload = rawMessage.Payload,
                ID = Guid.NewGuid(),
                TimestampReceived = DateTimeOffset.Now
            };
    }
}
