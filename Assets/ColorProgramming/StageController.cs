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

        [SerializeField] private Transform inventoryContent;

        void Start()
        {
            LoadStage();
        }


        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void LoadStage()
        {
            var inventory = FindObjectOfType<SceneInventory>();

            LoadItems(inventory.Items);
            GameManager.Instance.BoardController.SetBoard();
        }

        private void LoadItems(ItemController[] Items)
        {
            foreach (var item in Items)
            {
                Instantiate(item.gameObject, inventoryContent);
            }
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

            CloseButton.gameObject.SetActive(!GameManager.Instance.BoardController.isGlobalScope);
        }


    }
}
