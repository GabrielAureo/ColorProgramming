using UnityEngine;

namespace ColorProgramming
{
    using ColorProgramming.Core;
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    public class ItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool isPressed = false;
        private float holdTime = 0.25f;
        private float currentHold = 0f;

        [SerializeField]
        private GameObject movablePrefab;

        [SerializeField]
        private int itemQuantity;
        public int ItemCount { get; private set; }

        [Header("Components")]
        [SerializeField]
        private TextMeshProUGUI itemCountGUI;

        [SerializeField]
        private Image loadingRing;

        private void Awake()
        {
            UpdateCount(itemQuantity);
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
            ItemCount = newCount;
            itemCountGUI.text = ItemCount.ToString();
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
            if (ItemCount == 0)
                return;

            Movable movable;
            if (movablePrefab.TryGetComponent<NodeController>(out _))
            {
                var instantiatedNode = GameManager.Instance.BoardController.AddNode(movablePrefab);
                movable = instantiatedNode.GetComponent<Movable>();
            }
            else
            {
                var movableObject = Instantiate(movablePrefab);
                movable = movableObject.GetComponent<Movable>();
            }

            var movableService =
                GameManager.Instance.TouchController.TouchServiceManager.GetService<MovableService>();
            movableService?.Grab(movable);
            UpdateCount(ItemCount - 1);
        }
    }
}
