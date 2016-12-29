using UnityEngine;
using System.Collections.Generic;
using System;

[Flags]
public enum LogNameOptions
{
    TIMESTAMP = 1,
    METHOD = 2,
    SUBJECT = 4,
    COUNT = 8,
    TIMESTAMP_AND_METHOD = 3,
    TIMESTAMP_AND_SUBJECT = 5,
    TIMESTAMP_AND_COUNT = 9,
    METHOD_AND_SUBJECT = 6,
    TIMESTAMP_METHOD_AND_SUBJECT = 7
}

// AUTO: when application quit, increase index number
// MANUAL: control the other application
public enum NextOptions
{
    AUTO,
    MANUAL
}

public enum LogHeaderDelimeter
{
    SPACE = ' ',
    TAB = '\t',
    SLASH = '/',
    NEWLINE = '\n',
    UNDERSCORE = '_',
    COMMA = ','
}

public class LogManager : MonoBehaviour {
    #region SINGLETON
    private static LogManager _instance;
    public static LogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(LogManager)) as LogManager;
                if (_instance == null)
                {
                    Debug.LogError("There needs to be one active LogManager script on a GameObject in your scene.");
                }
            }

            return _instance;
        }
    }
    #endregion

    #region SUBJECT_METHOD
    [SerializeField]
    private List<string> subjectList;
    public List<string> SubjectList
    {
        get
        {
            return subjectList;
        }
    }
    [SerializeField]
    private List<string> methodList;
    public List<string> MethodList
    {
        get
        {
            return methodList;
        }
    }
    private int currentSubjectIdx;
    public int CurrentSubjectIdx
    {
        get
        {
            return currentSubjectIdx;
        }
    }
    private int currentMethodIdx;
    public int CurrentMethodIdx
    {
        get
        {
            return currentMethodIdx;
        }
    }
    public string CurrentMethod
    {
        get
        {
            return methodList[currentMethodIdx];
        }
    }
    public string CurrentSubject
    {
        get
        {
            return subjectList[currentSubjectIdx];
        }
    }
    #endregion

    #region COUNT_METADATA
    private const string COUNT_FILE_NAME = "data.txt";
    public int countData;
    #endregion

    [SerializeField]
    private List<LogWrapper> logList;

    public LogWrapper SelectedLogFile(string log_id)
    {
        return logList.Find(item => item.logID == log_id);
    }

    // not supported
    public void NextSubject()
    {
        if (subjectList.Count - 1 == currentSubjectIdx)
            return;

        currentSubjectIdx++;
        //CreateLogFile();
    }

    // not supported
    public void NextMethod()
    {
        if (methodList.Count - 1 == currentMethodIdx)
            return;

        currentMethodIdx++;
        //CreateLogFile();
    }

    void Awake()
    {
        currentSubjectIdx = 0;
        currentMethodIdx = 0;

#if UNITY_ANDROID || UNITY_EDITOR
        if (logList.Exists(item => item.logNextOption == NextOptions.AUTO))
        {
#if UNITY_ANDROID
            if (System.IO.File.Exists(Application.persistentDataPath + "/" + COUNT_FILE_NAME))
            {
                using (var f = new System.IO.FileStream(Application.persistentDataPath + "/" + COUNT_FILE_NAME, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
#elif UNITY_EDITOR
            if (System.IO.File.Exists(Application.dataPath + "/" + COUNT_FILE_NAME))
            {
                using (var f = new System.IO.FileStream(Application.dataPath + "/" + COUNT_FILE_NAME, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
#endif
                    using (var sw = new System.IO.StreamReader(f, System.Text.Encoding.Unicode))
                    {
                        countData = int.Parse(sw.ReadLine());
                    }
                }
            }
            else
            {
                countData = 0;
            }
        }
#endif

        // init
        logList.ForEach(x => x.CreateLogFile());
    }

    void OnApplicationQuit()
    {
        // increase subject or method number
        if (logList.Exists(item => item.logNextOption == NextOptions.AUTO) && System.IO.File.Exists(Application.dataPath + "/" + COUNT_FILE_NAME))
        {
            countData++;
#if UNITY_ANDROID
            LogHelper.WriteLog(Application.persistentDataPath + COUNT_FILE_NAME, System.IO.FileMode.Truncate, countData.ToString());
#elif UNITY_EDITOR
            LogHelper.WriteLog(Application.dataPath + "/" + COUNT_FILE_NAME, System.IO.FileMode.Truncate, countData.ToString());
#endif
        }
        else
        {
            countData++;
#if UNITY_ANDROID
            LogHelper.WriteLog(Application.persistentDataPath + COUNT_FILE_NAME, System.IO.FileMode.Create, countData.ToString());
#elif UNITY_EDITOR
            LogHelper.WriteLog(Application.dataPath + "/" + COUNT_FILE_NAME, System.IO.FileMode.Create, countData.ToString());
#endif
        }
    }
}
