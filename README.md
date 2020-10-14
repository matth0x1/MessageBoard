# MessageBoard
**Brief:** Please write a production ready dotnet core application that works like a simple message board. This app should expose a REST interface that allows an anonymous user to submit messages and to retrieve a list of the submitted messages. Please follow sound engineering practices and due to the limited time available, please document any trade-offs that you had to make whilst building this app.

**Trade Offs**
- Time.  The allotted time is of course not enough to produce *anything* production ready.
- Storage of messages is in-memory and not stored persistently; i.e. service not backed by a RDB or document store.
- Without being backed by storage, this service would be difficult effectively scale.
- Very limited error handling.
- No metrics.
- Very limited logging.
- No unit tests.  Only some very basic integration tests.
- No documentation.  Although Swagger integration is provided.
- No performance profiling.
- No dockerfile.
