using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Behavior
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    KnifeAttack,
    Pike,
    Shield
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

    [Header("플레이어와 적")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private Player player;
    private Enemy enemy;

    [Header("행동버튼 관련")]
    public GameObject tutorialUI;
    public GameObject[] behaviorButtons;
    public GameObject onBehaviorButton;
    public GameObject behaviorSetting;
    public Behavior nextBehavior;
    public Sprite[] keySprite; // 위 아래 왼쪽 오른쪽 , 공격
    public int keyInputCount = 0; // 스킬3개
    public int behaviorSequence = 0; // 1턴마다 3번 행동하는거
    public Image[] inGameKeyInput;
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
    public GameObject attack1Prefab;
    public GameObject attack2Prefab;

    void Start()
    {
        Tutorial();
        SetMap();
        PlayerSpawn();
        InitButtonIndex();
        HidePanel();
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

    public void PlayerSpawn()
    {
        player = Instantiate(playerPrefab, sliceMap[1, 0].transform.position, Quaternion.identity).GetComponent<Player>();
        enemy = Instantiate(enemyPrefab, sliceMap[1, 3].transform.position, Quaternion.identity).GetComponent<Enemy>();
    }

    public void InitButtonIndex()
    {
        for (int i = 0; i < behaviorButtons.Length; i++)
        {
            behaviorButtons[i].GetComponent<BehaviorButton>().buttonIndex = i;
        }
    }

    public void NextBehavior()
    {
        player.Move();
        
        enemy.Move();

        Debug.Log($"X: {player.currentX}, Y: {player.currentY}");
        player.transform.position = sliceMap[player.currentY, player.currentX].transform.position;
        enemy.transform.position = sliceMap[enemy.currentY, enemy.currentX].transform.position;

        behaviorSequence++;
        if(behaviorSequence >= 3)
        {
            behaviorSequence = 0;
            NextTurn();
        }
    }

    public void NextTurn()
    {
        turn++; // 1턴추가요
        player.InitBehavior(); // 행동넣은거 초기화
        enemy.EnemySetBehavior(); // 적 행동 다시설정

        keyInputCount = 0; // 행동입력한횟수 초기화
        for (int i=0; i<3; i++)
        {
            inGameKeyInput[i].sprite = null;
        }
        currentTurnText.text = $"{turn} Turn";
    }

    public void LoadPanel() // 행동버튼누르는패널 보이게
    {
        behaviorSetting.SetActive(true);
        onBehaviorButton.SetActive(false);
    }

    public void HidePanel() // 행동버튼누르는패널 안보이게
    {
        behaviorSetting.SetActive(false);
        onBehaviorButton.SetActive(true);
    }

    public void PressKey(int buttonIndex)
    {
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

    public void CheckHPMP()
    {
        Debug.Log("체력체크");
        playerHPText.text = $"{player.hp} / 100"; // UI 업데이트
        playerHPGauge.fillAmount = player.hp / 100f;

        playerMPText.text = $"{player.mp} / 100"; // UI 업데이트
        playerMPGauge.fillAmount = player.mp / 100f;

        enemyHPText.text = $"{enemy.hp} / 100";
        enemyHPGauge.fillAmount = enemy.hp / 100f;

        enemyMPText.text = $"{enemy.mp} / 100";
        enemyMPGauge.fillAmount = enemy.mp / 100f;



        if (player.hp <= 0 && enemy.hp <= 0)
        {
            Debug.Log("Draw!");
            drawPanel.SetActive(true);
        }
        else if (player.hp <= 0) // 피 0인지 체크
        {
            Debug.Log("Enemy is Win!");
            enemyWinPanel.SetActive(true);
        }
        else if (enemy.hp <= 0)
        {
            Debug.Log("Player is Win!");
            playerWinPanel.SetActive(true);
        }
    }

    public void ShowAttackCollision(int attackType, bool isplayer)
    {
        switch (attackType)
        {
            case (int)Behavior.KnifeAttack:
                    if (isplayer) // 플레이어 공격일시
                    {
                        player.mp -= 10;

                        GameObject attackEffect = Instantiate(attack1Prefab, player.attack1Trm.position, Quaternion.identity); // 공격이펙트
                        Destroy(attackEffect, attackDelayTime);
                        StartCoroutine(AttackDecision(true, 1, (0,0),(1,0) )); // player = true, 현재위치로부터 0,0과 1,0을 공격
                    }
                    else // 적의 공격일시
                    {
                        enemy.mp -= 10;

                        #region 묶주석
                        /*if (sliceMap[enemy.currentY, enemy.currentX - 1] != null)
                        {
                            sliceMap[enemy.currentY, enemy.currentX - 1].SetColor(Color.red); // 공격범위 표시
                        }
                        if (sliceMap[enemy.currentY, enemy.currentX] != null)
                        {
                            sliceMap[enemy.currentY, enemy.currentX].SetColor(Color.red);
                        }*/
                        #endregion
                        GameObject attackEffect = Instantiate(attack1Prefab, enemy.attack1Trm.position, Quaternion.identity); // 공격이펙트
                        Destroy(attackEffect, attackDelayTime);
                        StartCoroutine(AttackDecision(false, 1, (0, 0), (-1, 0))); // player = true, 현재위치로부터 0,0과 1,0을 공격
                    }
                    break;
                    // yield return new WaitForSeconds(attackDelayTime);

                    //if (isplayer) // 플레이어 공격일시
                    //{

                    //}
                    //else // 적의 공격일시
                    //{
                    //    if (sliceMap[enemy.currentY, enemy.currentX - 1] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    //        isDamage = true;
                    //}

                    //if (sliceMap[player.currentY, player.currentX] == sliceMap[enemy.currentY, enemy.currentX])
                    //{
                    //    Debug.Log("겹쳤다");
                    //    isDamage = true;
                    //}


                    //if (isDamage)
                    //{
                    //    Debug.Log("피까임");
                    //    if (isplayer)
                    //    {
                    //        enemy.hp -= 30;
                    //    }
                    //    else
                    //    {
                    //        player.hp -= 30;
                    //        sliceMap[enemy.currentY, enemy.currentX].SetColor(Color.white);
                    //        sliceMap[enemy.currentY, enemy.currentX - 1].SetColor(Color.white);
                    //    }
                    //}



                    // TileClear();


                

            case (int)Behavior.Pike:

                if (isplayer) // 플레이어 공격일시
                {
                    player.mp -= 40;

                    GameObject attackEffect = Instantiate(attack2Prefab, player.attack2Trm.position, Quaternion.identity); // 공격이펙트
                    Destroy(attackEffect, attackDelayTime);
                    StartCoroutine(AttackDecision(true, 2, (0, 0), (0, 1), (1,1),(1,0),(0,-1),(1,-1))); // player = true, 현재위치로부터 0,0과 1,0을 공격
                }
                else // 적의 공격일시
                {
                    enemy.mp -= 20;

                    GameObject attackEffect = Instantiate(attack2Prefab, enemy.attack2Trm.position, Quaternion.identity); // 공격이펙트
                    Destroy(attackEffect, attackDelayTime);
                    StartCoroutine(AttackDecision(false, 2, (0, 0), (0,1), (-1,1),(-1,0),(0,-1),(-1,-1))); // player = true, 현재위치로부터 0,0과 1,0을 공격
                }
                CheckHPMP();
                break;

            case (int)Behavior.Shield:
                Debug.Log("실드 미구현");
                break;
            default:
                Debug.Log("공격스킬이 아닌거같은데");
                break;
        }
        
    }

    public void TileClear()
    {
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<4; j++)
            {
                sliceMap[i, j].SetColor(Color.white);
            }
        }
    }

    public IEnumerator AttackDecision(bool isPlayer, int attackType, (int x, int y)? targetXY1 = null, (int x, int y)? targetXY2 = null, (int x, int y)? targetXY3 = null, (int x, int y)? targetXY4 = null,
        (int x, int y)? targetXY5 = null, (int x, int y)? targetXY6 = null, (int x, int y)? targetXY7 = null, (int x, int y)? targetXY8 = null, (int x, int y)? targetXY9 = null)
    {
        Debug.Log("이거 y:" + targetXY1.Value.y + ", x:" + targetXY1.Value.x);
        Debug.Log(targetXY1.HasValue);
        // 공격타일 색깔변화 Red
        if(isPlayer)
        {
            if(targetXY1 != null)
            {
                sliceMap[player.currentY + targetXY1.Value.y, player.currentX + targetXY1.Value.x].SetColor(Color.red);
            }
            if (targetXY2 != null)
            {
                sliceMap[player.currentY + targetXY2.Value.y, player.currentX + targetXY2.Value.x].SetColor(Color.red);
            }
            if (targetXY3 != null)
            {
                sliceMap[player.currentY + targetXY3.Value.y, player.currentX + targetXY3.Value.x].SetColor(Color.red);
            }
            if (targetXY4 != null)
            {
                sliceMap[player.currentY + targetXY4.Value.y, player.currentX + targetXY4.Value.x].SetColor(Color.red);
            }
            if (targetXY5 != null)
            {
                sliceMap[player.currentY + targetXY5.Value.y, player.currentX + targetXY5.Value.x].SetColor(Color.red);
            }
            if (targetXY6 != null)
            {
                sliceMap[player.currentY + targetXY6.Value.y, player.currentX + targetXY6.Value.x].SetColor(Color.red);
            }
            if (targetXY7 != null)
            {
                sliceMap[player.currentY + targetXY7.Value.y, player.currentX + targetXY7.Value.x].SetColor(Color.red);
            }
            if (targetXY8 != null)
            {
                sliceMap[player.currentY + targetXY8.Value.y, player.currentX + targetXY8.Value.x].SetColor(Color.red);
            }
            if (targetXY9 != null)
            {
                sliceMap[player.currentY + targetXY9.Value.y, player.currentX + targetXY9.Value.x].SetColor(Color.red);
            }

        }
        else
        {
            if (targetXY1 != null)
            {
                sliceMap[enemy.currentY + targetXY1.Value.y, enemy.currentX + targetXY1.Value.x].SetColor(Color.red);
            }
            if (targetXY2 != null)
            {
                sliceMap[enemy.currentY + targetXY2.Value.y, enemy.currentX + targetXY2.Value.x].SetColor(Color.red);
            }
            if (targetXY3 != null)
            {
                sliceMap[enemy.currentY + targetXY3.Value.y, enemy.currentX + targetXY3.Value.x].SetColor(Color.red);
            }
            if (targetXY4 != null)
            {
                sliceMap[enemy.currentY + targetXY4.Value.y, enemy.currentX + targetXY4.Value.x].SetColor(Color.red);
            }
            if (targetXY5 != null)
            {
                sliceMap[enemy.currentY + targetXY5.Value.y, enemy.currentX + targetXY5.Value.x].SetColor(Color.red);
            }
            if (targetXY6 != null)
            {
                sliceMap[enemy.currentY + targetXY6.Value.y, enemy.currentX + targetXY6.Value.x].SetColor(Color.red);
            }
            if (targetXY7 != null)
            {
                sliceMap[enemy.currentY + targetXY7.Value.y, enemy.currentX + targetXY7.Value.x].SetColor(Color.red);
            }
            if (targetXY8 != null)
            {
                sliceMap[enemy.currentY + targetXY8.Value.y, enemy.currentX + targetXY8.Value.x].SetColor(Color.red);
            }
            if (targetXY9 != null)
            {
                sliceMap[enemy.currentY + targetXY9.Value.y, enemy.currentX + targetXY9.Value.x].SetColor(Color.red);
            }
        }

        yield return new WaitForSeconds(attackDelayTime); // 공격타임 딜레이의시간 (이후 색깔원상복귀 및 공격판정확인)

        // 공격타일 색깔변화 White

        bool isDamage = false; // 공격판정 성공시 true로 변함

        if (isPlayer)
        {
            if (targetXY1 != null)
            {
                if (sliceMap[player.currentY + targetXY1.Value.y, player.currentX + targetXY1.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY1.Value.y, player.currentX + targetXY1.Value.x].SetColor(Color.white);
            }
            if (targetXY2 != null)
            {
                if (sliceMap[player.currentY + targetXY2.Value.y, player.currentX + targetXY2.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY2.Value.y, player.currentX + targetXY2.Value.x].SetColor(Color.white);
            }
            if (targetXY3 != null)
            {
                if (sliceMap[player.currentY + targetXY3.Value.y, player.currentX + targetXY3.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY3.Value.y, player.currentX + targetXY3.Value.x].SetColor(Color.white);
            }
            if (targetXY4 != null)
            {
                if (sliceMap[player.currentY + targetXY4.Value.y, player.currentX + targetXY4.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY4.Value.y, player.currentX + targetXY4.Value.x].SetColor(Color.white);
            }
            if (targetXY5 != null)
            {
                if (sliceMap[player.currentY + targetXY5.Value.y, player.currentX + targetXY5.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY5.Value.y, player.currentX + targetXY5.Value.x].SetColor(Color.white);
            }
            if (targetXY6 != null)
            {
                if (sliceMap[player.currentY + targetXY6.Value.y, player.currentX + targetXY6.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY6.Value.y, player.currentX + targetXY6.Value.x].SetColor(Color.white);
            }
            if (targetXY7 != null)
            {
                if (sliceMap[player.currentY + targetXY7.Value.y, player.currentX + targetXY7.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY7.Value.y, player.currentX + targetXY7.Value.x].SetColor(Color.white);
            }
            if (targetXY8 != null)
            {
                if (sliceMap[player.currentY + targetXY8.Value.y, player.currentX + targetXY8.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY8.Value.y, player.currentX + targetXY8.Value.x].SetColor(Color.white);
            }
            if (targetXY9 != null)
            {
                if (sliceMap[player.currentY + targetXY9.Value.y, player.currentX + targetXY9.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[player.currentY + targetXY9.Value.y, player.currentX + targetXY9.Value.x].SetColor(Color.white);
            }

        }
        else
        {
            if (targetXY1 != null)
            {
                if (sliceMap[enemy.currentY + targetXY1.Value.y, enemy.currentX + targetXY1.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY1.Value.y, enemy.currentX + targetXY1.Value.x].SetColor(Color.white);
            }
            if (targetXY2 != null)
            {
                if (sliceMap[enemy.currentY + targetXY2.Value.y, enemy.currentX + targetXY2.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY2.Value.y, enemy.currentX + targetXY2.Value.x].SetColor(Color.white);
            }
            if (targetXY3 != null)
            {
                if (sliceMap[enemy.currentY + targetXY3.Value.y, enemy.currentX + targetXY3.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY3.Value.y, enemy.currentX + targetXY3.Value.x].SetColor(Color.white);
            }
            if (targetXY4 != null)
            {
                if (sliceMap[enemy.currentY + targetXY4.Value.y, enemy.currentX + targetXY4.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY4.Value.y, enemy.currentX + targetXY4.Value.x].SetColor(Color.white);
            }
            if (targetXY5 != null)
            {
                if (sliceMap[enemy.currentY + targetXY5.Value.y, enemy.currentX + targetXY5.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY5.Value.y, enemy.currentX + targetXY5.Value.x].SetColor(Color.white);
            }
            if (targetXY6 != null)
            {
                if (sliceMap[enemy.currentY + targetXY6.Value.y, enemy.currentX + targetXY6.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY6.Value.y, enemy.currentX + targetXY6.Value.x].SetColor(Color.white);
            }
            if (targetXY7 != null)
            {
                if (sliceMap[enemy.currentY + targetXY7.Value.y, enemy.currentX + targetXY7.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY7.Value.y, enemy.currentX + targetXY7.Value.x].SetColor(Color.white);
            }
            if (targetXY8 != null)
            {
                if (sliceMap[enemy.currentY + targetXY8.Value.y, enemy.currentX + targetXY8.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY8.Value.y, enemy.currentX + targetXY8.Value.x].SetColor(Color.white);
            }
            if (targetXY9 != null)
            {
                if (sliceMap[enemy.currentY + targetXY9.Value.y, enemy.currentX + targetXY9.Value.x] == sliceMap[player.currentY, player.currentX]) // 공격판정
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY9.Value.y, enemy.currentX + targetXY9.Value.x].SetColor(Color.white);
            }
        }

        // 판정이 맞아떨어지면
        if (isDamage)
        {
            Debug.Log("피까임");
            if (isPlayer)
            {
                if(attackType == 1)
                enemy.hp -= 50;
                else if (attackType == 2)
                    enemy.hp -= 20;
            }
            else
            {
                if (attackType == 1)
                    player.hp -= 50;
                else if (attackType == 2)
                    player.hp -= 20;
            }
        }
        CheckHPMP();
    }
}

