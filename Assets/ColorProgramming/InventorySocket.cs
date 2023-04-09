using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.ColorProgramming
{
    public class InventorySocket : BaseSocket
    {
        [SerializeField] private Movable movablePrefab;
        public override Movable GetMovable(ARTouchData touchData)
        {
            return movablePrefab;
        }

        protected override void OnPlace(ARTouchData touchData, Movable movable)
        {

        }

        protected override bool ShouldPlace(Movable movable)
        {
            return false;
        }

        protected override bool TakeOperation()
        {
            throw new System.NotImplementedException();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}