using System;
using System.IO;

namespace ZFCTAPI.Core.Helpers
{
    public partial class LogsHelper
    {
        public static void WriteLog(string strLog)
        {
            string sFilePath = AppPath() + "\\log\\" + DateTime.Now.ToString("yyyyMMdd");
            string sFileName = DateTime.Now.ToString("ddHH") + ".log";
            sFileName = sFilePath + "\\" + sFileName; //文件的绝对路径
            if (!Directory.Exists(sFilePath))//验证路径是否存在
            {
                Directory.CreateDirectory(sFilePath);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (System.IO.File.Exists(sFileName))
            //验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   ---   " + strLog);
            sw.Close();
            fs.Close();
        }

        #region 获取项目路径

        public static string AppPath()
        {
            var path = System.AppContext.BaseDirectory;
            var current = path;
            if (path.IndexOf("bin", StringComparison.Ordinal) >= 0)
            {
                var indexSrc = path.IndexOf("bin");
                current = path.Substring(0, indexSrc);
            }
            return current;
        }

        #endregion 获取项目路径
    }
}