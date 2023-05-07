using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorProgramming
{
    [RequireComponent(typeof(Button))]
    public class ContextButton : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textMesh;

        [SerializeField]
        private Image icon;

        [SerializeField]
        private Button button;

        public void SetupButton(RuntimeContextMenuAction action)
        {
            textMesh.text = action.ActionTitle;
            icon.sprite = action.ActionIcon;
            button.onClick.AddListener(action.Action);
        }
    }
}
