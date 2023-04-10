using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo{
public class CharacterStats : MonoBehaviour
{
    public CharacterSO _data;

    public string characterName;
    public float responseSpeed;
    public int baseAttackDamage;
    public int skillAttackDamage;
    public float baseAttackCooltime;
    public int initHp;
    public int defense;
    public int speed;
    public int attackDistance;
    public CharacterClass characterClass;
    public EquipmentSO weapon;
    public EquipmentSO armor;

    public int age;
    public int xp;
    public int characterLevel;

    public int fatigue = 0;

    public Morale morale = Morale.NORMAL;

    public Rarity rarity;


    public int currHp;

    //void Start()
    public void CharacterSetup()
    {
            Debug.Log("ChracterStats starts");
            characterName = _data.characterName;
            responseSpeed = _data.responseSpeed;
            baseAttackDamage = _data.baseAttackDamage;
            skillAttackDamage = _data.skillAttackDamage;
            baseAttackCooltime = _data.baseAttackCooltime;
            initHp = _data.initHp;
            defense = _data.defense;
            speed = _data.speed;
            attackDistance = _data.attackDistance;
            characterClass = _data.characterClass;
            weapon = _data.defaultWeapon;
            armor = _data.defaultHelmet;

            // later..
            morale = _data.morale;
            fatigue = _data.fatigue;

            currHp = initHp;



            // later add to SO
            age = 150;
    }
}
}
