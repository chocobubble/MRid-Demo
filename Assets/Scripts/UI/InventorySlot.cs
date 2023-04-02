using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.EventSystems;
using UnityEngine.Scripting;

namespace MRidDemo{
public class InventorySlot : VisualElement
{
    public Image Icon;
    public Button button;
    public string ItemGuid;
    public InventorySlot(string container, string name)
    {
        button = new Button();
        button.name = name;
        button.text = "";
        button.AddToClassList("invisible__button");
        //button.clicked += click;
        Icon = new Image();
        Icon.Add(button);
        Add(Icon);
        Icon.AddToClassList("slotIcon");
        //AddToClassList("slotContainer");
        AddToClassList(container);
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