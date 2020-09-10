using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPieceTransition : MonoBehaviour {
    public GameObject setPieceTwo;
    public GameObject setPieceThree;

    public void TransitionOne() {
        StartCoroutine(ScaleTransition(setPieceTwo,36.667f));
    }

    public void TransitionTwo() {
        StartCoroutine(ScaleTransition(setPieceThree,95f));
    }

    IEnumerator ScaleTransition(GameObject inSetPiece, float delay = 0) {
        yield return new WaitForSeconds(delay);
        inSetPiece.transform.localScale = Vector3.one;
    }
}
