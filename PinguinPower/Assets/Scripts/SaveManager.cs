using UnityEngine;
using System.Collections;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

public class SaveManager {

    public bool SaveCheckpoint(int sceneindex, Vector3 position)
    {
        string posString = position.x + "," + position.y + "," + position.z;
        return this.WriteXML("Player", "Checkpoint", new string[] { "Scene" + sceneindex }, new string[] { posString });
    }

    public Vector3 LoadCheckpoint(int sceneindex)
    {
        try
        {
            string position = this.ReadXML("Player", "Checkpoint", "Scene" + sceneindex);

            String[] coordinates = position.Split(',');
            return new Vector3(float.Parse(coordinates[0]), float.Parse(coordinates[1]), float.Parse(coordinates[2]));
        }
        catch (Exception ex)
        {
            //Debug.Log("SaveManager exception: " + ex);
            return Vector3.zero;
        }
    }

    private string ReadXML(string filename, string title, string key)
    {
        string path = Application.persistentDataPath + "\\" + filename + ".xml";

        XElement baseElm;

        if (File.Exists(path))
        {
            baseElm = XElement.Load(path);

            try
            {
                return baseElm.Element(title).Element(key).Value;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

	private bool WriteXML(string filename, string title, string[] keys, string[] values)
    {
        try
        {
            string path = Application.persistentDataPath + "\\" + filename + ".xml";

            XElement baseElm;

            XElement inner;

            if (File.Exists(path))
            {
                try
                {
                    baseElm = XElement.Load(path);
                    if (baseElm.Name != filename)
                    {
                        baseElm.Name = filename;
                    }

                    File.Delete(path);

                    try
                    {
                        foreach (String key in keys)
                        {
                            baseElm.Elements(title).Elements(key).Remove();
                        }
                    }
                    catch (Exception ex)
                    {
                        //Nothing
                    }

                    inner = baseElm.Element(title);
                    if (inner == null)
                    {
                        inner = new XElement(title);
                        baseElm.Add(inner);
                    }
                }
                catch(Exception ex)
                {
                    File.Delete(path);
                    baseElm = new XElement(filename);
                    inner = new XElement(title);
                    baseElm.Add(inner);
                }
            }
            else
            {
                baseElm = new XElement(filename);
                inner = new XElement(title);
                baseElm.Add(inner);
            }

            for (int i = 0; i < keys.Length; i++)
            {
                XElement key = new XElement(keys[0]);
                key.Value = values[0];
                inner.Add(key);
            }

            baseElm.Save(path);

            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("SaveManager exception: " + ex);
            return false;
        }
    }

    public void DeleteSaves()
    {
        try
        {
            string path = Application.persistentDataPath + "\\Player.xml";

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Delete Saves exception: " + ex);
        }
    }

    //Returns true if a save is found, otherwise false
    public bool SaveAvailable()
    {
        try
        {
            string path = Application.persistentDataPath + "\\Player.xml";

            if (File.Exists(path))
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("SaveAvailable exception: " + ex);
        }
        return false;
    }
}
