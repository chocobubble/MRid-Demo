using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo{
public class EnemyCtrl : MonoBehaviour
{
	public CharacterStats _stats;

	[SerializeField]
	GameObject gameSceneCtrlGO;

	[SerializeField]
	GameSceneCtrl gameSceneCtrl;

	[SerializeField]
	GameManager gameManager;

	public int attackSpeed = 1;
	public int allyListNumber;
    /*
    int skillOneCooltime = 10 * 30;
    int skillTwoCooltime = 5 * 30;
    int skillThreeCooltime = 20 * 30;
    int skillFourCooltime = 80 * 30;
		*/
    public GameObject _attackTarget;
	public GameObject gmr; 

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
    bool isBaseAttackOn = true;

    bool swichOn = true;
    public GameObject _gob;
    // 나중에 배열로 만들자?
    //public GameObject ally;
	public List<GameObject> allyList = new List<GameObject>();

		// 공격이 가능한지.. 캐스팅 모션등으로..
		// bool canAttack = true;


    void Start()
	{
		_stats = this.GetComponent<CharacterStats>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gameSceneCtrl = gameSceneCtrlGO.GetComponent<GameSceneCtrl>();
		allyList = gameSceneCtrl.allyList;
		allyListNumber = allyList.Count;
	
		_stats.currHp = _stats.initHp;
		// make skill GOs and add to list..
		for(int i=0; i<5; i++)
		{
			skillGOs.Add(Instantiate(gmr));
			skillGOs[i].SetActive(false);
		}        
	    Collider2D[] ce = CheckEnemy(this.transform.position);
		StartCoroutine(SetStartCool());
		//gmr = GameObject.Find("GameMgr");
    }

	IEnumerator SetStartCool(){
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

    IEnumerator Judge() {
        //Debug.Log("Judge_Enemy");
        swichOn = false;
	    yield return new WaitForSeconds(attackSpeed); // 얘 공격속도
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

    IEnumerator FirstSkill(){ // 반반 쓸기
    	// 시전
    	isFirstSkillOn = false;
		//int rand = Random.Range(1,2);
        int rand = Random.Range(0,allyListNumber);
		StartCoroutine(FirstSkillRange(rand));
    	yield return new WaitForSeconds(30);
    	isFirstSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯
	
	IEnumerator FirstSkillRange(int r){ // 장판 생성
        Debug.Log("FirstSkill_Enemy");
		//GameObject _gob = GameObject.Find($"FirstSkillArea_{r}");
        //에디터에서 지정

        skillGOs[0].SetActive(true);
        skillGOs[0].transform.position = _attackTarget.transform.position;
		foreach(var ally in allyList){
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
		yield return new WaitForSeconds(10);
		/*
		foreach(var ally in allys){
			ally.hp -= 50;
		}
        */	
        //ally.hp -= 50;
        //_attackTarget.GetComponent<AllyCtrl>().changeHp(-50);
		foreach (var ally in allyList) {
			if(Physics2D.IsTouching(skillGOs[0].GetComponent<Collider2D>(), ally.GetComponent<Collider2D>())){
				//ally.GetComponent<AllyCtrl>().changeHp(-50); //TakeDamage(50); // takedamage 함수 구현하자
				gameSceneCtrl.CharacterHpChange(ally, -50, this.name);
			}
		}
		skillGOs[0].SetActive(false);
	}

    IEnumerator SecondSkill(){ // 일직선 레이저
		Debug.Log("Second_Skill_Enemy");
    	// 시전
    	isSecondSkillOn = false;
			// 일직선, gameobject 만들어두자. 
		
	
			// 참조형 변수로 혹은 포인터로 나중에 바꾸자
			// 나중에 allys로..
		foreach(var ally in allyList){
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
		int _rand = Random.Range(0, allyList.Count-1);
		//Quaternion rot = Quaternion.LookRotation(allyList[_rand].transform.position
		//										- transform.position);
		GameObject _randAlly = allyList[_rand];
		/*
		GameObject laser = Instantiate(secondSkillArea, _randAlly.transform.position, Quaternion.identity);
		laser.transform.Rotate(transform.position - _randAlly.transform.position);
		*/
		GameObject laser = skillGOs[1];
		laser.transform.position = _randAlly.transform.position;
		laser.transform.Rotate(transform.position - _randAlly.transform.position);
		laser.SetActive(true);
		
		yield return new WaitForSeconds(5);
		foreach (var ally in allyList) {
			if(Physics2D.IsTouching(laser.GetComponent<Collider2D>(), ally.GetComponent<Collider2D>())){
				//ally.GetComponent<AllyCtrl>().changeHp(-50); //TakeDamage(50); // takedamage 함수 구현하자
				gameSceneCtrl.CharacterHpChange(ally, -50, this.name);
			}
		}
		laser.SetActive(false);
    	yield return new WaitForSeconds(30);
    	isSecondSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯

    IEnumerator ThirdSkill(){ // 랜덤 범위 공격
		Debug.Log("Third_Skill_Enemy");
		foreach(var ally in allyList){
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
    	// 시전
    	isThirdSkillOn = false;
		GameObject _circle = skillGOs[2];//
		int _x = Random.Range(-4, 4);
		int _y = Random.Range(-4, 4);
		_circle.transform.position = new Vector2(_x,_y);
		_circle.SetActive(true);
		yield return new WaitForSeconds(3);
		foreach(var ally in allyList) {
			if(Physics2D.IsTouching(_circle.GetComponent<Collider2D>(), ally.GetComponent<Collider2D>())){
				//ally.GetComponent<AllyCtrl>().changeHp(-50); //TakeDamage(50);
				gameSceneCtrl.CharacterHpChange(ally, -50, this.name);
			}
		}
		_circle.SetActive(false);
    	yield return new WaitForSeconds(60);
    	isThirdSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯

    IEnumerator FourthSkill(){ // 타겟 대상 범위 분산데미지공격
    	Debug.Log("Fourth_Skill_Enemy");
		foreach(var ally in allyList){
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
		// 시전
    	isFourthSkillOn = false;
		GameObject _areaObject = skillGOs[3];//
		_areaObject.SetActive(true);
		// _attackTarget에 달 방법을 찾아 보자.
		StartCoroutine(FourthSkillAttach(_areaObject));

    	yield return new WaitForSeconds(60);
    	isFourthSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯

	IEnumerator FourthSkillAttach(GameObject ao)
	{
		float t = 0;
		while(t < 10)
		{
			ao.transform.position = _attackTarget.transform.position;
			yield return new WaitForSeconds(0.1f);
			t += 0.1f;
		}
		Collider2D[] _collsFourth = Physics2D.OverlapCircleAll(ao.transform.position,1.5f, 1<<6);
			//int num = Physics2D.GetContacts(_areaObject, allys);//collider2d 로?
		int num = _collsFourth.Length;
		if (num != 0){
			int _damage = 100 / num ;
			foreach(var _coll in _collsFourth){
				//_coll.gameObject.GetComponent<AllyCtrl>().changeHp(-_damage); //TakeDamage(_damage);
			}
		}
		ao.SetActive(false);
	}

    IEnumerator BaseAttack(){
        //Debug.Log("BaseAtk_Enemy");
    	//_attackTarget?.hp -= 1;
        //_attackTarget.GetComponent<AllyCtrl>().changeHp(-1);
    	isBaseAttackOn = false;
		gameSceneCtrl.CharacterHpChange(_attackTarget, -1, this.name);
    	yield return new WaitForSeconds(1);
    	isBaseAttackOn = true;
    }

	void Attack(){
		if(_attackTarget == null) return;
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
    }
    Collider2D[] CheckEnemy(Vector2 pos) {
    	// 주변의 적 추출
    	Collider2D[] colls = Physics2D.OverlapCircleAll(pos, 100.0f, 1<<7);//시야, 조건?없어도,,);
    	return colls;
        
    }   
    GameObject ChooseTarget(List<GameObject> l) 
	{ // Later, choose attack target by considering attack distance of enemy
		float m = Mathf.Infinity;
		int j = 0;
		for(int i=0; i<allyList.Count; i++)
		{
			if(m > (transform.position - allyList[i].transform.position).magnitude)
			{
				j = i;
				m = (transform.position - allyList[i].transform.position).magnitude;
			}
		}
		Debug.Log($"Enemy's target is {l[j]}");
        return l[j];
    }

    void Update()
	{
    	if(_stats.currHp > 0) {   // 나중에 수정하기
			if(swichOn) {
				StartCoroutine(Judge());
			}
        } else{
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
	
	/*
	public void changeHp(int chp){
		if(chp < 0) {
			chp += defense;
			if (chp < 0) {
				currHp -= chp;
			}
		} else {
			currHp += chp;
		    if(currHp < -1111110){  // 나중에 수정
			    currHp = 0;
			    Debug.Log("Enemy_Dead");
			    gameObject.SetActive(false);
            }
		}
	}
	*/
}
}