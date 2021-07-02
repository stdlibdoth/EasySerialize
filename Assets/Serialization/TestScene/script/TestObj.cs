using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;

[AllowNestedSerialization]
public class TestObj : MonoBehaviour
{

    [Savable] private float testFloat;
    [Savable] public TestObject.SEnum testEnum;

    [Savable] public string m_string1;
    [Savable] private string m_string2;
}
