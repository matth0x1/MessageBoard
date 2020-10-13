using System;
using System.Collections.Generic;
using System.Linq;
using MessageBoard.Logging;
using MessageBoard.MessageProcessing;
using MessageBoard.Messages;
using MessageBoard.MessageStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageBoard.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IRawMessageValidator _rawMessageValidator;
        private readonly IMessageFactory _messageFactory;
        private readonly IMessageStore _messageStore;

        public MessagesController(
            ILogger<MessagesController> logger,
            IRawMessageValidator rawMessageValidator,
            IMessageFactory messageFactory,
            IMessageStore messageStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rawMessageValidator = rawMessageValidator ?? throw new ArgumentNullException(nameof(rawMessageValidator));
            _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
            _messageStore = messageStore ?? throw new ArgumentNullException(nameof(messageStore));
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<IMessage> Get()
        {
            var messages = _messageStore.Retrieve();

            _logger.LogInformation(EventIds.MessagesRetrieved, "MessagesRetrieved {MessageCount}", messages.Count());

            return messages;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IMessage> Submit(RawMessage rawMessage)
        {
            if (!_rawMessageValidator.Validate(rawMessage))
            {
                _logger.LogError(EventIds.InvalidRawMessage, "InvalidRawMessage {RawMessage}", JsonConvert.SerializeObject(rawMessage));
                return BadRequest("Invalid message");
            }

            var message = _messageFactory.Create(rawMessage);
            _messageStore.Store(message);

            _logger.LogInformation(EventIds.MessageSubmitted, "MessageSubmitted {Message}", JsonConvert.SerializeObject(message));

            return Created(nameof(Submit), message);
        }
    }
}
