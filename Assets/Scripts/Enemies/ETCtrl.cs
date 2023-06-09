using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{
public class ETCtrl : EnemyCtrl
{
	[SerializeField]
	GameObject[] skillGOs;
    public bool isFirstSkillOn = false;
	public int firstSkillCoolTime = 10;
    public bool isSecondSkillOn = false;
	public int secondSkillCoolTime = 20;
    public bool isThirdSkillOn = false;
	public int thirdSkillCoolTime = 30;
    public bool isFourthSkillOn = false;
	public int fourthSkillCoolTime = 40;

    protected override void Awake()
    {
        base.Awake();

		skillGOs = GameObject.FindGameObjectsWithTag("ENEMYSKILL");
		foreach(GameObject go in skillGOs)
		{
			go.SetActive(false);
		}
	}

    protected override IEnumerator SetStartCool()
    {
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

    protected override void Attack()
    {
        base.Attack();
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

    IEnumerator FirstSkill(){ // 반반 쓸기
    	// 시전
    	isFirstSkillOn = false;
		//int rand = Random.Range(1,2);
        int rand = Random.Range(0,allyListNumber);
		StartCoroutine(FirstSkillRange(rand));
    	yield return new WaitForSeconds(5);
    	isFirstSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯
	
	IEnumerator FirstSkillRange(int r){ // 장판 생성
        Debug.Log("FirstSkill_Enemy");

        skillGOs[0].gameObject.SetActive(true);
        skillGOs[0].transform.position = _attackTarget.transform.position;
		foreach(var ally in allyList){
			//StopCoroutine(ally.GetComponent<AllyCtrl>().Judge()); 83f9829af5c0c14e7bd7930d0c47c8e333bc38fa
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
		yield return new WaitForSeconds(firstSkillCoolTime - 5);

		foreach (var ally in allyList) {
			if(Physics2D.IsTouching(skillGOs[0].GetComponent<Collider2D>(), ally.GetComponent<Collider2D>())){
				//ally.GetComponent<AllyCtrl>().changeHp(-50); //TakeDamage(50); // takedamage 함수 구현하자
				gameSceneCtrl.CharacterHpChange(ally, gameObject,-50, this.name);
			}
		}
		skillGOs[0].gameObject.SetActive(false);
	}

    IEnumerator SecondSkill(){ // 일직선 레이저
		Debug.Log("Second_Skill_Enemy");
    	// 시전
    	isSecondSkillOn = false;
			// 일직선, gameobject 만들어두자. 
		
	
			// 참조형 변수로 혹은 포인터로 나중에 바꾸자
			// 나중에 allys로..
		foreach(var ally in allyList){
			//StopCoroutine(ally.GetComponent<AllyCtrl>().Judge());
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
		int _rand = Random.Range(0, allyList.Count);
		//Quaternion rot = Quaternion.LookRotation(allyList[_rand].transform.position
		//										- transform.position);
		GameObject _randAlly = allyList[_rand];
		/*
		GameObject laser = Instantiate(secondSkillArea, _randAlly.transform.position, Quaternion.identity);
		laser.transform.Rotate(transform.position - _randAlly.transform.position);
		*/
		GameObject laser = skillGOs[1].gameObject;
		laser.transform.position = _randAlly.transform.position;
		laser.transform.Rotate(transform.position - _randAlly.transform.position);
		laser.SetActive(true);
		
		yield return new WaitForSeconds(5);
		foreach (var ally in allyList) {
			if(Physics2D.IsTouching(laser.GetComponent<Collider2D>(), ally.GetComponent<Collider2D>())){
				Debug.Log(ally.name + " hit second skill");
				//ally.GetComponent<AllyCtrl>().changeHp(-50); //TakeDamage(50); // takedamage 함수 구현하자
				gameSceneCtrl.CharacterHpChange(ally, gameObject,-50, this.name);
			}
		}
		laser.SetActive(false);
    	yield return new WaitForSeconds(secondSkillCoolTime - 5);
    	isSecondSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯

    IEnumerator ThirdSkill(){ // 랜덤 범위 공격
		Debug.Log("Third_Skill_Enemy");
		foreach(var ally in allyList){
			//StopCoroutine(ally.GetComponent<AllyCtrl>().Judge());
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
    	// 시전
    	isThirdSkillOn = false;
		GameObject _circle = skillGOs[2].gameObject;//
		int rnd = Random.Range(0, 4);
		int _x = Random.Range(-4, 4);
		int _y = Random.Range(-4, 4);
		_circle.transform.position = (Vector2)allyList[rnd].transform.position + (new Vector2(_x,_y));
		_circle.SetActive(true);
		yield return new WaitForSeconds(3);
		foreach(var ally in allyList) {
			if(Physics2D.IsTouching(_circle.GetComponent<Collider2D>(), ally.GetComponent<Collider2D>())){
				Debug.Log(ally.name + " hit third skill");
				gameSceneCtrl.CharacterHpChange(ally, gameObject,-50, this.name);
			}
		}
		_circle.SetActive(false);
    	yield return new WaitForSeconds(thirdSkillCoolTime - 3);
    	isThirdSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯

    IEnumerator FourthSkill(){ // 타겟 대상 범위 분산데미지공격
    	Debug.Log("Fourth_Skill_Enemy");
		foreach(var ally in allyList){
			//StopCoroutine(ally.GetComponent<AllyCtrl>().Judge());
			ally.GetComponent<AllyCtrl>().eventOn = true;
		}
		// 시전
    	isFourthSkillOn = false;
		GameObject _areaObject = skillGOs[3].gameObject;//
		_areaObject.SetActive(true);
		// _attackTarget에 달 방법을 찾아 보자.
		StartCoroutine(FourthSkillAttach(_areaObject));

    	yield return new WaitForSeconds(fourthSkillCoolTime);
    	isFourthSkillOn = true;
    }  // if t여야 스킬 함수 호출하게..해야할듯

	IEnumerator FourthSkillAttach(GameObject ao)
	{
		//float t = 0;
		/*
		while(t < 10)
		{
			ao.transform.position = _attackTarget.transform.position;
			yield return new WaitForSeconds(0.1f);
			t += 0.1f;
		}
		*/
		GameObject go = _attackTarget;
		go.GetComponent<AllyCtrl>().canAct = false;
		yield return new WaitForSeconds(5);
		go.GetComponent<AllyCtrl>().canAct = true;
		Collider2D[] _collsFourth = Physics2D.OverlapCircleAll(ao.transform.position,1.5f, 1<<6);
			//int num = Physics2D.GetContacts(_areaObject, allys);//collider2d 로?
		int num = _collsFourth.Length;
		if (num != 0){
			int _damage = 100 / num ;
			/*
			foreach(var _coll in _collsFourth){
				Debug.Log(_coll.name + " hit fourth skill");
				//_coll.gameObject.GetComponent<AllyCtrl>().changeHp(-_damage); //TakeDamage(_damage);
				gameSceneCtrl.CharacterHpChange(_coll.gameObject, gameObject, -_damage, this.name);
			*/
			foreach(var ally in allyList) {
			if(Physics2D.IsTouching(ao.GetComponent<Collider2D>(), ally.GetComponent<Collider2D>())){
				Debug.Log(ally.name + " hit fourth skill");
				gameSceneCtrl.CharacterHpChange(ally, gameObject, -_damage, this.name);
			
		}
		ao.SetActive(false);
	}
}
	}
}
}
