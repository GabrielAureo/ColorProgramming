using ColorProgramming.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ColorProgramming
{
    public class InventoryController : MonoBehaviour
    {
        private readonly List<Item> Items = new();
        private readonly List<Item> TemporaryItems = new();

        [SerializeField]
        private Transform inventoryContent;

        [SerializeField]
        private GameObject itemPrefab;

        public void AddTemporaryItems(List<Item> newItems)
        {
            TemporaryItems.AddRange(newItems);
            RefreshInventory();
        }

        public void ClearTemporaryItems()
        {
            TemporaryItems.Clear();
            RefreshInventory();
        }

        private void RefreshInventory()
        {
            foreach (Transform child in inventoryContent)
            {
                Destroy(child.gameObject);
            }

            SpawnControllers();
        }

        private void SpawnControllers()
        {
            var unionItems = Items.Concat(TemporaryItems);
            foreach (var item in unionItems)
            {
                var itemObj = Instantiate(itemPrefab, inventoryContent);
                var itemController = itemObj.GetComponent<ItemController>();

                itemController.Item = item;
            }
        }
    }
}
