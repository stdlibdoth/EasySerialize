using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using System.Reflection;

public interface ISerializer
{
    FieldInfo FieldInfo { set; }
    object Object { set; get; }
    bool Serialize(out string output);
    bool DeSerialize(string input);
}