using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Aufgabe : MonoBehaviour
{ 
    //Variablen

    public int zahl1;
    public int zahl2;
    public bool modus;

    //Funktionen

    public bool checkResult(int erg)
    {
        int result;
        if (modus)
        {
            result = zahl1 + zahl2;
        }
        else
        {
            result = zahl1 - zahl2;
        }
        return (erg == result);
    }
}
