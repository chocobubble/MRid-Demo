using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class MainScreenSample : MonoBehaviour
{
    Button m_MainScreenButton;
    Button m_ExitMainScreenButton;
    //Button _settingsButton;

    [SerializeField]
    // Drag & Drop in the inspector
    private VisualTreeAsset _settingsButtonTemplate;
    // Create based on the file above
    private VisualElement _settingsButtons;
    private VisualElement _buttonsWrapper;

    protected UIDocument m_document;

    protected VisualElement m_root;

    void Awake()
    {
        _buttonsWrapper = m_root.Q<VisualElement>("Buttons");
    
        //_settingsButton = m_root.Q<Button>("SettingsButton");
        //_settingsButton.clicked += SettingsButtonOnClicked;
        _settingsButtons = _settingsButtonTemplate.CloneTree();
        var backButton = _settingsButtons.Q<Button>("BackButton");
        backButton.clicked += BackButtonOnClicked;
        
    }


    void Start()
    {
        m_document = GetComponent<UIDocument>();
        m_root = m_document.rootVisualElement;

        m_MainScreenButton = m_root.Q<Button>("StartButton");
        m_MainScreenButton.clicked += ShowMainScene;
        //m_MainScreenButton.RegisterCallback<ClickEvent>(ShowMainScene);

        m_ExitMainScreenButton = m_root.Q<Button>("ExitButton");
        m_ExitMainScreenButton.clicked += ExitMainScene;
    }

    private void ShowMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void ShowMainScene(ClickEvent evt)
    {
        Debug.Log("Load Scene");
        SceneManager.LoadScene("SampleScene");
    }

    private void ExitMainScene()
    {
        Application.Quit();
    }

    private void SettingsButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(_settingsButtons);
    }

    private void BackButtonOnClicked()
    {
        _buttonsWrapper.Clear();
        _buttonsWrapper.Add(m_MainScreenButton);
        _buttonsWrapper.Add(_settingsButtons);
        _buttonsWrapper.Add(m_ExitMainScreenButton);
    }
}
