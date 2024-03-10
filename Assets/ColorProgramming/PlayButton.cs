using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorProgramming
{
    public class PlayButton : MonoBehaviour
    {

        public void OnPress()
        {
            GameManager.Instance.BoardController.WalkGraph();
        }
    }
}
