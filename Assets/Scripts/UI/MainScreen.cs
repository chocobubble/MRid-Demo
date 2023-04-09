using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
namespace MRidDemo{
public class MainScreen : MenuScreen
{
    // parent's variable
    // [SerializeField] UIManager m_UIManager;
    // GameManager gameManager;
    const string k_DungeonScreenButton = "DungeonButton";
    const string k_ShopScreenButton = "ShopButton";
    const string k_PubScreenButton = "PubButton";
    const string k_InventoryScreenButton = "InventoryButton";
    
    // const string

    const string k_ButtonActiveClass = "main__button--active";
    const string k_ButtonInactiveClass = "main__button";

    Button m_DungeonScreenButton;
    Button m_ShopScreenButton;
    Button m_PubScreenButton;
    Button m_InventoryScreenButton;

    VisualElement dateContainer;
    string dateContainerID = "DateContainer";
    string moneyLabelID = "MoneyLabel";
    VisualElement moneyContainer;
    string moneyContainerID = "MoneyContainer";
    string dateLabelID = "DateLabel";

    protected override void Awake()
    { // called when the scene loaded
        base.Awake();
    }
    void Start()
    { // called when the scene loaded
        // gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        dateContainer = m_Root.Q<VisualElement>(dateContainerID);
        moneyContainer = m_Root.Q<VisualElement>(moneyContainerID);
        SettingDate();
/*
        foreach(CharacterSO c in gameManager.GMcharacterList)
        {
            SpawnAllyCharacter(c);
        }
*/
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();
        
        m_DungeonScreenButton = m_Root.Q<Button>(k_DungeonScreenButton);
        m_ShopScreenButton = m_Root.Q<Button>(k_ShopScreenButton);
        m_PubScreenButton = m_Root.Q<Button>(k_PubScreenButton);
        m_InventoryScreenButton = m_Root.Q<Button>(k_InventoryScreenButton);

        if (m_DungeonScreenButton == null)
        {
            Debug.Log("NO dungeonscreen button");
        } else {
            Debug.Log("dungeonscreen is on");
        }
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        m_DungeonScreenButton?.RegisterCallback<ClickEvent>(ShowDungeonScreen);
        Debug.Log(m_DungeonScreenButton.text);
        m_DungeonScreenButton.clicked += ShowDungeonScreen;
        m_ShopScreenButton?.RegisterCallback<ClickEvent>(ShowShopScreen);
        m_PubScreenButton?.RegisterCallback<ClickEvent>(ShowPubScreen);
        m_InventoryScreenButton?.RegisterCallback<ClickEvent>(ShowInventoryScreen);
    }

    void ShowDungeonScreen()
    {
        Debug.Log("Showdungeon");
        ActivateButton(m_DungeonScreenButton);

    }
    void ShowDungeonScreen(ClickEvent evt)
    {
        Debug.Log("showdungeon");
        ActivateButton(m_DungeonScreenButton);
        m_UIManager?.ShowDungeonScreen();
    }
    void ShowShopScreen(ClickEvent evt)
    {
        ActivateButton(m_ShopScreenButton);
        m_UIManager?.ShowShopScreen();
    }
    void ShowPubScreen(ClickEvent evt)
    {
        ActivateButton(m_PubScreenButton);
        m_UIManager?.ShowPubScreen();
    }
    void ShowInventoryScreen(ClickEvent evt)
    {
        //Debug.Log(m_InventoryScreenButton);
        
        ActivateButton(m_InventoryScreenButton);
        m_UIManager?.ShowInventoryScreen();
    }

    void ActivateButton(Button screenButton)
    {
        //Debug.Log("Before activebutton");
        if (screenButton == null) return;

        //Debug.Log("ActiveButton");

        HighlightElement(screenButton, k_ButtonInactiveClass, k_ButtonActiveClass, m_Root);

    }

    void HighlightElement(VisualElement ele, string inactiveClass, string activeClass, VisualElement root)
    {
        if (ele == null) return;

        VisualElement curSelect = root.Q<VisualElement>(className: activeClass);

        if (curSelect == ele) return;

        // de-highlight whatever is currently active
        curSelect?.RemoveFromClassList(activeClass);
        curSelect?.AddToClassList(inactiveClass);

        ele.RemoveFromClassList(inactiveClass);
        ele.AddToClassList(activeClass);
    }
/*
    void SpawnAllyCharacter(CharacterSO data)
    {
        List<GameObject> prefabList = GameObject.Find("PubScreen").GetComponent<PubScreen>().prefabList;
        GameObject _ally = Instantiate<GameObject>(prefabList[(int)data.characterClass]);
        CharacterStats _stats = _ally.GetComponent<CharacterStats>();
        //AllyCtrl allyCtrl = _ally.GetComponent<AllyCtrl>();
        _ally.name = data.characterName;
        _stats._data = data;
        _stats.CharacterSetup();
        _ally.SetActive(false);
        gameManager.roster.Add(_ally);
    }
*/
    public void SettingDate()
    {
        int week = gameManager.date % 4 + 1;
        int month = (gameManager.date / 4) % 12 + 1;
        int year = ((gameManager.date / 4) / 12) + 1;
        dateContainer.Q<Label>(dateLabelID).text =
            $"{year}Year / {month}Month / {week}Week";
        moneyContainer.Q<Label>(moneyLabelID).text = $"{gameManager.money}";
    }

}

}
