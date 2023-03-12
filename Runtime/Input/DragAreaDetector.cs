using System;
using UnityEngine;

namespace Rotslib.Input {
    public class DragAreaDetector : MonoBehaviour {

        bool pointerDown;
        bool dragging;

        public Action<Vector2, Vector2> OnDragBegin;
        public Action<Vector2, Vector2> OnDragEnded;
        public Action<Vector2, Vector2> OnDrag;

        Vector3 lastMousePos;

        void Update() {
            if (HasInputDown()) {
                pointerDown = true;
            }

            if (pointerDown) {
                Touch touch = GetTouch();
                if (touch.phase == TouchPhase.Moved) {
                    if (!dragging) {
                        dragging = true;
                        DragStart();
                    }
                    else {
                        Drag();
                    }
                }

                if (touch.phase == TouchPhase.Ended) {
                    pointerDown = false;
                    if (dragging) {
                        dragging = false;
                        DragEnded();
                    }
                }
            }

            lastMousePos = UnityEngine.Input.mousePosition;

        }

        public bool HasInputDown() {
            return UnityEngine.Input.touchCount > 0 || UnityEngine.Input.GetMouseButton(0);
        }

        public Touch GetTouch() {
            if (UnityEngine.Input.GetMouseButtonDown(0)) {
                Touch touch = new Touch() {
                    deltaTime = Time.deltaTime,
                    fingerId = 0,
                    position = UnityEngine.Input.mousePosition,
                    deltaPosition = UnityEngine.Input.mousePosition - lastMousePos,
                    phase = TouchPhase.Began,
                };
                return touch;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(0)) {
                Touch touch = new Touch() {
                    deltaTime = Time.deltaTime,
                    fingerId = 0,
                    position = UnityEngine.Input.mousePosition,
                    deltaPosition = UnityEngine.Input.mousePosition - lastMousePos,
                    phase = TouchPhase.Ended,
                };
                return touch;
            }
            else if (UnityEngine.Input.GetMouseButton(0)) {
                Touch touch = new Touch() {
                    deltaTime = Time.deltaTime,
                    fingerId = 0,
                    position = UnityEngine.Input.mousePosition,
                    deltaPosition = UnityEngine.Input.mousePosition - lastMousePos,
                    phase = UnityEngine.Input.mousePosition == lastMousePos ? TouchPhase.Stationary : TouchPhase.Moved,
                };
                return touch;
            }
            else {
                return UnityEngine.Input.GetTouch(0);
            }
        }

        public void DragStart() {
            OnDragBegin?.Invoke(GetTouch().position, GetTouch().deltaPosition);
        }

        public void DragEnded() {
            OnDragEnded?.Invoke(GetTouch().position, GetTouch().deltaPosition);
        }

        public void Drag() {
            OnDrag?.Invoke(GetTouch().position, GetTouch().deltaPosition);
        }
    }
}