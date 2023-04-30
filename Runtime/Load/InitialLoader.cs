using System.Collections;
using UnityEngine;

namespace Rotslib.Load {
    public class InitialLoader : MonoBehaviour {

        public static bool hasLoaded;
        public static string debugScene;
        public static bool waiting;

        [Attributes.Scene]
        public string mainScene;

        
        void LoadInitialScene(string scene) {
            if (!string.IsNullOrEmpty(debugScene)) {
                Transition.Transition.Goto(debugScene);
            }
            else {
                Transition.Transition.Goto(scene);
            }
            hasLoaded = true;
        }

        private void Awake() {
            Application.targetFrameRate = 60;
        }

        private IEnumerator Start() {
            while (waiting) yield return new WaitForEndOfFrame();
            LoadInitialScene(mainScene);
        }
    }
}
