using System;
using DanielLochner.Assets.SimpleSideMenu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{

    public static Toast instance;

    [SerializeField]
    private TextMeshProUGUI textUGUI;
    [SerializeField] private Image image;

    [SerializeField] private SimpleSideMenu menuController;

    private Action onClose;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public static void Show(Image icon, string message, Action closeAction)
    {
        instance.textUGUI.text = message;
        instance.image = icon;
        instance.onClose = closeAction;

        instance.menuController.SetState(State.Open);
    }

    public static void Close()
    {
        instance.onClose.Invoke();
        instance.menuController.SetState(State.Closed);
    }


}