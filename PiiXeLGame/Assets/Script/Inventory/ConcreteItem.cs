using UnityEngine;

namespace Script.Inventory
{
    public class ConcreteItem : Item
    {
        public override void UseItem()
        {
            Debug.Log("overriden method");
            //TODO to actually implement
        }
    }
}
