﻿using UnityEngine;

namespace ColorProgramming
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static GameManager Instance
        {
            get { return instance; }
        }

        public ARTouchController TouchController;
        public MovableController MovableController;
        public ContextMenuController ContextMenuController;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
