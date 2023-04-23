using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class MovableController : MonoBehaviour
    {
        public Movable currentMovable;
        public bool isHolding;
        HingeJoint hinge;
        public Transform previewer;

        private IMovableProvider currentTargetSocket;

        private void Awake()
        {
            hinge = GetComponent<HingeJoint>();
            var touchController = GetComponent<ARTouchController>();
            SetupController(touchController);
        }

        public void SetupController(ARTouchController touchController)
        {
            isHolding = false;
            isTargeting = false;
            //touchController.onTouch.AddListener(Touch);
            touchController.OnHold.AddListener(Grab);
            touchController.OnRelease.AddListener(Release);
            previewer = new GameObject(
                "Previewer",
                typeof(MeshFilter),
                typeof(MeshRenderer)
            ).transform;
        }

        private void Grab(ARTouchData touchData)
        {
            if (!(touchData.selectedInteractable is IMovableProvider))
                return;
            var provider = touchData.selectedInteractable as IMovableProvider;
            isHolding = true;

            if (!provider.ShouldTake())
                return;

            var takenMovable = provider.GetMovable(touchData);
            if (takenMovable)
            {
                currentMovable = takenMovable;
                ConnectToHinge(takenMovable);
            }
        }

        public void Grab(Movable movable)
        {
            currentMovable = movable;
            ConnectToHinge(currentMovable);
        }

        private void Update()
        {
            CheckTarget(ARTouchController.touchData);
        }

        private bool isTargeting;

        void CheckTarget(ARTouchData touchData)
        {
            if (!isHolding)
                return;
            RaycastHit[] hits = new RaycastHit[10];
            var hitSize = Physics.RaycastNonAlloc(touchData.ray, hits);
            if (hitSize <= 0)
            {
                for (var i = 0; i < hitSize; i++)
                {
                    var hit = hits[i];
                    var isSocket = hit.transform.GetComponent<IMovableProvider>();
                    if (currentTargetSocket != null)
                    {
                        isTargeting = true;
                        break;
                    }
                }
            }
        }

        // private void SetPreviewer(MovablePlacementPose pose)
        // {
        //     previewer.position = pose.position;
        //     previewer.rotation = pose.rotation;
        //     previewer.localScale = pose.scale;

        //     var filter = previewer.GetComponent<MeshFilter>();
        //     filter.sharedMesh = currentMovable.mesh;

        //     previewer.gameObject.SetActive(true);

        // }

        void Release(ARTouchData touchData)
        {
            if (touchData.lastStatus != ARTouchData.Status.HOLDING)
                return;

            if (currentMovable is null)
                return;

            IMovableReceiver target = null;

            RaycastHit[] hits;
            hits = Physics.RaycastAll(touchData.ray);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit.transform.TryGetComponent(out target))
                        break;
                }
            }

            if (target == null)
                return;

            if (!target.ShouldPlace(currentMovable))
                return;

            target.OnPlace(touchData, currentMovable);

            hinge.connectedBody = null;
            currentMovable.rigidBody.isKinematic = true;
            isHolding = false;
            currentMovable = null;
        }

        public void ConnectToHinge(Movable movable)
        {
            //currentMovable = movable;
            movable.transform.parent = null;
            movable.rigidBody.isKinematic = false;
            hinge.connectedBody = movable.rigidBody;
        }
    }
}
