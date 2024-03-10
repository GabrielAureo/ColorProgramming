using ColorProgramming.Items;
using UnityEngine;

public class SceneInventory : MonoBehaviour
{
    public ItemController[] Items;

    void Awake()
    {
        Items = GetComponentsInChildren<ItemController>();
    }
}