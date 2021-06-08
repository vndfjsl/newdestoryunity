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
    KnifeAttack
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

    

    public TileBox[] inputMap;
    public TileBox[,] sliceMap = new TileBox[3, 4];

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private Player player;
    private Enemy enemy;


    public int turn = 0;

    public int keyInputCount = 0; // ��ų3��
    public int behaviorSequence = 0; // 1�ϸ��� 3�� �ൿ�ϴ°�

    public Sprite[] keySprite; // �� �Ʒ� ���� ������ , ����
    public Image[] inGameKeyInput;

    public Text currentTurnText;


    [Header("�ൿ��ư ����")]
    public GameObject tutorialUI;
    public GameObject[] behaviorButtons;
    public GameObject onBehaviorButton;
    public GameObject behaviorSetting;
    public Behavior nextBehavior;

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

    public void AttackProcess(int attackType, bool isplayer)
    {
            StartCoroutine(ShowAttackCollision(attackType, isplayer));
            

            // ��������
            // ���ݹ��� ����
    }

    public IEnumerator ShowAttackCollision(int attackType, bool isplayer)
    {
        bool isDamage = false;

        switch (attackType)
        {
            case (int)Behavior.KnifeAttack:
                {
                    if (isplayer) // �÷��̾� �����Ͻ�
                    {
                        player.mp -= 10;

                        if (sliceMap[player.currentY, player.currentX + 1] != null)
                        {
                            sliceMap[player.currentY, player.currentX + 1].SetColor(Color.red); // ���ݹ��� ǥ��
                        }
                        if (sliceMap[player.currentY, player.currentX] == null)
                        {
                            sliceMap[player.currentY, player.currentX].SetColor(Color.red);
                        }

                        GameObject attack1Effect = Instantiate(attack1Prefab, player.attack1Trm.position, Quaternion.identity); // ��������Ʈ
                        Destroy(attack1Effect, attackDelayTime);
                    }
                    else // ���� �����Ͻ�
                    {
                        enemy.mp -= 10;

                        if (sliceMap[enemy.currentY, enemy.currentX - 1] != null)
                        {
                            sliceMap[enemy.currentY, enemy.currentX - 1].SetColor(Color.red); // ���ݹ��� ǥ��
                        }
                        if (sliceMap[enemy.currentY, enemy.currentX] != null)
                        {
                            sliceMap[enemy.currentY, enemy.currentX].SetColor(Color.red);
                        }

                        GameObject attack1Effect = Instantiate(attack1Prefab, enemy.attack1Trm.position, Quaternion.identity); // ��������Ʈ
                        Destroy(attack1Effect, attackDelayTime);
                    }

                    yield return new WaitForSeconds(attackDelayTime);

                    if (isplayer) // �÷��̾� �����Ͻ�
                    {
                        if (sliceMap[player.currentY, player.currentX + 1] == sliceMap[enemy.currentY, enemy.currentX]) // ��������
                            isDamage = true;
                    }
                    else // ���� �����Ͻ�
                    {
                        if (sliceMap[enemy.currentY, enemy.currentX - 1] == sliceMap[player.currentY, player.currentX]) // ��������
                            isDamage = true;
                    }
                    
                    if (sliceMap[player.currentY, player.currentX] == sliceMap[enemy.currentY, enemy.currentX])
                    {
                        Debug.Log("���ƴ�");
                        isDamage = true;
                    }


                    if (isDamage)
                    {
                        Debug.Log("�Ǳ���");
                        if (isplayer)
                        {
                            enemy.hp -= 30;
                        }
                        else
                        {
                            player.hp -= 30;
                            sliceMap[enemy.currentY, enemy.currentX].SetColor(Color.white);
                            sliceMap[enemy.currentY, enemy.currentX - 1].SetColor(Color.white);
                        }
                    }

                    CheckHPMP();
                    TileClear();

                    
                }
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
}

