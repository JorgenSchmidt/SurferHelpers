using LineGetterCore.Entities;

namespace LineGetterModel.ConverterService
{
    public class CopyService
    {
        public static List<FileInformation> CopyFileInformationObject(List<FileInformation> input)
        {
            List<FileInformation> Result = new List<FileInformation>();

            foreach (var item in input)
            {
                Result.Add(
                    new FileInformation
                    {
                        Id = item.Id,
                        FilePath = item.FilePath,
                        IsValide = item.IsValide,
                        Message = item.Message
                    }    
                );
            }

            return Result;
        }
    }
}