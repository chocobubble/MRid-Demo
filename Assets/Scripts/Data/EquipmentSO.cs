using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{
    // represents inventory gear
    public enum EquipmentType
    {
        Weapon,
        Helmet,
/*
        Boots,
        Gloves,
        Shield,
        Accessories,
        All // for filtering
*/
    }
    /*
    public struct EquipData
    {
        public string equipmentName;
        public EquipmentType equipmentType;
        public Rarity rarity;
        public int points;
        public Sprite sprite;

        public EquipData(EquipmentSO SO)
        {
            equipmentName = SO.equipmentName;
            equipmentType = SO.equipmentType;
            rarity = SO.rarity;
            points = SO.points;
            sprite = SO.sprite;
        }

    }
    */

    [CreateAssetMenu(fileName = "Assets/Resources/GameData/EquipmentData/Equipment", menuName = "MRidDemo/Equipment", order = 2)]
    public class EquipmentSO : ScriptableObject
    {
        public string equipmentName;
        public EquipmentType equipmentType;
        public Rarity rarity;
        public int points;
        public Sprite sprite;
        public void OnDisable()
        {
            Debug.Log($"{this.equipmentName} has been disabled");
        }

        public void OnDestory()
        {
            Debug.Log($"{this.equipmentName} has been destoryed");
        }

/*
        public SetEquipmentSO(GameObject go)
        {
            equipmentName = go.eq
        }
    
*/
    }

}
