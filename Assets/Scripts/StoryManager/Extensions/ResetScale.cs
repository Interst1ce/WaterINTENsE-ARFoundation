using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScale : MonoBehaviour {
    public void ToOne(string gameObject) {
        GameObject.Find(gameObject).transform.localScale = Vector3.one;
    }
}
