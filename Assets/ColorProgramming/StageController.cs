using System.Collections;
using UnityEngine;
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

        private void Start()
        {
            GameManager.Instance.BoardController.SetBoard();
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
