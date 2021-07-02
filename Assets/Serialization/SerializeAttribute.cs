using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EasySave
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class Savable : Attribute
    {
        public readonly string serializer;
        public Savable(string serializer = "BasicSerializer")
        {
            this.serializer = serializer;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public class AllowNestedSerialization : Attribute
    {
        public readonly int depth;
        public AllowNestedSerialization(int depth = 2)
        {
            this.depth = depth;
        }
    }
}

