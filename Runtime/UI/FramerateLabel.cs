using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FramerateLabel : MonoBehaviour
{

    TextMeshProUGUI textMesh;

    private void Start() {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        textMesh.text = ((int)(1 / Time.deltaTime)).ToString();
    }
}
