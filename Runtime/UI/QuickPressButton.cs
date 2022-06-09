using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuickPressButton : Button, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

    public float maximumDistance = 1f;

    Vector2 position;

    public override void OnPointerClick(PointerEventData eventData) {
        
    }

    public override void OnPointerDown(PointerEventData eventData) {
        base.OnPointerDown(eventData);
        position = eventData.position;
    }

    public override void OnPointerUp(PointerEventData eventData) {
        base.OnPointerUp(eventData);
        if((eventData.position - position).magnitude < maximumDistance) {
            onClick.Invoke();
        }
    }

}

#if UNITY_EDITOR

[CustomEditor(typeof(QuickPressButton))]
public class QuickPressButtonEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
    }

}

#endif