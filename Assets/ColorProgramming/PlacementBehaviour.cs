
using ColorProgramming.Items;
using UnityEngine;
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
    }
}
