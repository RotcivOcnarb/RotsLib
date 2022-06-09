using UnityEngine;

namespace Rotslib.Load {
    public class InitialLoader : MonoBehaviour {

        public static bool hasLoaded;
        public static string debugScene;

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

        private void Start() {
            LoadInitialScene(mainScene);
        }
    }
}
