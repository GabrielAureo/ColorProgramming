﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ColorProgramming
{
    public class StageController : MonoBehaviour
    {
        [SerializeField]
        private Button PlayButton;

        [SerializeField]
        private Button CloseButton;

        //TODO implement stages data and use it to SETBOard on BoardController

        private void Start()
        {
            GameManager.Instance.BoardController.SetBoard();
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
