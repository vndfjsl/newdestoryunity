using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataVO
{
    public GameObject attackPrefab; // ���� ����Ʈ
    public int requireCost; // �Ҹ� ����
    public int attackDamage; // ������ ����
    public bool isBuff; // ��ȭ / ȸ���� ��ų�ΰ�?
    public string soundName; // ��ų������ �߻��ϴ� ������ �̸�

    public Vector3 attackEffectPosition; // ����Ʈ�� �߻��ϴ� ��ġ
    public List<int> showSkillCollisions; // ��ų���� �� ǥ�õ� ����
    public List<Vector2> attackPoints; // ���� ���� �� ��������

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
