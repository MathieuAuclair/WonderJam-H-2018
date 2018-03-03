using MeshSplitting.Splitables;
using UnityEngine;

namespace MeshSplitting.Demo
{
    public class DemoController : MonoBehaviour
    {
        public GameObject[] SplitablePrefabs;

        private int _materialIndex;

        private void Start()
        {
            if (SplitablePrefabs.Length > 0)
            {
                Instantiate(SplitablePrefabs[0], Vector3.up * 2f, Quaternion.identity);
            }
        }

        private void Update()
        {
            for (int i = 0; i < SplitablePrefabs.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    Splitable splitable = FindObjectOfType(typeof(Splitable)) as Splitable;
                    if (splitable != null)
                    {
                        if (splitable.transform.parent == null)
                            Destroy(splitable.gameObject);
                        else
                            Destroy(splitable.transform.parent.gameObject);
                    }

                    Instantiate(SplitablePrefabs[i]);
                }
            } 
        }
    }
}
