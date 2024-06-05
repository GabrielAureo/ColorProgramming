using UnityEngine;

namespace ColorProgramming.Items
{
    using ColorProgramming.Core;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public abstract class ItemController : MonoBehaviour, IPointerDownHandler
    {
        private int itemCount;
        public bool selected;

        private Outline outline;

        public abstract Item Item { get; set; }

        [Header("Components")]
        [SerializeField]
        private TextMeshProUGUI itemCountGUI;


        private void Start()
        {
            UpdateCount(Item.ItemQuantity);
        }


        private void UpdateCount(int newCount)
        {
            itemCount = newCount;
            itemCountGUI.text = itemCount.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            GameManager.Instance.InventoryController.SetSelectedItem(this);
        }

        public void SetSelected(bool selected)
        {
            if (selected)
            {
                if (!outline)
                    outline = gameObject.AddComponent<Outline>();

                outline.enabled = true;
                outline.effectColor = Color.green;
                outline.effectDistance = Vector2.one * 4f;
            }
            else
            {
                if (outline)
                    outline.enabled = false;
            }
        }

        public bool Spawn(out GameObject spawnedObject)
        {
            spawnedObject = null;
            if (itemCount == 0 || !Item.NodeGameObject.TryGetComponent<BaseNodeController>(out _))
                return false;


            var instantiatedNode = GameManager.Instance.BoardController.AddNode(
                Item.NodeGameObject
            );
            var instantiatedController = instantiatedNode.GetComponent<BaseNodeController>();
            SetupNode(instantiatedController);
            SetupController(instantiatedController);
            UpdateCount(itemCount - 1);

            spawnedObject = instantiatedNode.gameObject;
            GameManager.Instance.InventoryController.SetSelectedItem(this);
            return true;

        }

        public GameObject GetPreviewItem(Transform parent)
        {
            var instantiatedPreview = Instantiate(Item.NodeGameObject, parent);
            var instantiatedController = instantiatedPreview.GetComponent<BaseNodeController>();
            SetupNode(instantiatedController);
            SetupController(instantiatedController);
            return instantiatedPreview;
        }

        protected abstract void SetupNode(BaseNodeController baseController);

        protected abstract void SetupController(BaseNodeController baseController);
    }
}
