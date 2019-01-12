namespace Microsoft.Azure.ServiceBus
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public static class MischiefPluginExtensions
    {
        /// <summary>
        /// Throw ServiceBusTimeoutException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowServiceBusTimeoutException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<ServiceBusTimeoutException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw ServerBusyException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowServerBusyException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<ServerBusyException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw ServiceBusCommunicationException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowServiceBusCommunicationException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<ServiceBusCommunicationException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw MessageSizeExceededException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowMessageSizeExceededException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<MessageSizeExceededException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw MessagingEntityNotFoundException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowMessagingEntityNotFoundException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<MessagingEntityNotFoundException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw MessagingEntityDisabledException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowMessagingEntityDisabledException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<MessagingEntityDisabledException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw UnauthorizedException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowUnauthorizedException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<UnauthorizedException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw MessageNotFoundException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowMessageNotFoundException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<MessageNotFoundException>(message, exceptionMessage, numberOfFailures);
        }

        /// <summary>
        /// Throw QuotaExceededException exception when message is processed.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage">Exception message.</param>
        /// <param name="numberOfFailures">Number of times exception will be thrown. After that no exception will be raised.</param>
        public static void WillThrowQuotaExceededException(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue)
        {
            WillThrow<QuotaExceededException>(message, exceptionMessage, numberOfFailures);
        }

        static void WillThrow<T>(this Message message, string exceptionMessage, int numberOfFailures) where T : ServiceBusException
        {
            var exceptionName = typeof(T).Name;

            message.UserProperties["exception-to-throw"] = exceptionName;

            if (exceptionMessage != null)
            {
                message.UserProperties["exception-message"] = exceptionMessage;
            }
            else
            {
                message.UserProperties["exception-message"] = $"Simulated exception of type '{exceptionName}' was thrown.";
            }

            message.UserProperties["times-to-throw"] = numberOfFailures;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="timeToDelay"></param>
        public static void WillBeDelayedBy(this Message message, TimeSpan timeToDelay)
        {
            message.UserProperties["delay-with"] = timeToDelay;
        }
    }
}