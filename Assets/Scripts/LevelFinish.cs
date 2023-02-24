using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] float loadingNextLevelTime = 2f;
    bool hasWaterMelon = false;
    SpriteRenderer finishSpriteRenderer;

    void Start() 
    {
        finishSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && hasWaterMelon)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(loadingNextLevelTime);
        Scene scene = SceneManager.GetActiveScene();
        int nextScene = ChooseNextScene(scene.buildIndex);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(nextScene);
        FindObjectOfType<GameSession>().TurnOffExitUI();   
    }

    int ChooseNextScene(int index) 
    {
        return (index + 1) % 3 ;
    }

    public void GetWaterMelon()
    {
        hasWaterMelon = true;
        Vector4 actualColor = finishSpriteRenderer.color;
        actualColor.w = 1;
        finishSpriteRenderer.color = actualColor;
        FindObjectOfType<GameSession>().TurnOnExitUI();
    }
}
