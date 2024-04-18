using UnityEngine;

namespace Script.Inventory
{
    public class TestInventory : MonoBehaviour
    {
        [SerializeField] private Inventory inv;
        [SerializeField] private Item item1;
        [SerializeField] private Item item2;
        [SerializeField] private Item item3;
        [SerializeField] private EquipmentItem item4;
        private KeyCode _invKey = KeyCode.I;
        private KeyCode _addKey = KeyCode.A;
        private KeyCode _removeKey = KeyCode.R;
        private KeyCode _equipKey = KeyCode.E;
        private KeyCode _downKey = KeyCode.DownArrow;
        private KeyCode _upKey = KeyCode.UpArrow;
        private KeyCode _leftKey = KeyCode.LeftArrow;
        private KeyCode _rightKey = KeyCode.RightArrow;
        private bool _opened;
        private bool _added;
        private bool _equipped;
        void AddToInv()
        {
            inv.AddToInventory(item1);
            inv.AddToInventory(item2);
            inv.AddToInventory(item3);
            inv.AddToInventory(item4);
        }

        private void TestAdd()
        {
            Debug.Log("test Add");
            inv.AddToInventory(item1);
            //inv.DrawInventory();
        }

        private void TestEquip()
        {
            if (!_equipped)
            {
                inv.AddOrReplaceEquipment(item4);
                _equipped = true;
                //inv.DrawInventory();
            }
            else
            {
                inv.RemoveEquipment(item4);
                _equipped = false;
                //inv.DrawInventory();
            }
        }
        private void TestRemove()
        {
            inv.RemoveFromInventory(item1);
            //inv.DrawInventory();
        }

        private void TestHide()
        {
            inv.HideInventory();
        }

        private void TestShow()
        {
            inv.DrawInventory();
        }

        private void Update()
        {
            if (!_added)
            {
                _added = true;
                AddToInv();
            }
            if (Input.GetKeyDown(_invKey))
            {
                Debug.Log(_opened);
                if (_opened)
                {
                    //Debug.Log("hiding");
                    TestHide();
                    _opened = false;
                }
                else
                {
                    //Debug.Log("displaying");
                    TestShow();
                    _opened = true;
                }
            }
            if(Input.GetKeyDown(_addKey)) TestAdd();
            if(Input.GetKeyDown(_removeKey)) TestRemove();
            if(Input.GetKeyDown(_equipKey)) TestEquip();
            if(Input.GetKeyDown(_upKey)) inv.ChangeSelection(1, 0);
            if(Input.GetKeyDown(_downKey)) inv.ChangeSelection(-1, 0);
            if(Input.GetKeyDown(_leftKey)) inv.ChangeSelection(0, -1);
            if(Input.GetKeyDown(_rightKey)) inv.ChangeSelection(0, 1);
        }
    }
}
