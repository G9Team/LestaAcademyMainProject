using System.Collections;
using System.Collections.Generic;
using New;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoaderHandler : MonoBehaviour
{
    public void Continue()
    {
        transform.Find("Bob Panel").gameObject.SetActive(false);
        StartCoroutine(
                SceneLoader.MainCoroutine(gameObject, SceneManager.GetActiveScene().buildIndex + 1));
    }
}

public class SceneLoader
{
    public static void LoadScene(string sceneName)
    {
        LoadScene(SceneManager.GetSceneByName(sceneName).buildIndex);
    }
    public static void ShowResult()
    {
        GameObject sceneCanvas = GameObject.Instantiate(Resources.Load("SceneLoaderCanvas") as GameObject);
        GameObject.DontDestroyOnLoad(sceneCanvas);
        New.PlayerComponentManager manager = GameObject.FindObjectOfType<PlayerComponentManager>();
        int collected = 0;
        if (manager != null)
        {
            collected = ((New.PlayerData)manager.GetPlayerData()).Coins;
        }
        sceneCanvas.transform.Find("ResultWindow/ResultCount").GetComponent<TMPro.TextMeshProUGUI>().text =
            $"СОБРАНО ЧЕРНИЛЬНЫХ КАПЕЛЬ - {collected}/10";
        sceneCanvas.transform.Find("ResultWindow/Next").GetComponent<Button>().onClick.AddListener(() =>
        {
            sceneCanvas.GetComponent<SceneLoaderHandler>().StartCoroutine(
                MainCoroutine(sceneCanvas, SceneManager.GetActiveScene().buildIndex));
        });
        sceneCanvas.transform.Find("ResultWindow/Restart").GetComponent<Button>().onClick.AddListener(() =>
        {
            if (collected == 10)
                sceneCanvas.transform.Find("Bob Panel").gameObject.SetActive(true);
            else
                sceneCanvas.GetComponent<SceneLoaderHandler>().StartCoroutine(
                MainCoroutine(sceneCanvas, SceneManager.GetActiveScene().buildIndex + 1));
        });
        //sceneCanvas.GetComponent<SceneLoaderHandler>().StartCoroutine(MainCoroutine(sceneCanvas,sceneIndex));
    }
    public static void LoadScene(int sceneIndex)
    {
        GameObject sceneCanvas = GameObject.Instantiate(Resources.Load("SceneLoaderCanvas") as GameObject);
        GameObject.DontDestroyOnLoad(sceneCanvas);
        sceneCanvas.GetComponent<SceneLoaderHandler>().StartCoroutine(
            MainCoroutine(sceneCanvas, sceneIndex));
        //sceneCanvas.GetComponent<SceneLoaderHandler>().StartCoroutine(MainCoroutine(sceneCanvas,sceneIndex));
    }

    public static IEnumerator MainCoroutine(GameObject sceneCanvas, int sceneIndex)
    {
        SaveManager sm = GameObject.FindObjectOfType<SaveManager>();
        if (sm != null && sceneIndex != SceneManager.GetActiveScene().buildIndex)
            sm.ChangeLevel(sceneIndex);
        Animator animator = sceneCanvas.GetComponent<Animator>();
        animator.SetTrigger("Start");
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return new WaitForSeconds(Time.deltaTime);
        animator.StopPlayback();
        Text loadingText = sceneCanvas.transform.Find("LoadWindow/LoadingText").GetComponent<Text>();
        Text hintText = sceneCanvas.transform.Find("LoadWindow/Hint").GetComponent<Text>(); //idk about hints for now
        Image fadeImage = sceneCanvas.transform.Find("LoadWindow/Fade").GetComponent<Image>();


        Color col = Color.black;
        while (col.a > 0)
        {
            col.a -= Time.deltaTime * 1.5f;
            fadeImage.color = col;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sceneCanvas.transform.Find("LoadWindow").gameObject.SetActive(true);
        AsyncOperation sceneLoadAsync = SceneManager.LoadSceneAsync(sceneIndex);
        while (!sceneLoadAsync.isDone)
        {
            loadingText.text = $"Loading {(int)(sceneLoadAsync.progress * 100f)}%";
            yield return new WaitForSeconds(Time.deltaTime);
        }
        loadingText.text = $"Loading 100%";
        while (col.a < 1f)
        {
            col.a += Time.deltaTime * 2f;
            fadeImage.color = col;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        sceneCanvas.transform.Find("LoadWindow").gameObject.SetActive(false);
        animator.SetTrigger("End");
        float normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 1f;
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < normalizedTime)
            yield return new WaitForSeconds(0.1f);
        GameObject.Destroy(sceneCanvas);
    }
}
