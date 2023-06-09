using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace MRidDemo {
    enum Number {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        FIFTH
    };
public class DungeonScreen : MenuScreen
{
    Number n = Number.FIRST;

    Button baseButton;
    Button level01;
    GameDataSO gameData;
    //public List<LevelSO> dungeonLevels = new List<LevelSO>();
    public int curDungeonLevel;
    public int curDungeonNumber;

    Button startButton;
    [SerializeField] LevelSO levelData;
    VisualElement dungeonImage;
    VisualElement prepareScreen;
    VisualElement container;
    VisualElement leftSide;
    VisualElement middleSide;

    string dungeonImageID = "Image";
    string prepareScreenID = "PrepareScreen";
    string containerID = "MainScreen";
    string leftSideID = "LeftSide";
    string middleSideID = "MiddleSide";
    string buttonClickedID = "button-clicked";

    string backgroundFaderID = "background__fade--on";
    void Start()
    {
        baseButton = m_Root.Q<Button>("BaseButton");
        level01 = m_Root.Q<Button>("Level01");
        dungeonImage = m_Screen.Q<VisualElement>(dungeonImageID);
        startButton = m_Root.Q<Button>("StartButton");
        container = m_Root.Q<VisualElement>(containerID);
        prepareScreen = m_Root.Q<VisualElement>(prepareScreenID);
        leftSide = m_Screen.Q<VisualElement>(leftSideID);
        middleSide = m_Screen.Q<VisualElement>(middleSideID);

        dungeonImage.style.backgroundImage = null;
        gameData = gameManager.gameDataSO;

        //dungeonLevels = gameData.dungeons;

        curDungeonLevel = 0;
        if(gameData.dungeonLevels.Count == 0) Debug.LogWarning("there is no dungeonlevels on the game data");

        //baseButton.clicked += LeftButtonOnClick;
        //level01.clicked += LevelButtonOnClick;
    }

    public void SetDungeonScreen()
    {
        leftSide.Clear();
        for(int i=0; i<gameData.dungeonNumber; i++)
        {
            DungeonSlot _slot = new DungeonSlot((Number.FIRST+i).ToString());
            leftSide.Add(_slot);//DungeonSlot((Number.FIRST+i).ToString()));
            _slot.RegisterCallback<ClickEvent, int>(LeftButtonOnClick, i);
        }
    }

    void LeftButtonOnClick(ClickEvent cvt, int _i)
    {
        curDungeonNumber = _i;
        
        foreach(DungeonSlot d in leftSide.Children())
        {
            if(d.ClassListContains(buttonClickedID)) d.RemoveFromClassList(buttonClickedID);
        }
        ((Button)cvt.target).AddToClassList(buttonClickedID);
        // SO files destroy??
        middleSide.Clear();
        for(int i = 1; i <= gameData.dungeonLevels[_i]; i++)
        {
            DungeonSlot _slot = new DungeonSlot("Level " + i);
            _slot.dungeonLevel = i;
            middleSide.Add(_slot);
            _slot.RegisterCallback<ClickEvent, int>(LevelButtonOnClick, i);
        }
    }

    void LevelButtonOnClick(ClickEvent cvt, int _i)
    {
        curDungeonLevel = _i;
        DungeonSlot targetDungeon = cvt.target as DungeonSlot;
        foreach(DungeonSlot d in middleSide.Children())
        {
            if(d.ClassListContains(buttonClickedID)) d.RemoveFromClassList(buttonClickedID);
        }
        targetDungeon.AddToClassList(buttonClickedID);
        ShowVisualElement(dungeonImage, true);
        dungeonImage.style.backgroundImage = new StyleBackground(gameData.dungeons[curDungeonNumber].dungeonSprite);
        
        //((Button)cvt.target).AddToClassList(buttonClickedID);
        //Debug.Log("curDungeonNumber = " + curDungeonNumber);
        //Debug.Log("curDungeonLevel = " +curDungeonLevel);
        

        ShowVisualElement(startButton, true);
        startButton.clicked += StartButtonOnClick;
    }

    void StartButtonOnClick()
    {
        ShowVisualElement(prepareScreen, true);
        container.AddToClassList(backgroundFaderID);
        //container.style.opacity = 0.2f;
        //m_Root.Q<VisualElement>("RightContainer").AddToClassList(backgroundFaderID);
        GameObject.Find("PrepareScreen").GetComponent<PrepareScreen>().SetPrepareScreen();
    }
}
}
