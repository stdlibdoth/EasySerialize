using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics;

public class TestController : MonoBehaviour
{
    CubeScript[] m_objs;
    TestObject[] m_testObjects;
    TestObj[] m_testObjs;
    private IFormatter m_formatter;
    public string filePath;

    public Text m_console;

    private void Awake()
    {
        m_objs = FindObjectsOfType<CubeScript>();
        m_formatter = GetComponent<IFormatter>();
        m_testObjects = FindObjectsOfType<TestObject>();
        m_testObjs = FindObjectsOfType<TestObj>();
    }

    private void OnGUI()
    {
        //if (GUI.Button(new Rect(100, 200, 100, 50), "Registor"))
        //{
        //    foreach (var obj in m_objs)
        //    {
        //        RegisterSave(obj);
        //    }
        //}


        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 50;
        // Load and set Font
        Font myFont = (Font)Resources.Load("Fonts/comic", typeof(Font));
        myButtonStyle.font = myFont;
        // Set color for selected and unselected buttons
        myButtonStyle.normal.textColor = Color.white;
        myButtonStyle.hover.textColor = Color.red;

        if (GUI.Button(new Rect(100, 400, 300, 150), "Save", myButtonStyle))
        {
            foreach (var obj in m_objs)
            {
                RegisterSave(obj);
            }
            foreach (var obj in m_testObjects)
            {
                RegisterSave(obj);
            }

            SerializeAll();
        }


        if (GUI.Button(new Rect(100, 600, 300, 150), "Load", myButtonStyle))
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            m_console.text = "Loading.....";
            m_formatter.GenerateElements<SerializedElement>(Directory.GetCurrentDirectory() + "/" + filePath);
            m_formatter.Load(m_objs);
            m_formatter.Load(m_testObjects);
            m_console.text = "Saves Loaded to objects' fields";
            m_console.text += "\nMs:" + stopwatch.ElapsedMilliseconds;
        }

        if (GUI.Button(new Rect(100, 800, 300,150), "Reconstruct", myButtonStyle))
        {
            foreach (var obj in m_objs)
            {
                obj.SetPosition();
            }
            m_console.text = "Cubes Reconstructed";
        }
    }

    public void SerializeAll()
    {
        string path = Directory.GetCurrentDirectory() + "/test.txt";
        m_console.text = "Saving.....";
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        m_formatter.GenerateOutputAsync((output) =>
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(output);
            }
            Battlehub.Dispatcher.Dispatcher.Current.BeginInvoke(()=> m_console.text = "Saved to " + path);
            Battlehub.Dispatcher.Dispatcher.Current.BeginInvoke(()=> m_console.text += "\nMs:" + stopwatch.ElapsedMilliseconds);
        });
    }


    public void RegisterSave(object obj)
    {
        m_formatter.Register<SerializedElement>(obj);
    }
}