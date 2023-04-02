using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class HomeScreenControl : MonoBehaviour
{
    Button m_StartButton;
    Button m_ExitButton;
    Button m_SettingsButton;

    [SerializeField]
    // Drag & Drop in the inspector
    //private VisualTreeAsset _settingsButtonTemplate;
    // Create based on the file above
    //private VisualElement _settingsButtons;
    private VisualElement _buttonsWrapper;

    protected UIDocument m_document;

    protected VisualElement m_root;
    
    GameObject GO;

/*
    void Awake()
    {
        _buttonsWrapper = m_root.Q<VisualElement>("Buttons");
    
        //_settingsButtons = _settingsButtonTemplate.CloneTree();
        //var backButton = _settingsButtons.Q<Button>("BackButton");
        //backButton.clicked += BackButtonOnClicked;
        
    }
*/

    void Start()
    {
        m_document = GetComponent<UIDocument>();
        m_root = m_document.rootVisualElement;

        GO = GameObject.Find("GameObject");

        m_SettingsButton = m_root.Q<Button>("SettingsButton");
        m_SettingsButton.clicked += SettingsButtonOnClicked;

        m_StartButton = m_root.Q<Button>("StartButton");
        m_StartButton.clicked += ShowMainScene;
        //m_MainScreenButton.RegisterCallback<ClickEvent>(ShowMainScene);

        m_ExitButton = m_root.Q<Button>("ExitButton");
        m_ExitButton.clicked += ExitMainScene;
    }

    private void ShowMainScene()
    {
        //Debug.Log("ShowMainScreen");

        SceneManager.LoadScene("MainScene");
    }
    private void ShowMainScene(ClickEvent evt)
    {
        Debug.Log("Load Scene");
        SceneManager.LoadScene("Game");
    }

    private void ExitMainScene()
    {
        Application.Quit();
    }

    private void SettingsButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(m_SettingsButton);
    }

    private void BackButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(m_StartButton);
        _buttonsWrapper.Add(m_SettingsButton);
        _buttonsWrapper.Add(m_ExitButton);
    }
}
