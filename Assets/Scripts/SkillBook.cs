using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MRidDemo
{
public class Skill : MonoBehaviour
{
    /// Warrior skills
    public void Taunt(GameObject caster, GameObject target)
    {
        target.GetComponent<EnemyCtrl>()._attackTarget = caster;
    }
}
}