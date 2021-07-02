using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;


[AllowNestedSerialization]
[System.Serializable]
public class Obj01
{

    [Savable] public string s1 = "adf43taf";
    [Savable] public string s2 = "afs324df";
    [Savable] public int i1 = 1;
    [Savable] public float f1 = 3f;
    [Savable] public Obj02 obj02 = new Obj02();
    [Savable] public Obj03 obj03 = new Obj03();
}

[AllowNestedSerialization]
[System.Serializable]
public class Obj02
{
    private string s1 = "ad21f";
    private string s2 = "afsdf";
    [Savable] public int i1 = 1;
    private float f1 = 3f;
    [Savable] public Obj03 obj03 = new Obj03();
}


[AllowNestedSerialization]
[System.Serializable]
public class Obj03
{
    [Savable] public string s1 = "adf";
    private string s2 = "afsdf";
    [Savable] public int i1 = 1;
    private float f1 = 3f;

}