using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectPlacement : MonoBehaviour {
    [SerializeField]
    ARRaycastManager raycastManager;
    [SerializeField]
    GameObject ghostScene;
    [SerializeField]
    GameObject scene;

    [SerializeField]
    GameObject startConfirmButton;
    [SerializeField]
    GameObject confirmButton;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool sceneSpawned = false;

    private void Start() {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Update() {
        if (!sceneSpawned) {
            if (raycastManager.Raycast(Camera.main.ViewportPointToRay(new Vector3(0.5f,0.3f,0)),hits,TrackableType.PlaneWithinPolygon)) {
                ghostScene.SetActive(true);
                Vector3 hitPos = hits[0].pose.position;
                ghostScene.transform.position = new Vector3(hitPos.x,hitPos.y + 0.05f,hitPos.z);
                Vector3 lookPos = Camera.main.transform.position - ghostScene.transform.position;
                lookPos.y = 0;
                ghostScene.transform.rotation = Quaternion.LookRotation(lookPos,Vector3.up);
                if (Input.touchCount > 0) {
                    Touch touch = Input.GetTouch(0);
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hit;
                    if (Physics.Raycast(ray,out hit)) {
                        Debug.Log("" + hit.transform.name);
                        if (hit.transform.gameObject == ghostScene.transform.GetChild(0).gameObject) {
                            ghostScene.SetActive(false);
                            scene.SetActive(true);
                            scene.transform.position = ghostScene.transform.position;
                            scene.transform.rotation = ghostScene.transform.rotation;
                            HideARPlanes();
                            GameObject.Find("AR Session Origin").GetComponent<ARPlaneManager>().enabled = false;
                            sceneSpawned = true;
                            PauseMenu pauseMenu = GameObject.Find("PauseUI").GetComponent<PauseMenu>();
                            pauseMenu.Pause();
                            pauseMenu.ToggleSubMenu(0);
                            GameObject.Find("ResizePanel").GetComponent<ResizeScene>().Init();
                        }
                    }
                }
            } else ghostScene.SetActive(false);
        }
    }

    private void FixedUpdate() {
        //if (sceneSpawned) HideARPlanes();
    }

    public void HideARPlanes() {
        foreach(GameObject plane in GameObject.FindGameObjectsWithTag("ARPlane")) {
            plane.GetComponent<ARPlaneMeshVisualizer>().enabled = false;
            plane.GetComponent<MeshRenderer>().enabled = false;
            plane.GetComponent<LineRenderer>().enabled = false;
        }
    }

    public void SwapConfirmButtons(StoryManager storyManager) {
        PauseMenu pauseMenu = GameObject.Find("PauseUI").GetComponent<PauseMenu>();
        pauseMenu.ToggleSubMenu(0);
        pauseMenu.Pause();
        storyManager.StartStory();
        startConfirmButton.SetActive(false);
        confirmButton.SetActive(true);
    }
}