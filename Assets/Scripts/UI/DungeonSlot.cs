using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MRidDemo{
public class DungeonSlot : Button
{
    /*
    public Image Icon;
    public Button button;
    public string ItemGuid;
    /*/
    public int dungeonLevel;

    public DungeonSlot(string s)
    {
        this.name = s;
        this.text = s;

        this.style.backgroundColor = new StyleColor(new Color(0,0,0,0));

        this.AddToClassList("dungeon__container");
        //this.RegisterCallback<ClickEvent>()
    }

    void OnDisable()
    {
        Debug.Log(this.name + " dungeon slot is disabled!");
    }

    void OnDestroy()
    {
        Debug.Log(this.name + " dungeon slot is destroyed !");
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