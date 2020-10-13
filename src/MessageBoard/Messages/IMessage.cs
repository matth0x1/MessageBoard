using System;

namespace MessageBoard.Messages
{
    public interface IMessage: IEquatable<IMessage>
    {
        /// <summary>
        /// Message payload content.
        /// </summary>
        string Payload { get; set; }

        /// <summary>
        /// Timestamp received.
        /// </summary>
        DateTimeOffset TimestampReceived { get; set; }

        /// <summary>
        ///  Message unique ID.
        /// </summary>
        Guid ID { get; set; }
    }
}
