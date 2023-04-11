using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace MRidDemo{
public class PubScreen : MenuScreen
{
    public List<GameObject> prefabList = new List<GameObject>();
    public List<CharacterSO> characterSOs = new List<CharacterSO>();
    public List<GameObject> mercenaryList = new List<GameObject>();
    List<Button> mercenaryButtons = new List<Button>();

    QuestSO quest;
    Label questInfoLabel;
    VisualElement questButtonContainer;
    //public List<CharacterSO> mercenaryList = new List<CharacterSO>();
    string mercernayButtonID = "MercenaryButton";
    string mercenaryButtonActiveID = "mercenary__container__active";
    string mercenaryButtonDeactiveID = "mercenary__container";
    string questButtonUSS = "pubscreen__quest__button";
    string questButtonBlurUSS = "pubscreen__quest__button--accepted";

    int currentMercenaryIndex = -1;

    Button hireButton;
    void Start()
    {
        if(gameManager == null) gameManager = GameObject.Find("GameObject").GetComponent<GameManager>();

        hireButton = m_Root.Q<Button>("HireButton");

        hireButton.clicked += Hire;
        CreateMercenaryList();

        ShowMercenaryListInfo();

        questInfoLabel = m_Screen.Q<Label>("Quest_Info_Label");
        questButtonContainer = m_Screen.Q<VisualElement>("Quest_Button_Container");
        quest = gameManager.questSO;
        SetQuest();
    }

    void Hire()
    {
        if(currentMercenaryIndex != -1)
        {
            // if(gameManager.money >= money)
            Debug.Log(currentMercenaryIndex);
            //CharacterSO characterSO = new CharacterSO((mercenaryList[currentMercenaryIndex].GetComponent<CharacterStats>()));
            CharacterSO characterSO = ScriptableObject.CreateInstance<CharacterSO>();
            characterSO.SetCharacterSO((mercenaryList[currentMercenaryIndex].GetComponent<CharacterStats>()));
            characterSO.characterVisualsPrefab = prefabList[(int)characterSO.characterClass];
            gameManager.GMcharacterList.Add(characterSO);
            mercenaryList[currentMercenaryIndex] = null;
            //m_Root.Q<VisualElement>("Mercenary"+currentMercenaryIndex).style.display = DisplayStyle.None;
            //m_Root.Q<VisualElement>("Mercenary"+currentMercenaryIndex).style.visibility = Visibility.Hidden;
            mercenaryButtons[currentMercenaryIndex].style.visibility = Visibility.Hidden;
        }
    }

#region CreateMercenaryList
        private void CreateMercenaryList()
        {
            for(int i=0; i<4; i++)
            {
                mercenaryList.Add(CreateChracterGO(i));
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
        void ShowMercenaryListInfo()
        {
            for(int i=0; i<4; i++)
            {
                mercenaryButtons.Add(m_Root.Q<Button>(mercernayButtonID + i));
                VisualElement ve = m_Root.Q<VisualElement>("Mercenary" + i);
                //ve.Q<Label>("MercenaryInfoLabel").text =
                //    $"initHP = {go.GetComponent<CharacterStats>().initHp}";
                //if (m_Root != null) m_Root = m_Document.rootVisualElement;
                CharacterStats stats = mercenaryList[i].GetComponent<CharacterStats>();
                ve.Q<VisualElement>("MercenarySpriteContainer").style.backgroundImage = new StyleBackground(stats._data.visual);
                // later, allocate the gold that needs to hire the mercenary
                int rnd = Random.Range(500, 1000); // for testing,
                ve.Q<Label>("MercenaryInfoLabel").text = $"price : {rnd} gold"
                        + "\n" + $"initHP : {stats.initHp}    class : {stats.characterClass}";
                        //+ "\n" + ;
                //ve.Q<Button>("MercenaryButton").text = "BUtton!!";
                mercenaryButtons[i].RegisterCallback<ClickEvent>(MarkUp);
            }
        }
        
        void MarkUp(ClickEvent cvt)
        {
            Button targetButton = cvt.target as Button;
            //int pastMercenaryIndex = currentMercenaryIndex;
            if(currentMercenaryIndex != -1)
            {
                mercenaryButtons[currentMercenaryIndex].RemoveFromClassList(mercenaryButtonActiveID);
                mercenaryButtons[currentMercenaryIndex].AddToClassList(mercenaryButtonDeactiveID);
            }
            currentMercenaryIndex = targetButton.name[^1] - '0';
            if (currentMercenaryIndex == -1 || mercenaryList[currentMercenaryIndex] == null)
            {
                Debug.LogWarning("No mercernay choosen");
                return;
            }

            //if(pastMercenaryIndex != -1)
            //{
            //    mercenaryButtons[pastMercenaryIndex].RemoveFromClassList(mercenaryButtonActiveID);
            //    mercenaryButtons[pastMercenaryIndex].AddToClassList(mercenaryButtonDeactiveID);
            //}
            mercenaryButtons[currentMercenaryIndex].RemoveFromClassList(mercenaryButtonDeactiveID);
            mercenaryButtons[currentMercenaryIndex].AddToClassList(mercenaryButtonActiveID);
        }
#endregion


        void SetQuest()
        {
            /// quest Info
            questInfoLabel.text = $"Clear the {quest.questName} Dungeon!!";

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
            targetButton.RemoveFromClassList(questButtonBlurUSS);
            targetButton.text = "ACCEPT";
            quest.state = questState.NOTACCEPT;
            Number n = Number.FIRST;
            n += quest.questNumber;
            quest.questNumber += 1;
            quest.questName = n.ToString();
            gameManager.gameDataSO.dungeonNumber += 1;
            questInfoLabel.text = $"Clear the {quest.questName} Dungeon!!";
            targetButton.UnregisterCallback<ClickEvent>(CompleteQuest);
            targetButton.RegisterCallback<ClickEvent>(AcceptQuest);
        }
    } 
}
