using LineGetterCore.Configuration;
using LineGetterCore.Entities;
using LineGetterModel.ConverterService;
using LineGetterModel.FileService;
using LineGetterModel.ValidateService;
using Surfer_LineGetter.AppService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Surfer_LineGetter.ViewModels
{
    public class AppWindowViewModel : NotifyPropertyChanged
    {

        public int id;
        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                CheckChanges();
            }
        }

        public Command Convert
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        if (fileDatas.Count == 0 || fileDatas == null)
                        {
                            MessageBox.Show("В списке не осталось файлов.");
                            return;
                        }

                        if (!fileDatas.Exists(x => x.Id == Id))
                        {
                            MessageBox.Show("В списке не найдено файлов с таким ID.");
                            return;
                        }

                        var fileinfo_obj = fileDatas.Find(x => x.Id == Id);

                        if (!fileinfo_obj.IsValide)
                        {
                            MessageBox.Show("Выберите пригодный для конвертации файл.");
                            return;
                        }

                        var content = ContentGetters.GetContentFromFile(fileinfo_obj.FilePath, true);

                        if (String.IsNullOrEmpty(content))
                        {
                            return;
                        }

                        var dataLine = FormatConverter.GetLineDataList(content);

                        if (dataLine == null)
                        {
                            return;
                        }

                        var lineContent = FormatConverter.GetLineDataString(dataLine, true);
                        var pointContent = FormatConverter.GetLineDataString(dataLine, false);

                        if (String.IsNullOrEmpty(lineContent) || String.IsNullOrEmpty(pointContent))
                        {
                            MessageBox.Show("Возникла неизвестная ошибка при получении контента.");
                            return;
                        }

                        var entity = fileinfo_obj.GetFileName().Split('.').First();

                        var OutputFilesPath = Environment.CurrentDirectory 
                                + "\\" + DirectoryConfiguration.OutputDir 
                                + "\\" + entity;

                        if (Directory.Exists(OutputFilesPath))
                        {
                            MessageBox.Show("Такая папка уже существует (" + entity + ").");
                            return;
                        }
                        Directory.CreateDirectory(OutputFilesPath);

                        var lineFilePath = OutputFilesPath + "\\" + entity + "-linedata.dat";
                        var pointFilePath = OutputFilesPath + "\\" + entity + "-pointdata.dat";

                        ContentWriters.WriteContentToFile(lineFilePath, lineContent, true);
                        ContentWriters.WriteContentToFile(pointFilePath, pointContent, true);

                        var usedPath = Environment.CurrentDirectory
                            + "\\" + DirectoryConfiguration.InputDir
                            + "\\" + "Used";

                        var currentFilePath = fileinfo_obj.FilePath;

                        var targetFilePath = usedPath 
                            + "\\" + fileinfo_obj.GetFileName();

                        if (!Directory.Exists(usedPath))
                        {
                            MessageBox.Show("Папки \"Used\" не существует, перемещение файла " + fileinfo_obj.GetFileName() + "отменено.");
                        }

                        FileMovers.MoveFile(currentFilePath, targetFilePath, true);

                        AppData.InputFileInformation = InputFileChecker.CheckAndGet(false);
                        FileDatas = CopyService.CopyFileInformationObject(AppData.InputFileInformation);

                        MessageBox.Show("Успешно!");
                    }    
                );
            }
        }

        public Command Help
        {
            get
            {
                return new Command(
                    obj =>
                    {
                        string message = "Данная программа предназначена для перевода данных, в которых собраны координаты двух точек, " +
                            "по сути обозначающих начало и конец нужной линии, в удобный для построения в среде Surfer надписанных линий формат из двух файлов.\n" +
                            "Первый файл, который будет помечен как -linedata, позволяет построить непосредственно сами линии с помощью инструмента \"Base\".\n" +
                            "Второй файл, который будет помечен как -pointdata, позволяет сгенерировать специальный файл, в который будут помещены координаты" +
                            "точек подписи с дополнительным полем \"LabelData\". В самой среде Surfer можно уменьшить размер точки до нуля и включить подписи.";

                        MessageBox.Show(message);
                    }    
                );
            }
        }

        public List<FileInformation> fileDatas = CopyService.CopyFileInformationObject(AppData.InputFileInformation);

        public List<FileInformation> FileDatas
        {
            get
            {
                var Result = new List<FileInformation>();
                foreach (var item in fileDatas)
                {
                    Result.Add(
                        new FileInformation()
                        {
                            Id = item.Id,
                            FilePath = item.GetFileName(),
                            Message = item.GetValide()
                        }
                    );
                }
                return Result;
            }
            set
            {
                fileDatas = value;
                CheckChanges();
            }
        }
    }
}