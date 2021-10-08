/* 
 * Written on 2021-09-24
 * Code by MS VS 2019
 * Reference : 
 * - NanoXLSX 1.8.1 https://github.com/rabanti-github/NanoXLSX Nuget Package
 * - Newtonsoft.Json 13.0.1 https://github.com/JamesNK/Newtonsoft.Json/releases Nuget Package
 */

using System;
using System.IO;
using System.Collections.Generic;

using NanoXLSX;
using Newtonsoft.Json;

namespace DataConvert
{
    class ExcelToJson : IConvert
    {
        private readonly string[] EXCEL_EXTENSIONS = new string[] { ".xls", ".xlsx" };
        private readonly string RESULT_FILENAME = "Result.json";

        private bool m_isInit = false; // TODO : 필요한가?
        private string m_sourcePath = string.Empty;
        private DirectoryInfo m_dirInfo;

        private Dictionary<string, List<Dictionary<string, object>>> m_values = new Dictionary<string, List<Dictionary<string, object>>>();

        public ExcelToJson(string srcPath)
        {
            m_sourcePath = srcPath;

            Init();
        }

        public void Init()
        {
            m_isInit = true;
            m_dirInfo = new DirectoryInfo(m_sourcePath);

            m_values.Clear();
        }

        public void OnBeforeOperation() 
        {
            // 본 로직 진입 전 사전 예외 검사 및 처리 단계

            // 해당 경로에 엑셀 파일이 있는지 검사
            string path = Util.GetFullPath(Consts.SOURCE_PATH);
            FileInfo[] files = m_dirInfo.GetFiles();
            if (files.Length <= 0)
            {
                Util.ReportCode(Enums.ErrorCode.NotExistFiles, path);
                throw new Exception("Throwed Defined Exception");
            }

            // 확장자가 엑셀 파일인지 검사
            for(int idx = 0; idx < files.Length; idx++)
            {
                FileInfo fileInfo = files[idx];
                string filePath = Path.GetFullPath(fileInfo.Name);

                bool isAvailable = false;
                string fileExt = Path.GetExtension(filePath);
                for(int secIdx = 0; secIdx < EXCEL_EXTENSIONS.Length; secIdx++)
                {
                    string baseExt = EXCEL_EXTENSIONS[secIdx];
                    isAvailable |= fileExt == baseExt;
                }

                if (!isAvailable)
                    Util.ReportCode(Enums.ErrorCode.NotExcelFile, fileInfo.Name);
            }
        }

        public void Operation()
        {
            try
            {
                OnBeforeOperation();

                // 지정된 경로의 모든 파일들을 순회
                FileInfo[] files = m_dirInfo.GetFiles();
                for(int idx = 0; idx < files.Length; idx++)
                {
                    FileInfo fileInfo = files[idx];

                    // NanoXLSX라는 외부 패키지 사용, 엑셀 파일을 추상화해서 조작이 가능하게 함
                    string filePath = string.Format("{0}/{1}", m_sourcePath, fileInfo.Name);
                    Workbook workBook = Workbook.Load(filePath);
                    Util.ConsoleText("Current FilePath : {0}", filePath);

                    // 엑셀 파일안에 모든 시트를 순회
                    List<Worksheet> workSheets = workBook.Worksheets;
                    for(int secIdx = 0; secIdx < workSheets.Count; secIdx++)
                    {
                        Worksheet workSheet = workSheets[secIdx];
                        string sheetName = workSheet.SheetName;
                        Util.ConsoleText("Current SheetName : {0}", sheetName);

                        // 최종 데이터 초기화
                        if (!m_values.ContainsKey(sheetName))
                            m_values.Add(sheetName, new List<Dictionary<string, object>>());

                        List<Dictionary<string, object>> sheetValues = m_values[sheetName];
                        List<string> tempKeyValue = new List<string>();
                        int prevRow = -1;

                        // 시트의 각 행열을 순회, 첫 행은 데이터를 구분하기 위한 키값으로 사용
                        foreach(var item in workSheet.Cells)
                        {
                            Cell cell = item.Value;
                            int currentRow = cell.RowNumber;

                            // 신규 행 구분 변수, 엑셀의 데이터 하나하나를 다 순회하기 때문에 각 행을 구분할 수 있도록
                            bool isNewRow = prevRow != currentRow;
                            if (isNewRow)
                                prevRow = currentRow;

                            // 첫번째 행 구분 변수
                            bool isKeyRow = currentRow == 0;
                            if (isKeyRow)
                                tempKeyValue.Add(cell.Value.ToString()); // 임시 키값 기록
                            else
                            {
                                // 신규 행, 새로운 데이터 추가
                                if (isNewRow)
                                    sheetValues.Add(new Dictionary<string, object>());

                                int dataRow = currentRow - 1;
                                Dictionary<string, object> rowValues = sheetValues[dataRow];
                                rowValues.Add(tempKeyValue[cell.ColumnNumber], cell.Value);
                            }
                        }
                    }
                }

                OnAfterOperation();
            }
            catch(Exception ex)
            {
                Util.ReportCode(Enums.ErrorCode.Exception, string.Format("{0} {1}", ex.Message, ex.StackTrace));
            }
        }

        public void OnAfterOperation() 
        {
            // 내보내기 경로 확인
            string path = Util.GetFullPath(Consts.DEST_PATH);
            if (!Util.IsExistDirectory(path))
            {
                Util.ReportCode(Enums.ErrorCode.NotExistDirectory, path);

                DirectoryInfo dirInfo = Util.CreateDirectory(path);
                bool isSuccess = dirInfo.Exists;
                path = isSuccess ? path : null;
                Util.ConsoleText("Create Directory, Result : {0}, Path : {1}", isSuccess, path);
            }
        }

        public void Export()
        {
            string path = Util.GetFullPath(Consts.DEST_PATH);

            string resultJson = JsonConvert.SerializeObject(m_values, Formatting.Indented);
            string resultPath = string.Format("{0}/{1}", path, RESULT_FILENAME);
            File.WriteAllText(resultPath, resultJson);

            Util.ConsoleText("Convert Complete : {0}", path);
            Util.ConsoleText("FileName : {0}", RESULT_FILENAME);
        }

        public void OnError() { }
    }
}
