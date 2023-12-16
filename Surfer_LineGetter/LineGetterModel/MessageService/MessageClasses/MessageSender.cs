﻿using LineGetterCore.Interfaces;

namespace LineGetterModel.MessageService.MessageClasses
{
    public class MessageSender
    {
        private readonly IMessageService _messageBus;

        public MessageSender(IMessageService messageBus)
        {
            _messageBus = messageBus;
        }

        public void SendMessage(string Message)
        {
            // Отправка сообщения через шину
            _messageBus.Publish(Message);
        }
    }
}