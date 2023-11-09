using ColorProgramming.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace ColorProgramming
{
    public class CapsuleNodeController : ConcreteNodeController<CapsuleNode>
    {
        [SerializeField]
        private Bounds hologramBounds;

        [SerializeField]
        private GameObject hologramGameObject;

        [SerializeField]
        private Image alertImage;


        private void Awake()
        {
            hologramGameObject.SetActive(false);
        }

        protected override Dictionary<string, UnityAction> ActionSignalMap =>
            new()
            {
                { "connect", ConnectAction },
                { "disconnect", DisconnectAction },
                { "open-capsule", Open }
            };

        private void Open()
        {
            GameManager.Instance.BoardController.SetScope(ConcreteNode);
        }

        void Update()
        {
            alertImage.gameObject.SetActive(ConcreteNode.InvalidMessage != "");

        }


        public void MapToHologramBounds()
        {

            if (!GameManager.Instance.BoardController.Scopes.TryGetValue(ConcreteNode, out var scope))
            {
                return;
            }
            var nodeControllers = scope.NodeControllers;
            var bounds = GameManager.Instance.BoardController.CalculateCapsuleBounds(ConcreteNode);

            var scale = new Vector3(
                hologramBounds.size.x / bounds.size.x,
                1,
                hologramBounds.size.z / bounds.size.z
            );

            foreach (var nodeController in nodeControllers)
            {
                var obj = nodeController.gameObject;
                Vector3 translation = bounds.center - obj.transform.position;
                translation = Vector3.Scale(translation, scale);

                obj.transform.position = transform.position + translation + hologramBounds.center;
                obj.transform.localScale = Vector3.one * 0.3f;
            }

            scope.ShowScope();
        }

        public override void OnAgentTouch()
        {
            hologramGameObject.SetActive(true);
            MapToHologramBounds();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var bounds = GameManager.Instance.BoardController.CalculateCapsuleBounds(ConcreteNode);
            Debug.Log(bounds);
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            Gizmos.DrawLine(bounds.center, transform.position + hologramBounds.center);


            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + hologramBounds.center, hologramBounds.size);
        }
    }

}
