using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MRidDemo{
public class PubScreen : MenuScreen
{
    public List<GameObject> prefabList = new List<GameObject>();
    public List<CharacterSO> characterSOs = new List<CharacterSO>();
    public List<GameObject> mercernaryList = new List<GameObject>();
    List<Button> mercernaryButtons = new List<Button>();

    QuestSO quest;
    Label questInfoLabel;
    VisualElement questButtonContainer;
    //public List<CharacterSO> mercernaryList = new List<CharacterSO>();
    string mercernayButtonID = "MercernaryButton";
    string mercernaryButtonActiveID = "mercernary__container__active";
    string mercernaryButtonDeactiveID = "mercernary__container";
    string questButtonUSS = "pubscreen__quest__button";
    string questButtonBlurUSS = "pubscreen__quest__button--accepted";

    int currentMercernaryIndex = -1;

    Button hireButton;
    void Start()
    {
        if(gameManager == null) gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();

        hireButton = m_Root.Q<Button>("HireButton");

        hireButton.clicked += Hire;
        CreateMercernaryList();

        ShowMercernaryListInfo();

        questInfoLabel = m_Screen.Q<Label>("Quest_Header_Label");
        questButtonContainer = m_Screen.Q<VisualElement>("Quest_Button_Container");
        quest = gameManager.questSO;
        SetQuest();
    }

    void Hire()
    {
        if(currentMercernaryIndex != -1)
        {
            // if(gameManager.money >= money)
            Debug.Log(currentMercernaryIndex);
            //CharacterSO characterSO = new CharacterSO((mercernaryList[currentMercernaryIndex].GetComponent<CharacterStats>()));
            CharacterSO characterSO = ScriptableObject.CreateInstance<CharacterSO>();
            characterSO.SetCharacterSO((mercernaryList[currentMercernaryIndex].GetComponent<CharacterStats>()));
            characterSO.characterVisualsPrefab = prefabList[(int)characterSO.characterClass];
            gameManager.GMcharacterList.Add(characterSO);
            mercernaryList[currentMercernaryIndex] = null;
            //m_Root.Q<VisualElement>("Mercernary"+currentMercernaryIndex).style.display = DisplayStyle.None;
            //m_Root.Q<VisualElement>("Mercernary"+currentMercernaryIndex).style.visibility = Visibility.Hidden;
            mercernaryButtons[currentMercernaryIndex].style.visibility = Visibility.Hidden;
        }
    }

#region CreateMercernaryList
        private void CreateMercernaryList()
        {
            for(int i=0; i<4; i++)
            {
                mercernaryList.Add(CreateChracterGO(i));
            }
        }

        private GameObject CreateChracterGO(int i)
        {
            int rnd = Random.Range(0, 3);
            GameObject character = Instantiate(prefabList[rnd]);
            character.SetActive(false);
            character.name = prefabList[rnd].name + i;
            CharacterStats characterStats = character.GetComponent<CharacterStats>();
            characterStats._data = characterSOs[rnd];
            characterStats.CharacterSetup();
            int rareRnd = Random.Range(0, 3);

            CharacterRareStatsSetup((Rarity)rareRnd, characterStats);


            return character;
        }

        void CharacterRareStatsSetup(Rarity rare, CharacterStats _characterStats)
        {
            int k = 0;
            if (rare == Rarity.Common) k = 1;
            else if (rare == Rarity.Rare) k = 50;
            else if (rare == Rarity.Special) k = 75;
            else Debug.Log("Something happen wrong in rare");

            _characterStats.rarity = rare;
            _characterStats.baseAttackDamage += ((Random.Range(k, 101)) / 10);
            _characterStats.responseSpeed -= ((Random.Range(k, 101) * 8) / 1000.0f);
            _characterStats.initHp += Random.Range(k, 101);
        }
#endregion

#region PubScreenUIMange
        void ShowMercernaryListInfo()
        {
            for(int i=0; i<4; i++)
            {
                mercernaryButtons.Add(m_Root.Q<Button>(mercernayButtonID + i));
                VisualElement ve = m_Root.Q<VisualElement>("Mercernary" + i);
                //ve.Q<Label>("MercernaryInfoLabel").text =
                //    $"initHP = {go.GetComponent<CharacterStats>().initHp}";
                //if (m_Root != null) m_Root = m_Document.rootVisualElement;
                ve.Q<Label>("MercernaryInfoLabel").text =
                    $"initHP = {mercernaryList[i].GetComponent<CharacterStats>().initHp}";
                //ve.Q<Button>("MercernaryButton").text = "BUtton!!";
                mercernaryButtons[i].RegisterCallback<ClickEvent>(MarkUp);
            }
        }
        
        void MarkUp(ClickEvent cvt)
        {
            Button targetButton = cvt.target as Button;
            //int pastMercernaryIndex = currentMercernaryIndex;
            if(currentMercernaryIndex != -1)
            {
                mercernaryButtons[currentMercernaryIndex].RemoveFromClassList(mercernaryButtonActiveID);
                mercernaryButtons[currentMercernaryIndex].AddToClassList(mercernaryButtonDeactiveID);
            }
            currentMercernaryIndex = targetButton.name[^1] - '0';
            if (currentMercernaryIndex == -1 || mercernaryList[currentMercernaryIndex] == null)
            {
                Debug.LogWarning("No mercernay choosen");
                return;
            }

            //if(pastMercernaryIndex != -1)
            //{
            //    mercernaryButtons[pastMercernaryIndex].RemoveFromClassList(mercernaryButtonActiveID);
            //    mercernaryButtons[pastMercernaryIndex].AddToClassList(mercernaryButtonDeactiveID);
            //}
            mercernaryButtons[currentMercernaryIndex].RemoveFromClassList(mercernaryButtonDeactiveID);
            mercernaryButtons[currentMercernaryIndex].AddToClassList(mercernaryButtonActiveID);
        }
#endregion


        void SetQuest()
        {
            /// quest Info
            questInfoLabel.text = $"Clear the {quest.questName} !!";

            /// quest Button
            questButtonContainer.Clear();
            Button questButton = new Button();
            questButton.AddToClassList(questButtonUSS);
            switch(quest.state)
            {
                case questState.NOTACCEPT:
                    questButton.text = "ACCEPT";
                    //questButton.clicked += AcceptQuest;
                    questButton.RegisterCallback<ClickEvent>(AcceptQuest);
                    break;
                case questState.SUCCESS:
                    questButton.text = "COMPLETE";
                    //questButton.clicked += CompleteQuest;
                    questButton.RegisterCallback<ClickEvent>(CompleteQuest);
                    break;
                case questState.ACCEPT:
                    questButton.text = "NOT CLEARED";
                    questButton.AddToClassList(questButtonBlurUSS);
                    break;
            }
            questButtonContainer.Add(questButton);
        }  
        void AcceptQuest(ClickEvent cvt)
        {
            Button targetButton = cvt.target as Button;
            quest.state = questState.ACCEPT;
            targetButton.AddToClassList(questButtonBlurUSS);
        }

        void CompleteQuest(ClickEvent cvt)
        {
            Button targetButton = cvt.target as Button;
            targetButton.AddToClassList(questButtonBlurUSS);
            quest.state = questState.NOTACCEPT;
            quest.questNumber += 1;
            gameManager.gameDataSO.dungeonNumber += 1;
        }
    } 
}
