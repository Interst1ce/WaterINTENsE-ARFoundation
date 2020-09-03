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
                            //wait until user hit confirm button
                            GameObject.Find("EventSystem").GetComponent<StoryManager>().StartStory();
                        }
                    }
                }
            } else ghostScene.SetActive(false);
        }
    }
}
