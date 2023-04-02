using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using UnityEngine.UIElements;

using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            Configure();
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
    }

    private VisualElement m_Root;
    private VisualElement m_InventoryGrid;
    private static Label m_ItemDetailHeader;
    private static Label m_ItemDetailBody;
    private static Label m_itemDetailPrice;
    private bool m_IsInventoryReady;
    public static Dimensions SlotDimension {get; private set;}
    private async void Configure()
    {
        m_Root = GetComponentInChildren<UIDocument>().rootVisualElement;
        m_InventoryGrid = m_Root.Q<VisualElement>("Grid");
        VisualElement itemDetails = m_Root.Q<VisualElement>("ItemDetails");
        m_ItemDetailHeader = itemDetails.Q<Label>("Header");
        m_ItemDetailBody = itemDetails.Q<Label>("Body");
        m_itemDetailPrice = itemDetails.Q<Label>("SellPrice");

        await UniTask.WaitForEndOfFrame(this);

        ConfigureSlotDimensions();

        m_IsInventoryReady = true;

        ConfigureInventoryTelegraph();
    }

    private void ConfigureSlotDimensions()
    {
        VisualElement firstSlot = m_InventoryGrid.Children().First();
        SlotDimension = new Dimensions
        {
            Width = Mathf.RoundToInt(firstSlot.worldBound.width),
            Height = Mathf.RoundToInt(firstSlot.worldBound.height)
        };
    }

    public List<StoredItem> StoredItems = new List<StoredItem>();
    public Dimensions InventoryDimension;
    private async Task<bool> GetPositionForItem(VisualElement newItem)
    {
        for (int y=0; y<InventoryDimension.Height; y++)
        {
            for (int x=0; x<InventoryDimension.Width; x++)
            {
                // try position
                SetItemPosition(newItem, new Vector2(SlotDimension.Width*x,
                    SlotDimension.Height*y));
                await UniTask.WaitForEndOfFrame(this);
                StoredItem overlappingItem = StoredItems.FirstOrDefault(s =>
                    s.RootVisual != null &&
                    s.RootVisual.layout.Overlaps(newItem.layout));
                // Nothing is here! Place the item
                if (overlappingItem == null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static void SetItemPosition(VisualElement element, Vector2 vector)
    {
        element.style.left = vector.x;
        element.style.right = vector.y;
    }

    private void Start() => LoadInventory();

    private async void LoadInventory()
    {
        await UniTask.WaitUntil(()=>m_IsInventoryReady);
        foreach(StoredItem loadedItem in StoredItems)
        {
            ItemVisual inventoryItemVisual = new ItemVisual(loadedItem.Details);
            AddItemToInventoryGrid(inventoryItemVisual);
            bool inventoryHasSpace = await GetPositionForItem(inventoryItemVisual);
            if(!inventoryHasSpace)
            {
                Debug.Log("No space - Cannot pick up the item");
                RemoveItemFromInventoryGrid(inventoryItemVisual);
                continue;
            }

            ConfigureInventoryItem(loadedItem, inventoryItemVisual);
        }
    }

    private void AddItemToInventoryGrid(VisualElement item) => m_InventoryGrid.Add(item);
    private void RemoveItemFromInventoryGrid(VisualElement item) => m_InventoryGrid.Remove(item);
    private static void ConfigureInventoryItem(StoredItem item, ItemVisual visual)
    {
        item.RootVisual = visual;
        visual.style.visibility = Visibility.Visible;
    }

    private VisualElement m_Telegraph;

    private void ConfigureInventoryTelegraph()
    {
        m_Telegraph = new VisualElement {
            name="Telegraph",
            style = {
                position = Position.Absolute,
                visibility = Visibility.Hidden
            }
        };

        m_Telegraph.AddToClassList("slot-icon-highlighted");
        AddItemToInventoryGrid(m_Telegraph);
    }

    public (bool canPlace, Vector2 position) ShowPlacementTarget(ItemVisual draggedItem)
    {
        if (!m_InventoryGrid.layout.Contains(new Vector2(draggedItem.localBound.xMax,
            draggedItem.localBound.yMax)))
        {
            m_Telegraph.style.visibility = Visibility.Hidden;
            return (canPlace: false, position: Vector2.zero);
        }
        VisualElement targetSlot = m_InventoryGrid.Children().Where(x=>
            x.layout.Overlaps(draggedItem.layout)&&x != draggedItem).OrderBy(x=>
            Vector2.Distance(x.worldBound.position,
            draggedItem.worldBound.position)).First();

        m_Telegraph.style.width = draggedItem.style.width;

        m_Telegraph.style.height = draggedItem.style.height;

        SetItemPosition(m_Telegraph, new Vector2(targetSlot.layout.position.x,
            targetSlot.layout.position.y));

        m_Telegraph.style.visibility = Visibility.Visible;

        var overlappingItems = StoredItems.Where(x=>x.RootVisual != null &&
            x.RootVisual.layout.Overlaps(m_Telegraph.layout)).ToArray();
        
        if (overlappingItems.Length > 1)
        {
            m_Telegraph.style.visibility = Visibility.Hidden;
            return (canPlace: false, position: Vector2.zero);
        }

        return (canPlace: true, targetSlot.worldBound.position);
    }

    public static void UpdateItemDetails(ItemDefinition item)
    {
        m_ItemDetailHeader.text = item.FriendlyName;
        m_ItemDetailBody.text = item.Description;
        m_itemDetailPrice.text = item.SellPrice.ToString();
    }
}
