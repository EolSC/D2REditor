using UnityEngine;
using System.Runtime.InteropServices;
using System.IO;
using System;
public class TestNative
{
    [DllImport(@"GrannyLoader.dll", EntryPoint =
  "test_loading", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    public static extern int test_loading(string ptchar);
    public int Test()
    {
        string fileName = "Assets/TestResources/l_barrel01_lod0.model";
        int result = test_loading(fileName);
        return result;
    }
}
