using UnityEngine;

public class UIFOVCorrection : MonoBehaviour
{
    private Camera UICamera;

    private void Start()
    {
        UICamera = GetComponent<Camera>();
    }
    private void Update()
    {
        UICamera.fieldOfView = Camera.main.fieldOfView;
    }
}