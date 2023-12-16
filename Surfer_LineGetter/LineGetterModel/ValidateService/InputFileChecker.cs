using LineGetterCore.Configuration;
using LineGetterCore.Entities;
using LineGetterModel.MessageService.MessageClasses;

namespace LineGetterModel.ValidateService
{
    public class InputFileChecker
    {
        public static List<FileInformation> CheckAndGet(bool getMessage)
        {
            List<FileInformation> Result = new List<FileInformation>();
            try
            {
                string Path = Environment.CurrentDirectory + "\\" + DirectoryConfiguration.InputDir;

                if (!Directory.Exists(Path))
                {
                    throw new Exception("Проверьте наличие папки Input в корневой директории приложения.");
                }

                var FileList = Directory.GetFiles(Path);

                if (FileList.Length == 0)
                {
                    throw new Exception("Папка " + DirectoryConfiguration.InputDir + " пуста.");
                }

                var Counter = 1;
                foreach (var item in FileList)
                {
                    var content = File.ReadAllText(item).Replace("\r", "");

                    if (CheckFileContent(item, content))
                    {
                        Result.Add(
                            new FileInformation
                            {
                                Id = Counter,
                                FilePath = item,
                                IsValide = true,
                            }    
                        );
                    }
                    else
                    {
                        Result.Add(
                            new FileInformation
                            {
                                Id = Counter,
                                FilePath = item,
                                IsValide = false,
                            }
                        );
                    }
                    Counter++;
                }

                return Result;
            }
            catch (Exception ex)
            {
                if (getMessage)
                {
                    MessageObjects.Sender.SendMessage(InternalMessagePatterns.ErrorMessage + ex.Message);
                }
                return null;
            }
        }

        private static bool CheckFileContent(string filepath, string content)
        {
            try
            {
                if (String.IsNullOrEmpty(content))
                {
                    throw new Exception (
                        "Файл " + filepath.Split('\\').Last() + " оказался пустым."
                    );
                }

                var splits = content.Replace('.',',').Split('\n');

                if (splits.Length <= 1)
                {
                    throw new Exception(
                        "Невозможно принять элемент всего из одной строки. Файл " + filepath.Split('\\').Last()
                    );
                }

                var Counter = 1;
                foreach (var item in splits)
                {
                    var elements = item.Split("\t");

                    var d1 = 0.0;
                    var d2 = 0.0;
                    var d3 = 0.0;
                    var d4 = 0.0;

                    if (Counter == 1)
                    {
                        Counter++;
                        continue;
                    }

                    if (Counter == splits.Length && String.IsNullOrEmpty(item))
                    {
                        Counter++;
                        continue;
                    }

                    if (elements.Length != 5)
                    {
                        throw new Exception("Ошибка в строке " + Counter + ": количество элементов не равно 5.\n"
                            + "Имя файла - " + filepath.Split('\\').Last() + ".\n"
                            + "Формат файла должен быть следующим, с учётом того что линия строится ПО ДВУМ ТОЧКАМ: \n" 
                            + "<x1> <y1> <x2> <y2> <описание элемента>, все элементы (или координаты/описание) должны быть разделены табуляцией, " 
                            + "а элементы (или линии) переносами строки.");
                    }

                    if (!Double.TryParse(elements[0], out d1)
                        || !Double.TryParse(elements[1], out d2)
                        || !Double.TryParse(elements[2], out d3)
                        || !Double.TryParse(elements[3], out d4)
                    )
                    {
                        throw new Exception("Ошибка в строке " + Counter + ": один из элементов не является числом"
                            + "Имя файла - " + filepath.Split('\\').Last());
                    }

                    Counter++;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageObjects.Sender.SendMessage(InternalMessagePatterns.ErrorMessage + ex.Message);
                return false;
            }
        }
    }
}