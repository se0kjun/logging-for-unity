using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {
    public GameObject testObject;

	// Use this for initialization
	void Start () {
        //LogManager.Instance.SelectedLogFile("aa").LogFileFullPath
        //LogManager.Instance.CreateLogFile();
        //CommandManager.Instance.AddCommand(ref testObject, typeof(Vector3));
    }

    // Update is called once per frame
    void Update () {
        //LogManager.Instance.SelectedLogFile("aa").WriteLog("\t", "test111", "test");
        //LogManager.Instance.SelectedLogFile("bb").WriteLog("\t", "test222", "test333");
        //LogManager.Instance.WriteLog("\t", "test", "test");
        if (Input.GetMouseButton(0))
        {
            //CommandManager.Instance.ControlCommand(1.4f, "test");
            
            //CommandManager.Instance.SetValueRecursive(testObject, "transform.localScale.x", 140f);
        }
    }
}
