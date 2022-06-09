using UnityEngine;
namespace Rotslib.Particle {
    public class ParticleSystemAutoDestroy : MonoBehaviour {
        private ParticleSystem ps;

        public void Start() {
            ps = GetComponent<ParticleSystem>();
        }

        public void Update() {
            if (ps) {
                if (!ps.IsAlive()) {
                    Destroy(gameObject);
                }
            }
        }
    }
}