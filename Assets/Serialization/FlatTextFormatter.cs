using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Collections.Concurrent;

public class FlatTextFormatter : MonoBehaviour, IFormatter
{
    //save
    private int idCounter = 0;
    private Dictionary<Type, FieldInfo[]> m_typeFieldInfoMap = new Dictionary<Type, FieldInfo[]>();
    private Dictionary<object, string> m_saveObjectIdMap = new Dictionary<object, string>();
    private Dictionary<string, FlatTextElement> m_idElementMap = new Dictionary<string, FlatTextElement>();


    //load
    private Dictionary<string, object> m_loadedObjectIdMap = new Dictionary<string, object>();
    private Dictionary<string, Dictionary<string,FlatTextElement>> m_typeElementMap = new Dictionary<string, Dictionary<string,FlatTextElement>>();
    private Dictionary<string, FlatTextElement> m_loadedIdElementMap = new Dictionary<string, FlatTextElement>();

    private string m_output;
    private Mutex m_mut;
    private bool m_generationFlag = false;


    private ConcurrentQueue<Action> m_updateActions = new ConcurrentQueue<Action>();
    private Action<string> m_postOutputAction;
    private int m_taskCount;

    #region IFormatter Implementation

    public void GenerateOutputAsync(Action<string> callback)
    {
        m_mut = new Mutex();
        m_postOutputAction = callback;
        GenerateOutput();
    }
    public void Register<T>(object obj) where T : SerializedElement
    {
        Type type = obj.GetType();
        Attribute classAttr = type.GetCustomAttribute(typeof(AllowNestedSerialization));
        int depth = 0;
        if (classAttr != null)
            depth = ((AllowNestedSerialization)classAttr).depth;
        RegisterElement(obj, depth, idCounter);
        idCounter++;
    }
    public void Unregister<T>(object obj) where T : SerializedElement
    {
        if (!m_saveObjectIdMap.TryGetValue(obj, out string id))
            return;
        m_idElementMap.Remove(id);
        m_saveObjectIdMap.Remove(obj);
    }
    public void GenerateElements<T>(string filePath) where T : SerializedElement
    {
        using (StreamReader sw = new StreamReader(filePath))
        {
            m_loadedIdElementMap.Clear();
            m_typeElementMap.Clear();
            string line = sw.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                char[] headerChars = line.ToCharArray();

                //check element marker
                if (headerChars.Length>3 && headerChars[0] == '<' && headerChars[1] == '#' && headerChars[headerChars.Length-1] == '>')
                {
                    string id = line.Substring(2, headerChars.Length - 3);

                    //check element id
                    if (int.TryParse(id, out int value))
                    {
                        //element's object type
                        string type = sw.ReadLine();
                        if(!m_typeElementMap.ContainsKey(type))
                            m_typeElementMap.Add(type, new Dictionary<string, FlatTextElement>());
                        FlatTextElement element = new FlatTextElement(id, type);

                        //assign string to element
                        string entry = sw.ReadLine();
                        string elementValue = "";
                        while (entry != "</#>")
                        {
                            elementValue += (entry + '\n');
                            entry = sw.ReadLine();
                        }
                        elementValue = elementValue.TrimEnd(Environment.NewLine.ToCharArray());
                        element.SetString(elementValue);
                        m_typeElementMap[type].Add(id,element);
                        m_loadedIdElementMap.Add(id, element);
                    }
                }
                line = sw.ReadLine();
            }
        }
    }
    public void Load(object[] objs, LoadOption loadOption= LoadOption.NoDuplication)
    {
        m_loadedObjectIdMap.Clear();
        foreach (object obj in objs)
        {
            LoadObjectWODuplication(obj);
        }
    }

    #endregion


    #region FlatTextFormatter

   private void LoadObjectWODuplication(object obj)
    {
        if (obj == null)
            return;
        Type type = obj.GetType();
        if (!m_typeElementMap.ContainsKey(type.ToString()))
            return;

        Dictionary<string, FlatTextElement> elements = m_typeElementMap[type.ToString()];
        foreach (var element in elements)
        {
            string id = element.Key;
            if (!m_loadedObjectIdMap.ContainsKey(element.Key))
            {
                element.Value.LoadElement(obj);
                IEnumerable<string> children = element.Value.GetChildrenID();
                if (children.Count()>0)
                {
                    foreach (string childId in children)
                    {
                        FieldInfo fieldInfo = type.GetField(element.Value.GetChildFieldName(childId),BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static);
                        LoadObjectWODuplication(fieldInfo.GetValue(obj));
                    }
                }
                m_loadedObjectIdMap.Add(id, obj);
                return;
            }
        }
    }

    private void GenerateOutput()
    {
        m_output = "";
        m_taskCount = 0;
        int chunkCount = 500;

        List<object> objects = new List<object>(m_saveObjectIdMap.Keys);
        int eCountRemainder = objects.Count % chunkCount;
        int taskCount = eCountRemainder == 0 ? objects.Count / chunkCount : objects.Count / chunkCount + 1;

        m_generationFlag = true;
        for (int i = 0; i < taskCount; i++)
        {
            int length = (i == taskCount - 1) ? eCountRemainder : chunkCount;
            List<object> objs = objects.GetRange(i * chunkCount, length);
            Task task = new Task(() => SerializeElements(objs));
            m_taskCount++;
            task.Start();
        }
    }

    private void SerializeElements(List<object> objs)
    {
        string output = "";
        for (int i = 0; i < objs.Count; i++)
        {
            object obj = objs[i];
            if (obj != null)
            {
                string id = m_saveObjectIdMap[obj];
                FlatTextElement element = m_idElementMap[id];
                string header = "<#" + element.id + ">";
                output += header + "\n" + element.ConvertToString();
                output += "</#>" + "\n";
            }
        }
        m_updateActions.Enqueue(()=> UpdateOutputString(output));
    }

    private void UpdateOutputString(string input)
    {
        m_mut.WaitOne();
        m_output += input;
        m_taskCount--;
        m_mut.ReleaseMutex();
    }


    private void RegisterElement(object obj, int depth, int element_id)
    {
        idCounter++;
        Type type = obj.GetType();
        FieldInfo[] fieldInfos = GetFieldInfos(type);
        FlatTextElement element = new FlatTextElement(element_id.ToString(), type.ToString());
        fieldInfos.ToList().ForEach((field_info) =>
        {
            Attribute fieldAttr = field_info.GetCustomAttribute(typeof(Savable));
            if (fieldAttr != null)
            {
                Attribute classAttr = field_info.FieldType.GetCustomAttribute(typeof(AllowNestedSerialization));
                if (classAttr != null && depth > 0)
                {
                    element.SetEntry(field_info.Name, "<@" + idCounter.ToString() + ">");
                    RegisterElement(field_info.GetValue(obj), depth - 1, idCounter);
                }
                else if (classAttr != null && depth == 0)
                {
                    element.SetEntry(field_info.Name, "");
                }
                else if (classAttr == null)
                {
                    Type sType = Type.GetType(((Savable)fieldAttr).serializer);
                    ISerializer serializer = Activator.CreateInstance(sType) as ISerializer;
                    serializer.FieldInfo = field_info;
                    serializer.Object = obj;
                    element.SetEntry(field_info.Name, serializer);
                }
            }
        });
        m_saveObjectIdMap[obj] = element_id.ToString();
        m_idElementMap[element_id.ToString()] = element;
    }

    private FieldInfo[] GetFieldInfos(Type type)
    {
        if (!m_typeFieldInfoMap.ContainsKey(type))
            m_typeFieldInfoMap[type] = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        return m_typeFieldInfoMap[type];
    }

    #endregion


    #region Mono

    private void Update()
    {
        if (m_generationFlag && m_updateActions.Count > 0)
        {
            if (m_updateActions.TryDequeue(out Action action))
            {
                action.Invoke();
            }
        }
        if (m_taskCount == 0&& m_generationFlag)
        {
            m_postOutputAction.Invoke(m_output);
            m_generationFlag = false;
            m_mut.Dispose();
        }
    }

    #endregion



}