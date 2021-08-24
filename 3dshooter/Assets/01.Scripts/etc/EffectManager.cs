using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance;
    public GameObject bloodEffectPrefab;
    private List<GameObject> bloodEffectList = new List<GameObject>(); // * Queue는 빠르다, List는 조절이 쉽다

    private void Awake()
    {
        if(instance != null)
            Debug.LogError("다수의 이펙트매니저가 실행중입니다.");
        instance = this;

        for(int i=0; i<15; i++)
        {
            // * 여기서 15개의 이펙트를 미리 만들어줍니다. (풀링)
            GameObject effect = MakeBloodEffect();
            effect.SetActive(false);
            bloodEffectList.Add(effect);
        }
    }

    private GameObject MakeBloodEffect() // * 따로 함수로 뺄수도있다.
    {
        return Instantiate(bloodEffectPrefab, transform.position, Quaternion.identity, transform); // * 4번째 인자는 부모의 transform을 말함(여기선 EffectManager)
    }

    public static GameObject GetBloodEffect()
    {
        GameObject effect = instance.bloodEffectList.Find(x => !x.activeSelf); // * active가 false인 놈만 찾아오게하는 람다식 오버로딩(Predicate)
        if(effect == null) // * 꺼진게없으면 null을 반환하고
        {
            effect = instance.MakeBloodEffect(); // * 하나만들고
            instance.bloodEffectList.Add(effect); // 리스트에 넣고.
        }
        return effect; // 꺼진걸 리턴 or 없으면 만든걸 리턴
    }
}
