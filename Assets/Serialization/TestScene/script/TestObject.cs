using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;

[AllowNestedSerialization(3)]
public class TestObject : MonoBehaviour
{
    [System.Serializable]
    public enum SEnum
    {
        SEnum1,
        SEnum2,
        SEnum3,
    }


    [Savable] public string testString;
    [Savable] public int testInt;
    [Savable] public float testFloat;
    [Savable] public SEnum testEnum;

    //public string test;
    //[Savable] public string m_string1;
    //[Savable] public string m_string2;
    //private string m_string3;
    //[Savable] private string m_string4;
    [Savable]private TestObj m_tObject;

    [SerializeField] private TestObj m_ref;

    //[Savable] private string m_string5;
    [Savable] public Obj01 m_obj1 = new Obj01();

    [Savable] public string m_string6;
    [Savable] public string m_strin;
    [Savable] public string m_strig;
    [Savable] public string m_sting;
    [Savable] public string m_string7;
    [Savable] public string m_string8;
    [Savable] public string m_string9;
    [Savable] public string m_string0;
    [Savable] public string m_string56;
    [Savable] public string m_stri4567ng;
    [Savable] public string m_strewgi4567ng;
    [Savable] public string m_strin4576g;
    [Savable] public string m_string;
    [Savable] public string m_striwergng;
    [Savable] public string m_stwergring;
    [Savable] public string m_str4567ing;
    [Savable] public string m_strweging;
    [Savable] public string m_st6457ring;
    [Savable] public string m_stsdfgring;
    [Savable] public string m_stsd4er5fgring;
    [Savable] public string m_strisfgng;
    [Savable] public string m_strqerting;
    [Savable] public string m_ssdfgtring;
    //[Savable] private string m_stsgring;
    //[Savable] private string m_strwering;
    //[Savable] private string m_strweting;
    //[Savable] private string m_strewrting;
    //[Savable] private string m_stegring;
    //[Savable] private string m_strergierng;
    //[Savable] private string m_ssdgtring;
    //[Savable] private string m_strsdfging;
    //[Savable] private string m_str43rring;
    //[Savable] private string m_str342ring;
    //[Savable] private string m_strr342r32ing;
    //[Savable] private string m_str32ring;
    //[Savable] private string m_st32324rring;
    //[Savable] private string m_str34ring;
    //[Savable] private string m_str234rring;
    //[Savable] private string m_str3ing;
    //[Savable] private string m_str34asf2qring;
    //[Savable] private string m_str3asd4ring;
    //[Savable] private string m_str34adfring;
    //[Savable] private string m_strq3ring;
    //[Savable] private string m_st3qrfring;
    //[Savable] private string m_strr43ing;
    //[Savable] private string m_st3q2rring;
    //[Savable] private string m_str34rfing;
    //[Savable] private string m_stq2r43ring;
    //[Savable] private string m_str3rfing;
    //[Savable] private string m_strq3rfing;
    //[Savable] private string m_st3f34f3ring;
    //[Savable] private string m_str1rqing;
    //[Savable] private string m_striqrf34ng;
    //[Savable] private string m_stf4ring;
    //[Savable] private string m_sasftring;
    //[Savable] private string m_strwrtgving;
    //[Savable] private string m_strrbhing;
    //[Savable] private string m_strtgvring;
    //[Savable] private string m_strirbng;
    //[Savable] private string m_strasdfasdfing;
    //[Savable] private string m_stertbring;
    //[Savable] private string m_stasfring;
    //[Savable] private string m_strerbreing;
    //[Savable] private string m_stasdfring;
    //[Savable] private string m_staretbsdfring;
    //[Savable] private string m_strasdfing;
    //[Savable] private string m_str3t4ing;
    //[Savable] private string m_str3asdft4ing;
    //[Savable] private string m_stergvring;
    //[Savable] private string m_strerting;
    //[Savable] private string m_strwerging;
    //[Savable] private string m_strwerting;
    //[Savable] private string m_stretgering;
    //[Savable] private string m_striwertng;



    private void Start()
    {
        m_tObject = Instantiate(m_ref);
        //EasySaveManager.RegisterSave(this);
    }

}
