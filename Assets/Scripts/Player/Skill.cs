using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public SkillData data;

    public void OnClickSkill()
    {
        var enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemy.GetComponent<LivingEntity>().OnDamage(1, Vector3.zero, Vector3.zero);
    }

}
