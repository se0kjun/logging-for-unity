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
        LogManager.Instance.SelectedLogFile("aa").WriteLog("\t", "test111", "test");
        LogManager.Instance.SelectedLogFile("bb").WriteLog("\t", "test222", "test333");
        //LogManager.Instance.WriteLog("\t", "test", "test");
    }
}
