using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveFlightPath
{
    private string destination = "D:/result3.txt";
    public static string dataToSave = "";
    public void Save()
    {
        StreamWriter sw = new StreamWriter(destination);
        sw.Write(dataToSave);
        sw.Close();
    }
}
