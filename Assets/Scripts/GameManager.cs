using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace MRidDemo
{
    public class GameManager : MonoBehaviour
    {
        /// For Testing
        public List<EquipmentSO> defaultItems = new List<EquipmentSO>();
        /// For Testing end
        public QuestSO questSO;
        public GameDataSO gameDataSO;
        public bool Onskill = false;
        public bool isDungeonEnded = false;
        public bool isFailed = true;
        public static GameManager instance = null;
        //public GameObject[] enemies;
        //public GameObject[] allies;

        public List<GameObject> allyList = new List<GameObject>();
        //public List<GameObject> enemyList = new List<GameObject>();
        //public List<GameObject> roster = new List<GameObject>();
        //public List<CharacterSO> roster = new List<CharacterSO>();

        // Roster of the game
        public List<CharacterSO> GMcharacterList = new List<CharacterSO>();
        // the members who will fight in a dungeon.
        public List<CharacterSO> fightingMembers = new List<CharacterSO>();
        //public List<GameObject> fightingMembers = new List<GameObject>();

        // Equipment that purchased or earned in the fight. except equiped.
        public List<EquipmentSO> GMEquipmentList = new List<EquipmentSO>();
        // equiped equipment
        //public List<EquipmentSO> GMEquipedEquipmentList = new List<EquipmentSO>();

        public LevelSO levelSO;
        public int dungeonLevel;
        public int money;
        public int date = 150;

        void Awake()
        {
            Debug.Log("Game manager starts.");
            // when the instance is not allocated
            if (instance == null)
            {
                instance = this;
            }
            // instance에 할당된 클래스의 인스턴스가 다를 경우 새로 생성된 클래스를 의미함
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }

            // 다른 씬으로 넘어가더라도 삭제하지 않고 유지함
            DontDestroyOnLoad(this.gameObject);
        }

        void Start()
        {
            money = 500;

            string[] allPaths = Directory.GetFiles("Assets/Resources/GameData/Characters", "*.asset", SearchOption.AllDirectories);
            
            //string[] equipmentPaths = Directory.GetFiles("Assets/Resources/GameData/EquipmentData", "*.asset", SearchOption.AllDirectories);

            // later, extract to methods 
            foreach (string path in allPaths)
            {
                //string cleanedPath = path;//path.Replace("\\", "/");
                //m_ItemDatabase.Add((Item)AssetDatabase.LoadAssetAtPath(cleanedPath, typeof(Item)));

                //Debug.Log(path);
                GMcharacterList.Add((CharacterSO)AssetDatabase.LoadAssetAtPath(path, typeof(CharacterSO)));
            }


            /*
            foreach (string path in equipmentPaths)
            {
                GMEquipmentList.Add((EquipmentSO)AssetDatabase.LoadAssetAtPath(path, typeof(EquipmentSO)));
            }
            */
            Testing();
            SetGameData();

        }
        void Testing()
        {
            foreach (CharacterSO c in GMcharacterList)
            {
                Equip(c);
            }
        }
        void Equip(CharacterSO c)
        {

            EquipmentSO weapon = ScriptableObject.CreateInstance<EquipmentSO>();
            int rnd = Random.Range(0,3);
            //EquipData e = new EquipData(defaultItems[rnd]);
            SetEquipmentData(weapon, defaultItems[rnd]);
            weapon.isEquiped = true;
            c.defaultWeapon = weapon;
            GMEquipmentList.Add(c.defaultWeapon);
            rnd = Random.Range(3, 6);
            EquipmentSO armor = ScriptableObject.CreateInstance<EquipmentSO>();
            SetEquipmentData(armor, defaultItems[rnd]);
            armor.isEquiped = true;
            c.defaultHelmet = armor;
            GMEquipmentList.Add(c.defaultHelmet);

        }
        void SetEquipmentData(EquipmentSO _equip, EquipmentSO _default)
    {
        _equip.equipmentName = _default.equipmentName;
        _equip.equipmentType = _default.equipmentType;
        _equip.rarity = _default.rarity;
        _equip.points = _default.points;
        _equip.sprite = _default.sprite;
    }

    public void SetGameData()
    {
        gameDataSO.dungeonNumber= 1;
        for (int i=0; i<gameDataSO.dungeonLevels.Count; i++)
        {
            gameDataSO.dungeonLevels[i] = 1;
        }
        questSO.questName = "FIRST Dungeon";
        questSO.questNumber = 1;
        questSO.state = questState.NOTACCEPT;
        
    }
        //public GameObject MaxHpMaxCurrHp(){}
        /*
        public void SettingUnits()
        {
            //GameObject[] enemies = GameObject.FindGameObjectsWithTag("ENEMY");
            //enemyList = new List<GameObject>(enemies);
            enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("ENEMY"));

            //enemies = GameObject.FindGameObjectsWithTag("ENEMY");
            //allies = GameObject.FindGameObjectsWithTag("ALLY");
        }
        */

/*
        public void JudgeEndOrNot()
        {
            if ( enemyList.Count == 0)
            {
                Debug.Log("All enemies are dead");

                if (SceneManager.GetActiveScene().name == "Game")
                {
                    //SceneManager.LoadScene("MainScene");
                }
            }
            else
            {
                foreach (GameObject enemy in enemyList)
                {
                    if (enemy.GetComponent<CharacterStats>().currHp > 0)
                    {
                        Debug.Log("equal or more than one enemy alive");
                        return;
                    }
                }

                Debug.Log("All enemies are dead");
                if (SceneManager.GetActiveScene().name == "Game")
                {
                    isDungeonEnded = true;
                    // later, add all stop and .. wait for seconds
                    StartCoroutine(EndDungeon());
                    //SceneManager.LoadScene("MainScene");
                }
            }
        }
        */

        /*
        IEnumerator EndDungeon()
        {
            // isDungeonEnded = true; 

            foreach(GameObject fm in fightingMembers)
            {
                if(fm != null) 
                {
                    fm.SetActive(false);
                    yield return new WaitForSeconds(2);
                    Debug.Log(fm.name);
                    Destroy(fm);
                }
            }
            fightingMembers[0].SetActive(false);
            GetOld();
            SceneManager.LoadScene("MainScene");
        }

        void GetOld()
        {
            foreach (GameObject character in roster)
            {
                character.GetComponent<CharacterStats>().age += 1;
                date += 1;
                GameObject.Find("MainScreen").GetComponent<MainScreen>().SettingDate();
                Debug.Log("new day begins");
            }
        }
        */
/*
        public void EndDungeon()
    {
        foreach(GameObject ally in allyList)
        {
            ally.SetActive(false);
        }
        date += 1;
        

        isDungeonEnded = true;

        SceneManager.LoadScene("MainScene");
    }
*/
    public void UpdateDungeonLevel()
    {
        if(gameDataSO.dungeonNumber == levelSO.dungeonNumber)
        {
            if(gameDataSO.dungeonLevels[gameDataSO.dungeonNumber - 1] == 5) return;

            if(gameDataSO.dungeonLevels[gameDataSO.dungeonNumber - 1] == dungeonLevel)
            {
                gameDataSO.dungeonLevels[gameDataSO.dungeonNumber - 1] += 1;
            }
        }
    }
    public void CheckQuest()
    { // check whether clear the highest number of the dungeons
        if (questSO.state == questState.ACCEPT && gameDataSO.dungeonNumber == levelSO.dungeonNumber)
        {
            if(questSO != null)
            {
                //questSO.isSuccess = true;
                questSO.state = questState.SUCCESS;
            }
            else
            {
                Debug.LogWarning("There is no quest");
            }
        }
    }

    public void BackToMainScreen()
    {
        AfterFightInBattle();
        NextWeek();
        fightingMembers = new List<CharacterSO>();
        SceneManager.LoadScene("MainScene");
        //GameObject.Find("MainScreen").GetComponent<MainScreen>().SettingDate();
        Debug.Log("new day begins");
    }


        void AfterFightInBattle()
        {
            foreach (CharacterSO character in fightingMembers)
            {
                if(character != null)
                {
                    IncreaseStats(character);
                    //GetOlder(character);
                    //GameObject.Find("MainScreen").GetComponent<MainScreen>().SettingDate();
                    //Debug.Log("new day begins");
                }
                else Debug.Log("There is no character!!");
            }
        }
        public void IncreaseStats(CharacterSO characterSO)
        {
                    if (isFailed == true)
                    {
                        characterSO.fatigue += 40;
                        characterSO.morale -= 2;
                        if((int)characterSO.morale < -2) characterSO.morale = Morale.VERYBAD;
                    }
                    else
                    {
                        characterSO.fatigue += 25;
                        characterSO.morale += 1;
                        if((int)characterSO.morale > 2) characterSO.morale = Morale.VERYGOOD;
                    }
        }
        void GetOlder(CharacterSO characterSO)
        {
            characterSO.age += 1;
        }
        void NextWeek()
        {
            date += 1;
            foreach(CharacterSO c in GMcharacterList)
            {
                GetOlder(c);
                c.fatigue -= 10;
            }
        }
    }
}