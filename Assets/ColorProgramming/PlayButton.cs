using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorProgramming
{
    public class PlayButton : MonoBehaviour
    {

        void Update()
        {
            gameObject.SetActive(
                GameManager.Instance.BoardController.GetCurrentBoard().IsTraversable
            );
        }
        public void OnPress()
        {
            GameManager.Instance.BoardController.WalkGraph();
        }
    }
}
