using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace MRidDemo
{
public class InventoryScreen : MenuScreen
{
    List<InventorySlot> InventoryItems = new List<InventorySlot>();
    List<CharacterSO> characterList;
    List<EquipmentSO> equipmentList;

    public CharacterSO curChracterData;
/*
    VisualElement m_SlotContainer;
    VisualElement m_RosterContainer;
    VisualElement m_Weapon;
    VisualElement m_Armor;
*/

    VisualElement characterSprite;
    VisualElement rosterContainer;
    VisualElement equipmentContainer;
    VisualElement weaponSprite;
    VisualElement armorSprite;

    public List<Button> equipmentButtons = new List<Button>();

    Button weaponButton;
    Button armorButton;

    Label infoLeftLabel;
    Label infoMiddleLabel;
    Label statsLabel;

    Label characterLevel;
    Label characterXp;
    Label characterName;

    public Sprite defaultSprite;
/*
    string bagContainerID = "BagContainer";
    string rosterContainerID = "RosterContainer";
    string equipButtonID = "EquipButton";
    string infoLeftLabelID = "InfoLeftLabel";
    string infoMiddleLabelID = "InfoMiddleLabel";
    string statsLabelID = "StatsLabel";
    string weaponID = "Weapon";
    string ArmorID = "Armor";
*/
    string characterSlotID = "character__slot";
    string characterSlotSpriteID = "character__slot__sprite";


    int currentItemIdx = -1;

    protected override void Awake() 
    {
        base.Awake();
    }
    void Start()
    {
        //if (m_Root == null) m_Root = m_Document.rootVisualElement;
        /*
        m_SlotContainer = m_Root.Q<VisualElement>(bagContainerID);
        m_RosterContainer = m_Root.Q<VisualElement>(rosterContainerID);
        m_Armor = m_Root.Q<VisualElement>(ArmorID);
        m_Weapon = m_Root.Q<VisualElement>(weaponID);

        equipButton = m_Root.Q<Button>(equipButtonID);

        equipmentButtons = m_SlotContainer.Query<Button>().ToList();
        Debug.Log("EquipmentButtons in InventoryScreen is : " + equipmentButtons.Count);

        infoLeftLabel = m_Root.Q<Label>(infoLeftLabelID);
        infoMiddleLabel = m_Root.Q<Label>(infoMiddleLabelID);
        statsLabel = m_Root.Q<Label>(statsLabelID);
        */

        ///

        characterSprite = m_Screen.Q<VisualElement>("Sprite");
        weaponSprite = m_Screen.Q<VisualElement>("WeaponSprite");
        armorSprite = m_Screen.Q<VisualElement>("ArmorSprite");

        characterLevel = m_Screen.Q<Label>("LevelLabel");
        characterName = m_Screen.Q<Label>("CharacterName");
        characterXp = m_Screen.Q<Label>("XPLabel");

        rosterContainer = m_Screen.Q<VisualElement>("RosterContainer");
        equipmentContainer = m_Screen.Q<VisualElement>("EquipmentContainer");

        weaponButton = m_Screen.Q<Button>("WeaponButton");
        armorButton = m_Screen.Q<Button>("ArmorButton");

        //curChracterData = characterList[0];
        //SetInventoryScreen();


        //int m = characterList.Count;
        //int n = 8;
        //string s = "rosterContainer";
        //CreateContainer(s, characterList,  InventoryItems, m_RosterContainer, m, n);

        //SetInventoryScreen();
        //CreateContainer();
    }

    
    public void SetInventoryScreen()
    {
//        infoLeftLabel.text = "";
//        infoMiddleLabel.text = "";
        equipmentList = gameManager.GMEquipmentList;
        characterList = gameManager.GMcharacterList;

        rosterContainer.Clear();
        equipmentContainer.Clear();

        curChracterData = characterList[0];

        characterSprite.style.backgroundImage = new StyleBackground(curChracterData.visual);
        characterName.text = curChracterData.characterName;
        characterLevel.text = curChracterData.characterLevel.ToString();
        // later modify..
        characterXp.text = curChracterData.xp.ToString();

        SetRoster();
        SetBag();

    }

    void SetRoster()
    {
        for(int i=0; i<gameManager.GMcharacterList.Count; i++)
        {
            //ClickableSlot clickableSlot = new ClickableSlot(characterSlotID, characterSlotSpriteID, "Roster"+i);
            ClickableSlot clickableSlot = new ClickableSlot(gameManager.GMcharacterList[i], "Roster" + i);
            rosterContainer.Add(clickableSlot);
            clickableSlot.RegisterCallback<ClickEvent, int>(ResetEquiped, i);
        }
    }

    void ResetEquiped(ClickEvent cvt, int i)
    {
        //Button targetButton = cvt.target as Button;
        //int n = targetButton.name[^1];
        if(gameManager.GMcharacterList[i].defaultWeapon != null) weaponSprite.style.backgroundImage = new StyleBackground(gameManager.GMcharacterList[i].defaultWeapon.sprite);
        else weaponSprite.style.backgroundImage = null;
        if(gameManager.GMcharacterList[i].defaultHelmet != null) armorSprite.style.backgroundImage = new StyleBackground(gameManager.GMcharacterList[i].defaultHelmet.sprite);
        else armorSprite.style.backgroundImage = null;

        ResetCharacterInfo(i);

    }

    void ResetCharacterInfo(int i)
    {
        curChracterData = characterList[i];
        characterSprite.style.backgroundImage = new StyleBackground(curChracterData.visual);
        characterName.text = curChracterData.characterName;
        characterLevel.text = curChracterData.characterLevel.ToString();
        // later modify..
        characterXp.text = curChracterData.xp.ToString();
    }


    void SetBag()
    {
        for(int i=0; i<gameManager.GMEquipmentList.Count; i++)
        {
            ClickableSlot clickableSlot = new ClickableSlot(gameManager.GMEquipmentList[i], i.ToString());
            equipmentContainer.Add(clickableSlot);
            clickableSlot.RegisterCallback<ClickEvent>(ResetEquip);
        }
    }

    void ResetEquip(ClickEvent cvt)
    {
        Button targetButton = cvt.target as Button;
        int i = System.Convert.ToInt32(targetButton.name);
        if(gameManager.GMEquipmentList[i].equipmentType == EquipmentType.Weapon)
        {
            if(curChracterData.defaultWeapon != null)
            {
                EquipmentSO temp = curChracterData.defaultWeapon;
                curChracterData.defaultWeapon = gameManager.GMEquipmentList[i];
                gameManager.GMEquipmentList[i] = temp;
            }
            else
            {
                curChracterData.defaultWeapon = gameManager.GMEquipmentList[i];
                gameManager.GMEquipmentList[i] = null;
            }

            weaponSprite.style.backgroundImage = new StyleBackground(gameManager.GMEquipmentList[i].sprite);
        }
        else // EquipmentType.Helmet
        {
            if(curChracterData.defaultHelmet != null)
            {
                EquipmentSO temp = curChracterData.defaultHelmet;
                curChracterData.defaultHelmet = gameManager.GMEquipmentList[i];
                gameManager.GMEquipmentList[i] = temp;
            }
            else
            {
                curChracterData.defaultHelmet = gameManager.GMEquipmentList[i];
                gameManager.GMEquipmentList[i] = null;
            }        

            armorSprite.style.backgroundImage = new StyleBackground(gameManager.GMEquipmentList[i].sprite);   
        }
        targetButton.style.backgroundImage = new StyleBackground(gameManager.GMEquipmentList[i].sprite);

        //gameManager.GMEquipedEquipmentList.Add(gameManager.GMEquipmentList[i]);
        //gameManager.GMEquipmentList[i] = null;
        //targetButton.style.display = DisplayStyle.None;
        
    }




/*
        for (int i=0; i<equipmentList.Count ; i++)
        {
            equipmentButtons[i].RegisterCallback<ClickEvent>(inform);
            equipmentButtons[i].style.backgroundImage = new StyleBackground(equipmentList[i].sprite);

            if (equipmentList[i].sprite != null)
            {
            //    item.style.backgroundImage = 
            //        new StyleBackground(equipmentList[i].sprite);
                //characterList[i].characterVisualsPrefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        for (int i=equipmentList.Count; i<9; i++)
        {
            equipmentButtons[i].style.backgroundImage = new StyleBackground(defaultSprite);
        }
        /*
        for (int i=equipmentList.Count; i<9; i++)
        {
            InventorySlot item = new InventorySlot("slotContainer", i.ToString());
            InventoryItems.Add(item);
            m_SlotContainer.Add(item);
        }
        */
    /*

    void inform(ClickEvent evt)
    {
        Button targetButton = evt.target as Button;
        int n = targetButton.name[^1] - '0';
        if (equipmentList[n] == null) 
        {
            Debug.Log("THere is no item in baglist");
            return;
        }
        infoLeftLabel.text = $"Item Name : {equipmentList[n].name}";
        infoMiddleLabel.text = $"Item Points : {equipmentList[n].points}";

        if (currentItemIdx != -1)
            equipmentButtons[currentItemIdx]?.RemoveFromClassList("color__border");
        currentItemIdx = n;
        targetButton.AddToClassList("color__border");
        equipButton.style.display = DisplayStyle.Flex;
        if (equipButton.style.display == DisplayStyle.None)
        {
            equipButton.style.display = DisplayStyle.Flex;
            Debug.Log("is there somethign happen?");
            Debug.Log(equipButton.style.display);
        }
        equipButton.RegisterCallback<ClickEvent, int>(Equip, n);
    }

    void Equip(ClickEvent evt, int n)
    {
        if (curChracterData != null)
        {
            if (equipmentList[n].equipmentType == EquipmentType.Weapon)
            {
                if (curChracterData.defaultWeapon != null)
                {
                    equipmentList.Add(curChracterData.defaultWeapon);
                    gameManager.GMEquipedEquipmentList.Remove(curChracterData.defaultWeapon);
                }
                curChracterData.defaultWeapon = equipmentList[n];
            }

            else if (equipmentList[n].equipmentType == EquipmentType.Helmet)
            {
                if(curChracterData.defaultHelmet != null)
                {
                    equipmentList.Add(curChracterData.defaultHelmet);
                    gameManager.GMEquipedEquipmentList.Remove(curChracterData.defaultHelmet);
                }
                curChracterData.defaultHelmet = equipmentList[n];
            }

            else 
            {
                Debug.Log("there is no type to equip");
            }

            ResetItemInfo(n);
            UpdateEquipmentInfo(curChracterData);
        }
    }

    void ResetItemInfo(int n)
    {
        gameManager.GMEquipedEquipmentList.Add(equipmentList[n]);
        gameManager.GMEquipmentList.RemoveAt(n);
        //equipmentList.RemoveAt(n);
        SetInventoryScreen();
        //equipmentButtons[n].style.visibility = Visibility.Hidden;
        //equipmentButtons[n].ClearClassList();
        //equipmentButtons[n].RemoveFromClassList("color__border");
        //equipmentButtons[n].style.backgroundImage = new StyleBackground(defaultSprite);
        //equipmentButtons[n].RemoveFromClassList("bag__item2");
        //equipmentButtons[n].AddToClassList("bag__item2");
        //equipmentButtons[n].style.backgroundImage = 
        //equipmentButtons[n].UnregisterCallback<ClickEvent>(inform);
        //infoLeftLabel.text = "";
        //infoMiddleLabel.text = "";
    }
/*
    void CreateContainer<T>(string containerName, List<T> list, List<InventorySlot> slot,VisualElement ve, int m, int n) //where T: ScriptableObject
    {
        for(int i=0; i<m; i++)
        {
            InventorySlot item = new InventorySlot(containerName);
            slot.Add(item);
            ve.Add(item);
            if (list[i].visual != null)
            {
                item.style.backgroundImage = 
                    new StyleBackground(list[i].visual);
                //characterList[i].characterVisualsPrefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        InventoryItems = new List<InventorySlot>();
        for(int i=m; i<n; i++)
        {
            InventorySlot item = new InventorySlot("rosterContainer");
            InventoryItems.Add(item);
            m_RosterContainer.Add(item);
        }

    }
*/


/*
    public void CreateContainer()
    {
        /*
        int n = characterList.Count;
        if(m_RosterContainer.childCount != 0)
        {
            m_RosterContainer.Clear();
        }
        for(int i=0; i<n; i++)
        {
            InventorySlot item = new InventorySlot("rosterContainer", i.ToString());
            InventoryItems.Add(item);
            item.button.RegisterCallback<ClickEvent, CharacterSO>(something, characterList[i]);
            m_RosterContainer.Add(item);
            if (characterList[i].characterVisualsPrefab != null)
            {
                item.style.backgroundImage = 
                    new StyleBackground(characterList[i].characterVisualsPrefab.GetComponent<SpriteRenderer>().sprite);
                //characterList[i].characterVisualsPrefab.GetComponent<SpriteRenderer>().sprite;
            }
        }
        for(int i=n; i<8; i++)
        {
            InventorySlot item = new InventorySlot("rosterContainer", i.ToString());
            InventoryItems.Add(item);
            m_RosterContainer.Add(item);
        }
        */

    }
/*
    void something(ClickEvent evt, CharacterSO characterSO)
    {
        UpdateEquipmentInfo(characterSO);
    }

    void UpdateEquipmentInfo(CharacterSO characterSO)
    {
        if (characterSO.defaultWeapon != null && characterSO.defaultWeapon.sprite != null) 
        {
            m_Weapon.style.backgroundImage = new StyleBackground(characterSO.defaultWeapon.sprite);
        }
        else
        {
            m_Weapon.style.backgroundImage = null;
        }
        if (characterSO.defaultHelmet != null && characterSO.defaultHelmet.sprite != null) 
        {
            m_Armor.style.backgroundImage = new StyleBackground(characterSO.defaultHelmet.sprite);
        }
        else
        {
            m_Armor.style.backgroundImage = null;
        }
        if (characterSO != null)
        {
            curChracterData = characterSO;
            statsLabel.text = $"Character Name : {characterSO.characterName}";
            //statsLabel.text = $"Respond Speed : {characterSO.responseSpeed}";
        }
    }
    */

}
