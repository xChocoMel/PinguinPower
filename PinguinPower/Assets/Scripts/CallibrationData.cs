using UnityEngine;
using System.Collections;

public static class CallibrationData {
    public static bool callibrated;

    public static float maxRollLeft;
    public static float maxRollRight;

    public static float maxGrabPlayer;

    public static float maxPitchPlayer;

    private static float maxPalmZ = -70f;
    public static float maxSpeedZ;

    private static float minPercentage = 0.25f;

    public static float GetMaxPalmZ()
    {
        return maxPalmZ;
    }

    public static float GetMinPercentage()
    {
        return minPercentage;
    }

    public static string toString()
    {
        string output = "";
        if (callibrated)
        {
            output += "Succesvol gecallibreerd \n";
            output += "MaxRollLeft: " + maxRollLeft + "\n";
            output += "MaxRollRight: " + maxRollRight + "\n";
            output += "MaxGrabPlayer: " + maxGrabPlayer + "\n";
            output += "MaxPitchPlayer: " + maxPitchPlayer + "\n";
            output += "MaxSpeedZ: " + maxSpeedZ + "\n";
        }
        else
        {
            output += "Niet gecallibreerd \n";
        }
        return output;
    }
}
