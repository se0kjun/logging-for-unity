using UnityEngine;
using System.Collections;

public class LogHelper {
    protected string logFileFullPath;

    // plain text
    public void WriteLog(string log)
    {
        using (var f = new System.IO.FileStream(logFileFullPath, System.IO.FileMode.Append, System.IO.FileAccess.Write))
        {
            using (var sw = new System.IO.StreamWriter(f, System.Text.Encoding.Unicode))
            {
                sw.WriteLine(log);
            }
        }
    }

    // delim
    public void WriteLog(string delim, params string[] log)
    {
        WriteLog(string.Join(delim, log));
    }

    // delim
    public void WriteLog(LogHeaderDelimeter delim, params string[] log)
    {
        WriteLog(string.Join(((char)delim).ToString(), log));
    }

    // csv
    public void WriteLogCSV(params string[] log)
    {
        WriteLog("\t", log);
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
