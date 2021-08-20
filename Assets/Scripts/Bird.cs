using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public GameObject[] birdPrefabs;

    // 連鎖判定用の鳥（ドロップ）の距離
    const float BirdDistance = 1.4f;

    //最低連鎖数
    const int MinChain = 3;

    // クリックされた鳥をリスト格納
    private GameObject firstBird;
    private GameObject lastBird;
    private string currentName;
    List<GameObject> removableBirdList = new List<GameObject>();

    //連鎖が分かるようにラインを引く
    List<GameObject> lineBirdList = new List<GameObject>();

    //ドロップ生成位置範囲
    [SerializeField] private float generatorRange = 1.9f;
    [SerializeField] private float generatorHight = 4.0f;

    [SerializeField]
    private GameObject lineObj;

    public static int scorePoint = 0;//得点

    AudioSource shimatuSE;

    // Start is called before the first frame update
    void Start()
    {
        scorePoint = 0;
        shimatuSE = GetComponent<AudioSource>();

        if (GameManager.m_gameSetFlg == false)
        {

            TouchManager.Began += (info) =>
            {

            //クリック地点でヒットしているオブジェクトを取得
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                    Vector2.zero);
                if (hit.collider != null)
                {
                    GameObject hitObj = hit.collider.gameObject;
                //ヒットしたオブジェクトのタグがRespawn(ドロップについてるタグ)だったら初期化し、itObjを登録  
                if (hitObj.tag == "Respawn")
                    {
                        firstBird = hitObj;
                        lastBird = hitObj;
                        currentName = hitObj.name;
                        removableBirdList = new List<GameObject>();
                        PushToBirdList(hitObj);
                    }
                }

            };


            TouchManager.Moved += (info) =>
                {
                //クリック地点でヒットしているオブジェクトを取得
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(info.screenPoint),
                        Vector2.zero);
                    if (hit.collider != null)
                    {
                        GameObject hitObj = hit.collider.gameObject;
                    //ヒットしたオブジェクトのタグがRespawn(ドロップについてるタグ)かつ、名前が一緒
                    //最後にヒットしたオブジェクトと違う、リストには入っていない
                    if (hitObj.tag == "Respawn" && hitObj.name == currentName &&
                        lastBird != hitObj && 0 > removableBirdList.IndexOf(hitObj))
                        {

                            float distance = Vector2.Distance(hitObj.transform.position, lastBird.transform.position);
                            if (distance > BirdDistance)
                            {
                                return;
                            }
                            PushToBirdList(hitObj, lastBird);
                            lastBird = hitObj;
                            PushToBirdList(hitObj);
                        }
                    }
                };
            TouchManager.Ended += (info) =>
            {
                int count = removableBirdList.Count;

                if (count >= MinChain)
                {
                // リストにいるドロップを消す
                foreach (GameObject obj in removableBirdList)
                    {
                        if (count == MinChain)
                        {
                            scorePoint += 100;
                        }
                        else if (count == MinChain + 1)
                        {
                            scorePoint += 120;
                        }
                        else if (count == MinChain + 2)
                        {
                            scorePoint += 150;
                        }
                        else if (count == MinChain + 3)
                        {
                            scorePoint += 180;
                        }
                        else if (count == MinChain + 4)
                        {
                            scorePoint += 220;
                        }
                        else if (count >= MinChain + 5)
                        {
                            scorePoint += 260;
                        }
                        else if (count >= MinChain + 7)
                        {
                            scorePoint += 300;
                        }
                        Destroy(obj);
                        shimatuSE.PlayOneShot(shimatuSE.clip);
                    }
                    StartCoroutine(DropBird(count));

                }
                foreach (GameObject obj in lineBirdList)
                {
                    Destroy(obj);
                }

                foreach (GameObject obj in removableBirdList)
                {
                    ChangeColor(obj, 1.0f);
                }

                removableBirdList = new List<GameObject>();
                lineBirdList = new List<GameObject>();
                firstBird = null;
                lastBird = null;

            };

        }
        StartCoroutine(DropBird(52));
    }

    private void PushToBirdList(GameObject lastObj, GameObject hitObj)
    {
        GameObject line = (GameObject)Instantiate(lineObj);
        LineRenderer renderer = line.GetComponent<LineRenderer>();
        //線の太さ
        renderer.startWidth = 0.1f;
        renderer.endWidth = 0.1f;
        //頂点の数
        renderer.positionCount = 2;
        //頂点の数を設定
        renderer.SetPosition(0, new Vector3(lastObj.transform.position.x, lastObj.transform.position.y, -1.0f));
        renderer.SetPosition(1, new Vector3(hitObj.transform.position.x, hitObj.transform.position.y, -1.0f));
        //カラーを設定
        renderer.startColor = Color.white;
        renderer.endColor = Color.green;
        lineBirdList.Add(line);
    }


    private void PushToBirdList(GameObject obj)
    {
        removableBirdList.Add(obj);
        ChangeColor(obj, 0.5f);
    }

    private void ChangeColor(GameObject obj, float transparency)
    {
        SpriteRenderer birdTexture = obj.GetComponent<SpriteRenderer>();
        birdTexture.color = new Color(birdTexture.color.r,
            birdTexture.color.g,
            birdTexture.color.b,
            transparency);
    }



    //指定個数分鳥を発生させる
    IEnumerator DropBird(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //ランダム位置生成
            Vector2 pos = new Vector2 (Random.RandomRange(-1*generatorRange, generatorRange), generatorHight);
            int id = Random.Range(0, birdPrefabs.Length);

            //鳥を取り出す
            GameObject bird = (GameObject)Instantiate(birdPrefabs[id],
                pos,
                Quaternion.AngleAxis(Random.Range(-40, 40), Vector3.forward));
            //生成した鳥の名をIDを使って付け直す
            bird.name = "Bird" + id;

            //0.05秒待ってから次の処理
            yield return new WaitForSeconds(0.05f);

        }
    }

}
