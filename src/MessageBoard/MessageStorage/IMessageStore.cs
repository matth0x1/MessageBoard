using System.Collections.Generic;
using MessageBoard.Messages;

namespace MessageBoard.MessageStorage
{
    public interface IMessageStore
    {
        /// <summary>
        /// Stores the given message.
        /// </summary>
        /// <param name="rawMessage"></param>
        /// <returns></returns>
        void Store(IMessage message);

        /// <summary>
        /// Retrieves the given message
        /// </summary>
        /// <returns></returns>
        IEnumerable<IMessage> Retrieve();
    }
}
