using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Behavior
{
    UP,
    DOWN,
    LEFT,
    RIGHT
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

    

    public Transform[] inputMap;
    public Transform[,] sliceMap = new Transform[3, 4];

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    private Player player;
    private Enemy enemy;


    public int turn = 0;

    public int keyInputCount = 0;
    public int behaviorSequence = 0; // 1�ϸ��� 3�� �ൿ�ϴ°�

    public Sprite[] keySprite; // �� �Ʒ� ���� ������
    public Image[] inGameKeyInput;

    public Text currentTurnText;
    

    [Header("�ൿ��ư ����")]
    public GameObject[] behaviorButtons;
    public GameObject onBehaviorButton;
    public GameObject behaviorSetting;
    public int skillSequence = 0; // ���Ͽ� ��ų3��
    public Behavior nextBehavior;

    void Start()
    {
        SetMap();
        PlayerSpawn();
        InitButtonIndex();
        HidePanel();
    }

    //void Update()
    //{
    //      {
    //        //else if(Input.GetKeyDown(KeyCode.UpArrow))
    //        //{
    //        //    inputKeys.Add(KeyCode.UpArrow);
    //        //    keyInputCount++;
    //        //    DisplayKeyInput();
    //        //}
    //        //else if (Input.GetKeyDown(KeyCode.DownArrow))
    //        //{
    //        //    inputKeys.Add(KeyCode.DownArrow);
    //        //    keyInputCount++;
    //        //    DisplayKeyInput();
    //        //}
    //        //else if (Input.GetKeyDown(KeyCode.LeftArrow))
    //        //{
    //        //    inputKeys.Add(KeyCode.LeftArrow);
    //        //    keyInputCount++;
    //        //    DisplayKeyInput();
    //        //}
    //        //else if (Input.GetKeyDown(KeyCode.RightArrow))
    //        //{
    //        //    inputKeys.Add(KeyCode.RightArrow);
    //        //    keyInputCount++;
    //        //    DisplayKeyInput();
    //        //}
    //    }
    //    // inputWant�� Ʈ���϶� Ű3���ޱ�
    //}

    public void InitButtonIndex()
    {
        for(int i=0; i<behaviorButtons.Length; i++)
        {
            behaviorButtons[i].GetComponent<BehaviorButton>().buttonIndex = i;
        }
    }

    public void SetMap()
    {
        for(int i=0; i<3; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                sliceMap[i,j] = inputMap[4 * i + j];
                Debug.Log(sliceMap[i, j].position);
            }
        }
    }

    public void PlayerSpawn()
    {
        player = Instantiate(playerPrefab, sliceMap[1, 0].position, Quaternion.identity).GetComponent<Player>();
        enemy = Instantiate(enemyPrefab, sliceMap[1, 3].position, Quaternion.identity).GetComponent<Enemy>();
    }

    public void NextBehavior()
    {
        player.Move();
        enemy.Move();

        Debug.Log($"X: {player.currentX}, Y: {player.currentY}");
        player.transform.position = sliceMap[player.currentY, player.currentX].position;
        enemy.transform.position = sliceMap[enemy.currentY, enemy.currentX].position;

        behaviorSequence++;
        if(behaviorSequence >= 3)
        {
            NextTurn();
        }
    }

    public void NextTurn()
    {
        turn++; // 1���߰���
        behaviorSequence = 0;
        player.InitBehavior(); // �ൿ������ �ʱ�ȭ
        keyInputCount = 0; // �ൿ�Է���Ƚ�� �ʱ�ȭ
        for(int i=0; i<3; i++)
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
        player.nextBehavior.Add(buttonIndex);
        keyInputCount++;
        inGameKeyInput[skillSequence].sprite = keySprite[buttonIndex];
        skillSequence = (skillSequence + 1) % 3; // 0 1 2 ���ư��鼭 �ݺ��ϴ°�
    }
}
