using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;
using UnityEditor;

namespace MRidDemo{
public class ShopScreen : MenuScreen
{
    List<Button> sellingItemButtons = new List<Button>();
    List<EquipmentSO> defaultItems = new List<EquipmentSO>();
    public List<EquipmentSO> sellingItems = new List<EquipmentSO>();
    //List<EquipData> sellingItems = new List<EquipData>();
    Label itemInfo;
    Button purchaseButton;
    string sellingItemID = "SellingItem";
    string itemInfoID = "ItemInfo";
    string sellingItemActiveID = "selling__item";
    string sellingItemDeactiveID = "selling__item__deactive";
    string purchaseButtonID = "PurchaseButton";

    int currentItemIndex = -1;
    void Start()
    {
        itemInfo = m_Root.Q<Label>(itemInfoID);
        purchaseButton = m_Root.Q<Button>(purchaseButtonID);
        purchaseButton.clicked += Purchase;

        SetItemSprites();
        CreateSellingItems();
    }
    void Purchase()
    {
        if (currentItemIndex == -1 || sellingItems[currentItemIndex] == null)
        {
            Debug.LogWarning("The item isn't choosen");
            return;
        }

        sellingItemButtons[currentItemIndex].RemoveFromClassList(sellingItemActiveID);
        sellingItemButtons[currentItemIndex].AddToClassList(sellingItemDeactiveID);

        // It works!!
        gameManager.GMEquipmentList.Add(sellingItems[currentItemIndex]);
        //StartCoroutine(WaitOneSeconds());
        sellingItems[currentItemIndex] = null;
        m_Root.Q<Button>(sellingItemID + currentItemIndex).style.visibility = Visibility.Hidden;
    }
    IEnumerator WaitOneSeconds()
    {
        yield return new WaitForSeconds(2);
    }

    void SetItemSprites()
    {
        string[] equipmentPaths = Directory.GetFiles("Assets/Resources/GameData/EquipmentData", "*.asset", SearchOption.AllDirectories);

            // later, extract to methods 
            foreach (string path in equipmentPaths)
            {
                defaultItems.Add((EquipmentSO)AssetDatabase.LoadAssetAtPath(path, typeof(EquipmentSO)));
            }
    }

#region SettingSellingItems
    public void CreateSellingItems()
    {          
        for(int i=0; i<8; i++)
        {
            sellingItemButtons.Add(m_Root.Q<Button>(sellingItemID + i));
            EquipmentSO equip = ScriptableObject.CreateInstance<EquipmentSO>();
            int rnd = Random.Range(0,6);
            //EquipData e = new EquipData(defaultItems[rnd]);
            SetEquipmentData(equip, defaultItems[rnd]);
            sellingItems.Add(equip);
            sellingItemButtons[i].style.backgroundImage = new StyleBackground(equip.sprite);
            sellingItemButtons[i].RegisterCallback<ClickEvent>(ShowEquipmentInfo);
        }

    }

    void SetEquipmentData(EquipmentSO _equip, EquipmentSO _default)
    {
        _equip.equipmentName = _default.equipmentName;
        _equip.equipmentType = _default.equipmentType;
        _equip.rarity = _default.rarity;
        _equip.points = _default.points;
        _equip.sprite = _default.sprite;
    }
/*
    public void CreateSellingItems()
    {          
        for(int i=0; i<8; i++)
        {
            sellingItemButtons.Add(m_Root.Q<Button>(sellingItemID + i));
            EquipmentSO equip = ScriptableObject.CreateInstance<EquipmentSO>();
            int rnd = Random.Range(0,6);
            SetEquipmentData(equip, defaultItems[rnd]);
            sellingItems.Add(equip);
            sellingItemButtons[i].style.backgroundImage = new StyleBackground(equip.sprite);
            sellingItemButtons[i].RegisterCallback<ClickEvent, EquipmentSO>(ShowEquipmentInfo, equip);
        }

    }

    void SetEquipmentData(EquipmentSO _equip, EquipmentSO _default)
    {
        _equip.equipmentName = _default.equipmentName;
        _equip.equipmentType = _default.equipmentType;
        _equip.rarity = _default.rarity;
        _equip.points = _default.points;
        _equip.sprite = _default.sprite;
    }
*/
#endregion

    void ShowEquipmentInfo(ClickEvent cvt)
    {
        Button targetButton = cvt.target as Button; 
        if(currentItemIndex != -1)
        {
            sellingItemButtons[currentItemIndex].RemoveFromClassList(sellingItemActiveID);
            sellingItemButtons[currentItemIndex].AddToClassList(sellingItemDeactiveID);
            currentItemIndex = targetButton.name[^1] - '0';
        }
        else if (currentItemIndex == -1)
        {
            currentItemIndex = targetButton.name[^1] - '0';
            sellingItemButtons[currentItemIndex].RemoveFromClassList(sellingItemDeactiveID);
            sellingItemButtons[currentItemIndex].AddToClassList(sellingItemActiveID);

        }
        if(sellingItems[currentItemIndex] == null) return;
        itemInfo.text = $"Name : {sellingItems[currentItemIndex].equipmentName}" + '\n' 
            + $"Type : {sellingItems[currentItemIndex].equipmentType}" + '\n'
            + $"Points : {sellingItems[currentItemIndex].points}" + '\n';
        /*
        foreach (Button b in sellingItemButtons)
        {
            if (b.ClassListContains(sellingItemActiveID))
            {
            }
        }
        */
        if(targetButton.ClassListContains(sellingItemDeactiveID))
        {
            targetButton.RemoveFromClassList(sellingItemDeactiveID);
            targetButton.AddToClassList(sellingItemActiveID);
        }
    }
}
}
