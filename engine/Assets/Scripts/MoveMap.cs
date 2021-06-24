using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum Behavior
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    KnifeAttack,
    Pike,
    Shield,
    Spear
}

public class MoveMap : MonoBehaviour
{
    #region 싱글톤
    private static MoveMap instance;
    public static MoveMap Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<MoveMap>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newSingleton = new GameObject("singleton Class").AddComponent<MoveMap>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<MoveMap>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    
    [Header("맵 구성")]
    public TileBox[] inputMap;
    public TileBox[,] sliceMap = new TileBox[3, 4];
    public int stage = 1;

    [Header("플레이어와 적")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Player player;
    public Enemy enemy;

    [Header("행동버튼 관련")]
    public GameObject tutorialUI;
    public GameObject[] behaviorButtons;
    public GameObject onBehaviorButton;
    public Text onBehaviorText;
    public GameObject behaviorSetting;
    public Behavior nextBehavior;
    public Button nextBehaviorButton; // 행동 다정한후누르는거. 임시로받아옴
    public Sprite[] keySprite; // 위 아래 왼쪽 오른쪽 , 공격
    public int keyInputCount = 0; // 스킬3개
    public int behaviorSequence = 0; // 1턴마다 3번 행동하는거
    public Image[] inGameKeyInput;
    public Image[] enemyGameKeyInput;
    public Text currentTurnText;
    public int turn = 0;

    [Header("체력과 마나")]
    public Image playerHPGauge;
    public Image playerMPGauge;
    public Image enemyHPGauge;
    public Image enemyMPGauge;
    public Text playerHPText;
    public Text playerMPText;
    public Text enemyHPText;
    public Text enemyMPText;
    public GameObject playerWinPanel;
    public GameObject enemyWinPanel;
    public GameObject drawPanel;

    [Header("공격 관련")]
    public float attackDelayTime = 0.5f;
    public GameObject attack1Prefab; // 단검
    public GameObject attack2Prefab; // 파이크
    public GameObject attack3Prefab; // 방패
    public GameObject attack4Prefab; // 창찌르기
    public Image[] attackCollisionPoints; // 행동패널에서의 공격범위 표시.
    public int reminder;



    private AudioSource audioSource;
    [Header("소 리")]
    public AudioClip swordSound;
    public AudioClip spearSound;
    public AudioClip shieldSound;

    [Header("색")]
    public Color32 tileColor;


    private Dictionary<Behavior, SkillDataVO> typeData = new Dictionary<Behavior, SkillDataVO>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //타입별 마나코스트 책정
        SkillDataVO attackData1 = new SkillDataVO(attack1Prefab, 20, 40, new List<int>() {5,2,6,8 }, new List<Vector2>() { new Vector2(0,0), new Vector2(1,0), new Vector3(0,1), new Vector3(0,-1)}, new Vector3(0.5f, 0, 0), false, "sword");
        typeData.Add(Behavior.KnifeAttack, attackData1);
        Debug.Log(typeData[Behavior.KnifeAttack]);

        SkillDataVO attackData2 = new SkillDataVO(attack2Prefab, 30, 25, new List<int>() { 5,2,3,6,8,9 }, new List<Vector2>() { 
            new Vector2(0, 0), 
            new Vector2(0, 1), 
            new Vector2(1, 1), 
            new Vector2(1, 0), 
            new Vector2(0, -1), 
            new Vector2(1, -1) }, new Vector3(0.5f, 0, 0), false, "spear");
        typeData.Add(Behavior.Pike, attackData2);

        SkillDataVO buffData1 = new SkillDataVO(attack3Prefab, -20, 0, new List<int>() { 5 }, new List<Vector2>() { new Vector2(0, 0) }, new Vector3(0.5f,0,0), true, "shield"); // 방 어
        typeData.Add(Behavior.Shield, buffData1);

        SkillDataVO attackData3 = new SkillDataVO(attack4Prefab, 50, 50, new List<int>() { 4, 5, 6 }, new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0) }, new Vector3(0.5f, 0f, 0f), false, "sword");
        typeData.Add(Behavior.Spear, attackData3);

        //타입별 프리팹 책정

        
        Tutorial();
        SetMap();
        PlayerSpawn();
        SkillUnlock();
        InitButtonIndex();
    }

    public void Tutorial()
    {
        tutorialUI.SetActive(true);
    }

    public void SetMap()
    {
        for(int i=0; i<3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                sliceMap[i,j] = inputMap[4 * i + j];
                Debug.Log(sliceMap[i, j].transform.position);
            }
        }

        
    }

    public void SkillUnlock()
    {
        if(stage <= 1) // 1스테이지 이하면
        {
            typeData.Remove(Behavior.Spear);
            behaviorButtons[(int)Behavior.Spear].SetActive(false);
            enemy.specialAttack1able = false;
        }
        else // 2스테이지
        {
            enemy.hp += 50;
            SkillDataVO attackData3 = new SkillDataVO(attack4Prefab, 50, 50, new List<int>() { 4, 5, 6 }, new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0) }, new Vector3(0.5f, 0f, 0f), false, "sword");
            typeData.Add(Behavior.Spear, attackData3);
            behaviorButtons[(int)Behavior.Spear].SetActive(true);
            enemy.specialAttack1able = true;
        }
    }

    public void PlayerSpawn()
    {
        player = Instantiate(playerPrefab, sliceMap[1, 0].transform.position - new Vector3(0.5f,0f), Quaternion.identity).GetComponent<Player>();
        enemy = Instantiate(enemyPrefab, sliceMap[1, 3].transform.position + new Vector3(0.5f, 0f), Quaternion.identity).GetComponent<Enemy>();
    }

    public void InitButtonIndex()
    {
        for (int i = 0; i < behaviorButtons.Length; i++)
        {
            Debug.Log(i);
            behaviorButtons[i].GetComponent<BehaviorButton>().buttonIndex = i;
        }
    }

    public void PlaySound(string idle)
    {
        switch(idle)
        {
            case "sword":
                audioSource.clip = swordSound;
                break;
            case "spear":
                audioSource.clip = spearSound;
                break;
            case "shield":
                audioSource.clip = shieldSound;
                break;
        }

        if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void TempStartNextBehavior()
    {
        StartCoroutine(NextBehavior());
    }

    public IEnumerator NextBehavior()
    {
        behaviorSetting.SetActive(false);
        // HidePanel(); // 행동선택완료니까 패널은끄고
        yield return new WaitForSeconds(0.5f); // 패널끄는동안 대기시간

        if (keyInputCount < 3)
        {
            Debug.Log("스킬 3개를 등록하지 않았습니다!");
            yield break;
        }
        nextBehaviorButton.interactable = false;
        

        for (int i=0; i<3; i++)
        {
            player.Move();
            enemy.EnemySetBehavior();
            enemy.Move();
            
            enemyGameKeyInput[i].sprite = keySprite[enemy.nextBehavior[i]];
            // 적 행동 보여주기


            // Debug.Log($"X: {player.currentX}, Y: {player.currentY}");
            
            yield return new WaitForSeconds(2f);

            if(CheckHPMP())
            {
                yield break;
            }

            player.armor = 0;
            enemy.armor = 0; // 턴끝나서 방어도 초기화

            
            behaviorSequence++;
        }
        
        if(behaviorSequence >= 3)
        {
            behaviorSequence = 0;
            NextTurn();
        }
    }

    public void NextTurn()
    {
        Debug.Log("열");
        player.mp += 30;
        enemy.mp += 30;
        onBehaviorButton.GetComponent<Image>().DOFade(1f,0.5f); // 다시입력
        onBehaviorText.gameObject.SetActive(true);
        turn++; // 1턴추가요
        player.InitBehavior(); // 행동넣은거 초기화
        enemy.nextBehavior.Clear(); // 적의 행동리스트 초기화

        // enemy.EnemySetBehavior(); // 적 행동 다시설정
        InitShowEnemyBehavior();

        keyInputCount = 0; // 행동입력한횟수 초기화
        for (int i=0; i<3; i++)
        {
            inGameKeyInput[i].sprite = null; // 스킬UI 초기화
        }
        currentTurnText.text = $"{turn} Turn";
        nextBehaviorButton.interactable = true;
    }

    public void OnlyPlayerBhReset() // 고른거 리셋만해주는놈
    {
        keyInputCount = 0;
        player.InitBehavior(); // 행동넣은거 초기화
        for (int i = 0; i < 3; i++)
        {
            inGameKeyInput[i].sprite = null; // 스킬UI 초기화
        }
    }

    public void LoadPanel() // 행동버튼누르는패널 보이게
    {
        behaviorSetting.SetActive(true);
        // onBehaviorButton.GetComponent<Image>().DOFade(0f, 0.5f); // 추가입력방지
        onBehaviorButton.GetComponent<Image>().DOFade(0f, 0.5f);
        onBehaviorText.gameObject.SetActive(false);
    }

    public void HidePanel() // 행동버튼누르는패널 안보이게
    {
        onBehaviorButton.GetComponent<Image>().DOFade(1f, 0.5f); // 다시입력
        onBehaviorText.gameObject.SetActive(true);
        behaviorSetting.SetActive(false);
    }

    public void InitattackCollision()
    {
        foreach (Image image in attackCollisionPoints)
        {
            image.color = new Color32(231,231,231,255); // 초기화
        }
    }

    public void PressKey(int buttonIndex)
    {
        if (reminder == buttonIndex || buttonIndex <= 3) // 똑같은거면 || 이동이면
        {
            reminder = -1;
            if (buttonIndex <= 3) // 공격아니면 초기화, 아니면 보여줘야함
                InitattackCollision();

            if (keyInputCount < 3)
            {
                player.nextBehavior.Add(buttonIndex);
                inGameKeyInput[keyInputCount].sprite = keySprite[buttonIndex];
                keyInputCount++;
            }
            else
            {
                Debug.LogError("3개이상 입력하셨습니다.");
            }
        }
        else // 안똑같은거면
        {
            InitattackCollision();

            reminder = buttonIndex;
            SkillDataVO data = typeData[(Behavior)buttonIndex];
            bool playerPos = true;

            foreach(int index in data.skillCollisions)
            {
                if (playerPos) // 맨처음이면(플레이어위치)
                {
                    if (data.isBuff)
                    {
                        attackCollisionPoints[index - 1].color = Color.yellow;
                    }
                    else
                    {
                        attackCollisionPoints[index - 1].color = Color.blue; // 플레이어위치
                    }
                    playerPos = false;
                }
                else
                {
                    if(data.isBuff)
                    {
                        attackCollisionPoints[index - 1].color = Color.yellow;
                    }
                    else
                    {
                        attackCollisionPoints[index - 1].color = Color.red;
                    }
                }
            }
        }
    }

    public void InitShowEnemyBehavior()
    {
        for (int i = 0; i < 3; i++)
        {
            enemyGameKeyInput[i].sprite = null;
        }
    }

    public bool CheckHPMP() // true 리턴시 게임끝
    {

        if (player.hp <= 0 && enemy.hp <= 0)
        {
            Debug.Log("Draw!");
            drawPanel.SetActive(true);
            drawPanel.GetComponent<Image>().DOFade(1f, 0.5f);
            return true;
        }
        else if (player.hp <= 0) // 피 0인지 체크
        {
            Debug.Log("Enemy is Win!");
            enemyWinPanel.SetActive(true);
            enemyWinPanel.GetComponent<Image>().DOFade(1f, 0.5f);
            return true;
        }
        else if (enemy.hp <= 0)
        {
            Debug.Log("Player is Win!");
            playerWinPanel.SetActive(true);
            playerWinPanel.GetComponent<Image>().DOFade(1f, 0.5f);
            return true;
        }
        return false;
    }

    public void ShowHPMP()
    {
        Debug.Log("체력체크");
        playerHPText.text = $"{player.hp} / 100"; // UI 업데이트
        playerHPGauge.fillAmount = player.hp / 100f;

        playerMPText.text = $"{player.mp} / 100"; // UI 업데이트
        playerMPGauge.fillAmount = player.mp / 100f;

        enemyHPText.text = $"{enemy.hp} / 150";
        enemyHPGauge.fillAmount = enemy.hp / 150f;

        enemyMPText.text = $"{enemy.mp} / 100";
        enemyMPGauge.fillAmount = enemy.mp / 100f;
    }

    public IEnumerator ShowAttackCollision(Behavior attackType, bool isplayer)
    {
        yield return new WaitForSeconds(0.1f); // 이동보다 나중에 실행하기 위하여

        SkillDataVO data = typeData[attackType];
        Vector3 position; // 공격나오는위치

        if (isplayer) // 플레이어일 시
        {
            player.mp -= data.cost; // 플레이어 마나소모
            position = player.transform.position + data.position; // 위치조절
        }
        else // 적일 시
        {
            enemy.mp -= data.cost;
            position = enemy.transform.position - data.position;
        }


        
        StartCoroutine(AttackDecision(isplayer, data.damage, data.attackPoints, position, data.prefab, data.isBuff, data.soundName));
        
    }

    public void TileClear()
    {
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<4; j++)
            {
                // sliceMap[i, j].SetColor(Color.white);
                sliceMap[i, j].SetColor(tileColor);
                //if((i+1)%2 == (j+1)%2)
                //{
                //    sliceMap[i, j].SetColor(Color.white);
                //}
                //else
                //{
                //    sliceMap[i, j].SetColor(Color.black);
                //}
            }
        }
    }

    public IEnumerator AttackDecision(bool isPlayer, int attackDamage, List<Vector2> attackPoints, Vector2 attackEffectPosition, GameObject attackPrefab, bool buff, string soundName)
    {
        // 공격타일 색깔변화 Red

        bool isDamage = false; // 공격판정 성공시 true로 변함
        bool isBuff = buff; // 일반공격이 아닌 회복, 보호막 등의 판정시 true로 변함

        foreach (Vector2 v in attackPoints)
        {
                Vector2 attackInVector = v;
            if (!isPlayer) // 적이면
            {
                attackInVector.x *= -1;
                attackInVector += new Vector2(enemy.currentX, enemy.currentY);
            }
            else // 플레이어면
            {
                attackInVector += new Vector2(player.currentX, player.currentY);
            }


            if (attackInVector.x <= 3 && attackInVector.x >= 0 && attackInVector.y <= 2 && attackInVector.y >= 0) // x 0~3 , y 0~2
            {
                if (isBuff)
                {
                    if (isPlayer)
                    {
                        player.armor += 15;
                    }
                    else
                    {
                        enemy.armor += 15; // 임시
                    }
                    sliceMap[(int)attackInVector.y, (int)attackInVector.x].SetColor(Color.yellow); // 색깔변화 힐/보호막
                }
                else
                {
                    

                    if ((sliceMap[(int)attackInVector.y, (int)attackInVector.x] == sliceMap[enemy.currentY, enemy.currentX] && isPlayer) || // 플레이어 공격판정
                     (sliceMap[(int)attackInVector.y, (int)attackInVector.x] == sliceMap[player.currentY, player.currentX] && !isPlayer))// 적 공격판정
                    {
                        sliceMap[(int)attackInVector.y, (int)attackInVector.x].SetColor(Color.red); // 색깔변화
                        isDamage = true;
                    }
                    else
                    {
                        sliceMap[(int)attackInVector.y, (int)attackInVector.x].SetColor(Color.green); // 색깔변화(안맞음)
                    }
                }

                
            }

                //v가 공격범위 벗어났는지 체크하고
                // 해당 위치 붉게 변하게하고
        }
        

        yield return new WaitForSeconds(attackDelayTime); // 공격타임 딜레이의시간 (이후 색깔원상복귀 및 공격판정확인)

        GameObject attackEffect = Instantiate(attackPrefab, attackEffectPosition, Quaternion.identity); // 공격이펙트생성
        Destroy(attackEffect, attackDelayTime);

        PlaySound(soundName);

        Sequence seq = DOTween.Sequence();

        attackEffect.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
        seq.Append(attackEffect.GetComponent<SpriteRenderer>().DOFade(1f, 0.4f));

        if (!isPlayer) // 적이면 이펙트 회전
        {
            attackEffect.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        

        if (isDamage)
        {
            float shakePower = 0.5f;
            Debug.Log("피까임");


            if (isPlayer)
            {
                if(enemy.armor != 0)
                {
                    Debug.Log($"적 방어도 {enemy.armor}, 공격데미지 {attackDamage}");
                    sliceMap[enemy.currentY, enemy.currentX].SetColor(Color.cyan);
                    shakePower = 0.1f;
                    Mathf.Clamp(enemy.armor, 0, attackDamage);

                    seq.Append(enemy.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.cyan, 0.7f));
                    seq.Append(enemy.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f));
                }
                else
                {
                    seq.Append(enemy.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.7f));
                    seq.Append(enemy.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f));
                }
                if (attackPrefab.name == attack4Prefab.name) // 임시로 창 구별. 원래는 dataVO에 넣어야할듯. TODO
                {
                    if (enemy.currentX < 3)
                    {
                        enemy.PushOut();
                    }
                }
                enemy.hp -= attackDamage - enemy.armor; // 데미지에서 방어도 뺀값
            }
            else
            {
                if(player.armor != 0)
                {
                    Debug.Log($"플레이어 방어도 {player.armor}, 공격데미지 {attackDamage}");
                    sliceMap[player.currentY, player.currentX].SetColor(Color.cyan);
                    shakePower = 0.1f;
                    Mathf.Clamp(player.armor, 0, attackDamage);

                    seq.Append(player.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.cyan, 0.7f));
                    seq.Append(player.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f));
                }
                else
                {
                    seq.Append(player.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.red, 0.7f));
                    seq.Append(player.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.white, 0.3f));
                }
                if (attackPrefab.name == attack4Prefab.name) // 임시로 창 구별. 원래는 dataVO에 넣어야할듯. TODO
                {
                    if (player.currentX > 0)
                    {
                        player.PushOut();
                    }
                }
                player.hp -= attackDamage - player.armor; // 데미지에서 방어도 뺀값
                
            }

            Camera.main.DOShakePosition(2f, shakePower); // 공격받아서 화면흔들림
        }

        ShowHPMP();
        TileClear(); // 공격타일 색깔변화 White
    }
}

