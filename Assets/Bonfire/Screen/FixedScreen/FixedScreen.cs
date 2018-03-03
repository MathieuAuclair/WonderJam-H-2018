using UnityEngine;

namespace Bonfire
{
    public class FixedScreen : Screen
    {
        [SerializeField] Camera baseCamera;

        public override void Initialize()
        {
            Object.Instantiate(baseCamera);
        }

        public override void Register(Transform t)
        {
        }

        public override void Unregister(Transform t)
        {
        }

        public override void Update()
        {
        }
    }
}