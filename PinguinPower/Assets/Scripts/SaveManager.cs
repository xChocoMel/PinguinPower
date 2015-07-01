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

        return this.WriteXML("Player", "Checkpoint", new string[] { "Scene" + sceneindex }, new string[] { posString }, true);
    }

    public Vector3 LoadCheckpoint(int sceneindex)
    {
        try
        {
            string position = this.ReadXML("Player", "Checkpoint", "Scene" + sceneindex);
            String[] coordinates = position.Split(',');
            return new Vector3(float.Parse(coordinates[0]), float.Parse(coordinates[1]), float.Parse(coordinates[2]));
        }
        catch (Exception)
        {
            //Debug.Log("SaveManager exception: " + ex);
            return Vector3.zero;
        }
    }

    public bool SaveCharacterdata(int sceneindex, int[] values)
    {
        //Scene number makes no difference, save should be applied for all scenes
        sceneindex = 1;

        string value = values[0] + "," + values[1] + "," + values[2];
        return this.WriteXML("Player", "Characterdata", new string[] { "Scene" + sceneindex }, new string[] { value }, true);
    }

    public int[] LoadCharacterdata(int sceneindex)
    {
        //Scene number makes no difference, save should be applied for all scenes
        sceneindex = 1;

        try
        {
            string position = this.ReadXML("Player", "Characterdata", "Scene" + sceneindex);
            string[] values = position.Split(',');
            return new int[] { int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]) };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public bool SaveCollectedFriends(int sceneindex, Vector3[] positions)
    {
        //Scene number makes no difference, save should be applied for all scenes
        sceneindex = 1;

        List<string> positionStrings = new List<string>();

        foreach (Vector3 p in positions)
        {
            positionStrings.Add(p.x + "," + p.y + "," + p.z);
            Debug.Log("Position:" + p.x + "," + p.y + "," + p.z);
        }

        string[] posStrings = positionStrings.ToArray();
        return this.WriteXML("Player", "CollectedFriends", new string[] { "Scene" + sceneindex }, posStrings, false);
    }

    public Vector3[] LoadCollectedFriends(int sceneindex)
    {
        //Scene number makes no difference, save should be applied for all scenes
        sceneindex = 1;

        try
        {
            string[] positionStrings = this.ReadXML_MultipleValues("Player", "CollectedFriends", "Scene" + sceneindex);
            List<Vector3> positions = new List<Vector3>();

            foreach (string p in positionStrings)
            {
                string[] values = p.Split(',');

                try
                {
                    positions.Add(new Vector3 (float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2])));
                }
                catch (Exception) {}
            }
            
            return positions.ToArray();
        }
        catch (Exception)
        {
            return new Vector3[] { };
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
            catch (Exception)
            {
                return "";
            }
        }
        else
        {
            return "";
        }
    }

    private string[] ReadXML_MultipleValues (string filename, string title, string key)
    {
        string path = Application.persistentDataPath + "\\" + filename + ".xml";
        XElement baseElm;

        if (File.Exists(path))
        {
            baseElm = XElement.Load(path);
            List<string> valueList = new List<string>();

            try
            {
                foreach (XElement xel in baseElm.Element(title).Elements(key))
                {
                    valueList.Add(xel.Value);
                }

                return valueList.ToArray();
            }
            catch (Exception)
            {
                return valueList.ToArray();
            }
        }
        else
        {
            return new String[] {};
        }
    }

	private bool WriteXML(string filename, string title, string[] keys, string[] values, bool removeExistingKeys)
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

                    //Remove existing keys
                    if (removeExistingKeys)
                    {
                        try
                        {
                            foreach (String key in keys)
                            {
                                baseElm.Elements(title).Elements(key).Remove();
                            }
                        }
                        catch (Exception) {}
                    }

                    inner = baseElm.Element(title);

                    if (inner == null)
                    {
                        inner = new XElement(title);
                        baseElm.Add(inner);
                    }
                }
                catch(Exception)
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

            for (int i = 0; i < values.Length; i++)
            {
                XElement key;

                if (keys.Length > i)
                {
                    key = new XElement(keys[i]);
                }
                else
                {
                    key = new XElement(keys[0]);
                }
                 
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
