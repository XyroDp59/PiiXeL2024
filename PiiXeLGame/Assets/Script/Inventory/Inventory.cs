using System.Collections;
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
        [SerializeField] private float selectionDelay;
        [SerializeField] private int colCount;
        [SerializeField] private GameObject selector;
        [SerializeField] private TMP_Text itemDescriptionText;
        public int bagSize;
        public Vector2 itemSize;
        private Vector2 _itemPos;
        private Item _selectedItem;
        private List<Item> _bag;
        private List<EquipmentItem> _equipment;
        private SpriteRenderer _selectorRenderer;
        private SpriteRenderer _spriteRenderer;
        private Transform _selectTransform;
        private int _selectedPosInBag;
        private bool _canChangeSelection = true;
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

        public void AddToInventory(Item newItem)
        {
            if (!_selectedItem)
            {
                _selectedItem = newItem;
                _selectedPosInBag = _bag.Count;
            }

            newItem.GetComponent<SpriteRenderer>().enabled = false;
            newItem.number += 1;
            if (!_bag.Contains(newItem)) _bag.Add(newItem);
            
            if(_isDisplayed) DrawInventory();
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
                        _selectedItem = _bag[0];
                        _selectedPosInBag = 0;
                        ChangeSelection(0,0);
                    }
                }
            }
            if(_isDisplayed) DrawInventory();
        }

        public void RemoveEquipment(EquipmentItem equippedItem)
        {
            if (_equipment.Contains(equippedItem)) _equipment.Remove(equippedItem);
            AddToInventory(equippedItem);
            if (_isDisplayed)
            {
                DrawInventory();
                ChangeSelection(0,0);
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
            if(_isDisplayed) DrawInventory();
            //Debug.Log(_equipment.Count);
        }

        public void DrawInventory(int firstDrawnItem = 0)
        {
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
                _selectorRenderer.enabled = true;
                itemDescriptionText.text = _selectedItem.description;
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
            if (_selectedItem)
            {
                _selectedItem.UseItem();
            }
        }

        public void ChangeSelection(int verticalAmount, int horizontalAmount)
        {
            if (!_canChangeSelection) return;

            int equipmentAmount = _equipment.Count;
            int itemAmount = _bag.Count;
            int lastRawAmount = itemAmount % colCount;

            verticalAmount = _selectedIsEquipment ? equipmentAmount : _selectedPosInBag / colCount + verticalAmount;
            verticalAmount %= (itemAmount / colCount) + (itemAmount % colCount - 1 >= _selectedPosInBag % colCount ? 1 : 0);
            if (verticalAmount < 0) verticalAmount += itemAmount / colCount + 1;
            
            int itemsInLine = (verticalAmount == itemAmount / colCount ? lastRawAmount : colCount) + 1;
            if (equipmentAmount == 0) itemsInLine -= 1;
            //Debug.Log(verticalAmount);
            //Debug.Log(itemsInLine);

            int curHorPos = _selectedIsEquipment ? colCount : _selectedPosInBag % colCount;
            //Debug.Log(curHorPos);
            horizontalAmount = (horizontalAmount % itemsInLine + curHorPos) % itemsInLine;
            if (horizontalAmount < 0) horizontalAmount += itemsInLine;
            _selectedIsEquipment = (horizontalAmount == itemsInLine - 1 && equipmentAmount != 0);
            //Debug.Log(horizontalAmount);
            //Debug.Log(equipmentAmount);
            
            if(_selectedIsEquipment)
            {
                _selectedItem = _equipment[verticalAmount];
            }
            
            else
            {
                _selectedPosInBag = verticalAmount * colCount + horizontalAmount;

                if (_selectedPosInBag > itemAmount) _selectedPosInBag = itemAmount;
                _selectedItem = _bag[_selectedPosInBag];
            }

            itemDescriptionText.text = _selectedItem.description;
            if(_isDisplayed) DrawInventory();
            StartCoroutine(SelectionSwitchDelay());
        }

        private IEnumerator SelectionSwitchDelay()
        {
            _canChangeSelection = false;
            yield return new WaitForSeconds(selectionDelay);
            _canChangeSelection = true;
        }
    }
}