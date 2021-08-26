using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Neat.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public enum SceneIndexes
    {
        TITLE_SCREEN = 0,
        GAMEPLAY = 1
    }

    public static LoadingScreen instance;
    public GameObject loadingScreen;
    public ProgressBar bar;
    public TextMeshProUGUI textField;

    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    private void Awake()
    {
        instance = this;

        // SceneManager.LoadSceneAsync((int)SceneIndexes.TITLE_SCREEN, LoadSceneMode.Additive);
    }

    public void LoadGame()
    {
        loadingScreen.gameObject.SetActive(true);
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)SceneIndexes.TITLE_SCREEN));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes.GAMEPLAY, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    float totalSceneProgress;

    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;

                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;
                textField.text = "Hey";
                bar.current = Mathf.RoundToInt(totalSceneProgress);

                yield return null;
            }
        }
    }
}
