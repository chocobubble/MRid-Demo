using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace MRidDemo{
// Base class for a functional unit of the main menu
// MainScreen, DungeonScreen, ShopScreen, ...
// Toolbar, ...

public abstract class MenuScreen : MonoBehaviour
{
    [SerializeField] protected string m_ScreenName;

    [Header("UI Management")]
    [Tooltip("Set the Main Menu here explicitly (or get automatically from current GameObject).")]
    [SerializeField] protected UIManager m_UIManager;
    [Tooltip("Set the UI Document here explicitly (or get automatically from current GameObject).")]
    [SerializeField] protected UIDocument m_Document;

    [SerializeField]
    protected GameManager gameManager;

    // visual elements
    [SerializeField]
    protected VisualElement m_Screen;
    [SerializeField]
    protected VisualElement m_Root;

    //public event Action ScreenStarted;
    //public event Action ScreenEnded;

    //  UXML element name (defaults to the class name)
    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(m_ScreenName))
            m_ScreenName = this.GetType().Name;
    }

    protected virtual void Awake()
    {
        // set up UIManager and UI Document
        if (m_UIManager == null)
            m_UIManager = GetComponent<UIManager>();

        // default to current UIDocument if not set in Inspector
        if (m_Document == null)
            m_Document = GetComponent<UIDocument>();

        // alternately falls back to the MainMenu UI Document
        if (m_Document == null && m_UIManager != null)
            m_Document = m_UIManager.MainMenuDocument;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (m_Document == null)
        {
            Debug.LogWarning("MenuScreen " + m_ScreenName + ": missing UIDocument. Check Script Execution Order.");
            return;
        }
        else
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }
    }

    // The general workflow uses string IDs to query the VisualTreeAsset and find matching Visual Elements in the UXML.
    // Customize this for each MenuScreen subclass to identify any functional Visual Elements (buttons, controls, etc.).
    protected virtual void SetVisualElements()
    {
        // get a reference to the root VisualElement 
        if (m_Document != null)
            m_Root = m_Document.rootVisualElement;
        
        //Debug.Log("setvisualelement");
        if (m_Root == null) Debug.LogWarning("There is no m_Root");
        if (m_Screen == null) Debug.LogWarning("There is no m_Screen");

        m_Screen = GetVisualElement(m_ScreenName);

        if (m_Root == null || m_Screen == null) Debug.Log(m_ScreenName + "something wrong!!");
    }
    

    // Once you have the VisualElements, you can add button events here, using the RegisterCallback functionality. 
    // This allows you to use a number of different events (ClickEvent, ChangeEvent, etc.)
    protected virtual void RegisterButtonCallbacks() { 
        Debug.Log("registerbuttoncallback");
    }

    public bool IsVisible()
    {
        if (m_Screen == null)
            return false;

        return (m_Screen.style.display == DisplayStyle.Flex);
    }

    // Toggle a UI on and off using the DisplayStyle. 
    public static void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
            return;

        visualElement.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
    }

    // returns an element by name
    public VisualElement GetVisualElement(string elementName)
    {
        if (string.IsNullOrEmpty(elementName) || m_Root == null)
            return null;

        // query and return the element
        return m_Root.Q(elementName);
    }

    public virtual void ShowScreen()
    {
        ShowVisualElement(m_Screen, true);
        //ScreenStarted?.Invoke();
    }

    public virtual void HideScreen()
    {
        if (IsVisible())
        {
            ShowVisualElement(m_Screen, false);
            //ScreenEnded?.Invoke();
        }
    }
}
}
