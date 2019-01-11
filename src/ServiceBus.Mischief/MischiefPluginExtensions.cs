namespace Microsoft.Azure.ServiceBus
{
    /// <summary>
    ///
    /// </summary>
    public static class MischiefPluginExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exceptionMessage"></param>
        public static void WillThrow<T>(this Message message, string exceptionMessage = null) where T : ServiceBusException
        {
            message.UserProperties["exception-to-throw"] = typeof(T).Name;

            if (exceptionMessage != null)
            {
                message.UserProperties["exception-message"] = exceptionMessage;
            }
        }
    }
}