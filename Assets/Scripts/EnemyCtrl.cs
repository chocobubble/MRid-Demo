using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo{
public abstract class EnemyCtrl : MonoBehaviour
{
	public CharacterStats _stats;

	[SerializeField]
	protected GameSceneCtrl gameSceneCtrl;

	[SerializeField]
	protected GameManager gameManager;

	public int allyListNumber;
    /*
    int skillOneCooltime = 10 * 30;
    int skillTwoCooltime = 5 * 30;
    int skillThreeCooltime = 20 * 30;
    int skillFourCooltime = 80 * 30;
	*/
    public GameObject _attackTarget;
	

/*
	public List<GameObject> skillGOs;
    public bool isFirstSkillOn = false;
	public int firstSkillCoolTime = 10;
    public bool isSecondSkillOn = false;
	public int secondSkillCoolTime = 20;
	//public GameObject secondSkillArea;
    public bool isThirdSkillOn = false;
	public int thirdSkillCoolTime = 30;
	//public GameObject thirdSkillArea;
    public bool isFourthSkillOn = false;
	public int fourthSkillCoolTime = 40;
*/
    protected bool isBaseAttackOn = true;

    bool swichOn = true;
    public GameObject _gob;
    // 나중에 배열로 만들자?
    //public GameObject ally;
	public List<GameObject> allyList = new List<GameObject>();

		// 공격이 가능한지.. 캐스팅 모션등으로..
		// bool canAttack = true;


    protected virtual void Awake()
	{
		_stats = this.GetComponent<CharacterStats>();

		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		if(gameManager == null) Debug.LogWarning("gameManager refers null");
		gameSceneCtrl = GameObject.Find("GameSceneCtrl").GetComponent<GameSceneCtrl>();
		if(gameSceneCtrl == null) Debug.LogWarning("gameSceneCtrl refers null");
		/*
		// make skill GOs and add to list..
		for(int i=0; i<5; i++)
		{
			skillGOs.Add(Instantiate(gmr));
			skillGOs[i].SetActive(false);
		}        
		*/
	    Collider2D[] ce = CheckEnemy(this.transform.position);
		StartCoroutine(SetStartCool());
		//gmr = GameObject.Find("GameMgr");
    }

	void Start()
	{
		allyList = gameSceneCtrl.allyList;
		allyListNumber = allyList.Count;
		_stats.currHp = _stats.initHp;
		//SetEnemyStats(gameManager.dungeonLevel);
	}

	protected abstract IEnumerator SetStartCool();
	/*
	{
		/*
	    yield return new WaitForSeconds(5);
	    isSecondSkillOn = true;
        Debug.Log("second skill on");
	    yield return new WaitForSeconds(5);
	    isFirstSkillOn = true;
        Debug.Log("first skill on");
        yield return new WaitForSeconds(5);
		isThirdSkillOn = true;
        Debug.Log("third skill on");
		yield return new WaitForSeconds(5);
		isFourthSkillOn = true;
        Debug.Log("fourth skill on");
		
	}
	*/


    IEnumerator Judge() {
        //Debug.Log("Judge_Enemy");
        swichOn = false;
	    yield return new WaitForSeconds(_stats.responseSpeed); // 얘 공격속도
		if(_attackTarget)
		{
			Attack();  
		}
		else
		{
			//allyList = GameManager.instance.allyList;
	        //Collider2D[] ce = CheckEnemy(transform.position);  // 이거말고 ClosestPoint?
	    	if(allyList.Count != 0)
			{
	            //Debug.Log("ce is true_Enemy");
			    _attackTarget = ChooseTarget(allyList);
				Attack();
			}
			else
			{
				Debug.LogWarning("There is no ally to attack");
			    // 전투 종료
			}
	    }
	    swichOn = true;
    }

    protected virtual IEnumerator BaseAttack(){
        //Debug.Log("BaseAtk_Enemy");
    	//_attackTarget?.hp -= 1;
        //_attackTarget.GetComponent<AllyCtrl>().changeHp(-1);
    	isBaseAttackOn = false;
		gameSceneCtrl.CharacterHpChange(_attackTarget, gameObject,-_stats.baseAttackDamage, this.name);
    	yield return new WaitForSeconds(_stats.baseAttackCooltime);
    	isBaseAttackOn = true;
    }

	protected virtual void Attack(){
		if(_attackTarget == null)
		{
			Debug.Log("No Atk Target");
			return;
		} 
		/*
		if(isFourthSkillOn) {
			StartCoroutine(FourthSkill());
		} else if(isThirdSkillOn) {
			StartCoroutine(ThirdSkill());
		} else if(isSecondSkillOn) {
			StartCoroutine(SecondSkill());
		} else if(isFirstSkillOn) {
			StartCoroutine(FirstSkill());
		} else if(isBaseAttackOn) {
			StartCoroutine(BaseAttack());
		}
		*/
    }
    Collider2D[] CheckEnemy(Vector2 pos) {
    	// 주변의 적 추출
    	Collider2D[] colls = Physics2D.OverlapCircleAll(pos, 10.0f, 1<<7);//시야, 조건?없어도,,);
    	return colls;
        
    }   

    GameObject ChooseTarget(List<GameObject> goList) 
	{ // Later, choose attack target by considering attack distance of enemy
		float m = Mathf.Infinity;
		int j = 0;
		for(int i=0; i<allyList.Count; i++)
		{
			if (allyList[i].activeSelf == false) continue;
			float l = (transform.position - allyList[i].transform.position).sqrMagnitude; //.magnitude;
			if(m > l)
			{
				j = i;
				m = l;
			}
		}
		Debug.Log($"Enemy's target is {goList[j]}");
        return goList[j];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, gameObject.GetComponent<CircleCollider2D>().radius);
    }
    void Update()
	{
    	//if(_stats.currHp > 0) {   // 나중에 수정하기
			if(swichOn) {
				StartCoroutine(Judge());
			}
       // } else{
			//gameObject.SetActive(false);
			//gameManager.JudgeEndOrNot();
			
            //Debug.Log("Dead");
            //SetActive(false);
    		//StopAllCoroutine();
		    //GameManager.state = battleState.END;
    }
        /*
    	switch(_state){
	    //case behaveState.WAIT:
	     //   StartCoroutine(Judge());
	    //    break;           
	        case behaveState.ATTACK:
	    	    Attack();
	            break;		
                
			case behaveState.MOVE:
			    StartCoroutine(Move(targetPos));
	            break;
                
		}
        */
    }    
}
