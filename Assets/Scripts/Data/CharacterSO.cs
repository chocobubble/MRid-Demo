using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{
    public enum CharacterClass
    {
        Paladin,
        Priest,
        Hunter
        //Wizard,
        //Barbarian,
        //Necromancer
    }
    public enum Rarity
    {
        Common,
        Rare,
        Special
        //All, // for filtering
    }

    public enum AttackType
    {
        Melee,
        Magic,
        Ranged
    }

    // baseline data for a specific character

    [CreateAssetMenu(fileName ="Assets/Resources/GameData/Characters/CharacterGameData", menuName = "UIToolkitDemo/Character", order = 1)]
    public class CharacterSO : ScriptableObject
    {
        public float responseSpeed = 1.0f;
        public int baseAttackDamage = 1;
        public int skillAttackDamage = 10;
        public float baseAttackCooltime = 1.0f;
        public int initHp = 100;
        public int defense = 0;
        public int speed = 10;
        public int attackDistance = 1;
        public string characterName;
        public GameObject characterVisualsPrefab;
        public Sprite visual;
        public CharacterClass characterClass;
        public Rarity rarity;
        public AttackType attackType;
        public float basePointsCriticalHit;
        public int age = 0;
        public int xp = 0;
        public int characterLevel = 1;

        // skill1 unlocked at level 0, skill2 unlocked at level 3, skill3 unlocked at level 6
        public SkillSO skill1;
        //public SkillSO skill2;
        //public SkillSO skill3;

        // starting equipment (weapon, shield/armor, helmet, boots, gloves)
        public EquipmentSO defaultWeapon;
        //public EquipmentSO defaultShieldAndArmor;
        public EquipmentSO defaultHelmet;
        //public EquipmentSO defaultBoots;
        //public EquipmentSO defaultGloves;

        void OnValidate()
        {
            if (defaultWeapon != null && defaultWeapon.equipmentType != EquipmentType.Weapon) 
                defaultWeapon = null;

//            if (defaultShieldAndArmor != null && defaultShieldAndArmor.equipmentType != EquipmentType.Shield)
//                defaultShieldAndArmor = null;

            if (defaultHelmet != null && defaultHelmet.equipmentType != EquipmentType.Helmet)
                defaultHelmet = null;
/*
            if (defaultGloves != null && defaultGloves.equipmentType != EquipmentType.Gloves)
                defaultGloves = null;

            if (defaultBoots != null && defaultBoots.equipmentType != EquipmentType.Boots)
                defaultBoots = null;
*/
        }

        public CharacterSO(CharacterStats data)
        {
            responseSpeed = data.responseSpeed;
            baseAttackDamage = data.baseAttackDamage;
            skillAttackDamage =  data.skillAttackDamage;
            baseAttackCooltime = data.baseAttackCooltime;
            initHp = data.initHp;
            defense = data.defense;
            speed = data.speed;attackDistance = data.attackDistance;
            characterName = data.name;
            // characterVisualsPrefab = data.characterVisualsPrefab;
            // visual = data.visual;
            // characterClass;
            // public Rarity rarity;
            // public AttackType attackType;
            // public float basePointsCriticalHit;
         age = data.age;
        xp = data.xp;
        characterLevel = data.characterLevel;

        // skill1 unlocked at level 0, skill2 unlocked at level 3, skill3 unlocked at level 6
        //public SkillSO skill1;
        //public SkillSO skill2;
        //public SkillSO skill3;

        // starting equipment (weapon, shield/armor, helmet, boots, gloves)
         defaultWeapon = data.weapon;
        //public EquipmentSO defaultShieldAndArmor;
         defaultHelmet = data.armor;
        //public EquipmentSO defaultBoots;
        //public EquipmentSO defaultGloves;
            
        }
        public void SetCharacterSO(CharacterStats data)
        {
            responseSpeed = data.responseSpeed;
            baseAttackDamage = data.baseAttackDamage;
            skillAttackDamage =  data.skillAttackDamage;
            baseAttackCooltime = data.baseAttackCooltime;
            initHp = data.initHp;
            defense = data.defense;
            speed = data.speed;
            attackDistance = data.attackDistance;
            characterName = data.name;
            characterClass = data.characterClass;
            // characterVisualsPrefab = data.characterVisualsPrefab;
            // visual = data.visual;
            // characterClass;
            // public Rarity rarity;
            // public AttackType attackType;
            // public float basePointsCriticalHit;
            age = data.age;
            xp = data.xp;
            characterLevel = data.characterLevel;
            rarity = data.rarity;
        // skill1 unlocked at level 0, skill2 unlocked at level 3, skill3 unlocked at level 6
        //public SkillSO skill1;
        //public SkillSO skill2;
        //public SkillSO skill3;

        // starting equipment (weapon, shield/armor, helmet, boots, gloves)
         defaultWeapon = data.weapon;
        //public EquipmentSO defaultShieldAndArmor;
         defaultHelmet = data.armor;
        //public EquipmentSO defaultBoots;
        //public EquipmentSO defaultGloves;
            
        }
    }
}