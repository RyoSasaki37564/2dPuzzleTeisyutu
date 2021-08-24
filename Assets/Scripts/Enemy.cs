using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Enemy : MonoBehaviour
{
    //シリアライズでCSVファイルをドラッグアンドドロップ。そしてこれを入れたオブジェクトをボタンの窓に持ってって簡単読み込み。
    [SerializeField] TextAsset m_csv = default;

    [SerializeField] List<Sprite> m_enemySprites = new List<Sprite>(); //敵の画像リスト
    SpriteRenderer m_eneSprite = default; //
    public int m_enemyHpMax; //敵最大体力
    public float m_enemyPower; //敵攻撃力
    [SerializeField]
    Text m_enemyName; //敵の名前

    public static int m_currentEHp; //現在の敵のHP

    public Text m_enemyHPNum; //敵体力の表示

    public Slider m_eHPSlider; //敵体力のバー

    public static float m_enemyAttack; //敵攻撃力

    public string[,] m_enemyMasterData; //そのクエストの敵データを二次元配列化

    public int m_battleCountNum; //そのクエストにおける戦闘回数

    private int m_nextBattleReader = 0;

    StringReader er;

    private bool m_kamishibai = false; //敵イラストを綺麗に入れ替えるため

    [SerializeField] int m_miniDame = 200;
    [SerializeField] int m_nomaDame = 300;
    [SerializeField] int m_bigDame = 600;

    void Awake()
    {
        m_eneSprite = GetComponent<SpriteRenderer>();

        m_kamishibai = false;

        er = new StringReader(m_csv.text);//今回のみそ。CVSを1次元配列として読み込んだ後、さらに２次元配列に落とし込む。

        m_battleCountNum = int.Parse(er.ReadLine()); //最初に読み込むのはそのステージでの戦闘回数

        m_enemyMasterData = new string[m_battleCountNum, 4]; //ID,名前,体力,攻撃力の4つ

        if (er != null)
        {

            for (var i = 0; i < m_battleCountNum; i++)
            {
                var line = er.ReadLine(); //2行目からはステージのデータを読み込む。
                string[] m_eStatus = line.Split(',');

                m_enemyMasterData[i, 0] = m_eStatus[0]; //そして見込んだデータは２次元配列化。
                m_enemyMasterData[i, 1] = m_eStatus[1];
                m_enemyMasterData[i, 2] = m_eStatus[2];
                m_enemyMasterData[i, 3] = m_eStatus[3];

                Debug.Log(m_enemyMasterData[i, 0]);
                Debug.Log(m_enemyMasterData[i, 1]);
                Debug.Log(m_enemyMasterData[i, 2]);
                Debug.Log(m_enemyMasterData[i, 3]);
            }
        }

        m_enemyName.text = m_enemyMasterData[0, 1];

        m_enemyHpMax = int.Parse(m_enemyMasterData[0, 2]);
        m_eHPSlider.maxValue = m_enemyHpMax;

        m_enemyPower = int.Parse(m_enemyMasterData[0, 3]);

        m_currentEHp = m_enemyHpMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_eHPSlider = GameObject.Find("EHPSlider").GetComponent<Slider>();

        m_enemyHPNum.text = m_currentEHp.ToString();

        m_enemyAttack = m_enemyPower;
    }

    // Update is called once per frame
    void Update()
    {

        m_eHPSlider.value = m_currentEHp;

        //ダメージ処理を書く
        if (Bird.m_miniAttackFlag == true)
        {
            m_currentEHp -= m_miniDame;
            m_enemyHPNum.text = m_currentEHp.ToString();

            Bird.m_miniAttackFlag = false;
        }
        if (Bird.m_nomalAttackFlag == true)
        {
            m_currentEHp -= m_nomaDame;
            m_enemyHPNum.text = m_currentEHp.ToString();

            Bird.m_nomalAttackFlag = false;
        }
        if (Bird.m_bigAttackFlag == true)
        {
            m_currentEHp -= m_bigDame;
            m_enemyHPNum.text = m_currentEHp.ToString();

            Bird.m_bigAttackFlag = false;
        }

        if (m_currentEHp <= 0 && m_kamishibai == false)
        {
            m_nextBattleReader++;
            m_kamishibai = true;
        }

        if (GameManager.m_turn == GameManager.Turn.NextBattleTurn)
        {
            if (m_nextBattleReader < m_battleCountNum)
            {
                m_eneSprite.sprite = m_enemySprites[m_nextBattleReader];

                m_enemyName.text = m_enemyMasterData[m_nextBattleReader, 1];

                m_enemyHpMax = int.Parse(m_enemyMasterData[m_nextBattleReader, 2]);
                m_eHPSlider.maxValue = m_enemyHpMax;

                m_enemyPower = int.Parse(m_enemyMasterData[m_nextBattleReader, 3]);

                m_currentEHp = m_enemyHpMax;
                m_enemyHPNum.text = m_currentEHp.ToString();

                if (m_nextBattleReader == m_battleCountNum)
                {
                    GameManager.m_gameSetFlag = true;
                    StartCoroutine(ResultSet());
                }
                else
                {
                    m_kamishibai = false;
                    StartCoroutine(NextBattleSet());
                }
            }
        }

    }

    IEnumerator ResultSet()
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("Result");
    }

    IEnumerator NextBattleSet()
    {
        yield return new WaitForSeconds(2.0f);
        GameManager.m_turn = GameManager.Turn.ResetTurn;
    }
}
