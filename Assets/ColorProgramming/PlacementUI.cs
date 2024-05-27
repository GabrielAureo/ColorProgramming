using System.Collections;
using ColorProgramming;
using TMPro;
using UnityEngine;

public class PlacementUI : MonoBehaviour
{
    public RectTransform InventoryRect; // Reference to the first UI element
    public RectTransform PlacementRect; // Reference to the second UI element
    public DevicePoseUI devicePoseUI;
    public TextMeshProUGUI textUGUI;

    public RectTransform ItemsRect;
    public RectTransform StageMessageRect;

    // Update is called once per frame

    private bool debounceActive = false; // Flag to indicate debounce state
    private float debounceTime = 1.0f; // Time in seconds to debounce



    // Coroutine for debounce
    IEnumerator DebounceCoroutine()
    {
        debounceActive = true;
        yield return new WaitForSeconds(debounceTime);
        debounceActive = false;
    }



    private void ShowInventoryUI()
    {
        var (rect, other) = GameManager.Instance.PlacementBehavior.HasPlacedStage ? (ItemsRect, StageMessageRect) : (StageMessageRect, ItemsRect);
        other.gameObject.SetActive(false);
        rect.gameObject.SetActive(true);

    }

    void Update()
    {
        if (!debounceActive)
        {
            ShowInventoryUI();
            if (devicePoseUI.isTracked)
            {
                PlacementRect.anchoredPosition = Vector3.up * -PlacementRect.rect.height;
            }
            else
            {

                PlacementRect.anchoredPosition = Vector3.zero;
                textUGUI.text = devicePoseUI.statusMessage;
            }
            // Start debounce coroutine
            StartCoroutine(DebounceCoroutine());
        }
    }


}
