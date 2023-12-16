using LineGetterModel.MessageService.MessageClasses;

namespace LineGetterModel.FileService
{
    public class ContentGetters
    {
        public static string GetContentFromFile(string FilePath, bool GetMessage)
        {
            try
            {
                var Result = "";

                var file = new FileInfo(FilePath);
                if (!file.Exists || file.Length == 0)
                {
                    throw new Exception("Файл не существует, либо пустой: \n\n" + FilePath);
                }

                Result = File.ReadAllText(FilePath);

                return Result.Replace("\r", "");
            }
            catch (Exception ex)
            {
                if (GetMessage) { MessageObjects.Sender.SendMessage("Ошибка: \n" + ex.Message); }
                return null;
            }
        }
    }
}