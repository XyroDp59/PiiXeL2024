using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Script.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private float originX;
        [SerializeField] private float originY;
        [SerializeField] private float originEquipmentX;
        [SerializeField] private float originEquipmentY;
        [SerializeField] private float infoDisplayX;
        [SerializeField] private float infoDisplayY;
        [SerializeField] private GameObject selector;
        [SerializeField] private TMP_Text itemDescriptionText;
        public int bagSize;
        public int colCount;
        public Vector2 itemSize;
        private Vector2 _itemPos;
        private Item _selectedItem;
        private List<Item> _bag;
        private List<EquipmentItem> _equipment;
        private SpriteRenderer _selectorRenderer;
        private SpriteRenderer _spriteRenderer;
        private Transform _selectTransform;
        private int _selectedPosInBag;
        private int _selectedPosInEquipment;
        private int _firstDrawnLine;
        private bool _selectedIsEquipment;
        private bool _isDisplayed;

        private void Start()
        {
            _bag = new List<Item>();
            _equipment = new List<EquipmentItem>();
            _itemPos = new Vector2();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.enabled = false;
            _selectTransform = selector.GetComponent<Transform>();
            _selectorRenderer = selector.GetComponent<SpriteRenderer>();
        }

        public void AddToInventory(Item newItem, int amount = 1)
        {
            if (!_selectedItem)
            {
                _selectedItem = newItem;
                _selectedPosInBag = _bag.Count;
            }

            newItem.GetComponent<SpriteRenderer>().enabled = false;
            newItem.number += amount;
            if (!_bag.Contains(newItem))
            {
                _bag.Add(newItem);
                newItem.number = amount;
            }

            if (_isDisplayed) DrawInventory(_firstDrawnLine);
        }

        public void RemoveFromInventory(Item discardedItem)
        {
            if (_bag.Contains(discardedItem))
            {
                discardedItem.number -= 1;
                if (discardedItem.number == 0)
                {
                    discardedItem.HideItem();
                    _bag.Remove(discardedItem);
                    if (discardedItem == _selectedItem)
                    {
                        if (_bag.Count != 0)
                        {
                            if(_selectedPosInBag == _bag.Count) _selectedPosInBag -= 1;
                            _selectedItem = _bag[_selectedPosInBag];
                        }
                        ChangeSelection(0, 0);
                    }
                }

                if (_bag.Count == 0 && _equipment.Count == 0) _selectorRenderer.enabled = false;
            }

            if (_isDisplayed) DrawInventory(_firstDrawnLine);
        }

        public void RemoveEquipment(EquipmentItem equippedItem)
        {
            if (_equipment.Contains(equippedItem)) _equipment.Remove(equippedItem);
            AddToInventory(equippedItem);
            if (_isDisplayed)
            {
                DrawInventory(_firstDrawnLine);
                ChangeSelection(0, 0);
            }
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

            if (_isDisplayed) DrawInventory(_firstDrawnLine);
        }

        public void DrawInventory(int firstDrawnLine = 0)
        {
            _firstDrawnLine = firstDrawnLine;
            _isDisplayed = true;
            _spriteRenderer.enabled = true;
            int iter = 0;
            _itemPos.x = originEquipmentX;
            _itemPos.y = originEquipmentY;
            foreach (EquipmentItem possessedEquipment in _equipment)
            {
                possessedEquipment.DrawItemInIventory(_itemPos);
                _itemPos.y += itemSize.y;
            }

            _itemPos.x = originX;
            _itemPos.y = originY;
            foreach (Item possessedItem in _bag)
            {
                if (iter / colCount < firstDrawnLine)
                {
                    iter += 1;
                    continue;
                }

                if (iter % colCount == 0)
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

            itemDescriptionText.enabled = true;
            if (_selectedItem)
            {
                _selectTransform.position = _selectedItem.transform.position;
                if (!(_bag.Count == 0 && _equipment.Count == 0))
                {
                    _selectorRenderer.enabled = true;
                    itemDescriptionText.text = _selectedItem.description;
                }
            }
        }

        public void HideInventory()
        {
            _selectorRenderer.enabled = false;
            _isDisplayed = false;
            _spriteRenderer.enabled = false;
            foreach (Item possessedItem in _bag)
            {
                possessedItem.HideItem();
            }

            foreach (EquipmentItem possessedEquipment in _equipment)
            {
                possessedEquipment.HideItem();
            }

            itemDescriptionText.enabled = false;
        }

        public void UseSelectedItem()
        {
            if (_selectedItem is EquipmentItem)
            {
                if(_selectedIsEquipment) RemoveEquipment((EquipmentItem)_selectedItem);
                else AddOrReplaceEquipment((EquipmentItem)_selectedItem);
            }
            
            if (_selectedItem)
            {
                _selectedItem.UseItem();
            }
        }

        public void ChangeSelection(int verticalAmount, int horizontalAmount)
        {
            int equipmentAmount = _equipment.Count;
            int itemAmount = _bag.Count;
            int lastRawAmount = itemAmount % colCount;
            int nonModifiedVerticalPos = verticalAmount;

            if (equipmentAmount == 0 && itemAmount == 0)
            {
                _selectorRenderer.enabled = false;
                return;
            }

            _selectorRenderer.enabled = true;

            if (itemAmount != 0)
            {
                verticalAmount = _selectedPosInBag / colCount + verticalAmount;
                verticalAmount %=
                    (itemAmount / colCount) + (itemAmount % colCount - 1 >= _selectedPosInBag % colCount ? 1 : 0);

                if (verticalAmount < 0)
                    verticalAmount += _selectedPosInBag % colCount > lastRawAmount - 1
                        ? itemAmount / colCount
                        : itemAmount / colCount + 1;
            }

            int itemsInLine = (verticalAmount == itemAmount / colCount ? lastRawAmount : colCount) + 1;
            if (equipmentAmount == 0) itemsInLine -= 1;

            int curHorPos = _selectedIsEquipment ? colCount : _selectedPosInBag % colCount;
            horizontalAmount = (horizontalAmount % itemsInLine + curHorPos) % itemsInLine;
            if (horizontalAmount < 0) horizontalAmount += itemsInLine;
            _selectedIsEquipment = (horizontalAmount == itemsInLine - 1 && equipmentAmount != 0) || itemAmount == 0;

            if (_selectedIsEquipment)
            {
                _selectedPosInEquipment = (nonModifiedVerticalPos + _selectedPosInEquipment) % equipmentAmount;
                if (_selectedPosInEquipment < 0) _selectedPosInEquipment += equipmentAmount;
                _selectedItem = _equipment[_selectedPosInEquipment];
                _selectedPosInBag = _selectedPosInEquipment * colCount;
                if (_selectedPosInBag >= itemAmount) _selectedPosInBag = itemAmount - 1;
            }

            else
            {
                _selectedPosInBag = verticalAmount * colCount + horizontalAmount;
                if (_selectedPosInBag > itemAmount) _selectedPosInBag = itemAmount;
                _selectedItem = _bag[_selectedPosInBag];
                _selectedPosInEquipment = verticalAmount >= equipmentAmount ? equipmentAmount - 1 : verticalAmount;
            }
            
            itemDescriptionText.text = _selectedItem.description;
            if (_isDisplayed) DrawInventory(_firstDrawnLine);
        }
    }
}