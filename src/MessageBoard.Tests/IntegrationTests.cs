using MessageBoard.Messages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoard.Tests
{
    /// <summary>
    /// Simple integration tests.
    /// </summary>
    /// <remarks>
    /// Each test run gets a new _server and _client
    /// </remarks>
    public class IntegrationTests
    {
        private TestServer _server;
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _server = new TestServer(new WebHostBuilder().UseEnvironment("Devlopment").UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [Test]
        public async Task SubmitMessage()
        {
            // Submit a message.
            var rawMessage = new RawMessage()
            {
                Payload = "Ahoy"
            };

            var timestamp = DateTimeOffset.Now;

            var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rawMessage)));
            textContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await _client.PostAsync("/messages", textContent);

            Assert.AreEqual(HttpStatusCode.Created, result.StatusCode);

            // The message payload is returned with an ID and timestamp.
            var message = JsonConvert.DeserializeObject<Message>(await result.Content.ReadAsStringAsync());
            Assert.AreEqual("Ahoy", message.Payload);
            Assert.IsTrue(message.TimestampReceived > timestamp && message.TimestampReceived < timestamp.AddSeconds(10), "Check timestamp is sane.");
            Assert.IsTrue(message.ID != Guid.Empty);
        }

        [Test]
        public async Task RetrieveMessages_NoMessages()
        {
            // Retrieve all messages, there will be no messages
            var result = await _client.GetAsync("/messages");
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.AreEqual("[]", await result.Content.ReadAsStringAsync());
        }

        [Test]
        public async Task SubmitMessages_RetrieveMessages()
        {
            // Create and submit 100 messages
            var messageCount = 100;
            var messages = new List<Message>();

            for (int i = 0; i < messageCount; i++)
            {
                var rawMessage = new RawMessage()
                {
                    Payload = $"Message {i}"
                };

                var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(rawMessage)));
                textContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var submitResponse = await _client.PostAsync("/messages", textContent);
                Assert.AreEqual(HttpStatusCode.Created, submitResponse.StatusCode);

                var message = JsonConvert.DeserializeObject<Message>(await submitResponse.Content.ReadAsStringAsync());
                Assert.AreEqual(rawMessage.Payload, message.Payload);
                messages.Add(message);
            }

            Assert.AreEqual(messageCount, messages.Count);

            // Retrieve all messages
            var retrieveResponse = await _client.GetAsync("/messages");
            Assert.AreEqual(HttpStatusCode.OK, retrieveResponse.StatusCode);

            var retrievedMessages = JsonConvert
                .DeserializeObject<IEnumerable<Message>>(await retrieveResponse.Content.ReadAsStringAsync())
                .ToList();

            // Compare all retrieved message to the submitted messages.
            Assert.AreEqual(messages.Count, retrievedMessages.Count);
            Assert.AreEqual(
               messages.OrderBy(m => m.TimestampReceived),
               retrievedMessages.OrderBy(m => m.TimestampReceived));
        }

        [TestCase("")]
        [TestCase("{}")]
        [TestCase("[]")]
        [TestCase("{{\"Payload\":\"\"}}")]
        [TestCase("{{\"Payload\":\" \"}}")]
        [TestCase("{{\"Payload\":1}}")]
        public async Task SubmitInvalidMessage(string body)
        {
            // Submit an invalid request.
            var textContent = new ByteArrayContent(Encoding.UTF8.GetBytes(body));
            textContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = await _client.PostAsync("/messages", textContent);

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
