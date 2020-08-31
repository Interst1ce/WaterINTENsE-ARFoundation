using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacement : MonoBehaviour {
    [SerializeField]
    ARRaycastManager raycastManager;
    [SerializeField]
    GameObject scene;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
    }

    void Update() {
        if(Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began) {
                if (raycastManager.Raycast(touch.position,hits,TrackableType.PlaneWithinPolygon)) {
                    Pose hitPose = hits[0].pose;
                    scene.SetActive(true);
                    scene.transform.position = new Vector3(hitPose.position.x,scene.transform.position.y,hitPose.position.z);
                    scene.transform.rotation = hitPose.rotation;
                    scene.transform.Rotate(0,180,0,Space.Self);
                    StoryManager storyManager = GameObject.Find("EventSystem").GetComponent<StoryManager>();
                    storyManager.StartStory();
                }
            }
        }
    }
}
