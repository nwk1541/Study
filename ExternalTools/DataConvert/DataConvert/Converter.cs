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
        private IConvert m_convertMethod = null;
        private Methods m_method = Methods.None;

        public void Run(Methods method)
        {
            string srcPath = string.Empty;

            m_method = method;
            switch(method)
            {
                case Methods.ExcelToJson:
                    m_convertMethod = new ExcelToJson();
                    break;
            }

            string destPath = string.Empty;
        }

        private void InputPath()
        {

        }

        private void Export()
        {

        }
    }
}
