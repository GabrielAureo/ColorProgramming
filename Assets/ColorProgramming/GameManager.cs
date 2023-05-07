using UnityEngine;

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
        public MovableService MovableController;
        public ContextMenuController ContextMenuController;

        private void OnEnable()
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
