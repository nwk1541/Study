namespace DataConvert
{
    public class Enums
    {
        public enum ErrorCode
        {
            None = -1,
            WrongPath,              // 잘못된 경로 접근
            NotExistDirectory,      // 디렉토리 존재하지 않음
            NotExistFiles,          // 파일이 존재하지 않음
            Exception,              // 예외 발생
            NotExcelFile,           // 엑셀 파일 또는 확장자가 아님
        }
    }
}
