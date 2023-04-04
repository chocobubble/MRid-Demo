using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Serialization;
using System;

namespace MRidDemo{
// high-level manager for the various parts of the Main Menu UI. Here we use one master UXML and one UIDocument.
// We allow the individual parts of the user interface to have separate UIDocuments if needed (but not shown in this example).

[RequireComponent(typeof(UIDocument))]
public class UIManager : MonoBehaviour
{


    [Header("Modal Menu Screens")]
    [Tooltip("Only one modal interface can appear on-screen at a time.")]
    [SerializeField] MainScreen m_MainModalScreen;
    [SerializeField] DungeonScreen m_DungeonModalScreen;
    [SerializeField] ShopScreen m_ShopModalScreen;
    [SerializeField] PubScreen m_PubModalScreen;
    [SerializeField] InventoryScreen m_InventoryModalScreen;
    /*

    [Header("Toolbars")]
    [Tooltip("Toolbars remain active at all times unless explicitly disabled.")]
    [SerializeField] OptionsBar m_OptionsToolbar;
    [SerializeField] MenuBar m_MenuToolbar;

    [Header("Full-screen overlays")]
    [Tooltip("Full-screen overlays block other controls until dismissed.")]
    [SerializeField] MenuScreen m_InventoryScreen;
    [SerializeField] SettingsScreen m_SettingsScreen;
*/
    List<MenuScreen> m_AllModalScreens = new List<MenuScreen>();

    UIDocument m_MainMenuDocument;
    public UIDocument MainMenuDocument => m_MainMenuDocument;

    [SerializeField]
    GameObject prepareScreenGO;

    void OnEnable()
    {
        m_MainMenuDocument = GetComponent<UIDocument>();
        SetupModalScreens();
        //ShowMainScreen();
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void SetupModalScreens()
    {
        if (m_MainModalScreen != null)
            m_AllModalScreens.Add(m_MainModalScreen);

        if (m_DungeonModalScreen != null)
            m_AllModalScreens.Add(m_DungeonModalScreen);
/*
        if (m_PrepareModalScreen != null)
            m_AllModalScreens.Add(m_PrepareModalScreen);
*/
        if (m_InventoryModalScreen != null)
            m_AllModalScreens.Add(m_InventoryModalScreen);
        
        if (m_ShopModalScreen != null)
            m_AllModalScreens.Add(m_ShopModalScreen);
        
        if (m_PubModalScreen != null)
            m_AllModalScreens.Add(m_PubModalScreen);
    
        
    }
    // shows one screen at a time
    void ShowModalScreen(MenuScreen modalScreen)
    {
        foreach (MenuScreen m in m_AllModalScreens)
        {
            if (m == modalScreen)
            {
                m?.ShowScreen();
            }
            else
            {
                m?.HideScreen();
            }
        }
    }

    // methods to toggle screens on/off

    // modal screen methods 
    public void ShowMainScreen()
    {
        ShowModalScreen(m_MainModalScreen);
    }

    // note: screens with tabbed menus default to showing the first tab
    public void ShowDungeonScreen()
    {
        Debug.Log("showdungeonscreen");
        ShowModalScreen(m_DungeonModalScreen);
        m_DungeonModalScreen.SetDungeonScreen();
        //prepareScreenGO.SetActive(true);
    }
    public void ShowInventoryScreen()
    {
        ShowModalScreen(m_InventoryModalScreen);
        //m_InventoryModalScreen.curChracterData = gameManager.GMcharacterList[0];
        m_InventoryModalScreen.SetInventoryScreen();
        //m_InventoryModalScreen.CreateContainer();
    }
    
    public void ShowShopScreen()
            {
                ShowModalScreen(m_ShopModalScreen);
            }
        /*

            // opens the Shop Screen directly to a specific tab (e.g. to gold or gem shop) from the Options Bar
    public void ShowShopScreen(string tabName)
            {
                m_MenuToolbar?.ShowShopScreen();
                m_ShopModalScreen?.SelectTab(tabName);
            }
    */

    public void ShowPubScreen()
            {
                ShowModalScreen(m_PubModalScreen);
            }
    
/*
            // overlay screen methods
            public void ShowSettingsScreen()
            {
                m_SettingsScreen?.ShowScreen();
            }
*/
    
}
}

