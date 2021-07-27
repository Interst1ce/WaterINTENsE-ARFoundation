using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectHit : MonoBehaviour
{
    [SerializeField]
    Text debugtext;

    private void OnCollisionEnter(Collision collision)
    {
        debugtext.text = "hit " + this.gameObject.name;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
