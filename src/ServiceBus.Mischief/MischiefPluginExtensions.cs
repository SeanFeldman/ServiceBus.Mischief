namespace Microsoft.Azure.ServiceBus
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public static class MischiefPluginExtensions
    {
        ///  <summary>
        ///
        ///  </summary>
        ///  <param name="message"></param>
        ///  <param name="exceptionMessage"></param>
        /// <param name="numberOfFailures"></param>
        public static void WillThrow<T>(this Message message, string exceptionMessage = null, int numberOfFailures = int.MaxValue) where T : ServiceBusException
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
        public static void DelayWith(this Message message, TimeSpan timeToDelay)
        {
            message.UserProperties["delay-with"] = timeToDelay;
        }
    }
}