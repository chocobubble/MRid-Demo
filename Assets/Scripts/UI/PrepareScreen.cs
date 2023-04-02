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
    Button backButton;
    Button battleStartButton;

    [SerializeField]
    List<Button> rosterButtons = new List<Button>();
    [SerializeField]
    List<Button> fightingListButtons = new List<Button>();

    // GameManager gameManager;
    VisualElement prepareScreen;
    const string backButtonID = "BackButton";
    const string battleStartButtonID = "BattleStartButton";
    const string rosterButtonID = "RosterButton";
    const string fightingListButtonID = "FightListButton";

    //[SerializeField]
    //List<CharacterSO> characterList = new List<CharacterSO>();
    
    List<CharacterSO> fightingList = new List<CharacterSO>(4);
    [SerializeField]
    //List<GameObject> fightingList = new List<GameObject>(4);

    void OnEnable()
    {
        backButton = m_Root.Q<Button>(backButtonID);
        battleStartButton = m_Root.Q<Button>(battleStartButtonID);

        prepareScreen = m_Root.Q<VisualElement>("PrepareScreen");
        dungeonScreen = m_Root.Q<VisualElement>("DungeonScreen");
        backButton.clicked += BackToMainScreen;
        battleStartButton.clicked += StartBattleButtonOnClick;

        Debug.Log("Prepare Screen OnEnable");
    }

    public void SetPrepareScreen()
    {
        // gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("Prepare screen Start");
        //characterList = gameManager.GMcharacterList;
        for(int i=0; i<4; i++) 
        {
            fightingList.Add(null);
        }
        SettingPrepareScreen();
        

        /*
        foreach(CharacterSO c in gameManager.characterList)
        {
            characterList.Add(c);
        }
        for (int i=1; i <= characterList.Count; i++)
        {
            ShowVisualElement(rosterButtons[i+1], true);
            Debug.Log("roster..");
            rosterButtons[i].text = characterList[i].characterName;
            Debug.Log(rosterButtons[i].text);
        } 
        */
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

        for (int i=1; i<5; i++) 
        {
            string t = fightingListButtonID + i;
            //Debug.Log(t);
            Button temp = m_Root.Q<Button>(t);
            if (temp == null) Debug.Log("fk");
            fightingListButtons.Add(temp);
        }
        
        for (int i=1; i<9; i++)
        {
            string t = rosterButtonID + i;
            rosterButtons.Add(m_Root.Q<Button>(t));
            //Debug.Log(m_Root.Q<Button>(t).text);
        }
/*
        foreach(CharacterSO c in gameManager.GMcharacterList)
        {
            characterList.Add(c);
        }
*/
        for (int i=0; i < gameManager.GMcharacterList.Count; i++)
        {
            ShowVisualElement(rosterButtons[i], true);
            //Debug.Log("roster..");
            if(rosterButtons[i].text == null) Debug.Log("null button's text");
            string cname = gameManager.GMcharacterList[i].characterName;
            //if(characterList[i].characterName == null) Debug.Log("null character name");
            //Debug.Log(rosterButtons[i].text + " " + characterList[i].characterName);
            
            //rosterButtons[i].text = characterList[i].characterName;
            rosterButtons[i].text = cname;
            //rosterButtons[i].clicked += SetToFightingRoster;
            rosterButtons[i].RegisterCallback<ClickEvent, VisualElement>(SetToFightingRoster, rosterButtons[i]);
            Debug.Log(rosterButtons[i].text);
        }
    }

    void SetToFightingRoster(ClickEvent evt, VisualElement ve) 
    {
        int fightingRosterIdx = 0;
        while (fightingRosterIdx < 4)
        {
            if (fightingList[fightingRosterIdx] != null) fightingRosterIdx++;
            else
            {
                Debug.Log("fightingRosterIdx = " + fightingRosterIdx);
                break;
            }
        }
        if (fightingRosterIdx == 4) Debug.Log("The roster is full");
        else 
        {
            string t = ve.name;
            char c = t[^1];
            Debug.Log("string : " + t + " / char : " + c );
            int characterIdx = c - '1';
            Debug.Log("characteridx = " + characterIdx + " / listsize = " + gameManager.GMcharacterList.Count);//characterList.Count);
            fightingList[fightingRosterIdx] = gameManager.GMcharacterList[characterIdx];
            //int curFightingRosterIdx = fightingRosterIdx + 1;
            fightingListButtons[fightingRosterIdx].text =
                fightingList[fightingRosterIdx].characterName;
            fightingListButtons[fightingRosterIdx].RegisterCallback<ClickEvent, VisualElement>(returnToRoster, fightingListButtons[fightingRosterIdx]);
            
            // onceClicked -> deactivate
            ve.UnregisterCallback<ClickEvent, VisualElement>(SetToFightingRoster);

            /// later, add a function to pop character from fightingList when this button clicked
        }
    }

    void returnToRoster(ClickEvent evt, VisualElement ve)
    {
        string t =  ve.name;
        char c = t[^1];
        Debug.Log("string : " + t + " / char : " + c );
        int idx = c - '1';
        Debug.Log("characteridx = " + idx + " / listsize = " + gameManager.GMcharacterList.Count);
        int rosterIdx = -1;
        for(int i=0; i < gameManager.GMcharacterList.Count; i++)
        {
            if(gameManager.GMcharacterList[i].characterName == fightingListButtons[idx].text)
            {
                rosterIdx = i;
                break;
            } 
        }
        fightingList[idx] = null;

        fightingListButtons[idx].text = "";

        if (rosterIdx == -1) Debug.Log("fk!!");
        else Debug.Log("rosteridx = " + rosterIdx);
        rosterButtons[rosterIdx].RegisterCallback<ClickEvent, VisualElement>(SetToFightingRoster, rosterButtons[rosterIdx]);
        ve.UnregisterCallback<ClickEvent, VisualElement>(returnToRoster);
    }
}
}