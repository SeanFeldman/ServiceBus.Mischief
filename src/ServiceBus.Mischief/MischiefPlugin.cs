namespace Microsoft.Azure.ServiceBus
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Core;
    using Management;

    /// <summary>
    ///
    /// </summary>
    public class MischiefPlugin : ServiceBusPlugin
    {
        /// <inheritdoc />
        public override string Name { get; } = nameof(MischiefPlugin);

        /// <inheritdoc />
        public override bool ShouldContinueOnException { get; } = false;

        delegate T Creator<T>(params object[] parameters);

        /// <inheritdoc />
        public override async Task<Message> BeforeMessageSend(Message message)
        {
            if (message.UserProperties.TryGetValue("delay-with", out var delay))
            {
                await Task.Delay((TimeSpan) delay);
            }

            if (MessageNotSupposedToFail(out var exception) || ShouldStopThrowing())
            {
                return await base.BeforeMessageSend(message);
            }

            var msg = message.UserProperties["exception-message"];

            switch (exception)
            {
                // Naive method
                // var constructor = typeof(ServiceBusTimeoutException).GetConstructor(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new []{typeof(string)}, null);
                // var instance = (ServiceBusTimeoutException)constructor.Invoke(new object[] {"test"});
                // throw instance;

                case nameof(ServiceBusTimeoutException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<ServiceBusTimeoutException>(msg);

                case nameof(ServerBusyException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<ServerBusyException>(msg);

                case nameof(ServiceBusCommunicationException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<ServiceBusCommunicationException>(msg);

                case nameof(MessageSizeExceededException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<ServiceBusCommunicationException>(msg);

                // TODO: verify when/how this is possible to be thrown
                case nameof(SessionLockLostException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<SessionLockLostException>(msg);

                // TODO: verify when/how this is possible to be thrown
                case nameof(SessionCannotBeLockedException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<SessionCannotBeLockedException>(msg);

                case nameof(MessagingEntityNotFoundException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<MessagingEntityNotFoundException>(msg);

                case nameof(MessagingEntityDisabledException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<MessagingEntityDisabledException>(msg);

                // TODO: verify when/how this is possible to be thrown
                case nameof(MessagingEntityAlreadyExistsException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<MessagingEntityAlreadyExistsException>(msg);

                case nameof(UnauthorizedException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<UnauthorizedException>(msg);

                case nameof(MessageNotFoundException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<MessageNotFoundException>(msg);

                case nameof(QuotaExceededException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<QuotaExceededException>(msg);

                // TODO: verify when/how this is possible to be thrown
                case nameof(MessageLockLostException):
                    throw ObjectCreation.CreateInstanceUsingLambdaExpression<MessageLockLostException>(msg);

                default:
                    throw new Exception($"Unknown Service Bus exception of type '{exception}' was requested.");
            }

            bool MessageNotSupposedToFail(out object ex)
            {
                return !message.UserProperties.TryGetValue("exception-to-throw", out ex);
            }

            bool ShouldStopThrowing()
            {
                var timesToThrow = (int)message.UserProperties["times-to-throw"];

                message.UserProperties["times-to-throw"] = timesToThrow - 1;

                return timesToThrow <= 0;
            }
        }
    }

    static class ObjectCreation
    {
        // source: http://mattgabriel.co.uk/2016/02/10/object-creation-using-lambda-expression/

        public delegate T Creator<out T>(params object[] args);

        public static Creator<T> GetCreator<T>()
        {
            // Get constructor information?
            var constructors = typeof(T).GetConstructors();

            // Is there at least 1?
            if (constructors.Length >= 0)
            {
                // Get our one constructor.
                var constructor = constructors[0];

                // Yes, does this constructor take some parameters?
                var paramsInfo = constructor.GetParameters();

                if (paramsInfo.Length > 0)
                {
                    // Create a single param of type object[].
                    var param = Expression.Parameter(typeof(object[]), "args");

                    // Pick each arg from the params array and create a typed expression of them.
                    var argsExpressions = new Expression[paramsInfo.Length];

                    for (var i = 0; i < paramsInfo.Length; i++)
                    {
                        Expression index = Expression.Constant(i);
                        var paramType = paramsInfo[i].ParameterType;
                        Expression paramAccessorExp = Expression.ArrayIndex(param, index);
                        Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);
                        argsExpressions[i] = paramCastExp;
                    }

                    // Make a NewExpression that calls the constructor with the args we just created.
                    var newExpression = Expression.New(constructor, argsExpressions);

                    // Create a lambda with the NewExpression as body and our param object[] as arg.
                    var lambda = Expression.Lambda(typeof(Creator<T>), newExpression, param);

                    // Compile it
                    var compiled = (Creator<T>)lambda.Compile();

                    // Success
                    return compiled;
                }
            }

            return null;
        }

        /// <summary>Create instance of T with parameters using ConstructorInfo.Invoke</summary>
        public static T CreateInstanceUsingLambdaExpression<T>(params object[] args)
        {
            var createdActivator = GetCreator<T>();
            return createdActivator(args);
        }
    }
}