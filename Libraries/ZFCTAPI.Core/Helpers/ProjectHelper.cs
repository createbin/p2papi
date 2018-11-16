using System;
using System.Collections.Generic;
using System.Text;
using ZFCTAPI.Core.Enums;

namespace ZFCTAPI.Core.Helpers
{
    public class ProjectHelper
    {
        public static string GenerateCheckCode(int index = 5)
        {
            Random random = new Random();
            var checkCode = "";
            for (int i = 0; i < index; i++)
            {
                var number = random.Next();

                char code;
                if (number % 2 == 0)
                    code = (char)('1' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }
            return checkCode;
        }

        /// <summary>
        /// 渤海交易类型
        /// </summary>
        /// <param name="dataCode"></param>
        /// <returns></returns>
        public static string GenerateBHTransType(int dataCode)
        {
            switch (dataCode)
            {
                case 378:
                    return QueryTransType.ExperBonus.ToString();
                case 460:
                    return QueryTransType.Drawings.ToString();
            }
            return "";
        }
    }
}
