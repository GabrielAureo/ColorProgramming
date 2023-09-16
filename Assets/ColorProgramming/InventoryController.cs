using ColorProgramming.Items;
using UnityEngine;

namespace ColorProgramming
{
    public class InventoryController : MonoBehaviour
    {
        private ItemController[] items;
        private ItemController[] temporaryItems;

        [SerializeField]
        private Transform inventoryContent;

        [SerializeField]
        private GameObject itemPrefab;

        private void Awake()
        {
            items = inventoryContent.GetComponentsInChildren<ItemController>();
        }

        public void SetTemporaryItems(ItemController[] newItems)
        {
            temporaryItems = newItems;
            RefreshInventory();
        }

        public void ClearTemporaryItems()
        {
            temporaryItems = new ItemController[] {};
            RefreshInventory();
        }

        private void RefreshInventory()
        {
            foreach (Transform child in inventoryContent)
            {
                if(child.CompareTag("Temporary"))
                {
                    Destroy(child.gameObject);
                }
            }

            SpawnControllers();
        }

        private void SpawnControllers()
        {
            foreach (var item in temporaryItems)
            {
                var obj = Instantiate(item.gameObject, inventoryContent);
                obj.tag = "Temporary";
            }
        }
    }
}
