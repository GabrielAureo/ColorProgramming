using UnityEngine;

namespace ColorProgramming
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class ItemController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private bool isPressed = false;
        private float holdTime = 0.5f; // Adjust as needed

        [SerializeField] private GameObject movablePrefab;
        [SerializeField] private int itemQuantity;
        public int itemCount { get; private set; }

        [Header("Components")]
        [SerializeField] private TextMeshProUGUI itemCountGUI;

        private void Awake()
        {
            UpdateCount(itemQuantity);
        }
        private void Update()
        {
            if (isPressed)
            {
                holdTime -= Time.deltaTime;
                if (holdTime <= 0)
                {
                    OnHold();
                    isPressed = false;
                }
            }
        }

        private void UpdateCount(int newCount)
        {
            itemCount = newCount;
            itemCountGUI.text = itemCount.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            holdTime = 0.5f; // Reset hold time
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
            holdTime = 0.5f; // Reset hold time
        }

        private void OnHold()
        {
            if (itemCount == 0) return;
            var movableObject = Instantiate(movablePrefab);
            var movable = movableObject.GetComponent<Movable>();
            GameManager.Instance.MovableController.Grab(movable);
            UpdateCount(itemCount- 1);


        }
    }
}
