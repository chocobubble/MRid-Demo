using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MRidDemo{
public class ClickableSlot : Button
{
    public Image Icon;
    public Button button;
    public string ItemGuid;
    /*
    public ClickableSlot(string container, string slotSprite, string name)
    {
        button = new Button();
        button.name = name;
        button.text = "";
        button.AddToClassList(container);
        //button.clicked += click;
        Icon = new Image();
        Icon.Add(button);
        Add(Icon);
        Icon.AddToClassList(slotSprite);
        //AddToClassList("slotContainer");
        //AddToClassList(container);
    }
    */

    public ClickableSlot(CharacterSO characterSO, string name)
    {
        //button = new Button();
        //button.name = name;
        //button.text = "";
        this.name = name;
        this.text = "";
        //button.AddToClassList("character__slot");
        //button.clicked += click;
        this.style.backgroundColor = new StyleColor(new Color(0,0,0,0));
        /*
        Icon = new Image();
        Icon.AddToClassList("character__slot__sprite");
        if (characterSO.visual == null) Debug.Log("characterslot visual null");
        if (Icon.ClassListContains("character__slot__sprite")) Debug.Log("yes!");
        Icon.style.backgroundImage = new StyleBackground(characterSO.visual);
        */
        this.style.backgroundImage = new StyleBackground(characterSO.visual);
        //this.style.width = Length.Percent(14);
        //this.style.height = Length.Percent(50);
        //Icon.Add(button);
        //Add(Icon);
        //AddToClassList("slotContainer");
        this.AddToClassList("character__slot");
    }

    public ClickableSlot(EquipmentSO equipmentSO, string name)
    {
        this.name = name;
        this.text = "";
        this.AddToClassList("bag__item__slot");
        this.style.backgroundColor = new StyleColor(new Color(0,0,0,0));
        if(equipmentSO.sprite == null)
        {
            this.style.backgroundColor = new StyleColor(new Color(255, 255, 255, 255));
        }
        else 
        {
            this.style.backgroundImage = new StyleBackground(equipmentSO.sprite);
        }
        if(equipmentSO.isEquiped == true)
        {
            this.AddToClassList("bag__item__slot--equiped");
        }
    }

    void click()
    {
        Debug.Log("clicked!!");
    }
    /*
    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
    */
}
}