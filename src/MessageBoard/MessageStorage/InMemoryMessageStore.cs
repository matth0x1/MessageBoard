using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MessageBoard.Logging;
using MessageBoard.Messages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageBoard.MessageStorage
{
    public class InMemoryMessageStore : IMessageStore
    {
        private readonly ILogger<InMemoryMessageStore> _logger;
        private readonly ConcurrentDictionary<Guid, IMessage> _messages = new ConcurrentDictionary<Guid, IMessage>();

        public InMemoryMessageStore(ILogger<InMemoryMessageStore> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<IMessage> Retrieve()
        {
            return _messages.Values;
        }

        public void Store(IMessage message)
        {
            if (_messages.TryAdd(message.ID, message))
            {
                _logger.LogDebug(EventIds.MessageStored, "MessageStored {Message}", JsonConvert.SerializeObject(message));

            }
            else
            {
                _logger.LogError(EventIds.MessageIdHasConflict, "MessageIdHasConflict, Message not stored {Message}", JsonConvert.SerializeObject(message));

                throw new MessageIdConflictException();
            }
        }
    }
}
