using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace MRidDemo
{
    public class AllyCtrl : MonoBehaviour
    {
        [SerializeField]
        GameObject gameSceneCtrlGO;
        public GameSceneCtrl gameSceneCtrl;
        public CharacterStats _stats;
        public CharacterSO _data;
        
        // dictionary 로 나중에 바꾸기
        //List<Vector2> positionList = new List<Vector2>();

        [SerializeField]
        Pathfinding path;

        public List<Vector2> positions = new List<Vector2>();

        public List<Vector2> movePosList = new List<Vector2>();

        public GameManager gameManager;

        // bool isOrdered = false;
        public bool isSkillOn = true;
        // bool canAttack = false;
        public bool swichOn = true;
        public bool isBaseAttackOn = true;
        public Vector2 targetPos = new Vector2(-1, -1);
        public int mid;
        Vector2 startPos;

        Vector2 lastPos;

        public bool canAct = true;
        //
        public bool forceMoveOn = false;

        // When the event happend
        public bool eventOn = false;
        List<Vector2> savedPos;

        // moving by player's order
        bool moveOrderOn = false;

        public List<GameObject> enemyList;
        public GameObject attackTarget;
        public List<Vector2> l;
        public Queue<Vector2> q;

        public int currHp;
        int maxX;
        int maxY;

        //public int t=0;

        public enum behaveState { STAY, ATTACK, MOVE, DIE };
        enum priorityState { };
        enum possibleAttack { SKILL, BASE };

        GameObject[] enemies;
        priorityState _pState;
        public behaveState _state;
        public virtual void Awake()
        {
            
        }
        public virtual void Start()
        {
            gameSceneCtrlGO = GameObject.Find("GameSceneCtrl");
            gameSceneCtrl = gameSceneCtrlGO.GetComponent<GameSceneCtrl>();
            //if(gameSceneCtrl == null) gameSceneCtrl = GameObject.Find("GameSceneCtrl").GetComponent<GameSceneCtrl>();
            //else Debug.Log(gameSceneCtrl);

            _stats = this.GetComponent<CharacterStats>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            //uIManager = _UIManager.GetComponent<GameSceneUIManager>();
            float rnd = UnityEngine.Random.Range(0, 1);
            for (int i = 0; i < 8; i++)
            {
                positions.Add(new Vector2(Mathf.Cos(Mathf.PI * rnd + (Mathf.PI * i / 4.0f)), Mathf.Sin(Mathf.PI * rnd + (Mathf.PI * i / 4.0f))));
                Debug.Log(positions[i]);
            }

            maxX = GameObject.Find("Grid").GetComponent<GridMap>().length;
            maxY = GameObject.Find("Grid").GetComponent<GridMap>().height;


            enemyList = gameSceneCtrl.enemyList;
            // 나중에 수정하기
            //enemies = gameManager.enemies;
            //GameObject attackTarget = enemies[0];
            //GameObject attackTarget = GameObject.Find("Enemy");
            //		savedPos = // 어딘가 저장해 둔걸 가져오자

            //Collider[] ce = CheckEnemy(this.transform.position);// 나중에 지우기
            //attackTarget = chooseTarget(ce);

            path = GameObject.Find("GridManager").GetComponent<Pathfinding>();
        }

        IEnumerator Judge()
        {
            swichOn = false;
            yield return new WaitForSeconds(_stats.responseSpeed);

            RaycastHit2D hit = Physics2D.Raycast(transform.position,
                    Vector2.zero, 1.0f, (1 << 6));
            if (eventOn)
            {
                Debug.Log("eventOn");
                // eventOn = false;
                if (moveOrderOn)
                {
                    eventOn = false;
                    ForceMove();
                    //Debug.Log("ForceMovingEnd");
                    // ??? 나중에 아래꺼랑 합치던지 함수로 바꾸던지 해야하나..
                }
                else
                {
                    eventOn = false;
                    //Debug.Log("eventOff4");
                    // later, modify this. 
                    // it need to be renew when event happen, while forcemoveOn is true
                    if (forceMoveOn == true) {}
                    else if (hit.collider)
                    {
                        //Debug.Log("hit.collider.");
                        targetPos = FindEscapeLocation(transform.position);
                        //movePosList.Clear();
                        movePosList = path.FindPath((int)this.transform.position.x, (int)this.transform.position.y, (int)targetPos.x, (int)targetPos.y);
                        // why?? later, modify..
                        yield return new WaitForSeconds(0.1f);
                        StartCoroutine(ForceMove());
                        //Debug.Log("ForceMovingEnd");
                        //yield break; // ??
                    }
                }
            }
            // it needs to be changed like up..
            if (forceMoveOn == true) {}
            else if (hit.collider)
            {
                Debug.Log(this.name + "hit.collider.");
                targetPos = FindEscapeLocation(transform.position);
                //movePosList.Clear();
                movePosList = path.FindPath((int)this.transform.position.x, (int)this.transform.position.y, (int)targetPos.x, (int)targetPos.y);
                // why?? later, modify..
                yield return new WaitForSeconds(0.1f);
                StartCoroutine(ForceMove());
            }
            if (forceMoveOn)
            {
                swichOn = true;
                yield break; // ??
            }
            //Collider2D ce = CheckEnemy(transform.position);
            if (true)
            { // (ce) {
                //Debug.Log("ce is true");
                //state 하나 더? 공격?ㅇㅇㅇ
                //attackTarget = ChooseTarget(ce);

                // later modify to method.
                Debug.Log(this.name + "normal judge start");
                attackTarget = enemyList[0];
                float distance = (attackTarget.transform.position
                        - this.transform.position).magnitude
                        - attackTarget.GetComponent<CircleCollider2D>().radius;
                // 혹은 ColliderDistance2D Distance(Collider2D colliderA, Collider2D colliderB);
                //Debug.Log($"{distance}");

                //if (distance >= _stats.attackDistance-1&&distance <= _stats.attackDistance)
                if (distance <= _stats.attackDistance)
                {
                    _state = behaveState.ATTACK;
                }
                else
                {
                    //targetPos = (attackTarget.transform.position.normalized)*(distance-1.0f);
                    //targetPos = GetPositionToTarget(attackTarget);
                    _state = behaveState.MOVE;
                    //movePosList.Clear();
                    movePosList = GetPositionToTarget(attackTarget);
                    //targetPos = (attackTarget.transform.position);
                    Debug.Log(this.name + " MoveToAttack");
                }

            }
            swichOn = true;
        }
/*
        private Vector2 GetPositionToTarget(GameObject attackTarget)
        {
            Vector3 targetPosition = attackTarget.transform.position - this.transform.position;
            float distanceToTarget = targetPosition.magnitude;
            float targetRadius = attackTarget.GetComponent<CircleCollider2D>().radius;
            //float rand = _stats.attackDistance * UnityEngine.Random.Range(0, 1) + targetRadius;
            float rand = UnityEngine.Random.Range(0, 1);
            //Debug.Log(1 - (rand / distanceToTarget));
            Debug.DrawRay(this.transform.position, attackTarget.transform.position - (targetPosition.normalized * (targetRadius + _stats.attackDistance - 0.5f)  ), Color.green);
            //return transform.position + (targetPosition * (1 - (rand / distanceToTarget)));
            return attackTarget.transform.position - (targetPosition.normalized * (targetRadius + _stats.attackDistance - rand));//0.5f)  );
        }
*/
        private List<Vector2> GetPositionToTarget(GameObject attackTarget)
        {
            float targetRadius = attackTarget.GetComponent<CircleCollider2D>().radius;
            // later, modify distance beging changed randomly
            float distance = targetRadius + _stats.attackDistance;
        
            return path.FindAtkPath((int)this.transform.position.x, (int)this.transform.position.y,
                    (int)attackTarget.transform.position.x, (int)attackTarget.transform.position.y, distance);
        }

#region ATTACK_AND_SKILL
        void Attack()
        {
            if (isSkillOn)
            {
                //StartCoroutine(Skill());
                Skill();
            }
            else if (isBaseAttackOn)
            {
                StartCoroutine(BaseAttack());
            }
        }
/*
        public IEnumerator Skill()
        {
            Debug.Log("Parent_skill");
            isSkillOn = false;
            canAct = false;
            yield return new WaitForSeconds(1);
            canAct = true;
            //attackTarget.GetComponent<EnemyCtrl>().changeHp(-_stats.skillAttackDamage);
            gameSceneCtrl.CharacterHpChange(attackTarget, -_stats.skillAttackDamage);
            yield return new WaitForSeconds(10);
            isSkillOn = true;
        }
*/
        public void Skill()
        {
            switch(_stats.characterClass)
            {
                case CharacterClass.Paladin:
                    Debug.Log("Paladin Skill cast");
                    StartCoroutine(Taunt(gameObject, attackTarget));
                    break;
                case CharacterClass.Priest:
                    Debug.Log("Priest skill cast");
                    StartCoroutine(Healing(gameObject));
                    break;
                case CharacterClass.Hunter:
                    Debug.Log("Hunter skill cast");
                    StartCoroutine(StrongArrow(gameObject, attackTarget));
                    break;
            }

        }

        IEnumerator BaseAttack()
        {
            isBaseAttackOn = false;
            //attackTarget.GetComponent<EnemyCtrl>().changeHp(-_stats.baseAttackDamage);
            if(attackTarget == null)
            {
                Debug.Log($"no AtkTarget of {this.name}");

            }
            else
            {
                gameSceneCtrl.CharacterHpChange(attackTarget, -_stats.baseAttackDamage, this.name);
            }
            yield return new WaitForSeconds(_stats.baseAttackCooltime);
            isBaseAttackOn = true;
        }
        #endregion

        // 나중에 이동 판단 여부 좀 더 수정해부좌..
        void Move(Vector2 _targetPos)
        {
            Vector2 _thisPos = transform.position;
            RaycastHit2D hit = Physics2D.Raycast((_targetPos - _thisPos).normalized, Vector2.zero, 0.01f, 63);
            //Debug.DrawLine(_thisPos, _targetPos, Color.red);
            //Debug.DrawRay(_thisPos, _targetPos, Color.black);
            if (hit.collider == null || hit.collider == this.GetComponent<Collider2D>() || forceMoveOn)
            {
                transform.Translate((_targetPos - _thisPos).normalized * Time.deltaTime);
                //Debug.Log("Move");
            }
            else
            {
                Debug.Log(hit.transform.name);
            }
        }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stats.attackDistance);
    }

        void Update()
        {
            if(attackTarget != null && targetPos != null)
            {
                //Debug.DrawLine(this.transform.position, attackTarget.transform.position, Color.red);
                Debug.DrawLine(this.transform.position, targetPos, Color.yellow);
            }
            
            //Debug.Log("Update"); 
            if (canAct)
            {
                if (swichOn)
                {
                    StartCoroutine(Judge());
                }
                else
                {
                    switch (_state)
                    {
                        //case behaveState.WAIT:
                        //   StartCoroutine(Judge());
                        //    break;            
                        case behaveState.ATTACK:
                            Attack();
                            break;
                        case behaveState.MOVE:
                        /*
                            if (targetPos.x == -1)
                            {
                                _state = behaveState.STAY;
                                break;
                            }
                        */
                            StartCoroutine(Move());
                            transform.Translate((targetPos - (Vector2)this.transform.position).normalized * Time.deltaTime * _stats.speed * 0.1f);
                            //Move(targetPos);
                            /*
                            Vector2 thisPos = transform.position;
                            if (thisPos == targetPos)
                                _state = behaveState.ATTACK;
                            */
                            break;
                    }
                }
            }
        }
        IEnumerator Move()
        {
            _state = behaveState.MOVE;

            if(movePosList == null || movePosList.Count == 0)
            {
                targetPos = (Vector2)transform.position;
                yield break;
            }

            int t=0;

            targetPos = movePosList[t];

            int x=0;
            while (x++ <= 10)
            {
                if (Vector2.Distance(this.transform.position, targetPos) < 0.7f)
                {
                    t += 1;
                    //movePosList.RemoveAt(0);
                    if(movePosList.Count > t && t >= 0)
                    {
                        targetPos = movePosList[t];
                    } else {
                        _state = behaveState.ATTACK;
                        t = 0;
                        break;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        Vector2 FindEscapeLocation(Vector2 currPos)
        {
            Debug.Log("Escape");
            q = new Queue<Vector2>();
            l = new List<Vector2>();
            mid = 0;
            //System.Random rnd = new System.Random();
            for (int i = 7; i >= 0; i--)
            {
                //int n = rnd.Next(0, i + 1);
                int n = UnityEngine.Random.Range(0, i+1);
                l.Add(positions[n]);

                //Debug.Log(n + " " + positions[n]);
                var temp = positions[n];
                positions[n] = positions[i];
                positions[i] = temp;
            }
            Vector2 v;
            int _n = l.Count;
            for (int i = 0; i < l.Count; i++)
                //q.Enqueue(currPos);
                q.Enqueue(l[i]);
            while (q.Count != 0 && mid < 1000)
            {
                v = q.Dequeue();
                Debug.Log(v);
                RaycastHit2D hit = Physics2D.Raycast(v + (Vector2)this.transform.position, Vector2.zero);//, 0.1f, 1<<9);//??
                                                                      //Debug.Log($"{v.x}, {v.y}");

                if (hit.collider == null)
                {
                    /*
                    if (Mathf.Abs(v.y) > 10)
                    { //Mathf.Abs(v.x) > 12 ||
                        Debug.Log("WALL");
                    }
                    */
                    if (v.x < 0 || v.y < 0 || v.x > maxX || v.y > maxY) {}
                    else
                    {
                        Debug.Log("HIT");
                        return v + (Vector2)this.transform.position;// + (l[mid % _n] * 0.5f); // 수정하기
                    }
                }
                q.Enqueue(v + l[mid % _n]);
                mid++;
            }
            if (mid == 1000) Debug.LogWarning("mid is 1000");
            Debug.LogWarning(this.name + "This not gonna happen!!");
            return (Vector2)this.transform.position;
        }
/*
        IEnumerator ForceMove()
        { // Vector2 pos
            forceMoveOn = true;
            _state = behaveState.MOVE;
            while (Mathf.Abs(transform.position.x - targetPos.x) > 0.01f ||
                    Mathf.Abs(transform.position.y - targetPos.y) > 0.01f)
            {
                yield return new WaitForSeconds(0.2f);
            }
            forceMoveOn = false;	
        }
    */

        IEnumerator ForceMove()
        {
            forceMoveOn = true;
            StopCoroutine(Move());
            _state = behaveState.MOVE;

            if(movePosList == null || movePosList.Count == 0)
            {
                Debug.LogWarning("MovePosList is empty");
                forceMoveOn = false;
                yield break;
            }

            int t=0;

            targetPos = movePosList[t];

            int x=0;
            while (x++ <= 10)
            {
                if (Vector2.Distance(this.transform.position, targetPos) < 0.7)
                {
                    t += 1;
                    //movePosList.RemoveAt(0);
                    if(movePosList.Count > t && t >= 0)
                    {
                        targetPos = movePosList[t];
                    } else {
                        t = 0;
                        break;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
            forceMoveOn = false;
        }


        public void AfterWeek()
        {
            _stats.fatigue -= 10;
        }

        public void AfterFightInBattle()
        {
            if (gameManager.isFailed == true)
            {
                _stats.fatigue += 40;
                _stats.morale -= 2;
                if((int)_stats.morale < -2) _stats.morale = Morale.VERYBAD;
            }
            else
            {
                _stats.fatigue += 25;
                _stats.morale += 1;
                if((int)_stats.morale > 2) _stats.morale = Morale.VERYGOOD;
            }
        }



#region Skills
    /// Warrior skills
    public IEnumerator Taunt(GameObject caster, GameObject target)
    {   
        isSkillOn = false;
        canAct = false;
        yield return new WaitForSeconds(1);
        canAct = true;
        target.GetComponent<EnemyCtrl>()._attackTarget = caster;
        yield return new WaitForSeconds(10);
        isSkillOn = true;
    }

    /// Hunter skills
    public IEnumerator StrongArrow(GameObject cast, GameObject target)
    {
        isSkillOn = false;
        canAct = false;
        yield return new WaitForSeconds(1);
        canAct = true;
        gameSceneCtrl.CharacterHpChange(target, -_stats.skillAttackDamage, this.name);
        yield return new WaitForSeconds(10);
        isSkillOn = true;
    }

    /// Priest skills
    public IEnumerator Healing(GameObject caster)
    {
        isSkillOn = false;
        canAct = false;
        GameObject healTarget = gameObject;
        float minHp = 1.1f;
        foreach(var c in gameSceneCtrl.allyList)
        {
            float curHpProportion = (float)c.GetComponent<CharacterStats>().currHp / c.GetComponent<CharacterStats>().initHp;
            Debug.Log($"{c.name}'s curHPProportion = {curHpProportion}");
            if (curHpProportion < minHp)
            {
                healTarget = c;
                minHp = curHpProportion;
            }
        }
        yield return new WaitForSeconds(1);
        canAct = true;
        gameSceneCtrl.CharacterHpChange(healTarget, _stats.skillAttackDamage, this.name);
        Debug.Log($"Healed '{healTarget}' !!");
        yield return new WaitForSeconds(10);
        isSkillOn = true;
    }

#endregion
    }
}