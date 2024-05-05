
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

        void Awake()
        {
            GameManager.Instance.InventoryController.OnSelectItem.AddListener(SetPreviewGameObject);
        }

        private void SetPreviewGameObject(ItemController itemController)
        {
            foreach (Transform child in PreviewTransform.transform)
            {
                Destroy(child.gameObject);
            }

            if (itemController)
            {
                var obj = Instantiate(itemController.Item.NodeGameObject, PreviewTransform);
                obj.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }


        }

        public void OnAutomaticHitTest(HitTestResult result)
        {
            PreviewTransform.position = result.Position;
        }

        public void OnInteractiveHitTest(HitTestResult result)
        {
            if (IsOverUI() || !GameManager.Instance.InventoryController.SelectedItem)
                return;
            foreach (Transform child in PreviewTransform.transform)
            {
                Destroy(child.gameObject);
            }

            GameManager.Instance.InventoryController.SelectedItem.Spawn(out GameObject spawnedObject);
            spawnedObject.transform.SetLocalPositionAndRotation(result.Position, result.Rotation);
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
