using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TicketSystemDesign
{
    public class ErrorMsgHelper
    {
        /// <summary>
        /// Write the error log 
        /// </summary>
        /// <param name="message"></param>
        public static void WriteLog(string message)
        {
            string DIRNAME = Directory.GetCurrentDirectory() + "\\ErrorLog\\";
            string FILENAME = DIRNAME + "ErrorLog_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            if (!Directory.Exists(DIRNAME))
                Directory.CreateDirectory(DIRNAME);

            if (!File.Exists(FILENAME))
            {
                // The File.Create method creates the file and opens a FileStream on the file. You neeed to close it.
                File.Create(FILENAME).Close();
            }

            using (StreamWriter sw = File.AppendText(FILENAME))
            {
                Log(message, sw);
            }
        }

        /// <summary>
        /// log to the txt
        /// </summary>
        /// <param name="logMessage"></param>
        /// <param name="w"></param>
        private static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }
    }
}
