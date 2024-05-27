using System.Collections;
using System.Text.RegularExpressions;
using ColorProgramming.Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ColorProgramming
{
    public class StageController : MonoBehaviour
    {
        [SerializeField]
        private GameObject VictoryScreen;
        [SerializeField]
        private GameObject FailureScreen;

        [SerializeField]
        private Button PlayButton;

        [SerializeField]
        private Button CloseButton;

        [SerializeField]
        private RectTransform BuildLoopUI;

        void Start()
        {
            LoadStage();
        }


        public void LoadMainMenu()
        {
            VictoryScreen.SetActive(false);
            FailureScreen.SetActive(false);

            SceneManager.LoadScene("MainMenu");
        }

        public void LoadStage()
        {
            var inventory = FindObjectOfType<SceneInventory>();
            var agent = FindObjectOfType<AgentController>();
            var target = FindObjectOfType<TargetNodeController>();


            LoadItems(inventory.Items);
            GameManager.Instance.BoardController.SetBoard(agent, target);
            GameManager.Instance.PlacementBehavior.SetPreviewStage(agent, target);
        }

        private void LoadItems(ItemController[] Items)
        {

            GameManager.Instance.InventoryController.SetupInventory(Items);

        }

        public void CompleteStage()
        {
            VictoryScreen.SetActive(true);
        }

        public void FailStage()
        {
            FailureScreen.SetActive(true);
        }

        public void ResetStage()
        {
            VictoryScreen.SetActive(false);
            FailureScreen.SetActive(false);

            StageLoader.ResetStage();
        }


        private void Update()
        {
            PlayButton.gameObject.SetActive(
                GameManager.Instance.BoardController.GetCurrentBoard().IsTraversable
            );

            // CloseButton.gameObject.SetActive(!GameManager.Instance.BoardController.isGlobalScope);
            BuildLoopUI.gameObject.SetActive(!GameManager.Instance.BoardController.isGlobalScope);
        }


    }
}
