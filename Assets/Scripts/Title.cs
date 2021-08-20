using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
	AudioSource startBombSE;
    [SerializeField]
    private Text omaenokachi;

    private void Start()
    {
        omaenokachi.text = "貴様の価値は時価" + Bird.scorePoint.ToString() + "円";
        startBombSE = GetComponent<AudioSource>();
    }

    public void OnClick()
	{
		startBombSE.PlayOneShot(startBombSE.clip);
        StartCoroutine(GameRoad());
	}

    IEnumerator GameRoad()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Main");
    }
}
