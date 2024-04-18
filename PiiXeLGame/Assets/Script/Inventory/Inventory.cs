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
        [SerializeField] private float originEquipment;
        [SerializeField] private int colCount;
        [SerializeField] private float infoDisplayX;
        [SerializeField] private float infoDisplayY;
        [SerializeField] private GameObject selector;
        [SerializeField] private TMP_Text itemDescriptionText;
        private Transform _selectTransform;
        public int bagSize;
        public Vector2 itemSize;
        private List<Item> _bag;
        private List<EquipmentItem> _equipment;
        private Vector2 _itemPos;
        private SpriteRenderer _spriteRenderer;
        private Item _selectedItem;
        private bool _isDisplayed;
        private SpriteRenderer _selectorRenderer;
        private int _selectedPosInBag;
        private bool _canChangeSelection = true;

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
        }

        public void DrawInventory()
        {
            _isDisplayed = true;
            _spriteRenderer.enabled = true;
            int iter = 0;
            _itemPos.x = originX;
            _itemPos.y = originEquipment;
            foreach (EquipmentItem possessedEquipment in _equipment)
            {
                possessedEquipment.DrawItemInIventory(_itemPos);
                _itemPos.y += itemSize.y;
            }

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

            if (_selectedItem)
            {
                _selectTransform.position = _selectedItem.transform.position;
                _selectorRenderer.enabled = true;
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
            
            int itemAmount = _bag.Count;
            int lastRawAmount = itemAmount % colCount;
            int lineAmount;

            verticalAmount = _selectedPosInBag / colCount + verticalAmount;
            verticalAmount %= (itemAmount / colCount) + (itemAmount % colCount - 1 >= _selectedPosInBag % colCount ? 1 : 0);
            if (verticalAmount < 0) verticalAmount += itemAmount / colCount + 1;
            
            lineAmount = verticalAmount == itemAmount / colCount ? lastRawAmount : colCount;
            Debug.Log(verticalAmount);
            Debug.Log(lineAmount);
            
            horizontalAmount = (horizontalAmount % lineAmount + _selectedPosInBag % lineAmount) % lineAmount;
            if (horizontalAmount < 0) horizontalAmount += lineAmount;
            //Debug.Log(horizontalAmount);
            
            _selectedPosInBag = verticalAmount * colCount + horizontalAmount;
            
            if (_selectedPosInBag > itemAmount) _selectedPosInBag = itemAmount;
            _selectedItem = _bag[_selectedPosInBag];
            
            itemDescriptionText.text = _selectedItem.description;
            if(_isDisplayed) DrawInventory();
            StartCoroutine(SelectionSwitchDelay());
        }

        private IEnumerator SelectionSwitchDelay()
        {
            _canChangeSelection = false;
            yield return new WaitForSeconds(0.1f);
            _canChangeSelection = true;
        }
    }
}