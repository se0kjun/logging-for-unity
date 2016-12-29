using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //LogManager.Instance.SelectedLogFile("aa").LogFileFullPath
        //LogManager.Instance.CreateLogFile();
	}
	
	// Update is called once per frame
	void Update () {
        LogHelper.WriteLog(LogManager.Instance.SelectedLogFile("aa").LogFileFullPath, "\t", "test111", "test");
        LogHelper.WriteLog(LogManager.Instance.SelectedLogFile("bb").LogFileFullPath, "\t", "test222", "test333");
        //LogManager.Instance.WriteLog("\t", "test", "test");
    }
}
