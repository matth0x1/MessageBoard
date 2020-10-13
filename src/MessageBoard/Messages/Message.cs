using System;
using System.Diagnostics.CodeAnalysis;

namespace MessageBoard.Messages
{
    public class Message : IMessage
    {
        public DateTimeOffset TimestampReceived { get; set; }
        public string Payload { get; set; }
        public Guid ID { get; set; }

        public override bool Equals(object? obj) => 
            Equals(obj as IMessage);

        public bool Equals([AllowNull] IMessage other) =>
            other != null &&
            TimestampReceived.Equals(other.TimestampReceived) &&
            Payload == other.Payload &&
            ID.Equals(other.ID);

        public override int GetHashCode() => 
            HashCode.Combine(TimestampReceived, Payload, ID);
    }
}
