using UnityEngine;

namespace ColorProgramming
{
    public class MovableService : ARTouchService
    {
        public Movable currentMovable;
        public bool isHolding;
        private HingeJoint hinge;
        public Transform previewer;

        public MovableService(HingeJoint hinge)
            : base(false)
        {
            this.hinge = hinge;
        }

        public override void OnTap(ARTouchData touchData) { }

        public override void OnHold(ARTouchData touchData)
        {
            Grab(touchData);
        }

        public override void OnRelease(ARTouchData touchData)
        {
            Release(touchData);
        }

        private void Grab(ARTouchData touchData)
        {
            if (touchData.selectedInteractable is not IMovableProvider)
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
            movable.transform.parent = null;
            movable.rigidBody.isKinematic = false;
            hinge.connectedBody = movable.rigidBody;
        }
    }
}
