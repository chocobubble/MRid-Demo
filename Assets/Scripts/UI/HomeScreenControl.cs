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

    private VisualElement _buttonsWrapper;

    protected UIDocument m_document;

    protected VisualElement m_root;
    
    GameObject GO;

    void Start()
    {
        m_document = GetComponent<UIDocument>();
        m_root = m_document.rootVisualElement;

        GO = GameObject.Find("GameObject");

        m_SettingsButton = m_root.Q<Button>("SettingsButton");
        m_SettingsButton.clicked += SettingsButtonOnClicked;

        m_StartButton = m_root.Q<Button>("StartButton");
        m_StartButton.clicked += ShowMainScene;

        m_ExitButton = m_root.Q<Button>("ExitButton");
        m_ExitButton.clicked += ExitMainScene;
    }

    private void ShowMainScene()
    {
        SceneManager.LoadScene("MainScene");
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
}
