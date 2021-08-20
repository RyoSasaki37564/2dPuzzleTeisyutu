using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "貴様の価値は時価" + Bird.scorePoint.ToString() + "円";
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "貴様の価値は時価" + Bird.scorePoint.ToString() + "円";
    }
}
