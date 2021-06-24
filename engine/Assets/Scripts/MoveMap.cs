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
    public int stage = 1;

    [Header("�÷��̾�� ��")]
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Player player;
    public Enemy enemy;

    [Header("�ൿ��ư ����")]
    public GameObject tutorialUI;
    public GameObject[] behaviorButtons;
    public GameObject onBehaviorButton;
    public Text onBehaviorText;
    public GameObject behaviorSetting;
    public Behavior nextBehavior;
    public Button nextBehaviorButton; // �ൿ �������Ĵ����°�. �ӽ÷ι޾ƿ�
    public Sprite[] keySprite; // �� �Ʒ� ���� ������ , ����
    public int keyInputCount = 0; // ��ų3��
    public int behaviorSequence = 0; // 1�ϸ��� 3�� �ൿ�ϴ°�
    public Image[] inGameKeyInput;
    public Image[] enemyGameKeyInput;
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
    public GameObject attack1Prefab; // �ܰ�
    public GameObject attack2Prefab; // ����ũ
    public GameObject attack3Prefab; // ����
    public GameObject attack4Prefab; // â���
    public Image[] attackCollisionPoints; // �ൿ�гο����� ���ݹ��� ǥ��.
    public int reminder;



    private AudioSource audioSource;
    [Header("�� ��")]
    public AudioClip swordSound;
    public AudioClip spearSound;
    public AudioClip shieldSound;

    [Header("��")]
    public Color32 tileColor;


    private Dictionary<Behavior, SkillDataVO> typeData = new Dictionary<Behavior, SkillDataVO>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //Ÿ�Ժ� �����ڽ�Ʈ å��
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

        SkillDataVO buffData1 = new SkillDataVO(attack3Prefab, -20, 0, new List<int>() { 5 }, new List<Vector2>() { new Vector2(0, 0) }, new Vector3(0.5f,0,0), true, "shield"); // �� ��
        typeData.Add(Behavior.Shield, buffData1);

        SkillDataVO attackData3 = new SkillDataVO(attack4Prefab, 50, 50, new List<int>() { 4, 5, 6 }, new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0) }, new Vector3(0.5f, 0f, 0f), false, "sword");
        typeData.Add(Behavior.Spear, attackData3);

        //Ÿ�Ժ� ������ å��

        
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
        if(stage <= 1) // 1�������� ���ϸ�
        {
            typeData.Remove(Behavior.Spear);
            behaviorButtons[(int)Behavior.Spear].SetActive(false);
            enemy.specialAttack1able = false;
        }
        else // 2��������
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
        // HidePanel(); // �ൿ���ÿϷ�ϱ� �г�������
        yield return new WaitForSeconds(0.5f); // �гβ��µ��� ���ð�

        if (keyInputCount < 3)
        {
            Debug.Log("��ų 3���� ������� �ʾҽ��ϴ�!");
            yield break;
        }
        nextBehaviorButton.interactable = false;
        

        for (int i=0; i<3; i++)
        {
            player.Move();
            enemy.EnemySetBehavior();
            enemy.Move();
            
            enemyGameKeyInput[i].sprite = keySprite[enemy.nextBehavior[i]];
            // �� �ൿ �����ֱ�


            // Debug.Log($"X: {player.currentX}, Y: {player.currentY}");
            
            yield return new WaitForSeconds(2f);

            if(CheckHPMP())
            {
                yield break;
            }

            player.armor = 0;
            enemy.armor = 0; // �ϳ����� �� �ʱ�ȭ

            
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
        Debug.Log("��");
        player.mp += 30;
        enemy.mp += 30;
        onBehaviorButton.GetComponent<Image>().DOFade(1f,0.5f); // �ٽ��Է�
        onBehaviorText.gameObject.SetActive(true);
        turn++; // 1���߰���
        player.InitBehavior(); // �ൿ������ �ʱ�ȭ
        enemy.nextBehavior.Clear(); // ���� �ൿ����Ʈ �ʱ�ȭ

        // enemy.EnemySetBehavior(); // �� �ൿ �ٽü���
        InitShowEnemyBehavior();

        keyInputCount = 0; // �ൿ�Է���Ƚ�� �ʱ�ȭ
        for (int i=0; i<3; i++)
        {
            inGameKeyInput[i].sprite = null; // ��ųUI �ʱ�ȭ
        }
        currentTurnText.text = $"{turn} Turn";
        nextBehaviorButton.interactable = true;
    }

    public void OnlyPlayerBhReset() // ���� ���¸����ִ³�
    {
        keyInputCount = 0;
        player.InitBehavior(); // �ൿ������ �ʱ�ȭ
        for (int i = 0; i < 3; i++)
        {
            inGameKeyInput[i].sprite = null; // ��ųUI �ʱ�ȭ
        }
    }

    public void LoadPanel() // �ൿ��ư�������г� ���̰�
    {
        behaviorSetting.SetActive(true);
        // onBehaviorButton.GetComponent<Image>().DOFade(0f, 0.5f); // �߰��Է¹���
        onBehaviorButton.GetComponent<Image>().DOFade(0f, 0.5f);
        onBehaviorText.gameObject.SetActive(false);
    }

    public void HidePanel() // �ൿ��ư�������г� �Ⱥ��̰�
    {
        onBehaviorButton.GetComponent<Image>().DOFade(1f, 0.5f); // �ٽ��Է�
        onBehaviorText.gameObject.SetActive(true);
        behaviorSetting.SetActive(false);
    }

    public void InitattackCollision()
    {
        foreach (Image image in attackCollisionPoints)
        {
            image.color = new Color32(231,231,231,255); // �ʱ�ȭ
        }
    }

    public void PressKey(int buttonIndex)
    {
        if (reminder == buttonIndex || buttonIndex <= 3) // �Ȱ����Ÿ� || �̵��̸�
        {
            reminder = -1;
            if (buttonIndex <= 3) // ���ݾƴϸ� �ʱ�ȭ, �ƴϸ� ���������
                InitattackCollision();

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
        else // �ȶȰ����Ÿ�
        {
            InitattackCollision();

            reminder = buttonIndex;
            SkillDataVO data = typeData[(Behavior)buttonIndex];
            bool playerPos = true;

            foreach(int index in data.skillCollisions)
            {
                if (playerPos) // ��ó���̸�(�÷��̾���ġ)
                {
                    if (data.isBuff)
                    {
                        attackCollisionPoints[index - 1].color = Color.yellow;
                    }
                    else
                    {
                        attackCollisionPoints[index - 1].color = Color.blue; // �÷��̾���ġ
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

    public bool CheckHPMP() // true ���Ͻ� ���ӳ�
    {

        if (player.hp <= 0 && enemy.hp <= 0)
        {
            Debug.Log("Draw!");
            drawPanel.SetActive(true);
            drawPanel.GetComponent<Image>().DOFade(1f, 0.5f);
            return true;
        }
        else if (player.hp <= 0) // �� 0���� üũ
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
        Debug.Log("ü��üũ");
        playerHPText.text = $"{player.hp} / 100"; // UI ������Ʈ
        playerHPGauge.fillAmount = player.hp / 100f;

        playerMPText.text = $"{player.mp} / 100"; // UI ������Ʈ
        playerMPGauge.fillAmount = player.mp / 100f;

        enemyHPText.text = $"{enemy.hp} / 150";
        enemyHPGauge.fillAmount = enemy.hp / 150f;

        enemyMPText.text = $"{enemy.mp} / 100";
        enemyMPGauge.fillAmount = enemy.mp / 100f;
    }

    public IEnumerator ShowAttackCollision(Behavior attackType, bool isplayer)
    {
        yield return new WaitForSeconds(0.1f); // �̵����� ���߿� �����ϱ� ���Ͽ�

        SkillDataVO data = typeData[attackType];
        Vector3 position; // ���ݳ�������ġ

        if (isplayer) // �÷��̾��� ��
        {
            player.mp -= data.cost; // �÷��̾� �����Ҹ�
            position = player.transform.position + data.position; // ��ġ����
        }
        else // ���� ��
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
        // ����Ÿ�� ����ȭ Red

        bool isDamage = false; // �������� ������ true�� ����
        bool isBuff = buff; // �Ϲݰ����� �ƴ� ȸ��, ��ȣ�� ���� ������ true�� ����

        foreach (Vector2 v in attackPoints)
        {
                Vector2 attackInVector = v;
            if (!isPlayer) // ���̸�
            {
                attackInVector.x *= -1;
                attackInVector += new Vector2(enemy.currentX, enemy.currentY);
            }
            else // �÷��̾��
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
                        enemy.armor += 15; // �ӽ�
                    }
                    sliceMap[(int)attackInVector.y, (int)attackInVector.x].SetColor(Color.yellow); // ����ȭ ��/��ȣ��
                }
                else
                {
                    

                    if ((sliceMap[(int)attackInVector.y, (int)attackInVector.x] == sliceMap[enemy.currentY, enemy.currentX] && isPlayer) || // �÷��̾� ��������
                     (sliceMap[(int)attackInVector.y, (int)attackInVector.x] == sliceMap[player.currentY, player.currentX] && !isPlayer))// �� ��������
                    {
                        sliceMap[(int)attackInVector.y, (int)attackInVector.x].SetColor(Color.red); // ����ȭ
                        isDamage = true;
                    }
                    else
                    {
                        sliceMap[(int)attackInVector.y, (int)attackInVector.x].SetColor(Color.green); // ����ȭ(�ȸ���)
                    }
                }

                
            }

                //v�� ���ݹ��� ������� üũ�ϰ�
                // �ش� ��ġ �Ӱ� ���ϰ��ϰ�
        }
        

        yield return new WaitForSeconds(attackDelayTime); // ����Ÿ�� �������ǽð� (���� ������󺹱� �� ��������Ȯ��)

        GameObject attackEffect = Instantiate(attackPrefab, attackEffectPosition, Quaternion.identity); // ��������Ʈ����
        Destroy(attackEffect, attackDelayTime);

        PlaySound(soundName);

        Sequence seq = DOTween.Sequence();

        attackEffect.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0);
        seq.Append(attackEffect.GetComponent<SpriteRenderer>().DOFade(1f, 0.4f));

        if (!isPlayer) // ���̸� ����Ʈ ȸ��
        {
            attackEffect.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        

        if (isDamage)
        {
            float shakePower = 0.5f;
            Debug.Log("�Ǳ���");


            if (isPlayer)
            {
                if(enemy.armor != 0)
                {
                    Debug.Log($"�� �� {enemy.armor}, ���ݵ����� {attackDamage}");
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
                if (attackPrefab.name == attack4Prefab.name) // �ӽ÷� â ����. ������ dataVO�� �־���ҵ�. TODO
                {
                    if (enemy.currentX < 3)
                    {
                        enemy.PushOut();
                    }
                }
                enemy.hp -= attackDamage - enemy.armor; // ���������� �� ����
            }
            else
            {
                if(player.armor != 0)
                {
                    Debug.Log($"�÷��̾� �� {player.armor}, ���ݵ����� {attackDamage}");
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
                if (attackPrefab.name == attack4Prefab.name) // �ӽ÷� â ����. ������ dataVO�� �־���ҵ�. TODO
                {
                    if (player.currentX > 0)
                    {
                        player.PushOut();
                    }
                }
                player.hp -= attackDamage - player.armor; // ���������� �� ����
                
            }

            Camera.main.DOShakePosition(2f, shakePower); // ���ݹ޾Ƽ� ȭ����鸲
        }

        ShowHPMP();
        TileClear(); // ����Ÿ�� ����ȭ White
    }
}

