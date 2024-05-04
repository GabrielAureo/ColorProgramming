using System.Collections.Generic;
using System.Linq;
using ColorProgramming.Items;
using UnityEngine;
using UnityEngine.Events;

namespace ColorProgramming
{
    public class InventoryController : MonoBehaviour
    {
        private ItemController[] items;
        private ItemController[] temporaryItems;

        public ItemController SelectedItem;

        [SerializeField]
        private Transform inventoryContent;

        [SerializeField]
        private GameObject itemPrefab;

        public ItemSelectEvent OnSelectItem = new ItemSelectEvent();

        public void SetSelectedItem(ItemController selectedItem)
        {
            if (selectedItem == SelectedItem)
            {
                selectedItem.SetSelected(false);
                SelectedItem = null;
            }
            else
            {

                SelectedItem = selectedItem;
                foreach (var item in items)
                {
                    item.SetSelected(false);
                }
                SelectedItem.SetSelected(true);
            }

            OnSelectItem.Invoke(selectedItem);
        }

        public void SetupInventory(ItemController[] items)
        {
            var newItems = new List<ItemController>();
            foreach (Transform oldItem in inventoryContent)
            {
                Destroy(oldItem.gameObject);
            }
            foreach (var item in items)
            {
                var itemObj = Instantiate(item.gameObject, inventoryContent);
                var itemController = itemObj.GetComponent<ItemController>();
                newItems.Add(itemController);
            }

            this.items = newItems.ToArray();
        }



        public void SetTemporaryItems(ItemController[] newItems)
        {
            temporaryItems = newItems;
            RefreshInventory();
        }

        public void ClearTemporaryItems()
        {
            temporaryItems = new ItemController[] { };
            RefreshInventory();
        }

        public void RefreshInventory()
        {
            foreach (Transform child in inventoryContent)
            {
                if (child.CompareTag("Temporary"))
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
