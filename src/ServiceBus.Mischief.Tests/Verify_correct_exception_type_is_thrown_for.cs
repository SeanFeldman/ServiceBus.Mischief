namespace ServiceBus.Mischief.Tests
{
    using Microsoft.Azure.ServiceBus;
    using Xunit;

    public class Verify_correct_exception_type_is_thrown_for
    {
        [Fact]
        public void WillThrowMessageNotFoundException()
        {
            var message = new Message();
            var exceptionName = typeof(MessageNotFoundException).Name;

            MischiefPluginExtensions.WillThrowMessageNotFoundException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowMessageSizeExceededException()
        {
            var message = new Message();
            var exceptionName = typeof(MessageSizeExceededException).Name;

            MischiefPluginExtensions.WillThrowMessageSizeExceededException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowMessagingEntityDisabledException()
        {
            var message = new Message();
            var exceptionName = typeof(MessagingEntityDisabledException).Name;

            MischiefPluginExtensions.WillThrowMessagingEntityDisabledException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowQuotaExceededException()
        {
            var message = new Message();
            var exceptionName = typeof(QuotaExceededException).Name;

            MischiefPluginExtensions.WillThrowQuotaExceededException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowMessagingEntityNotFoundException()
        {
            var message = new Message();
            var exceptionName = typeof(MessagingEntityNotFoundException).Name;

            MischiefPluginExtensions.WillThrowMessagingEntityNotFoundException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowServerBusyException()
        {
            var message = new Message();
            var exceptionName = typeof(ServerBusyException).Name;

            MischiefPluginExtensions.WillThrowServerBusyException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowServiceBusCommunicationException()
        {
            var message = new Message();
            var exceptionName = typeof(ServiceBusCommunicationException).Name;

            MischiefPluginExtensions.WillThrowServiceBusCommunicationException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowServiceBusTimeoutException()
        {
            var message = new Message();
            var exceptionName = typeof(ServiceBusTimeoutException).Name;

            MischiefPluginExtensions.WillThrowServiceBusTimeoutException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }

        [Fact]
        public void WillThrowUnauthorizedException()
        {
            var message = new Message();
            var exceptionName = typeof(UnauthorizedException).Name;

            MischiefPluginExtensions.WillThrowUnauthorizedException(message);

            Assert.Equal(exceptionName, message.UserProperties["exception-to-throw"]);
        }
    }
}