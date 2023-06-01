using UnityEngine;
using UnityEngine.UI;

namespace ChittaExorcist.InventorySettings
{
    [CreateAssetMenu(fileName = "NewInventoryItemData", menuName = "Custom Data/Inventory/Item Data")]
    public class InventoryItemSO : ScriptableObject
    {
        public Sprite itemImage;
        public string itemName;
        [TextArea]
        public string itemDescription;
    }
}