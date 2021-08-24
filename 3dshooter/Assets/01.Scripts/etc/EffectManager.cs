using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject bloodEffectPrefab;
    private List<GameObject> bloodEffectList = new List<GameObject>(); // * Queue�� ������, List�� ������ ����

    private void Awake()
    {
        if(instance != null)
            Debug.LogError("�ټ��� ����Ʈ�Ŵ����� �������Դϴ�.");
        instance = this;

        for(int i=0; i<15; i++)
        {
            // * ���⼭ 15���� ����Ʈ�� �̸� ������ݴϴ�. (Ǯ��)
            GameObject effect = MakeBloodEffect();
            effect.SetActive(false);
            bloodEffectList.Add(effect);
        }
    }

    private GameObject MakeBloodEffect() // * ���� �Լ��� �������ִ�.
    {
        return Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity, transform); // * 4��° ���ڴ� �θ��� transform�� ����(���⼱ EffectManager)
    }

    public static GameObject GetBloodEffect()
    {
        GameObject effect = instance.bloodEffectList.Find(x => !x.activeSelf); // * active�� false�� �� ã�ƿ����ϴ� ���ٽ� �����ε�(Predicate)
        if(effect == null) // * �����Ծ����� null�� ��ȯ�ϰ�
        {
            effect = instance.MakeBloodEffect(); // * �ϳ������
            instance.bloodEffectList.Add(effect); // ����Ʈ�� �ְ�.
        }
        return effect; // ������ ���� or ������ ����� ����
    }
}
