using UnityEngine;

namespace Script.Inventory
{
    public class TestInventory : MonoBehaviour
    {
        [SerializeField] private Inventory inv;
        [SerializeField] private Item item1;
        [SerializeField] private Item item2;
        [SerializeField] private Item item3;
        [SerializeField] private Item item4;
        private KeyCode _invKey = KeyCode.I;
        private KeyCode _addKey = KeyCode.A;
        private KeyCode _removeKey = KeyCode.R;
        private bool _opened;
        private bool _added;
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
            inv.DrawInventory();
        }

        private void TestRemove()
        {
            inv.RemoveFromInventory(item1);
            inv.DrawInventory();
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
                if (_opened)
                {
                    TestHide();
                    _opened = false;
                }
                else
                {
                    TestShow();
                    _opened = true;
                }
            }
            if(Input.GetKeyDown(_addKey)) TestAdd();
            if(Input.GetKeyDown(_removeKey)) TestRemove();
        }
    }
}
