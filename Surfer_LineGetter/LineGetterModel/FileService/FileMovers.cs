using LineGetterModel.MessageService.MessageClasses;

namespace LineGetterModel.FileService
{
    public class FileMovers
    {
        public static void MoveFile(string CurrentLocation, string TargetLocation, bool GetMessage)
        {
            try
            {
                if (File.Exists(CurrentLocation) && Directory.Exists(Path.GetDirectoryName(TargetLocation)))
                {
                    File.Move(CurrentLocation, TargetLocation);
                }
                else
                {
                    throw new Exception("Файл " + CurrentLocation.Split('\\').Last() + " и/или папка" 
                        + Path.GetDirectoryName(TargetLocation).Split('\\').Last() + " не существуют.");
                }
            }
            catch (Exception ex)
            {
                if (GetMessage) { MessageObjects.Sender.SendMessage("Ошибка: \n" + ex.Message); }
            }
        }
    }
}