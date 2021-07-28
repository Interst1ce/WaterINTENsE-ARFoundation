using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GlovesDisable : MonoBehaviour {

    [SerializeField]
    GameObject gloves;

    [SerializeField]
    Text debugText;
    public IEnumerator DisableTheGloves()
    {
        yield return new WaitForSeconds(0.5f);
        foreach(MeshRenderer renderer in gloves.GetComponentsInChildren<MeshRenderer>()){
            renderer.enabled = false;
        }
    }
    public async void DisableGloves() {
        
        try
        {
            await Task.Delay(TimeSpan.FromSeconds(0.833f));
            foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.enabled = false;
            }
        }
        catch
        {
            debugText.text = "something went wrong";
        }

    }
}
