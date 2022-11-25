using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public bool object_spawned;
    public Camera arCam;
    public Text overlay;
    public List<GameObject> unspawnedObjects;
    public List<GameObject> sceneObjects;
    private ARRaycastManager rayMan;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject erlMenu;

    // Start is called before the first frame update
    void Start()
    {
        object_spawned = false;
        rayMan = GetComponent<ARRaycastManager>();
        erlMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Toucheingabe
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (!object_spawned)
                {
                    InitializeScene(touch.position);
                }
                else
                {
                    
                    HitObject(touch);
                }

            }
        }


    }

    //Objekt-Spawns und Objekt-Bewegungen
    void HitObject(Touch touch)
    {
        Ray ray = arCam.ScreenPointToRay(touch.position);
        RaycastHit hitObject;
        //Tracking hits bei Objekten
        if (Physics.Raycast(ray, out hitObject))
        {
            GameObject selectObject = hitObject.collider.gameObject;
            if (selectObject.CompareTag("zahl"))
            {
                foreach (GameObject current in sceneObjects)
                {
                    if (current.GetInstanceID() == selectObject.GetInstanceID())
                    {
                        current.GetComponent<ZahlObjekt>().isSelected = true;

                        //Update Material bei korrektem Ergebnis
                        if (this.GetComponent<Aufgabe>().checkResult(getResult()))
                        {
                            current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().rightRes;
                            erlMenu.SetActive(true);
                        }
                        //Update Material bei falschem Ergebnis
                        else
                        {
                            current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().wrongRes;
                        }

                        
                    }
                    else
                    {
                        current.GetComponent<ZahlObjekt>().isSelected = false;
                        if(current.GetComponent<ZahlObjekt>().zahl != this.GetComponent<Aufgabe>().zahl1)
                        {
                            current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().defaultMat;
                        }
                        else
                        {
                            current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().selectMat;
                        }
                        
                    }
                }
            }
            if (selectObject.CompareTag("plane"))
            {
                TransformObjects(hitObject.point);
            }

        }
    }

    void InitializeScene(Vector2 touchPos)
    {
        //Tracking hits auf Plane
        if (rayMan.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitpose = hits[0].pose;
            //Spawning Objekt
            if (!object_spawned)
            {
                foreach (GameObject current in unspawnedObjects)
                {

                    Vector3 pos = new Vector3((hitpose.position.x + ((float)current.GetComponent<ZahlObjekt>().zahl / 5f)), hitpose.position.y, hitpose.position.z);
                    Vector3 rot = new Vector3(hitpose.rotation.x, hitpose.rotation.y, hitpose.rotation.z);
                    current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().defaultMat;
                    current.GetComponent<ZahlObjekt>().isSelected = false;
                    sceneObjects.Add(Instantiate(current, pos, Quaternion.Euler(0,rot.y, 0)));
                }
                object_spawned = true;
                ladeAufgabe(this.GetComponent<Aufgabe>().modus);
            }
        }
    }

    void TransformObjects(Vector3 hitpos)
    {
            if (object_spawned)
            {
                foreach (GameObject current in sceneObjects)
                {
                    Vector3 pos = new Vector3((hitpos.x + ((float)current.GetComponent<ZahlObjekt>().zahl / 5f)), hitpos.y, hitpos.z);
                    Vector3 rot = new Vector3(arCam.transform.rotation.x, arCam.transform.rotation.y, arCam.transform.rotation.z);
                    current.transform.position = pos;
                    current.transform.rotation = Quaternion.Euler(0, rot.y, 0);
                }
                    
            }
    }

    // Levelcontroller

    public int getResult()
    {
        foreach(GameObject current in sceneObjects)
        {
            if (current.GetComponent<ZahlObjekt>().isSelected)
            {
                return current.GetComponent<ZahlObjekt>().zahl;
            }
        }
        return -10;
    }

    public void ladeAufgabe(bool modus)
    {
        //Update Aufgabentext UI
        string aufgabenText;
        if (modus)
        {
            aufgabenText = "Berechne: " + this.GetComponent<Aufgabe>().zahl1 + " + " + this.GetComponent<Aufgabe>().zahl2;
        }
        else
        {
            aufgabenText = "Berechne: " + this.GetComponent<Aufgabe>().zahl1 + " - " + this.GetComponent<Aufgabe>().zahl2;
        }
        overlay.text = aufgabenText;

        //Aktualisiere Material der Sceneobjects -> Objekt mit Zahl1 wird gelb
        foreach(GameObject current in sceneObjects)
        {
            if(current.GetComponent<ZahlObjekt>().zahl == this.GetComponent<Aufgabe>().zahl1)
            {
                current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().selectMat;
            }
        }
        
    }
}