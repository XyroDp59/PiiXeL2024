using TMPro;
using UnityEngine;

namespace Script.Inventory
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private TMP_Text amount;
        public Sprite inventoryImage;
        public int number;
        public string itemName; //didn't name it 'name' cuz that's already a property of GameObjects
        public string description;
        public GameObject loot;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void DrawItemInIventory(Vector2 position)
        {
            transform.position = position;
            _spriteRenderer.enabled = true;
            amount.text = $"{number}";
        }

        public void HideItem()
        {
            _spriteRenderer.enabled = false;
        }

        public void UseItem()
        {
            //TODO à implémenter
        }
    }
}
