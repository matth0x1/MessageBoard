using Microsoft.Extensions.Logging;

namespace MessageBoard.Logging
{
    public class EventIds
    {
        public static readonly EventId MessageSubmitted = new EventId(1000, nameof(MessageSubmitted));
        public static readonly EventId InvalidRawMessage = new EventId(1001, nameof(InvalidRawMessage));
        public static readonly EventId MessagesRetrieved = new EventId(1002, nameof(MessagesRetrieved));
        public static readonly EventId MessageStored = new EventId(1003, nameof(MessageStored));
        public static readonly EventId MessageIdHasConflict = new EventId(1004, nameof(MessageIdHasConflict));
        public static readonly EventId ServiceException = new EventId(1100, nameof(ServiceException));
    }
}
