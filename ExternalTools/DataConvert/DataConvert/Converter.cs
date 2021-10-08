using System;
using System.Collections.Generic;
using System.IO;

namespace DataConvert
{
    public enum Methods
    {
        None,
        ExcelToJson,
        Max
    }

    class Converter
    {
        private IConvert m_convertMethod = null;
        private Methods m_method = Methods.None;

        public void Run(Methods method)
        {
            string sourcePath = GetSourcePath();
            if(string.IsNullOrEmpty(sourcePath))
            {
                Util.ReportCode(Enums.ErrorCode.WrongPath, sourcePath);
                return;
            }

            SetConvertMethod(sourcePath, method);
            Convert();

            Export();
        }

        private string GetSourcePath()
        {
            // 현재 실행파일 경로를 기반으로 소스 경로를 만듦
            string path = Util.GetFullPath(Consts.SOURCE_PATH);
            if(!Util.IsExistDirectory(path))
            {
                Util.ReportCode(Enums.ErrorCode.NotExistDirectory, path);

                DirectoryInfo dirInfo = Util.CreateDirectory(path);
                bool isSuccess = dirInfo.Exists;
                path = isSuccess ? path : null;

                Util.ConsoleText("Create Directory, Result : {0}, Path : {1}", isSuccess, path);
            }

            Util.ConsoleText("SourcePath : {0}", path);

            return path;
        }

        private void SetConvertMethod(string srcPath, Methods method)
        {
            m_method = method;

            switch (method)
            {
                case Methods.ExcelToJson:
                    m_convertMethod = new ExcelToJson(srcPath);
                    break;
            }
        }

        private void Convert()
        {
            if (m_method == Methods.None || m_convertMethod == null)
                return;

            m_convertMethod.Operation();
        }

        private void Export()
        {
            m_convertMethod.Export();
        }
    }
}
