using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Controller : MonoBehaviour
{
    public bool object_spawned;
    public Camera arCam;
    private Vector2 touchPos = default;
    public List<GameObject> unspawnedObjects;
    public List<GameObject> sceneObjects;
    private ARRaycastManager rayMan;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        object_spawned = false;
        rayMan = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Touching touchscreen
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                if (!object_spawned)
                {
                    InitializeScene(touchPos);
                }
                else
                {
                    //TransformObjects(touchPos);
                    HitObject(touch);
                }

            }
        }
    }

    void HitObject(Touch touch)
    {
        Ray ray = arCam.ScreenPointToRay(touch.position);
        RaycastHit hitObject;
        //Tracking hits on Object
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
                        current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().selectMat;
                    }
                    else
                    {
                        current.GetComponent<ZahlObjekt>().isSelected = false;
                        current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().defaultMat;
                    }
                }
            }

        }
    }

    void InitializeScene(Vector2 touchPos)
    {
        //Tracking hits on Plane
        if (rayMan.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitpose = hits[0].pose;
            //Spawning Object
            if (!object_spawned)
            {
                foreach (GameObject current in unspawnedObjects)
                {

                    Vector3 pos = new Vector3((hitpose.position.x + ((float)current.GetComponent<ZahlObjekt>().zahl / 2f)), hitpose.position.y, hitpose.position.z);

                    current.GetComponent<MeshRenderer>().material = current.GetComponent<ZahlObjekt>().defaultMat;
                    current.GetComponent<ZahlObjekt>().isSelected = false;
                    sceneObjects.Add(Instantiate(current, pos, hitpose.rotation));
                }
                object_spawned = true;
            }
        }
    }

    void TransformObjects(Vector2 touchPos)
    {
        //Tracking hits on Plane
        if (rayMan.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitpose = hits[0].pose;
            //translation to new position if ray hits the plane
            if (object_spawned)
            {
                foreach (GameObject current in sceneObjects)
                {
                    current.transform.position = new Vector3((hitpose.position.x + current.GetComponent<ZahlObjekt>().zahl), hitpose.position.y, hitpose.position.z);
                    //current.transform.rotation = hitpose.rotation;
                }
            }
        }
    }

}