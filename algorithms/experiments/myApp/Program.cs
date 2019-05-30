using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;
using System.IO;

namespace myApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}

class Room{
    Vector2Int postition;
    Dictionary<string, string> entrances = new Dictionary<string, string>(){
        {"Left", "Right"},
        {"Right", "Left"},
        {"Up", "Down"},
        {"Down", "Up"}
    };

    public Room()
    {
        
    }
}
