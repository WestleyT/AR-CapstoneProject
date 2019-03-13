using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class PlaneExtractor : MonoBehaviour {

    //this script senses and displays the real-world planes for the viewer to choose from
    //it is inactive at the very start of the game, activated during the SelectPlaneState's beginning
    //and deactivated again at the end of the SelectPlaneState

    public Transform BBoxTransform;
    public Vector3 BBoxExtents;
    public GameObject PlanePrefab;

    [BitMask(typeof(MLWorldPlanesQueryFlags))]
    public MLWorldPlanesQueryFlags QueryFlags;

    private float timeout = 2.0f;
    private float timeSinceLastRequest = 0.0f;

    private MLWorldPlanesQueryParams _queryParams = new MLWorldPlanesQueryParams();
    private List<GameObject> planeCache = new List<GameObject>();

    //inactive at start of game
    private void Awake() {
        //gameObject.SetActive(false);
    }

    private void Start() {
        MLWorldPlanes.Start();
    }

    private void OnDestroy() {
        MLWorldPlanes.Stop();
    }

    private void Update() {
        timeSinceLastRequest += Time.deltaTime;
        if (timeSinceLastRequest > timeout) {
            timeSinceLastRequest = 0f;
            RequestPlanes();
        }
    }

    //declaring the parameters for the Magic Leap's search area for planes
    void RequestPlanes() {
        _queryParams.Flags = QueryFlags;
        _queryParams.MaxResults = 100;
        _queryParams.BoundsCenter = BBoxTransform.position;
        _queryParams.BoundsRotation = BBoxTransform.rotation;
        _queryParams.BoundsExtents = BBoxExtents;

        MLWorldPlanes.GetPlanes(_queryParams, HandleOnRecievedPlanes);
    }

    private void HandleOnRecievedPlanes(MLResult result, MLWorldPlane[] planes, MLWorldPlaneBoundaries[] boundaries) {
        //destroy the old planes...
        DestroyOldPlanes();

        //...before creating the new ones
        GameObject newPlane;
        for (int i = 0; i < planes.Length; ++i) {
            newPlane = Instantiate(PlanePrefab);
            newPlane.transform.position = planes[i].Center;
            newPlane.transform.rotation = planes[i].Rotation;
            newPlane.transform.localScale = new Vector3(planes[i].Width, planes[i].Height, 1f);
            planeCache.Add(newPlane);
        }
    }

    public void DestroyOldPlanes() {
        for (int i = planeCache.Count - 1; i >= 0; --i) {
            Destroy(planeCache[i]);
            planeCache.Remove(planeCache[i]);
        }
    }
}
