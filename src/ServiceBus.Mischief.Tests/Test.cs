namespace ServiceBus.Mischief.Tests
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Azure.ServiceBus.Core;
    using Xunit;

    public class Test
    {
        [Fact]
        public async Task Can_throw_ServiceBusTimeoutException()
        {
            var plugin = new MischiefPlugin();
            var message = new Message();

            message.WillThrow<ServiceBusTimeoutException>();

            var exception = await Assert.ThrowsAsync<ServiceBusTimeoutException>(() => plugin.BeforeMessageSend(message));

            Assert.Equal("Simulated exception of type 'ServiceBusTimeoutException' was thrown.", exception.Message);
        }

        [Fact]
        public async Task Can_throw_ServerBusyException()
        {
            var plugin = new MischiefPlugin();
            var message = new Message();

            message.WillThrow<ServerBusyException>();

            var exception = await Assert.ThrowsAsync<ServerBusyException>(() => plugin.BeforeMessageSend(message));

            Assert.Equal("Simulated exception of type 'ServerBusyException' was thrown.", exception.Message);
        }

        [Fact]
        public async Task Can_throw_custom_exception_message()
        {
            var plugin = new MischiefPlugin();
            var message = new Message();

            message.WillThrow<ServerBusyException>("I'm busy!");

            var exception = await Assert.ThrowsAsync<ServerBusyException>(() => plugin.BeforeMessageSend(message));

            Assert.Equal("I'm busy!", exception.Message);
        }

        [Fact]
        public async Task Can_throw_the_requested_number_of_times()
        {
            var plugin = new MischiefPlugin();
            var message = new Message();

            message.WillThrow<ServerBusyException>(numberOfFailures: 2);

            await Assert.ThrowsAsync<ServerBusyException>(() => plugin.BeforeMessageSend(message));
            await Assert.ThrowsAsync<ServerBusyException>(() => plugin.BeforeMessageSend(message));
            await plugin.BeforeMessageSend(message);
        }

        [Fact]
        public async Task Can_delay_operation()
        {
            var plugin = new MischiefPlugin();
            var message = new Message();

            var delay3secs = TimeSpan.FromSeconds(3);

            message.DelayWith(delay3secs);
            message.WillThrow<ServerBusyException>();

            var stopwatch = Stopwatch.StartNew();
            await Assert.ThrowsAsync<ServerBusyException>(() => plugin.BeforeMessageSend(message));
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Assert.InRange(stopwatch.ElapsedMilliseconds, delay3secs.TotalMilliseconds, delay3secs.Add(TimeSpan.FromSeconds(0.5)).TotalMilliseconds);
        }

        [Fact]
        public async Task Real_use()
        {
            var plugin = new MischiefPlugin();

            var sender = new MessageSender("Endpoint=sb://fake.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=fake", "queue");
            sender.RegisterPlugin(plugin);

            var message = new Message();
            message.WillThrow<ServerBusyException>("I'm busy!");

            var exception = await Assert.ThrowsAsync<ServerBusyException>(() => sender.SendAsync(message));

            Assert.Equal("I'm busy!", exception.Message);
        }
    }
}