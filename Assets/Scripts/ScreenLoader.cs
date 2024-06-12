using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenLoader : MonoBehaviour
{
    public static bool firstTime = true;
    public static bool loadLevel = false;
    [SerializeField] GameObject loadingScreen , menuScreen, nameScreen;
    [SerializeField] Slider slider;
    Coroutine c_load;
    // Start is called before the first frame update
    void Start()
    {
        c_load = StartCoroutine(LoadScreen());
    }

    void Update(){
        if(loadLevel){
            c_load = StartCoroutine(LoadScreen());
            loadLevel = false;
        }
    }

    IEnumerator LoadScreen(){
        float elapsedTime = 0f;
        float timeChosen = 1f;

        
        if(loadLevel){
            loadingScreen.SetActive(true);
            menuScreen.SetActive(false);
            AsyncOperation op =  SceneManager.LoadSceneAsync(1);
            while(!op.isDone){
                float progress = Mathf.Clamp01(op.progress / 0.9f);
                slider.value = progress;
                yield return null;
            }

        }
        else{
            while (elapsedTime < timeChosen)
            {
                        
                elapsedTime += Time.deltaTime;
                slider.value = elapsedTime / timeChosen;
                yield return null; // Wait for the next frame
            }
            loadingScreen.SetActive(false);
            
            if(firstTime){
                
                nameScreen.SetActive(true);
                GameManager.LoadLeaderboard();
                firstTime = false;        
            }
            else{
                GameManager.LoadLeaderboard();
                GameManager.UpdateRankAndScore();
                menuScreen.SetActive(true);
                
            }
        }
        
        
    }
}
