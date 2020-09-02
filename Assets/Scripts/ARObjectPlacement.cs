using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacement : MonoBehaviour {
    [SerializeField]
    ARRaycastManager raycastManager;
    [SerializeField]
    GameObject ghostScene;
    [SerializeField]
    GameObject scene;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool sceneSpawned = false;

    private void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
    }

    void Update() {
        if (!sceneSpawned) {
            if (raycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0)),hits,TrackableType.PlaneWithinPolygon)) {
                ghostScene.SetActive(true);
                Vector3 hitPos = hits[0].pose.position;
                ghostScene.transform.position = new Vector3(hitPos.x,hitPos.y + 0.05f,hitPos.z);
                Vector3 lookPos = Camera.main.transform.position - ghostScene.transform.position;
                lookPos.y = 0;
                ghostScene.transform.rotation = Quaternion.LookRotation(lookPos);
                if (Input.touchCount > 0) {
                    Touch touch = Input.GetTouch(0);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray,out hit)) {
                        Debug.Log("" + hit.transform.name);
                        if (hit.transform.gameObject == ghostScene) {
                            ghostScene.SetActive(false);
                            scene.SetActive(true);
                            scene.transform.position = ghostScene.transform.position;
                            scene.transform.rotation = ghostScene.transform.rotation;
                            sceneSpawned = true;
                            //pull up the scaling UI
                        }
                    }
                }
            } else ghostScene.SetActive(false);
        }

        //cast ray from center of the screen along the forward vector of the camera
        //if ray hits trackable plane make ghost version of scene visible
        //user taps on ghost version of the scene to place the full version of the scene
        //scale and height sliders appear to allow fine tuning of placement
        //user taps on UI button to confirm final placement
        //start story
        /*
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began && !sceneSpawned) {
                if (raycastManager.Raycast(touch.position,hits,TrackableType.PlaneWithinPolygon)) {
                    sceneSpawned = true;
                    Pose hitPose = hits[0].pose;
                    scene.SetActive(true);
                    scene.transform.position = hitPose.position;
                    scene.transform.rotation = hitPose.rotation;
                    scene.transform.Rotate(0,180,0,Space.Self);
                    GameObject.Find("EventSystem").GetComponent<StoryManager>().StartStory();
                }
            }
        }*/
    }
}
