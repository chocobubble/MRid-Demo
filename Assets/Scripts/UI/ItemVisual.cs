using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;

public class ItemVisual : VisualElement
{
    private readonly ItemDefinition m_Item;
    public ItemVisual(ItemDefinition item)
    {
        m_Item = item;
        name = $"{m_Item.FriendlyName}";
        style.height = m_Item.SlotDimension.Height *
                PlayerInventory.SlotDimension.Height;
        style.width = m_Item.SlotDimension.Width *
                PlayerInventory.SlotDimension.Width;
        style.visibility = Visibility.Hidden;
        
        VisualElement icon = new VisualElement
        {
            style = {backgroundImage = m_Item.Icon.texture}
        };

        Add(icon);

        icon.AddToClassList("visual-icon");
        AddToClassList("visual-icon-container");

        RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
    }
    
    public void SetPosition(Vector2 pos)
    {
        style.left = pos.x;
        style.right = pos.y;
    }

    private Vector2 m_OriginalPosition;
    private bool m_IsDragging;
    private (bool canPlace, Vector2 position) m_PlacementResults;
    private void OnMouseUpEvent(MouseUpEvent mouseEvent)
    {
        if(!m_IsDragging)
        {
            StartDrag();
            return;
        }
        m_IsDragging = false;
        if(m_PlacementResults.canPlace)
        {
            SetPosition(new Vector2(
                m_PlacementResults.position.x - parent.worldBound.position.x,
                m_PlacementResults.position.y - parent.worldBound.position.y
            ));
            return;
        }
        SetPosition(new Vector2(m_OriginalPosition.x, m_OriginalPosition.y));

        PlayerInventory.UpdateItemDetails(m_Item);
    }

    public void StartDrag()
    {
        m_IsDragging = true;
        m_OriginalPosition = worldBound.position - parent.worldBound.position;
        Debug.Log(worldBound.position);
        Debug.Log(parent.worldBound.position);
        BringToFront();
    }

    private void OnMouseMoveEvent(MouseMoveEvent mouseEvent)
    {
        if (!m_IsDragging) { return; }
        SetPosition(GetMousePosition(mouseEvent.mousePosition));
        m_PlacementResults = PlayerInventory.Instance.ShowPlacementTarget(this);
    }

    public Vector2 GetMousePosition(Vector2 mousePosition) =>
        new Vector2(mousePosition.x - (layout.width / 2) -
        parent.worldBound.position.x,
        mousePosition.y - (layout.height / 2) -
        parent.worldBound.position.y);
    
    ~ItemVisual()
    {
        UnregisterCallback<MouseMoveEvent>(OnMouseMoveEvent);
        UnregisterCallback<MouseUpEvent>(OnMouseUpEvent);
    }
}