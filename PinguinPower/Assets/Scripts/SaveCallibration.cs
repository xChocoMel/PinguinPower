using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class SaveCallibration {

    public static void saveCallibration()
    {
        DateTime date = DateTime.Now;
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "//Callibration_" + date.ToString("dd-MM-yyyy_HH-mm-ss") + ".txt");
        writer.Write(CallibrationData.toString());
        writer.Close();
    }

    public static void loadCallibration()
    {
        //TODO
    }
}
