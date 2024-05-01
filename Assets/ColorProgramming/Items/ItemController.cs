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

        private void OnHold()
        {
            if (itemCount == 0)
                return;

            Movable movable;
            if (Item.NodeGameObject.TryGetComponent<BaseNodeController>(out _))
            {
                var instantiatedNode = GameManager.Instance.BoardController.AddNode(
                    Item.NodeGameObject
                );
                var instantiatedController = instantiatedNode.GetComponent<BaseNodeController>();
                SetupNode(instantiatedController);
                SetupController(instantiatedController);
                movable = instantiatedNode.GetComponent<Movable>();
            }
            else
            {
                var movableObject = Instantiate(Item.NodeGameObject);
                movable = movableObject.GetComponent<Movable>();
            }


            var movableService =
                GameManager.Instance.TouchController.TouchServiceManager.GetService<MovableService>();
            movableService?.Grab(movable);
            UpdateCount(itemCount - 1);
        }

        protected abstract void SetupNode(BaseNodeController baseController);

        protected abstract void SetupController(BaseNodeController baseController);
    }
}
