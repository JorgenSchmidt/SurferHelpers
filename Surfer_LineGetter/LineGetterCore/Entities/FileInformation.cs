namespace LineGetterCore.Entities
{
    /// <summary>
    /// Объект, содержащий информацию о файле и его пригодности для конвертации
    /// </summary>
    public class FileInformation
    {
        public int Id { get; set; }
        public string? FilePath { get; set; }
        public string? Message { get; set; }

        public bool IsValide { get; set; }

        public string GetFileName () 
        { 
            var Result = FilePath.Split('\\').Last();
            return Result; 
        }

        public string GetValide ()
        {
            if (IsValide)
            {
                return "Готов для конвертации.";
            }
            else
            {
                return "Не может быть конвертирован.";
            }
        }
    }
}