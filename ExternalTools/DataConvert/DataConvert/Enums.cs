using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConvert
{
    public class Enums
    {
        public enum ErrorCode
        {
            None = -1,
            WrongPath,              // 잘못된 경로 접근
            NotExistDirectory,      // 디렉토리 존재하지 않음
        }
    }
}
