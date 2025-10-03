using System;
using UnityEngine;
using System.Collections.Generic;

public class Card : MonoBehaviour
{

    public double Apply(double input)
    {
        return input;
    }

    public string GetCardName()
    {
        return "";
    }

    public void AssignRole(string name, string description, Func<double, double> application)
    {
        
    }
}
