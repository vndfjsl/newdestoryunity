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

    void Start()
    {
        SetMap();
        PlayerSpawn();
        InitButtonIndex();
        HidePanel();
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

    public void CheckHP()
    {
        Debug.Log("ü��üũ");
        playerHPText.text = player.hp.ToString(); // UI ������Ʈ
        playerHPGauge.fillAmount = player.hp / 100f;

        enemyHPText.text = enemy.hp.ToString();
        enemyHPGauge.fillAmount = enemy.hp / 100f;



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
        if (isplayer)
        {
            StartCoroutine(ShowAttackCollision(attackType));
        }
        else
        {
            // ���� ����.
        }
            

            // ��������
            // ���ݹ��� ����
    }

    public IEnumerator ShowAttackCollision(int attackType)
    {
        switch(attackType)
        {
            case (int)Behavior.KnifeAttack:
                {
                    bool isDamage = false;

                    sliceMap[player.currentY, player.currentX + 1].SetColor(Color.red);
                    sliceMap[player.currentY, player.currentX].SetColor(Color.red);

                    yield return new WaitForSeconds(1f);

                    if (sliceMap[player.currentY, player.currentX + 1] == sliceMap[enemy.currentY, enemy.currentX])
                        isDamage = true;
                    if (sliceMap[player.currentY, player.currentX] == sliceMap[enemy.currentY, enemy.currentX])
                    {
                        Debug.Log("���ƴ�");
                        isDamage = true;
                    }
                    if (isDamage)
                    {
                        Debug.Log("�Ǳ���");
                        enemy.hp -= 30;
                    }
                    CheckHP();
                    sliceMap[player.currentY, player.currentX].SetColor(Color.white);
                    sliceMap[player.currentY, player.currentX + 1].SetColor(Color.white);
                    
                }
                break;
            default:
                Debug.Log("���ݽ�ų�� �ƴѰŰ�����");
                break;
        }
        
    }
}
