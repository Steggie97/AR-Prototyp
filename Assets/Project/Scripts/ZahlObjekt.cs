using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZahlObjekt : MonoBehaviour
{
    public int zahl;
    public bool isSelected=false;
    public Material defaultMat;
    public Material selectMat;
    public Material rightRes;
    public Material wrongRes;

    public void UpdateMaterial()
    {
        if (this.isSelected)
        {
            this.GetComponent<MeshRenderer>().material = selectMat;
        }
        else
        {
            this.GetComponent<MeshRenderer>().material = defaultMat;
        }
    }
 
}
