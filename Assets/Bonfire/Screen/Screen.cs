using UnityEngine;

namespace Bonfire
{
    public abstract class Screen : MonoBehaviour
    {
        // ElasticScreen: Fits all registered entities on screen. Screen becomes larger or smaller, may have limits.

        public abstract void Initialize();

        public abstract void Register(Transform target);

        public abstract void Unregister(Transform target);

        public abstract void Update();
    }
}
