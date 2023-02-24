using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numOfScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if (numOfScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
                DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }

}
