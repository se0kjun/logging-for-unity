using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class LogWrapper : LogHelper {
    // LOG IDENTIFIER: REQUIRED
    public string logID;

    #region LOG_OPTIONS
    public LogNameOptions logNameOption;
    public NextOptions logNextOption;
    public string logFileNamePrefix = "";
    public string logExtension;
    #endregion

    #region LOG_HEADER
    public List<string> logFileHeader;
    public LogHeaderDelimeter logFileHeaderDelim;
    #endregion

    #region LOG_PATH
    public string logFilePath;

    private string currentLogFileName;
    public string CurrentLogFileName
    {
        get
        {
            return currentLogFileName;
        }
        private set
        {
            currentLogFileName = value;
            base.logFileFullPath = logFilePath + "/" + logFileNamePrefix + value + "." + logExtension;
        }
    }
    #endregion

    public string CreateLogFile(string name = "")
    {
#if UNITY_ANDROID || UNITY_EDITOR
        if (logFilePath == string.Empty)
        {
#if UNITY_ANDROID
            logFilePath = Application.persistentDataPath;
#elif UNITY_EDITOR
            logFilePath = Application.dataPath;
#endif
        }
#endif

        // exists
        if (System.IO.File.Exists(logFilePath + name))
        {
            // open existing file
            if (name != "")
            {
                CurrentLogFileName = name;
                WriteLog(string.Join(((char)logFileHeaderDelim).ToString(), logFileHeader.ToArray()));

                return name;
            }
            else
            {
                return null;
            }
        }

        CurrentLogFileName = "";
        if ((logNameOption & LogNameOptions.METHOD) != 0)
        {
            CurrentLogFileName += ("_" + LogManager.Instance.CurrentMethod + "_");
        }
        if ((logNameOption & LogNameOptions.SUBJECT) != 0)
        {
            CurrentLogFileName += ("_" + LogManager.Instance.CurrentSubject + "_");
        }
        if ((logNameOption & LogNameOptions.TIMESTAMP) != 0)
        {
            CurrentLogFileName += DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_");
        }
        if ((logNameOption & LogNameOptions.COUNT) != 0 && logNextOption == NextOptions.AUTO)
        {
            CurrentLogFileName += LogManager.Instance.countData;
        }
        // create file, write header
        WriteLog(string.Join(((char)logFileHeaderDelim).ToString(), logFileHeader.ToArray()));

        return CurrentLogFileName;
    }
}
