using UnityEngine;
using System.Collections;

public class LogHelper {
    // plain text
    public static void WriteLog(string path, string log)
    {
        using (var f = new System.IO.FileStream(/*LogFileFullPath*/path, System.IO.FileMode.Append, System.IO.FileAccess.Write))
        {
            using (var sw = new System.IO.StreamWriter(f, System.Text.Encoding.Unicode))
            {
                sw.WriteLine(log);
            }
        }
    }

    // delim
    public static void WriteLog(string path, string delim, params string[] log)
    {
        WriteLog(path, string.Join(delim, log));
    }

    // delim
    public static void WriteLog(string path, LogHeaderDelimeter delim, params string[] log)
    {
        WriteLog(path, string.Join(((char)delim).ToString(), log));
    }

    // csv
    public static void WriteLogCSV(string path, params string[] log)
    {
        WriteLog(path, "\t", log);
    }

    // write a plain text for specific file
    public static void WriteLog(string path, System.IO.FileMode option, string log)
    {
        using (var f = new System.IO.FileStream(path, option, System.IO.FileAccess.Write))
        {
            using (var sw = new System.IO.StreamWriter(f, System.Text.Encoding.Unicode))
            {
                sw.WriteLine(log);
            }
        }
    }
}
