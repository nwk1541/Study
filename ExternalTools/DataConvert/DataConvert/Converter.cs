using System;
using System.Collections.Generic;
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
        private const string DEST_PATH = @"";

        private IConvert m_convertMethod = null;
        private Methods m_method = Methods.None;

        public void Run(Methods method)
        {
            string srcPath = GetSourcePath();

            m_method = method;
            switch(method)
            {
                case Methods.ExcelToJson:
                    m_convertMethod = new ExcelToJson(srcPath);
                    break;
            }

            string destPath = string.Empty;
        }

        private string GetSourcePath()
        {
            string result = string.Empty;

            while(string.IsNullOrEmpty(result))
            {

            }

            return result;
        }

        private void Export()
        {

        }
    }
}
