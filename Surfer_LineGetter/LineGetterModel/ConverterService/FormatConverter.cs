using LineGetterCore.Configuration;
using LineGetterCore.Entities;
using LineGetterModel.MessageService.MessageClasses;

namespace LineGetterModel.ConverterService
{
    public class FormatConverter
    {
        public static List<LineData> GetLineDataList (string content)
        {
            try
            {
                List<LineData> Result = new List<LineData>();

                var splits = content.Replace("\r", "").Replace('.',',').Split('\n');

                double Counter = 1;
                foreach (var line in splits)
                {
                    var elements = line.Split('\t');

                    if (Counter == 1)
                    {
                        var d1 = 0.0; var i1 = 0;
                        var d2 = 0.0; var i2 = 0;
                        var d3 = 0.0; var i3 = 0;
                        var d4 = 0.0; var i4 = 0;

                        if (   (Double.TryParse(elements[0], out d1) || Int32.TryParse(elements[0], out i1))
                            && (Double.TryParse(elements[1], out d2) || Int32.TryParse(elements[1], out i2))
                            && (Double.TryParse(elements[2], out d3) || Int32.TryParse(elements[2], out i3))
                            && (Double.TryParse(elements[3], out d4) || Int32.TryParse(elements[3], out i4))
                        )
                        {
                            Result.Add(
                                new LineData
                                {
                                    X1 = Convert.ToDouble(elements[0]),
                                    Y1 = Convert.ToDouble(elements[1]),
                                    X2 = Convert.ToDouble(elements[2]),
                                    Y2 = Convert.ToDouble(elements[3]),
                                    Label = elements[4]
                                }
                            );
                        }

                        Counter++;
                        continue;
                    }
                    
                    if (Counter == splits.Length && String.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    Result.Add(
                        new LineData
                        {
                            X1 = Convert.ToDouble(elements[0]),
                            Y1 = Convert.ToDouble(elements[1]),
                            X2 = Convert.ToDouble(elements[2]),
                            Y2 = Convert.ToDouble(elements[3]),
                            Label = elements[4]
                        }
                    );

                    Counter++;
                }

                return Result;
            }
            catch (Exception ex)
            {
                MessageObjects.Sender.SendMessage(InternalMessagePatterns.ErrorMessage + ex.Message);
                return null;
            }
        }

        public static string GetLineDataString (List<LineData> list, bool getLineData)
        {
            string Result = "";

            if (getLineData)
            {
                foreach (var line in list)
                {
                    Result += line.ToString();
                }
            }
            else
            {
                Result += "X\tY\tLabelData\n";
                foreach (var line in list)
                {
                    Result += line.GetPointFileContent();
                }
            }

            return Result;
        }
    }
}