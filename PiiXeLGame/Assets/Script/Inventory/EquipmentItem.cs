using Script.GridSystem;

namespace Script.Inventory
{
    public class EquipmentItem : Item
    {
        public float durability;
        public float defBoost;
        public float healthBoost;
        public float speedBoost;
        public float sleepBoost;
        public GameGrid grid;
        public Body bodypart;
        public enum Body //I added these parts by default, feel free to change
        {
            Hand,
            Torso,
            Pants,
            Shoes,
            Accessory
        }
    }
}
