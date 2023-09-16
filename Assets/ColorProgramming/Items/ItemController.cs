using UnityEngine;

namespace ColorProgramming.Items
{
    using ColorProgramming.Core;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public abstract class ItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool isPressed = false;
        private float holdTime = 0.25f;
        private float currentHold = 0f;

        private int itemCount;

        public abstract Item Item { get; set; }

        [Header("Components")]
        [SerializeField]
        private TextMeshProUGUI itemCountGUI;

        [SerializeField]
        private Image loadingRing;

        private void Start()
        {
            UpdateCount(Item.ItemQuantity);
        }

        private void Update()
        {
            if (isPressed)
            {
                currentHold += Time.deltaTime;
                if (currentHold >= holdTime)
                {
                    isPressed = false;
                    OnHold();
                }
            }
            loadingRing.fillAmount = (currentHold / holdTime);
        }

        private void UpdateCount(int newCount)
        {
            itemCount = newCount;
            itemCountGUI.text = itemCount.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            currentHold = 0f; // Reset hold time
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
            currentHold = 0f; // Reset hold time
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
    }
}
