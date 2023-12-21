using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ColorProgramming
{
    public class CloseButton : MonoBehaviour
    {
        void Update()
        {
            gameObject.SetActive(!GameManager.Instance.BoardController.isGlobalScope);
        }

        public void OnPress()
        {
            GameManager.Instance.BoardController.SetScope();
        }
    }
}
