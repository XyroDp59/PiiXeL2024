using System.Collections.Generic;
using UnityEngine;

namespace Script.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private float originX;
        [SerializeField] private float originY;
        [SerializeField] private int rawCount;
        public int bagSize;
        public Vector2 itemSize;
        private List<Item> _bag;
        private List<EquipmentItem> _equipment;
        private Vector2 _itemPos;
        private SpriteRenderer _spriteRenderer;

        private void Start()
        {
            _bag = new List<Item>();
            _equipment = new List<EquipmentItem>();
            _itemPos = new Vector2();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
        }

        public void AddToInventory(Item newItem)
        {
            newItem.GetComponent<SpriteRenderer>().enabled = false;
            newItem.number += 1;
            if (!_bag.Contains(newItem)) _bag.Add(newItem);
        }

        public void RemoveFromInventory(Item discardedItem)
        {
            if (_bag.Contains(discardedItem))
            {
                discardedItem.number -= 1;
                if(discardedItem.number == 0)
                {
                    discardedItem.HideItem();
                    _bag.Remove(discardedItem);
                }
            }
        }

        public void RemoveEquipment(EquipmentItem equippedItem)
        {
            if (_equipment.Contains(equippedItem)) _equipment.Remove(equippedItem);
            AddToInventory(equippedItem);
        }

        public void AddOrReplaceEquipment(EquipmentItem equippedItem)
        {
            if (_bag.Contains(equippedItem))
            {
                RemoveFromInventory(equippedItem);
                foreach (EquipmentItem equipment in _equipment)
                {
                    if (equipment.bodypart == equippedItem.bodypart)
                    {
                        RemoveEquipment(equipment);
                        break;
                    }
                }
                _equipment.Add(equippedItem);
            }
        }

        public void DrawInventory()
        {
            _spriteRenderer.enabled = true;
            int iter = 0;
            _itemPos.x = originX;
            _itemPos.y = originY;
            foreach (EquipmentItem possessedEquipment in _equipment)
            {
                possessedEquipment.DrawItemInIventory(_itemPos);
                _itemPos.y += itemSize.y;
            }
            
            foreach (Item possessedItem in _bag)
            {
                if (iter % rawCount == 0)
                {
                    _itemPos.x = originX;
                    _itemPos.y += itemSize.y;
                }
                else
                {
                    _itemPos.x += itemSize.x;
                }
                possessedItem.DrawItemInIventory(_itemPos);
                iter += 1;
            }
        }

        public void HideInventory()
        {
            _spriteRenderer.enabled = false;
            foreach (Item possessedItem in _bag)
            {
                possessedItem.HideItem();
            }

            foreach (EquipmentItem possessedEquipment in _equipment)
            {
                possessedEquipment.HideItem();
            }
        }
    }
}
