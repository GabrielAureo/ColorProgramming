
using System.Collections.Generic;
using ColorProgramming.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using Vuforia;

namespace ColorProgramming
{

    public class PlacementBehaviour : MonoBehaviour
    {
        [SerializeField]
        private Transform PreviewTransform;

        public bool hasPlacedPlayer;
        public bool hasPlacedTarget;

        private AgentController player;
        private TargetNodeController target;


        public bool HasPlacedStage
        {
            get
            {
                return hasPlacedPlayer && hasPlacedTarget;
            }
        }



        public void SetPreviewStage(AgentController player, TargetNodeController target)
        {
            // foreach (Transform child in PreviewTransform.transform)
            // {
            //     Destroy(child.gameObject);
            // }
            hasPlacedPlayer = false;
            hasPlacedTarget = false;

            this.player = player;
            this.target = target;

            player.transform.SetParent(PreviewTransform);
            player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        }


        private void SetPreviewItem(ItemController itemController)
        {
            foreach (Transform child in PreviewTransform.transform)
            {
                Destroy(child.gameObject);
            }

            if (itemController)
            {
                var obj = itemController.GetPreviewItem(PreviewTransform);
                obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

        }

        public void OnAutomaticHitTest(HitTestResult result)
        {
            if (HasPlacedStage && (GameManager.Instance.InventoryController.SelectedItem == null || GameManager.Instance.InventoryController.SelectedItem.Item.ItemQuantity == 0))
            {
                foreach (Transform child in PreviewTransform.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            PreviewTransform.position = result.Position;
        }

        public void OnInteractiveHitTest(HitTestResult result)
        {
            if (IsOverUI())
            {
                return;
            }
            if (hasPlacedPlayer && hasPlacedTarget)
            {
                ItemHitTest(result);
            }
            else
            {
                StageHitTest(result);
            }
        }

        private void ItemHitTest(HitTestResult result)
        {
            if (!GameManager.Instance.InventoryController.SelectedItem)
                return;
            foreach (Transform child in PreviewTransform.transform)
            {
                Destroy(child.gameObject);
            }


            GameManager.Instance.InventoryController.SelectedItem.Spawn(out GameObject spawnedObject);
            spawnedObject.transform.SetLocalPositionAndRotation(result.Position, result.Rotation);
        }

        private void StageHitTest(HitTestResult result)
        {

            if (!hasPlacedPlayer)
            {
                hasPlacedPlayer = true;
                player.transform.SetParent(null);
                player.transform.SetPositionAndRotation(result.Position, result.Rotation);


                target.transform.SetParent(PreviewTransform);
                target.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                return;

            }

            if (!hasPlacedTarget)
            {
                hasPlacedTarget = true;
                target.transform.SetParent(null);
                target.transform.SetPositionAndRotation(result.Position, result.Rotation);


                GameManager.Instance.InventoryController.OnSelectItem.AddListener(SetPreviewItem);
            }

        }

        private bool IsOverUI()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
    var position = Input.GetTouch(0).position;
#else
            var position = Input.mousePosition;
#endif
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = position
            };
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}
