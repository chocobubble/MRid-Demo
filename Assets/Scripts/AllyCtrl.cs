using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{
    public enum behaveState
    {
        STAY,
        ATTACK,
        MOVE,
        DIE
    };
    
    [RequireComponent(typeof(CharacterStats))]
    [RequireComponent(typeof(Collider2D))]
    public class AllyCtrl : MonoBehaviour
    {
        [SerializeField]
        GameSceneCtrl gameSceneCtrl;

        [SerializeField]
        CharacterStats stats;

        [SerializeField]
        Pathfinding path; // A* algorithm to find the position to move

        [SerializeField]
        GameManager gameManager;

        [SerializeField]
        List<Vector2> positions = new List<Vector2>(); // 8 random directions to find a location to escape form the area attack

        [SerializeField]
        List<Vector2> movePosList = new List<Vector2>(); // path of movement

        [SerializeField]
        bool isSkillOn = true;

        [SerializeField]
        bool swichOn = true; // determines whether 'Judge' is possible or not

        [SerializeField]
        bool isBaseAttackOn = true;

        [SerializeField]
        Vector2 targetPos; // position to move

        [SerializeField]
        int mid; // prevent infinite looping

        [SerializeField]
        int t;

        // when cast a skill or being stunned
        public bool canAct = true;

        // the character is on moving state
        [SerializeField]
        bool moveOn = false;

        // When the event happend
        public bool eventOn = false;

        [SerializeField]
        List<GameObject> enemyList;

        // main attack target
        public GameObject attackTarget;

        [SerializeField]
        Collider2D collider2;

        // list that contains 8 random direction to escape from the area attack
        [SerializeField]
        List<Vector2> l;

        // queue for finding a location to escape by minimum distance order
        [SerializeField]
        Queue<Vector2> q;

        // max coordinate can place
        int maxX;
        int maxY;

        int areaAttackLayerMask;
        int obstacleLayerMask;

        // character behave state
        public behaveState state;

        void Awake()
        {
            gameSceneCtrl = GameObject.Find("GameSceneCtrl").GetComponent<GameSceneCtrl>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            stats = this.GetComponent<CharacterStats>();
            collider2 = this.GetComponent<Collider2D>();
            if (collider2 == null) Debug.LogWarning("No collider");

            // random 8 directions for finding the escape location 
            float rnd = UnityEngine.Random.Range(0, 1);
            for (int i = 0; i < 8; i++)
            {
                // directions are separated by 45 degree from each other.
                positions.Add(new Vector2(Mathf.Cos(Mathf.PI * rnd + (Mathf.PI * i / 4.0f)), Mathf.Sin(Mathf.PI * rnd + (Mathf.PI * i / 4.0f))));
            }

            path = GameObject.Find("GridManager").GetComponent<Pathfinding>();

            maxX = GameObject.Find("Grid").GetComponent<GridMap>().length;
            maxY = GameObject.Find("Grid").GetComponent<GridMap>().height;

            /// later, 
            enemyList = gameSceneCtrl.enemyList;
            /// later, add a method to choose attack target 
            attackTarget = enemyList[0];

            targetPos = attackTarget.transform.position;

            areaAttackLayerMask = LayerMask.GetMask("AreaAttack");
            obstacleLayerMask = LayerMask.GetMask("Tile");
        }

        // the character choose what to do
        // if the character is on the area attack, find a place to escape, and move
        // if the character is not on the area attack and the attack target is far, find a place to attack, and move
        // if the attack target is in the distance that can attack, attack
        IEnumerator Judge()
        {
            // stop calling the method until next response
            swichOn = false;

            // wait for a response
            yield return new WaitForSeconds(stats.responseSpeed);

            // if the character is still moving, skip the judge
            if (moveOn)
            {
                swichOn = true;
                yield break;
            }

            /// check whether the character is touching an area attack's collider,
            bool isTouch = collider2.IsTouchingLayers(areaAttackLayerMask);

            // if the enemies trigger an event
            if (eventOn)
            {
                eventOn = false;
            }

            // the character is touching an area attack collider
            if (isTouch == true)
            {
                Debug.Log(name + "hit.collider.");
                Vector2 v2 = FindEscapeLocation(transform.position);
                //movePosList.Clear();
                Debug.Log("location to escape : " + v2);
                // find a path to escape
                movePosList = path.FindPath((int)this.transform.position.x, (int)this.transform.position.y, (int)v2.x, (int)v2.y);
                StartCoroutine(Move());
            }
            /// check whether the attack target is within the character's attack range or not
            else
            {
                // later modify to method.
                attackTarget = enemyList[0];

                float distance = (attackTarget.transform.position
                        - this.transform.position).magnitude
                        - attackTarget.GetComponent<CircleCollider2D>().radius;

                if (distance <= stats.attackDistance)
                {
                    state = behaveState.ATTACK;
                }
                else
                {
                    StopCoroutine(Move());
                    state = behaveState.MOVE;
                    movePosList = GetPositionToTarget(attackTarget);
                    StartCoroutine(Move());
                }
            }

            swichOn = true;
        }

        private List<Vector2> GetPositionToTarget(GameObject attackTarget)
        {
            float targetRadius = attackTarget.GetComponent<CircleCollider2D>().radius;
            // later, modify distance beging changed randomly

            // the character's attack range
            float distance = targetRadius + stats.attackDistance;

            return path.FindAtkPath((int)this.transform.position.x, (int)this.transform.position.y,
                    (int)attackTarget.transform.position.x, (int)attackTarget.transform.position.y, distance);
        }


        void Update()
        {
            if (canAct)
            {
                if (swichOn)
                {
                    StartCoroutine(Judge());
                }
                else
                {
                    switch (state)
                    {
                        case behaveState.ATTACK:
                            Attack();
                            break;
                        case behaveState.MOVE:
                            transform.Translate((targetPos - (Vector2)this.transform.position).normalized * Time.deltaTime * stats.speed * 0.1f);
                            break;
                    }
                }
            }
        }
        #region ATTACK
        void Attack()
        {
            if (isSkillOn)
            {
                Skill();
            }
            else if (isBaseAttackOn)
            {
                StartCoroutine(BaseAttack());
            }
        }

        public void Skill()
        {
            switch (stats.characterClass)
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
            if (attackTarget == null)
            {
                Debug.LogWarning($"no AtkTarget of {this.name}");
            }
            else
            {
                gameSceneCtrl.CharacterHpChange(attackTarget, gameObject, -stats.baseAttackDamage, this.name);
            }
            yield return new WaitForSeconds(stats.baseAttackCooltime);
            isBaseAttackOn = true;
        }
        #endregion

        IEnumerator Move()
        {
            state = behaveState.MOVE;
            moveOn = true;

            if (movePosList == null || movePosList.Count == 0)
            {
                moveOn = false;
                targetPos = (Vector2)attackTarget.transform.position;
                yield break;
            }

            t = 0;

            targetPos = movePosList[t];

            int x = 0;
            while (x++ <= 100)
            {
                if (Vector2.Distance(this.transform.position, targetPos) < 0.1f)
                {
                    t += 1;
                    if (movePosList.Count > t)
                    {
                        targetPos = movePosList[t];
                    }
                    else
                    {
                        Debug.Log(this.name + "moving is done");
                        t = 0;
                        break;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
            moveOn = false;
        }

        Vector2 FindEscapeLocation(Vector2 currPos)
        {
            Debug.Log(this.name + "  Escape");
            q = new Queue<Vector2>();
            l = new List<Vector2>();
            mid = 0;
            for (int i = 7; i >= 0; i--)
            {
                int n = UnityEngine.Random.Range(0, i + 1);
                l.Add(positions[n]);

                var temp = positions[n];
                positions[n] = positions[i];
                positions[i] = temp;
            }
            Vector2 v;
            int _n = l.Count;
            for (int i = 0; i < l.Count; i++)
                q.Enqueue(l[i]);
            while (q.Count != 0 && mid < 1000)
            {
                v = q.Dequeue();
                RaycastHit2D hit = Physics2D.Raycast(v + (Vector2)this.transform.position, Vector2.zero, 0.01f, ((areaAttackLayerMask) + (obstacleLayerMask)));

                if (hit.collider == null)
                {
                    if (v.x < 0 || v.y < 0 || v.x > maxX || v.y > maxY) { }
                    else
                    {
                        RaycastHit2D hit2 = Physics2D.Raycast((v + l[mid % _n]) + (Vector2)this.transform.position, Vector2.zero, 0.01f, ((areaAttackLayerMask) + (obstacleLayerMask)));
                        if (hit2.collider == null)
                        {
                            //Debug.Log("HIT");
                            return (v + l[mid % _n]) + (Vector2)this.transform.position;
                        }
                    }
                }
                q.Enqueue(v + l[mid % _n]);
                mid++;
            }
            if (mid == 1000) Debug.LogWarning("mid is 1000");
            return (Vector2)this.transform.position;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, stats.attackDistance);
            Gizmos.DrawRay(transform.position, targetPos);
        }

        #region Skills
        /// Warrior skills
        public IEnumerator Taunt(GameObject caster, GameObject target)
        {
            isSkillOn = false;
            canAct = false;
            gameSceneCtrl.AnimateLoadingBar(stats);
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
            gameSceneCtrl.AnimateLoadingBar(stats);
            yield return new WaitForSeconds(1);
            canAct = true;
            gameSceneCtrl.CharacterHpChange(target, gameObject, -stats.skillAttackDamage, this.name);
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
            foreach (var c in gameSceneCtrl.allyList)
            {
                float curHpProportion = (float)c.GetComponent<CharacterStats>().currHp / c.GetComponent<CharacterStats>().initHp;
                Debug.Log($"{c.name}'s curHPProportion = {curHpProportion}");
                if (curHpProportion < minHp)
                {
                    healTarget = c;
                    minHp = curHpProportion;
                }
            }
            gameSceneCtrl.AnimateLoadingBar(stats);
            yield return new WaitForSeconds(1);
            canAct = true;
            gameSceneCtrl.CharacterHpChange(healTarget, gameObject, stats.skillAttackDamage, this.name);
            Debug.Log($"Healed '{healTarget}' !!");
            yield return new WaitForSeconds(10);
            isSkillOn = true;
        }

        #endregion
    }
}