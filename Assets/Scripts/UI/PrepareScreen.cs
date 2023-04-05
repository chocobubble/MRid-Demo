using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace MRidDemo
{
public class PrepareScreen : MenuScreen
{
    VisualElement dungeonScreen;
    VisualElement fightListBox;
    VisualElement rosterBox;
    Button backButton;
    Button battleStartButton;

    [SerializeField]
    List<Button> rosterButtons = new List<Button>();
    [SerializeField]
    List<Button> fightingListButtons = new List<Button>(4);

    // GameManager gameManager;
    VisualElement prepareScreen;
    const string backButtonID = "BackButton";
    const string battleStartButtonID = "BattleStartButton";
    const string rosterButtonID = "RosterButton";
    const string fightingListButtonID = "FightListButton";

    //[SerializeField]
    //List<CharacterSO> characterList = new List<CharacterSO>();
    
    public List<CharacterSO> fightingList = new List<CharacterSO>(4);
    public List<ClickableSlot> rosterSlotList = new List<ClickableSlot>();
    //[SerializeField]
    //List<GameObject> fightingList = new List<GameObject>(4);

    void OnEnable()
    {
        backButton = m_Root.Q<Button>(backButtonID);
        battleStartButton = m_Root.Q<Button>(battleStartButtonID);

        prepareScreen = m_Root.Q<VisualElement>("PrepareScreen");
        dungeonScreen = m_Root.Q<VisualElement>("DungeonScreen");
        
        fightListBox = m_Screen.Q<VisualElement>("FightListBox");
        rosterBox = m_Screen.Q<VisualElement>("Roster");

        backButton.clicked += BackToMainScreen;
        battleStartButton.clicked += StartBattleButtonOnClick;

        for (int i=1; i<5; i++) 
        {
            string t = fightingListButtonID + i;
            Button tempButton = m_Screen.Q<Button>(t);
            tempButton.RegisterCallback<ClickEvent, int>(returnToRoster, i);
            fightingListButtons.Add(tempButton);
        }

        Debug.Log("Prepare Screen OnEnable");
    }
    void returnToRoster(ClickEvent cvt, int _i)
    {
        Button targetButton = cvt.target as Button;
        fightingList[_i-1] = null;

        fightingListButtons[_i-1].text = "";

        rosterSlotList[System.Convert.ToInt32(targetButton.name)].isClicked = false;
    }

    public void SetPrepareScreen()
    {
        //Debug.Log("Prepare screen Start");


        SettingPrepareScreen();
    }

    void BackToMainScreen()
    {
        //ShowVisualElement(prepareScreen, false);
        //ShowVisualElement(dungeonScreen, true);
        m_Root.Q<VisualElement>("MainScreen").RemoveFromClassList("background__fade--on");
        m_Root.Q<VisualElement>("MainScreen").AddToClassList("background__fade--off");
        HideScreen();

    }

    void StartBattleButtonOnClick()
    {
        // fighting roster list must include equal or more than one character
        if(fightingList.Count == 0) Debug.Log("U must include one or more characters in ur roster");
        else
        {
            // Destroy trash SO files
            DestoryEquipmentSO();

            gameManager.fightingMembers = fightingList;
/*
            foreach(GameObject go in gameManager.fightingMembers)
            {
                if (go != null) DontDestroyOnLoad(go);
            }
*/
            gameManager.isDungeonEnded = false;
            SceneManager.LoadScene("Game");
        }
    }

    void DestoryEquipmentSO()
    {
        ShopScreen shopScreen = GameObject.Find("ShopScreen").GetComponent<ShopScreen>();
        for(int i =0; i < 8; i++)
        {
            if (shopScreen.sellingItems[i] != null)
            {
                Debug.Log("destroy");
                Destroy(shopScreen.sellingItems[i]);
            }
        }
    }

    public void SettingPrepareScreen()
    {
        for (int i=0; i<fightingList.Count; i++)
        {
            fightingList[i] = null;
        }

        for (int i=fightingList.Count; i<4; i++)
        {
            fightingList.Add(null);
        }
        // fightListBox.Clear();
        rosterBox.Clear();

        for (int i=0; i < gameManager.GMcharacterList.Count; i++)
        {
            ClickableSlot rosterSlot = new ClickableSlot(gameManager.GMcharacterList[i].characterName, i);
            rosterSlot.icon.style.backgroundImage = new StyleBackground(gameManager.GMcharacterList[i].visual);
            rosterSlot.info.text = $"Name : {rosterSlot.name} \n" + $"Level : {gameManager.GMcharacterList[i].characterLevel}";
            rosterSlot.RegisterCallback<ClickEvent, ClickableSlot>(SetToFightingRoster, rosterSlot);
            rosterBox.Add(rosterSlot);
            rosterSlotList.Add(rosterSlot);
            //Debug.Log(rosterButtons[i].text);
        }
    }

    void SetToFightingRoster(ClickEvent evt, ClickableSlot cs) 
    {
        if (cs.isClicked == true) return;
        cs.isClicked = true;
        int emptyIdx = 0;
        while (emptyIdx < 4)
        {
            if (fightingList[emptyIdx] != null) emptyIdx++;
            else
            {
                Debug.Log("emptyIdx = " + emptyIdx);
                break;
            }
        }
        if (emptyIdx == 4) Debug.Log("The roster is full");
        else 
        {
            //string t = ve.name;
            //char c = t[^1];
            //Debug.Log("string : " + t + " / char : " + c );
            int characterIdx = cs.characterIdx;
            //Debug.Log("characteridx = " + characterIdx + " / listsize = " + gameManager.GMcharacterList.Count);//characterList.Count);
            fightingList[emptyIdx] = gameManager.GMcharacterList[characterIdx];
            //int curemptyIdx = emptyIdx + 1;
            fightingListButtons[emptyIdx].text = fightingList[emptyIdx].characterName;
            fightingListButtons[emptyIdx].name = characterIdx.ToString();
            
            //fightingListButtons[emptyIdx].RegisterCallback<ClickEvent, ClickableSlot>(returnToRoster, cs);
            
            // onceClicked -> deactivate
            // cs.UnregisterCallback<ClickEvent, VisualElement>(SetToFightingRoster);

            /// later, add a function to pop character from fightingList when this button clicked
        }
    }

}
}