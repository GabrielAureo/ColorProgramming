using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlacementUI : MonoBehaviour
{
    public RectTransform InventoryRect; // Reference to the first UI element
    public RectTransform PlacementRect; // Reference to the second UI element

    public bool isTracked;


    // Update is called once per frame
    void Update()
    {
        // Check for variable change condition
        // For demonstration, I'll use a random variable change
        if (isTracked)
        {
            InventoryRect.position = Vector3.up;
            PlacementRect.position = Vector3.zero;
        }
        else
        {
            InventoryRect.position = Vector3.zero;
            PlacementRect.position = Vector3.up;
        }
    }


}
