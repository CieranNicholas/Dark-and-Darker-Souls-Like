using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace LS
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new();

        [Header("Items")]
        private List<Item> items = new();

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }else
            {
                Destroy(gameObject);
            }

            foreach(var weapon in weapons)
            {
                items.Add(weapon);
            }

            // ASIGN EVERY ITEM A UNIQUE ITEM ID
            for(int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }
    
        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
    }
}