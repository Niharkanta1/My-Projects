using System;
using System.IO;

namespace SupremeMemCleaner
{
    public class LogControl
    {
        private static String _Path = String.Empty;
        private static String subfolder = "log";
        private static String directory = String.Empty;
        private static String logName = "Applog.txt";
        private static bool DEBUG = true;

        public static void Info(string msg)
        {
            msg = "INFO: " + msg;
            _Path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            directory = Path.Combine(_Path, subfolder);
            try
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (StreamWriter w = File.AppendText(Path.Combine(directory, logName)))
                {
                    Log(msg, w);
                }
                if (DEBUG)
                    Console.WriteLine(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: in LogControl: " + e.Message);
            }
        }

        public static void Error(string msg)
        {
            msg = "ERROR: " + msg;
            _Path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            directory = Path.Combine(_Path, subfolder);
            try
            {
                if (!Directory.Exists(directory))
                {
                   Directory.CreateDirectory(directory);
                }
                using (StreamWriter w = File.AppendText(Path.Combine(directory, logName)))
                {
                    Log(msg, w);
                }
                if (DEBUG)
                    Console.WriteLine(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: in LogControl: " + e.Message);
            }
        }

        static private void Log(string msg, TextWriter w)
        {
            try
            {
                w.Write("[{0} {1}]", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.Write("\t");
                w.Write(" {0}", msg);
                w.Write(Environment.NewLine);
            }
            catch (Exception e)
            {
               Console.WriteLine("ERROR: in LogControl: "+e.StackTrace);
            }
        }
    }
}
