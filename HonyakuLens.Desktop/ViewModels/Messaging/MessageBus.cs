using System.Collections.Generic;

namespace HonyakuLens.Desktop.ViewModels.Messaging
{
    class MessageBus
    {
        public static MessageBus Default { get; } = new MessageBus();

        private Dictionary<string, List<Received>> _registrations = new Dictionary<string, List<Received>>();

        public void Register(string destination, Received received)
        {
            lock (_registrations)
            {
                if (_registrations.TryGetValue(destination, out List<Received> receivedList))
                {
                    receivedList.Add(received);
                }
                else
                {
                    _registrations.Add(destination, new List<Received>() { received });
                }
            }
        }

        public void Unregister(string destination, Received received)
        {
            lock (_registrations)
            {
                if (_registrations.TryGetValue(destination, out List<Received> receivedList))
                {
                    receivedList.Remove(received);

                    if (receivedList.Count == 0)
                    {
                        _registrations.Remove(destination);
                    }
                }
            }
        }

        public void Send(string destination, Message message)
        {
            lock (_registrations)
            {
                if (_registrations.TryGetValue(destination, out List<Received> receivedList))
                {
                    foreach (var received in receivedList)
                    {
                        received(message);
                    }
                }
            }
        }
    }
}
