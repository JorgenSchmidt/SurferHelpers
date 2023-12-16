using LineGetterCore.Interfaces;
using LineGetterModel.MessageService.MessageBoxService;

namespace LineGetterModel.MessageService.MessageClasses
{
    public class MessageReceiver
    {
        private readonly IMessageService _messageBus;

        public MessageReceiver(IMessageService messageBus)
        {
            _messageBus = messageBus;
            _messageBus.Subscribe((string message) =>
            {
                GetMessageBox.Show(message);
            });
        }
    }
}