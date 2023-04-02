using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace MRidDemo {
public class DungeonScreen : MenuScreen
{
    Button baseButton;
    Button level01;

    Button startButton;
    [SerializeField] LevelSO levelData;
    VisualElement dungeonImage;
    VisualElement prepareScreen;
    VisualElement container;

    string dungeonImageID = "DungeonImage";
    string prepareScreenID = "PrepareScreen";
    string containerID = "MainScreen";

    string backgroundFaderID = "background__fade--on";
    void Start()
    {
        baseButton = m_Root.Q<Button>("BaseButton");
        level01 = m_Root.Q<Button>("Level01");
        dungeonImage = m_Root.Q<VisualElement>(dungeonImageID);
        startButton = m_Root.Q<Button>("StartButton");

        container = m_Root.Q<VisualElement>(containerID);
        prepareScreen = m_Root.Q<VisualElement>(prepareScreenID);

        baseButton.clicked += LeftButtonOnClick;
        level01.clicked += LevelButtonOnClick;
    }

    void LeftButtonOnClick()
    {
        // parent's method
        ShowVisualElement(level01, true);
    }

    void LevelButtonOnClick()
    {
        ShowVisualElement(dungeonImage, true);
        startButton.clicked += StartButtonOnClick;
        
        Debug.Log("Let's go Game Scene");
    }

    void StartButtonOnClick()
    {
        ShowVisualElement(prepareScreen, true);
        container.AddToClassList(backgroundFaderID);
        //container.style.opacity = 0.2f;
        //m_Root.Q<VisualElement>("RightContainer").AddToClassList(backgroundFaderID);
        GameObject.Find("PrepareScreen").GetComponent<PrepareScreen>().SetPrepareScreen();
    

// later, delete below
/*
        ShopScreen shopScreen = GameObject.Find("ShopScreen").GetComponent<ShopScreen>();
        for(int i =0; i < 8; i++)
        {
            if (shopScreen.sellingItems[i] != null)
            {
                Debug.Log("destroy");
                Destroy(shopScreen.sellingItems[i]);
            }
        }
*/

    }


        //SceneManager.LoadScene("Game");
    
/*
    void ShowElement(VisualElement ele, bool state)
    {
        ele.style.display = (state) ? DisplayStyle.Flex : DisplayStyle.None;
    }
*/
}
}
