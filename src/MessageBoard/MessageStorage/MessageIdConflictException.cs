using MessageBoard.Messages;
using System;
using System.Runtime.Serialization;

namespace MessageBoard.MessageStorage
{
    [Serializable]
    internal class MessageIdConflictException : Exception
    {
        public readonly IMessage ConflictingMessage;
        public MessageIdConflictException() { }
        public MessageIdConflictException(string message) : base(message) { }
        public MessageIdConflictException(string message, Exception innerException) : base(message, innerException) { }
        protected MessageIdConflictException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public MessageIdConflictException(IMessage conflictingMessage)
        {
            ConflictingMessage = conflictingMessage;
        }
    }
}