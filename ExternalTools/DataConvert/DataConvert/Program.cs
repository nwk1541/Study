namespace DataConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            Converter converter = new Converter();
            converter.Run(Methods.ExcelToJson);
        }
    }
}
