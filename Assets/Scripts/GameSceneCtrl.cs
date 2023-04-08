using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace MRidDemo{
public class GameSceneCtrl : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;

    GameObject mainCamera;

    [SerializeField]
    GameObject warrior;
    [SerializeField]
    GameObject healer;
    [SerializeField]
    GameObject hunter;

    CharacterSO characterData; 

    //string warriorID = "paladin";

    public List<CharacterSO> allyDataList = new List<CharacterSO>();

    public List<GameObject> allyList = new List<GameObject>();
    public List<GameObject> enemyList = new List<GameObject>();

    [SerializeField]
    UIDocument m_Document;
    [SerializeField]
    VisualElement m_Root;
    public GameObject _ui;
    GameSceneUIManager gameSceneUIManager;
    List<VisualElement> healthBarlist = new List<VisualElement>();
    void OnEnable()
    {
        if (gameManager == null) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }
    void Start()
    {
        Debug.Log("GameSceneCtrl start");
        mainCamera = GameObject.Find("Main Camera");
        if(mainCamera == null) Debug.LogWarning("maincamera refers null");

        // later, change to fightingMebers
        //allyDataList = gameManager.fightingMembers;
        /*
        foreach(CharacterSO c in gameManager.GMcharacterList)
        {
            if(c != null) 
            {
                allyDataList.Add(c);
            }
        }
        */


        if(gameManager.fightingMembers.Count == 0) Debug.Log("error");
        SpawnEnemyCharacter();
        
        enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("ENEMY"));
        allyDataList = gameManager.fightingMembers;
        /*
        foreach(CharacterSO ally in allyDataList)
        {
            if (ally.characterClass == CharacterClass.Paladin)
            {
                Debug.Log("Spawn ally character");
                StartCoroutine(SpawnAllyCharacter(ally, warrior));
            }
            else if (ally.characterClass == CharacterClass.Priest)
            {
                Debug.Log("Spawn ally Priest");
                StartCoroutine(SpawnAllyCharacter(ally, healer));
            }
            else if (ally.characterClass == CharacterClass.Hunter)
            {
                Debug.Log("Spawn ally hunter");
                StartCoroutine(SpawnAllyCharacter(ally, hunter));
            }
            else Debug.Log("SOmething wrong");
        }
        */


        if(m_Document == null) Debug.LogWarning("There is no m_Document");
        else m_Root = m_Document.rootVisualElement;

        if(gameSceneUIManager == null)
        {
            gameSceneUIManager = _ui.GetComponent<GameSceneUIManager>();
        }
        else Debug.Log("There is a gamesceneuimanager");
        healthBarlist = gameSceneUIManager.healthBars;
/*
        for(int i=0; i<allyList.Count; i++) 
        {
            //Debug.Log(healthBarlist[i] + " setting");
            healthBarlist[i].name = allyList[i].GetComponent<CharacterStats>().characterName + "_HpBar";
            Debug.Log($"{healthBarlist[i].name}");
            healthBarlist[i].Q<Label>("txt_Loading").text = allyList[i].GetComponent<CharacterStats>().characterName;
            healthBarlist[i].Q<Label>("txt_Percentage").text = $"100%";
            healthBarlist[i].Q<VisualElement>("bar_Progress").style.width = Length.Percent(90); 
        }
*/

       StartCoroutine( SettingUnits());
    }

    void SpawnEnemyCharacter()
    {
        foreach(GameObject go in gameManager.levelSO.mainEnemies)
        {
            GameObject enemy = Instantiate(go);
            enemy.transform.position = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
            //Instantiate(go, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0), Quaternion.identity);
            setEnemyStats(enemy);
        }
    }

    void setEnemyStats(GameObject go)
    {
        CharacterStats goStats = go.GetComponent<CharacterStats>();
        int hardness = gameManager.dungeonLevel;
        //goStats.responseSpeed = goStats._data.responseSpeed * 
        goStats.baseAttackDamage = goStats._data.baseAttackDamage * hardness;
        goStats.baseAttackCooltime = goStats._data.baseAttackCooltime - (0.1f * hardness);
        goStats.initHp = goStats._data.initHp * hardness;
        goStats.defense = goStats._data.defense * hardness;
        goStats.skillAttackDamage = goStats._data.skillAttackDamage * hardness;
    }

    void SetUI()
    {
        Debug.Log("SetUI");
        for(int i=0; i<allyList.Count; i++) 
        {
            Debug.Log(healthBarlist[i] + " setting");
            healthBarlist[i].style.display = DisplayStyle.Flex;
            healthBarlist[i].name = allyList[i].GetComponent<CharacterStats>().characterName + "_HpBar";
            //Debug.Log($"{healthBarlist[i].name}");
            healthBarlist[i].Q<Label>("txt_Loading").text = allyList[i].GetComponent<CharacterStats>().characterName;
            healthBarlist[i].Q<Label>("txt_Percentage").text = $"100%";
            healthBarlist[i].Q<VisualElement>("bar_Progress").style.width = Length.Percent(90); 
        }
        if(allyList.Count == 0) Debug.Log("allyList is empty");
    }
    /*
    void SpawnAllyCharacter()
    {
        foreach(charcso _ally in gameManager.fightingMembers)
        {
            if (_ally != null){
            CharacterStats _stats = _ally.GetComponent<CharacterStats>();
            ApplyEquipment(_stats);
            allyList.Add(_ally);
            System.Random rnd = new System.Random();
            float ranX = (float) rnd.NextDouble() * 5;
            float ranY = (float) rnd.NextDouble() * 5;
            _ally.transform.position = new Vector2(ranX, ranY);
            _ally.SetActive(true);
            Debug.Log("complete spawn");
            }
            else 
            { Debug.Log("there is no ally");}
        }
    }
*/

    void SpawnAllyCharacter(CharacterSO data,GameObject prefab)
    {    
        GameObject _ally = Instantiate<GameObject>(prefab);
        CharacterStats _stats = _ally.GetComponent<CharacterStats>();
        //AllyCtrl allyCtrl = _ally.GetComponent<AllyCtrl>();
        _ally.name = data.characterName;
        _stats._data = data;
        _stats.CharacterSetup();
        ApplyEquipment(_stats);
        //allyCtrl.gameManager = gameManager;
        //_ally.AllyCtrl?.data = data;
        //gameManager.SettingUnits();
        //gameManager.allyDataList.Add(_ally);
        allyList.Add(_ally);
        System.Random rnd = new System.Random();
        float ranX = (float) rnd.NextDouble() * 5;
        float ranY = (float) rnd.NextDouble() * 5;
        // _ally.transform.position = new Vector2(ranX, ranY);
        _ally.transform.position = new Vector2(ranX + 8, ranY + 8);
    }

    void ApplyEquipment(CharacterStats cs)
    {
        if(cs.weapon != null)
        {
            cs.baseAttackDamage += cs.weapon.points;
            cs.skillAttackDamage += cs.weapon.points;
        }
        if(cs.armor != null)
        {
            cs.defense += cs.armor.points;
        }
    }

    public void CharacterHpChange(GameObject go, int chp, string name)
    {
        Debug.Log(name + " damaged " + chp);
        //AllyCtrl _allyCtrl = go.GetComponent<AllyCtrl>();
        CharacterStats _stats = go.GetComponent<CharacterStats>();
        if (chp >= 0)
        {
            _stats.currHp += chp;
            if (_stats.currHp > _stats.initHp)
            {
                _stats.currHp =  _stats.initHp;
            }
        }
        else 
        {
            chp += _stats.defense; // later, add vulnerable
            if (chp < 0)
            {
                _stats.currHp += chp;
                if(_stats.currHp < 0)
                {
                    _stats.currHp = 0;
                    Debug.Log($"{_stats.characterName} is dead");
                    //gameManager.JudgeEndOrNot();
                    if (go.tag == "ENEMY")
                    {
                        Debug.LogWarning("dead one is enemy");
                        go.GetComponent<EnemyCtrl>().StopAllCoroutines();
                        go.SetActive(false);
                        //JudgeEndOrNot();
                        if(gameManager.isDungeonEnded == false) JudgeEndOrNot();
                        else Debug.LogWarning("whi.... wrong");
                    }
                    else
                    {
                        Debug.Log("not end yet");
                    //go.SetActive(false);
                    }
                }
            }
        } 
        CharacterHpUIChange(_stats, _stats.currHp);
    }

    void CharacterHpUIChange(CharacterStats _stats, int _currHp)
    {
        //Debug.Log("before change Hpbar");
        //Debug.Log(_stats.characterName + "_HpBar");
        VisualElement ve = m_Root.Q<VisualElement>(_stats.characterName + "_HpBar");
        
        //float f = ((float)_currHp / _stats.initHp) * 100;
        int h = (int)(((float)_currHp / _stats.initHp) * 100);
        ve.Q<Label>("txt_Percentage").text = $"{h, 2}%";
        ve.Q<VisualElement>("bar_Progress").style.width = Length.Percent(90 * h / 100);
        //Debug.Log("after hp change");
    }

    IEnumerator SettingUnits()
    {
        yield return new WaitForSeconds(1);

        foreach(CharacterSO ally in allyDataList)
        {
            if (ally == null) {}
            else if (ally.characterClass == CharacterClass.Paladin)
            {
                Debug.Log("Spawn ally character");
                SpawnAllyCharacter(ally, warrior);
            }
            else if (ally.characterClass == CharacterClass.Priest)
            {
                Debug.Log("Spawn ally Priest");
                SpawnAllyCharacter(ally, healer);
            }
            else if (ally.characterClass == CharacterClass.Hunter)
            {
                Debug.Log("Spawn ally hunter");
                SpawnAllyCharacter(ally, hunter);
            }
            else Debug.Log("SOmething wrong");
        }

        //enemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("ENEMY"));
        SetUI();
        /*
        foreach(GameObject e in enemies)
        {
            gameManager.enemyList.Add(e);
        }
        /*
        foreach(GameObject e in enemyList)
        {
            gameManager.enemyList.Add(e);
        }
        */
    }

    void JudgeEndOrNot()
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
                gameManager.isDungeonEnded = true;
                // later, add all stop and .. wait for seconds
                StartCoroutine(EndDungeon());
                //SceneManager.LoadScene("MainScene");
            }
    }
    

        IEnumerator EndDungeon()
        {
            // isDungeonEnded = true; 

            foreach(GameObject fm in allyList)
            {
                if(fm != null) 
                {
                    fm.GetComponent<AllyCtrl>().StopAllCoroutines();
                    fm.SetActive(false);
                    //yield return new WaitForSeconds(2);
                    //Debug.Log(fm.name);
                    //Destroy(fm);
                }
            }
            gameManager.fightingMembers = new List<CharacterSO>();
            yield return new WaitForSeconds(2);
            gameManager.BackToMainScreen();
            //SceneManager.LoadScene("MainScene");
        }
}
}
