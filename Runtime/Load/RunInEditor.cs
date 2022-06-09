using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rotslib.Load {
    public class RunInEditor : MonoBehaviour {
        void Awake() {
#if UNITY_EDITOR
            if (!InitialLoader.hasLoaded) {
                InitialLoader.debugScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("Load");
            }
#else
        Destroy(gameObject);
#endif
        }
    }

}