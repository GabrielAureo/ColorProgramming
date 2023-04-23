using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    [RequireComponent(typeof(Rigidbody))]
    public class Movable : MonoBehaviour
    {
        public Vector3 bottomAnchor;
        public Quaternion placementRotation = Quaternion.identity;

        [HideInInspector]
        public Rigidbody rigidBody;

        [HideInInspector]
        public Mesh mesh;
        public UnityAction<IARInteractable, IARInteractable> releaseAction;
        private Material materialInstance;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            mesh = GetComponent<MeshFilter>().sharedMesh;
            materialInstance = GetComponent<MeshRenderer>().material;
        }
    }
}
