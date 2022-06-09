using UnityEngine;

namespace Rotslib.Audio {
    public class OSTMusicChanger : MonoBehaviour {
        public AudioClip musicToChange;

        public void Start() {
            if (OSTManager.Instance) {
                OSTManager.Instance.PlayClip(musicToChange);
            }
        }
    }
}
