using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Raycast_script : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spawnObject;
    public bool object_spawned;

    ARRaycastManager rayMan;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        object_spawned = false;
        rayMan = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Spawning an object after touching the touchscreen
      if (Input.touchCount>0)
        {
            if(rayMan.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitpose = hits[0].pose;
                if(!object_spawned)
                {
                    spawnObject = Instantiate(prefab, hitpose.position, hitpose.rotation);
                    object_spawned = true;
                }
            }
        }
    }
}
