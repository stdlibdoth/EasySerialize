using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using System.Reflection;
using System;

public class V3Serializer : ISerializer
{
    public FieldInfo FieldInfo { private get; set; }
    public object Object { get; set; }
    public bool DeSerialize(string input)
    {
        if (Object == null)
            return false;
        Type type = FieldInfo.FieldType;

        bool successful = true;

        if (type == typeof(Vector3))
        {
            string[] value = input.Split(',');
            if (value.Length != 3)
                return false;
            float x = 0, y = 0, z = 0;
            if (float.TryParse(value[0], out x) && float.TryParse(value[1], out y) && float.TryParse(value[2], out z))
                FieldInfo.SetValue(Object, new Vector3(x, y, z));
        }
        else
            successful = false;

        return successful;
    }


    public bool Serialize(out string output)
    {
        output = "";
        if (Object == null)
            return false;

        Type type = FieldInfo.FieldType;
        if (type == typeof(Vector3))
        {
            Vector3 v = (Vector3)FieldInfo.GetValue(Object);
            output = v.x.ToString() + ',' + v.y.ToString() + ',' + v.z.ToString();
            return true;
        }
        return false;
    }
}
