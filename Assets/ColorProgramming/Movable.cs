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

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            //mesh = GetComponent<MeshFilter>().sharedMesh;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + bottomAnchor, new Vector3(3, 0, 3));
        }
    }
}
