/* 
 * Written on 2021-19-24
 * Code by MS VS 2019
 * Reference : 
 * - NanoXLSX 1.8.1 https://github.com/rabanti-github/NanoXLSX by Nuget Package
 * - Newtonsoft.Json 13.0.1 https://github.com/JamesNK/Newtonsoft.Json/releases by Nuget Package
 */

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NanoXLSX;
using Newtonsoft.Json;

namespace DataConvert
{
    class ExcelToJson : IConvert
    {
        [Serializable]
        public class WrappedData
        {
            public string m_sheedName = string.Empty;
            public Dictionary<string, List<object>> m_values = new Dictionary<string, List<object>>();
        }

        private string m_sourcePath = string.Empty;

        public ExcelToJson(string srcPath)
        {
            m_sourcePath = srcPath;

            Init();
        }

        public void Init()
        {

        }

        public void OnBeforeOperation()
        {

        }

        public string Operation()
        {
            OnBeforeOperation();

            string result = string.Empty;

            OnAfterOperation();

            return result;
        }

        public void OnAfterOperation()
        {

        }

        public void OnError()
        {

        }
    }
}
