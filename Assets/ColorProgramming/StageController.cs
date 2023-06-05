using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ColorProgramming
{
    public class StageController : MonoBehaviour
    {
        [SerializeField]
        private Button PlayButton;

        //TODO implement stages data and use it to SETBOard on BoardController

        private void Awake()
        {
            GameManager.Instance.BoardController.SetBoard();
        }

        private void Update()
        {
            var isTraversable = GameManager.Instance.BoardController
                .GetCurrentBoard()
                .IsTraversable;

            Debug.Log(isTraversable);
            PlayButton.gameObject.SetActive(
                GameManager.Instance.BoardController.GetCurrentBoard().IsTraversable
            );
        }
    }
}
