using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class ContextMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private RectTransform buttonsContainer;

        [SerializeField]
        private Canvas contextCanvas;

        public void Awake()
        {
            GameManager.Instance.TouchController.TouchServiceManager.RegisterService(
                new ContextMenuService(false)
            );
            EnableMenu(false);
        }

        public void SetContextMenu(ContextMenu menu)
        {
            ResetButtons();
            EnableMenu(true);
            foreach (var action in menu.data)
            {
                action.Action += () => EnableMenu(false);
                var buttonObject = Instantiate(buttonPrefab, buttonsContainer);
                var contextButton = buttonObject.GetComponent<ContextButton>();
                contextButton.SetupButton(action);
            }

            transform.position = menu.worldPosition;
        }

        public void EnableMenu(bool enabled)
        {
            contextCanvas.enabled = enabled;
        }

        private void ResetButtons()
        {
            for (var i = 0; i < buttonsContainer.childCount; i++)
            {
                var child = buttonsContainer.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        // void Update()
        // {
        //     transform.rotation = Quaternion.LookRotation(
        //         transform.position - Camera.main.transform.position
        //     );
        // }

        private void Update()
        {
            Transform transform1;
            (transform1 = transform).LookAt(2 * transform.position - Camera.main.transform.position, Vector3.up);
            var eulerAngles = transform1.eulerAngles;
            eulerAngles.x = 0;
            transform1.localEulerAngles = eulerAngles;
        }
    }
}
