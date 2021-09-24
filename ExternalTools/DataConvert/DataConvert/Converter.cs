using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string srcPath = GetSourcePath();
            if(string.IsNullOrEmpty(srcPath))
            {
                Util.ReportCode(Enums.ErrorCode.WrongPath, srcPath);
                return;
            }

            SetConvertMethod(srcPath, method);
            ConvertOperation();

            Export();
        }

        private string GetSourcePath()
        {
            string path = Util.GetFullPath(Consts.SOURCE_PATH);
            if(!Util.IsDirectoryExists(path))
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

        private void SetConvertMethod(string _srcPath, Methods method)
        {
            m_method = method;

            switch (method)
            {
                case Methods.ExcelToJson:
                    m_convertMethod = new ExcelToJson(_srcPath);
                    break;
            }
        }

        private void ConvertOperation()
        {
            if (m_method == Methods.None || m_convertMethod == null)
                return;

            m_convertMethod.Operation();
        }

        private void Export()
        {
            // TODO : 
            // Const.DEST_PATH
        }
    }
}
