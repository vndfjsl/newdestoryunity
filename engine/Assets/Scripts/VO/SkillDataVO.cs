using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataVO 
{
    public GameObject prefab; // Attack Prefab
    public int cost; // Attack MP Cost
    public int damage; // Attack Damage
    public List<Vector2> attackPoints;
    public Vector3 position;
    public bool isBuff; // Not Attack
    public string soundName;

    public SkillDataVO(GameObject prefab, int cost, int damage, List<Vector2> attackPoints, Vector3 position, bool isBuff, string sound)
    {
        this.prefab = prefab;
        this.cost = cost;
        this.damage = damage;
        this.attackPoints = attackPoints;
        this.position = position;
        this.isBuff = isBuff;
        this.soundName = sound;
    }
}
