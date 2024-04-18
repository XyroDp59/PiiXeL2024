using TMPro;
using UnityEngine;

namespace Script.Inventory
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private TMP_Text amount;
        private RectTransform _rectTransform;
        public Sprite inventoryImage;
        public int number;
        public string itemName; //didn't name it 'name' cuz that's already a property of GameObjects
        public string description;
        public GameObject loot;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rectTransform = amount.GetComponent<RectTransform>();    
        }

        public void DrawItemInIventory(Vector2 position)
        {
            transform.position = position;
            _rectTransform.position = position;
            _spriteRenderer.enabled = true;
            if (this.GetType() != typeof(EquipmentItem))
            {
                amount.enabled = true;
                amount.text = $"{number}";
            }
            else
            {
                amount.enabled = false;
            }
        }

        public void HideItem()
        {
            _spriteRenderer.enabled = false;
            amount.enabled = false;
        }

        public void UseItem()
        {
            Debug.Log("Not overriden method");
            return;
        }
    }
}
