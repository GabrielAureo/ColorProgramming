using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorProgramming
{
    public class ContextMenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject buttonPrefab;

        [SerializeField]
        private RectTransform buttonsContainer;

        public void SetContextMenu(ContextMenu menu)
        {
            ResetButtons();
            foreach (var action in menu.data)
            {
                var buttonObject = Instantiate(buttonPrefab, buttonsContainer);
                var contextButton = buttonObject.GetComponent<ContextButton>();
                contextButton.SetupButton(action);
            }

            transform.position = menu.worldPosition;
        }

        private void ResetButtons()
        {
            for (var i = 0; i < buttonsContainer.childCount; i++)
            {
                var child = buttonsContainer.GetChild(i);
                Destroy(child.gameObject);
            }
        }

        void Update()
        {
            //transform.LookAt(Camera.main.transform, Vector3.up);
            transform.rotation = Quaternion.LookRotation(
                transform.position - Camera.main.transform.position
            );
        }
    }
}
