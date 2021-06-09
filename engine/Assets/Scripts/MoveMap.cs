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
    #region �̱���
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

    
    [Header("�� ����")]
    public TileBox[] inputMap;
    public TileBox[,] sliceMap = new TileBox[3, 4];

    [Header("�÷��̾�� ��")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    private Player player;
    private Enemy enemy;

    [Header("�ൿ��ư ����")]
    public GameObject tutorialUI;
    public GameObject[] behaviorButtons;
    public GameObject onBehaviorButton;
    public GameObject behaviorSetting;
    public Behavior nextBehavior;
    public Sprite[] keySprite; // �� �Ʒ� ���� ������ , ����
    public int keyInputCount = 0; // ��ų3��
    public int behaviorSequence = 0; // 1�ϸ��� 3�� �ൿ�ϴ°�
    public Image[] inGameKeyInput;
    public Text currentTurnText;
    public int turn = 0;

    [Header("ü�°� ����")]
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

    [Header("���� ����")]
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
        turn++; // 1���߰���
        player.InitBehavior(); // �ൿ������ �ʱ�ȭ
        enemy.EnemySetBehavior(); // �� �ൿ �ٽü���

        keyInputCount = 0; // �ൿ�Է���Ƚ�� �ʱ�ȭ
        for (int i=0; i<3; i++)
        {
            inGameKeyInput[i].sprite = null;
        }
        currentTurnText.text = $"{turn} Turn";
    }

    public void LoadPanel() // �ൿ��ư�������г� ���̰�
    {
        behaviorSetting.SetActive(true);
        onBehaviorButton.SetActive(false);
    }

    public void HidePanel() // �ൿ��ư�������г� �Ⱥ��̰�
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
            Debug.LogError("3���̻� �Է��ϼ̽��ϴ�.");
        }
    }

    public void CheckHPMP()
    {
        Debug.Log("ü��üũ");
        playerHPText.text = $"{player.hp} / 100"; // UI ������Ʈ
        playerHPGauge.fillAmount = player.hp / 100f;

        playerMPText.text = $"{player.mp} / 100"; // UI ������Ʈ
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
        else if (player.hp <= 0) // �� 0���� üũ
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
                    if (isplayer) // �÷��̾� �����Ͻ�
                    {
                        player.mp -= 10;

                        GameObject attackEffect = Instantiate(attack1Prefab, player.attack1Trm.position, Quaternion.identity); // ��������Ʈ
                        Destroy(attackEffect, attackDelayTime);
                        StartCoroutine(AttackDecision(true, 1, (0,0),(1,0) )); // player = true, ������ġ�κ��� 0,0�� 1,0�� ����
                    }
                    else // ���� �����Ͻ�
                    {
                        enemy.mp -= 10;

                        #region ���ּ�
                        /*if (sliceMap[enemy.currentY, enemy.currentX - 1] != null)
                        {
                            sliceMap[enemy.currentY, enemy.currentX - 1].SetColor(Color.red); // ���ݹ��� ǥ��
                        }
                        if (sliceMap[enemy.currentY, enemy.currentX] != null)
                        {
                            sliceMap[enemy.currentY, enemy.currentX].SetColor(Color.red);
                        }*/
                        #endregion
                        GameObject attackEffect = Instantiate(attack1Prefab, enemy.attack1Trm.position, Quaternion.identity); // ��������Ʈ
                        Destroy(attackEffect, attackDelayTime);
                        StartCoroutine(AttackDecision(false, 1, (0, 0), (-1, 0))); // player = true, ������ġ�κ��� 0,0�� 1,0�� ����
                    }
                    break;
                    // yield return new WaitForSeconds(attackDelayTime);

                    //if (isplayer) // �÷��̾� �����Ͻ�
                    //{

                    //}
                    //else // ���� �����Ͻ�
                    //{
                    //    if (sliceMap[enemy.currentY, enemy.currentX - 1] == sliceMap[player.currentY, player.currentX]) // ��������
                    //        isDamage = true;
                    //}

                    //if (sliceMap[player.currentY, player.currentX] == sliceMap[enemy.currentY, enemy.currentX])
                    //{
                    //    Debug.Log("���ƴ�");
                    //    isDamage = true;
                    //}


                    //if (isDamage)
                    //{
                    //    Debug.Log("�Ǳ���");
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

                if (isplayer) // �÷��̾� �����Ͻ�
                {
                    player.mp -= 40;

                    GameObject attackEffect = Instantiate(attack2Prefab, player.attack2Trm.position, Quaternion.identity); // ��������Ʈ
                    Destroy(attackEffect, attackDelayTime);
                    StartCoroutine(AttackDecision(true, 2, (0, 0), (0, 1), (1,1),(1,0),(0,-1),(1,-1))); // player = true, ������ġ�κ��� 0,0�� 1,0�� ����
                }
                else // ���� �����Ͻ�
                {
                    enemy.mp -= 20;

                    GameObject attackEffect = Instantiate(attack2Prefab, enemy.attack2Trm.position, Quaternion.identity); // ��������Ʈ
                    Destroy(attackEffect, attackDelayTime);
                    StartCoroutine(AttackDecision(false, 2, (0, 0), (0,1), (-1,1),(-1,0),(0,-1),(-1,-1))); // player = true, ������ġ�κ��� 0,0�� 1,0�� ����
                }
                CheckHPMP();
                break;

            case (int)Behavior.Shield:
                Debug.Log("�ǵ� �̱���");
                break;
            default:
                Debug.Log("���ݽ�ų�� �ƴѰŰ�����");
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
        Debug.Log("�̰� y:" + targetXY1.Value.y + ", x:" + targetXY1.Value.x);
        Debug.Log(targetXY1.HasValue);
        // ����Ÿ�� ����ȭ Red
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

        yield return new WaitForSeconds(attackDelayTime); // ����Ÿ�� �������ǽð� (���� ������󺹱� �� ��������Ȯ��)

        // ����Ÿ�� ����ȭ White

        bool isDamage = false; // �������� ������ true�� ����

        if (isPlayer)
        {
            if (targetXY1 != null)
            {
                if (sliceMap[player.currentY + targetXY1.Value.y, player.currentX + targetXY1.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY1.Value.y, player.currentX + targetXY1.Value.x].SetColor(Color.white);
            }
            if (targetXY2 != null)
            {
                if (sliceMap[player.currentY + targetXY2.Value.y, player.currentX + targetXY2.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY2.Value.y, player.currentX + targetXY2.Value.x].SetColor(Color.white);
            }
            if (targetXY3 != null)
            {
                if (sliceMap[player.currentY + targetXY3.Value.y, player.currentX + targetXY3.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY3.Value.y, player.currentX + targetXY3.Value.x].SetColor(Color.white);
            }
            if (targetXY4 != null)
            {
                if (sliceMap[player.currentY + targetXY4.Value.y, player.currentX + targetXY4.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY4.Value.y, player.currentX + targetXY4.Value.x].SetColor(Color.white);
            }
            if (targetXY5 != null)
            {
                if (sliceMap[player.currentY + targetXY5.Value.y, player.currentX + targetXY5.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY5.Value.y, player.currentX + targetXY5.Value.x].SetColor(Color.white);
            }
            if (targetXY6 != null)
            {
                if (sliceMap[player.currentY + targetXY6.Value.y, player.currentX + targetXY6.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY6.Value.y, player.currentX + targetXY6.Value.x].SetColor(Color.white);
            }
            if (targetXY7 != null)
            {
                if (sliceMap[player.currentY + targetXY7.Value.y, player.currentX + targetXY7.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY7.Value.y, player.currentX + targetXY7.Value.x].SetColor(Color.white);
            }
            if (targetXY8 != null)
            {
                if (sliceMap[player.currentY + targetXY8.Value.y, player.currentX + targetXY8.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY8.Value.y, player.currentX + targetXY8.Value.x].SetColor(Color.white);
            }
            if (targetXY9 != null)
            {
                if (sliceMap[player.currentY + targetXY9.Value.y, player.currentX + targetXY9.Value.x] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                    isDamage = true;

                sliceMap[player.currentY + targetXY9.Value.y, player.currentX + targetXY9.Value.x].SetColor(Color.white);
            }

        }
        else
        {
            if (targetXY1 != null)
            {
                if (sliceMap[enemy.currentY + targetXY1.Value.y, enemy.currentX + targetXY1.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY1.Value.y, enemy.currentX + targetXY1.Value.x].SetColor(Color.white);
            }
            if (targetXY2 != null)
            {
                if (sliceMap[enemy.currentY + targetXY2.Value.y, enemy.currentX + targetXY2.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY2.Value.y, enemy.currentX + targetXY2.Value.x].SetColor(Color.white);
            }
            if (targetXY3 != null)
            {
                if (sliceMap[enemy.currentY + targetXY3.Value.y, enemy.currentX + targetXY3.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY3.Value.y, enemy.currentX + targetXY3.Value.x].SetColor(Color.white);
            }
            if (targetXY4 != null)
            {
                if (sliceMap[enemy.currentY + targetXY4.Value.y, enemy.currentX + targetXY4.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY4.Value.y, enemy.currentX + targetXY4.Value.x].SetColor(Color.white);
            }
            if (targetXY5 != null)
            {
                if (sliceMap[enemy.currentY + targetXY5.Value.y, enemy.currentX + targetXY5.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY5.Value.y, enemy.currentX + targetXY5.Value.x].SetColor(Color.white);
            }
            if (targetXY6 != null)
            {
                if (sliceMap[enemy.currentY + targetXY6.Value.y, enemy.currentX + targetXY6.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY6.Value.y, enemy.currentX + targetXY6.Value.x].SetColor(Color.white);
            }
            if (targetXY7 != null)
            {
                if (sliceMap[enemy.currentY + targetXY7.Value.y, enemy.currentX + targetXY7.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY7.Value.y, enemy.currentX + targetXY7.Value.x].SetColor(Color.white);
            }
            if (targetXY8 != null)
            {
                if (sliceMap[enemy.currentY + targetXY8.Value.y, enemy.currentX + targetXY8.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY8.Value.y, enemy.currentX + targetXY8.Value.x].SetColor(Color.white);
            }
            if (targetXY9 != null)
            {
                if (sliceMap[enemy.currentY + targetXY9.Value.y, enemy.currentX + targetXY9.Value.x] == sliceMap[player.currentY, player.currentX]) // ��������
                    isDamage = true;

                sliceMap[enemy.currentY + targetXY9.Value.y, enemy.currentX + targetXY9.Value.x].SetColor(Color.white);
            }
        }

        // ������ �¾ƶ�������
        if (isDamage)
        {
            Debug.Log("�Ǳ���");
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

