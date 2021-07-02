using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime;
using EasySave;
using System.Reflection;

public class BasicSerializer : ISerializer
{
    public FieldInfo FieldInfo { private get; set; }
    public object Object { get; set; }



    //public bool ConvertToEntry(out Entry entry, string id, object[] data)
    //{
    //    entry = new Entry();
    //    string str= "";
    //    bool successful = true;
    //    Type type = data.GetType().GetElementType();
    //    if (type == typeof(int) ||
    //        type == typeof(float) ||
    //        type == typeof(string) ||
    //        type.IsEnum)
    //    {
    //        for (int i = 0; i < data.Length; i++)
    //        {
    //            str += Convert.ToString(data) + ";";
    //        }
    //        entry = new Entry(id, str);
    //    }
    //    else
    //    {
    //        successful = false;
    //    }
    //    return successful;
    //}

    public bool Serialize(out string output)
    {
        output = "";
        if (Object == null)
            return false;

        Type type = FieldInfo.FieldType;
        if (type == typeof(int) ||
            type == typeof(float) ||
            type == typeof(string) ||
            type.IsEnum)
        {
            output = Convert.ToString(FieldInfo.GetValue(Object));
            return true;
        }
        return false;
    }


    public bool DeSerialize(string input)
    {
        if (Object == null)
            return false;
        Type type = FieldInfo.FieldType;

        bool successful = true;
        object obj = null;

        if(type == typeof(string))
            obj = (string)input;
        else if (type == typeof(int))
            obj = int.Parse(input);
        else if (type == typeof(float))
            obj = float.Parse(input);
        else if (type.IsEnum)
            obj = Enum.Parse(type, input);
        else
            successful = false;

        FieldInfo.SetValue(Object, obj);

        return successful;
    }
}
