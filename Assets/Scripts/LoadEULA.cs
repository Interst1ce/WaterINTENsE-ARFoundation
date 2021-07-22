using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadEULA : MonoBehaviour {
    [SerializeField]
    string eulaVer;

    GameObject checkBox;
    ScrollRect eulaBox;

    bool buttonSetup = false;

    private void Awake() {
        if (!CheckEULA()) SceneManager.LoadScene("EULA",LoadSceneMode.Additive);
    }

    private void Start() {
        if (SceneManager.GetSceneByName("EULA").isLoaded) {
            checkBox = GameObject.Find("Toggle");
            checkBox.SetActive(false);
            eulaBox = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        }
    }

    private void Update() {
        if (SceneManager.GetSceneByName("EULA").isLoaded) {
            if (eulaBox.verticalNormalizedPosition <= 0.05f && !checkBox.activeSelf) checkBox.SetActive(true);
            if (checkBox.activeSelf && checkBox.GetComponent<Toggle>().isOn && !buttonSetup) GameObject.Find("Button").GetComponent<Button>().onClick.AddListener(delegate { AcceptEULA(); });
        }
    }

    private bool CheckEULA() {
        var directory = Application.persistentDataPath + "/eula";
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
        var eula = directory + "/eula.txt";
        if (!File.Exists(eula)) {
            StreamWriter sw = File.CreateText(eula);
            sw.WriteLine("eula=false");
            sw.WriteLine(eulaVer);
            sw.Close();
            return false;
        } else {
            StreamReader sr = File.OpenText(eula);
            string fileContents = "";
            fileContents += sr.ReadLine();
            fileContents += sr.ReadLine();
            if (fileContents.Equals("eula=true" + eulaVer)) return true;
        }
        return false;
    }

    public void AcceptEULA() {
        var eula = Application.persistentDataPath + "/eula/eula.txt";
        File.Delete(eula);
        StreamWriter sw = File.CreateText(eula);
        sw.WriteLine("eula=true");
        sw.WriteLine(eulaVer);
        sw.Close();
        SceneManager.UnloadSceneAsync("EULA");
    }
}
