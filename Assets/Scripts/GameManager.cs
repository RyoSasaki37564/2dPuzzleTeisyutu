using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float m_playTime = 60.0f; //操作時間のもと
    private int m_tachableTime; //実際の操作可能時間

    [SerializeField]
    private Text m_gameStateText;//ゲームの状態を通達する
    [SerializeField]
    private Text m_timerText; //残り時間表示

    public static bool m_gameSetFlg = false; //ゲームの終わりに振るフラグ。このフラグが立つと操作が無効になる

    [SerializeField]
    private AudioSource m_finishSE;

    // Start is called before the first frame update
    void Start()
    {
        m_gameSetFlg = false;
        m_gameStateText.text = "はじめい！";
        StartCoroutine(TextResetter());
        StartCoroutine(GameSetter());
        StartCoroutine(ResultTimer());
        m_tachableTime = (int)m_playTime;
        m_timerText.text = m_tachableTime.ToString();
        m_finishSE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        m_playTime -= Time.deltaTime;
        m_tachableTime = (int)m_playTime;
        m_timerText.text = "残り" +  m_tachableTime.ToString() + "秒";//残り時間表示
        if(m_playTime <= 0)
        {
            m_playTime = 0;
        }
    }

    IEnumerator TextResetter()
    {
        yield return new WaitForSeconds(2.0f);
        m_gameStateText.text = ""; //ゲーム状態のテキストの空白化
    }
    IEnumerator GameSetter()
    {
        yield return new WaitForSeconds(m_playTime);
        m_gameSetFlg = true;
        m_gameStateText.text = "そこまで！";
        m_finishSE.PlayOneShot(m_finishSE.clip);
    }
    IEnumerator ResultTimer()
    {
        yield return new WaitForSeconds(m_playTime + 2.0f);
        SceneManager.LoadScene("Title");//今はタイトルに飛ばす。リザルトができ次第そちらに。
    }
}
