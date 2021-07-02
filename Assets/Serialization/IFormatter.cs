using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySave;
using System;
using System.IO;


namespace EasySave
{
    public interface IFormatter
    {
        public void Register<T>(object obj) where T : SerializedElement;
        public void Unregister<T>(object obj) where T : SerializedElement;
        public void GenerateOutputAsync(Action<string> callback);
        public void GenerateElements<T>(string filePath) where T : SerializedElement;
        public void Load(object[] objs, LoadOption loadOption = LoadOption.NoDuplication);
    }
    public enum LoadOption
    {
        NoDuplication,
        SameTypeRandom,
        SameTypeFixed,
    }

}
