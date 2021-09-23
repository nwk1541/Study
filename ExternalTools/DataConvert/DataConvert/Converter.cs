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
            Util.ConsoleText("SourcePath : {0}", srcPath);

            SetConvertMethod(srcPath, method);

            string destPath = string.Empty;
        }

        private string GetSourcePath()
        {
            string result = Util.GetFullPath(Const.SOURCE_PATH);

            if(string.IsNullOrEmpty(result))
            {
                Util.ConsoleText("SourcePath : {0} is NULL or Empty", result);
                result = null;
            }

            if(!Util.IsDirectoryExists(result))
            {
                DirectoryInfo dirInfo = Util.CreateDirectory(result);
                bool isSuccess = dirInfo.Exists;
                result = isSuccess ? result : null;

                Util.ConsoleText("Is Directory Not Exists : {0}", result);
                Util.ConsoleText("Create Directory : {0}", result);
                Util.ConsoleText("{0}", isSuccess ? "Success" : "Fail");
            }

            return result;
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

            m_convertMethod.Operation();
        }

        private void Export()
        {
            // TODO : 
            // Const.DEST_PATH
        }
    }
}
