namespace LineGetterCore.Entities
{
    /// <summary>
    /// Содержит координаты начальной и конечной точек линии, переопределена функция ToString()
    /// </summary>
    public class LineData
    {
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
        public string? Label { get; set; }

        public override string ToString()
        {
            string Result = "2\t1\n";

            Result += X1.ToString() + '\t' + Y1.ToString() + '\n';
            Result += X2.ToString() + '\t' + Y2.ToString() + '\n';

            return Result.Replace(',','.');
        }

        public string GetPointFileContent()
        {
            string Result = "";

            Result += X1.ToString() + '\t' + Y1.ToString() + '\t' + Label + '\n';
            Result += X2.ToString() + '\t' + Y2.ToString() + '\t' + Label + '\n';

            return Result.Replace(',', '.');
        }
    }
}