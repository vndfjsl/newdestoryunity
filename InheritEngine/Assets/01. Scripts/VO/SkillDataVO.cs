using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataVO
{
    public GameObject attackPrefab; // 공격 이펙트
    public int requireCost; // 소모 마나
    public int attackDamage; // 입히는 피해
    public bool isBuff; // 강화 / 회복류 스킬인가?
    public string soundName; // 스킬시전시 발생하는 사운드의 이름

    public Vector3 attackEffectPosition; // 이펙트가 발생하는 위치
    public List<int> showSkillCollisions; // 스킬선택 시 표시될 범위
    public List<Vector2> attackPoints; // 실제 공격 시 판정범위

    public SkillDataVO(GameObject prefab, int cost, int damage,
        bool buff, string sound, Vector3 position,
        List<int> skillCollisions, List<Vector2> points)
    {
        this.attackPrefab = prefab;
        this.requireCost = cost;
        this.attackDamage = damage;
        this.isBuff = buff;
        this.soundName = sound;

        this.attackEffectPosition = position;
        this.showSkillCollisions = skillCollisions;
        this.attackPoints = points;
    }
}
