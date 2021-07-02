using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using System.Reflection;
using System;
using System.IO;

namespace EasySave
{
    public abstract class SerializedElement
    {
        public readonly string type;
        public readonly string id;

        protected string value;

        protected Dictionary<string, ISerializer> m_entries = new Dictionary<string, ISerializer>();
        protected SerializedElement(string id, string objType)
        {
            this.id = id;
            this.type = objType;
        }

        #region String Generation
        abstract public string ConvertToString();
        abstract public void SetEntry(string fieldName, ISerializer serializer);
        #endregion

        #region Element Generation 
        abstract public bool LoadElement(object obj);
        abstract public void SetString(string value);
        #endregion

    }
    public class FlatTextElement : SerializedElement
    {
        protected Dictionary<string, string> m_childrenIdFieldMap = new Dictionary<string, string>();
        protected Dictionary<string, string> m_elementEntries = new Dictionary<string, string>();

        public FlatTextElement(string id, string type) : base(id, type)
        {

        }


        public override void SetEntry(string id, ISerializer serializer)
        {
            m_entries[id] = serializer;
        }
        public override void SetString(string value)
        {
            this.value = value;
        }
        public void SetEntry(string id, string value)
        {
            m_elementEntries[id] = value;
        }

        public IEnumerable<string> GetChildrenID()
        {
            return m_childrenIdFieldMap.Keys;
        }

        public string GetChildFieldName(string id)
        {
            return m_childrenIdFieldMap[id];
        }

        public override string ConvertToString()
        {
            value = type + "\n";
            foreach (var entry in m_entries)
            {
                entry.Value.Serialize(out string value);
                this.value += entry.Key + "=" + value + "\n";
            }
            foreach (var elementEntry in m_elementEntries)
            {
                value += elementEntry.Key + "=" + elementEntry.Value + "\n";
            }
            return value;
        }

        public override bool LoadElement(object obj)
        {
            string[] fields = value.Split('\n');
            Type t = obj.GetType();
            bool successful = true;
            foreach (var field in fields)
            {
                string[] fieldInfoString = field.Split('=');
                char[] childChars = fieldInfoString[1].ToCharArray();
                FieldInfo fInfo = t.GetField(fieldInfoString[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                Attribute attr = fInfo.GetCustomAttribute(typeof(Savable));
                if (attr == null)
                    continue;

                //Check child element id
                if (childChars.Length > 3 && childChars[0] == '<' && childChars[1] == '@' && childChars[childChars.Length-1] == '>')
                {
                    string childId = fieldInfoString[1].Substring(2, childChars.Length - 3);
                    m_childrenIdFieldMap.Add(childId, fieldInfoString[0]);
                }
                else
                {
                    ISerializer serializer = Activator.CreateInstance(Type.GetType(((Savable)attr).serializer)) as ISerializer;
                    serializer.FieldInfo = fInfo;
                    serializer.Object = obj;
                    successful &= serializer.DeSerialize(fieldInfoString[1]);
                }
            }
            return successful;
        }

    }
}
