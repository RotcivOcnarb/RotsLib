using UnityEngine;

namespace Rotslib.Input {
    public static class GeneralTouch {
        static Vector3 lastMousePos;

        public static Touch GetTouch(int index) {

            if (index == 0) {
                if (UnityEngine.Input.GetMouseButtonDown(0)) {
                    Touch touch = new Touch() {
                        deltaTime = Time.deltaTime,
                        fingerId = 0,
                        position = UnityEngine.Input.mousePosition,
                        deltaPosition = UnityEngine.Input.mousePosition - lastMousePos,
                        phase = TouchPhase.Began,
                    };
                    lastMousePos = UnityEngine.Input.mousePosition;
                    return touch;
                }
                if (UnityEngine.Input.GetMouseButtonUp(0)) {
                    Touch touch = new Touch() {
                        deltaTime = Time.deltaTime,
                        fingerId = 0,
                        position = UnityEngine.Input.mousePosition,
                        deltaPosition = UnityEngine.Input.mousePosition - lastMousePos,
                        phase = TouchPhase.Ended,
                    };
                    lastMousePos = UnityEngine.Input.mousePosition;
                    return touch;
                }
                if (UnityEngine.Input.GetMouseButton(0)) {
                    Touch touch = new Touch() {
                        deltaTime = Time.deltaTime,
                        fingerId = 0,
                        position = UnityEngine.Input.mousePosition,
                        deltaPosition = UnityEngine.Input.mousePosition - lastMousePos,
                        phase = UnityEngine.Input.mousePosition == lastMousePos ? TouchPhase.Stationary : TouchPhase.Moved,
                    };
                    lastMousePos = UnityEngine.Input.mousePosition;
                    return touch;
                }

                 return UnityEngine.Input.GetTouch(index);
            }
            else {
                return UnityEngine.Input.GetTouch(index - GetMouseTouches());
            }

        }

        static int GetMouseTouches() {
            if (UnityEngine.Input.GetMouseButton(0) || UnityEngine.Input.GetMouseButtonUp(0)) return 1;
            return 0;
        }

        public static int GetTouchCount() {
            return UnityEngine.Input.touchCount + GetMouseTouches();
        }
    }
}