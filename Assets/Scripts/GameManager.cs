using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] GameObject TransparentScreen, QuestionScreen, EndScreen, PauseScreen, MenuScreen, NameScreen, LeaderboardScreen, mutePause, muteMenu;
    [SerializeField] TMP_Text rank, score, textOfNameInput;
    [SerializeField] TMP_InputField NameInput, emailInput, numberInput;
    [SerializeField] PlayfabManager manager;
    [SerializeField] GameObject boardHolder;

    TouchScreenKeyboard key;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        if(AudioListener.volume == 0){
            if(mutePause != null){
                mutePause.SetActive(true);
            }
            if(muteMenu != null){
                muteMenu.SetActive(true);
            }
        }
        else{
            if(mutePause != null){
                mutePause.SetActive(false);
            }
            if(muteMenu != null){
                muteMenu.SetActive(false);
            }            
        }

        
        
    }

    // Update is called once per frame
    void Update()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount=0;
        // Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow, new RefreshRate() { numerator = 60, denominator = 1 });
    }


    public static void QuestionOn(){
        Instance.TransparentScreen.SetActive(true);
        Instance.QuestionScreen.SetActive(true);
    }
    public static void QuestionOff(){
        Instance.TransparentScreen.SetActive(false);
        Instance.QuestionScreen.SetActive(false);
    }

    public static void GameOver(){
        Instance.TransparentScreen.SetActive(true);
        Instance.EndScreen.SetActive(true);
        Instance.QuestionScreen.SetActive(false);
        Instance.manager.SendLeaderboard(ScoreManager.GetScore());
        
    }
    public static void Pause(){
        Time.timeScale = 0;
        Instance.TransparentScreen.SetActive(true);
        Instance.PauseScreen.SetActive(true);
    }
    public static void Unpause(){
        Instance.PauseScreen.SetActive(false);
        Instance.TransparentScreen.SetActive(false);
        
        Time.timeScale = 1;
    }
    public static void ShowMenuScreen(){
        UpdateRankAndScore();
        Instance.NameScreen.SetActive(false);
        Instance.MenuScreen.SetActive(true);
        Instance.LeaderboardScreen.SetActive(false);
        
    }
    public static void ShowNameScreen(){
        Instance.NameScreen.SetActive(true);
        

    }

    public static TMP_InputField GetNameInput(){
        return Instance.NameInput;
    }
    public static TMP_InputField GetEmail(){
        return Instance.emailInput;
    }
    public static TMP_InputField GetNumber(){
        return Instance.numberInput;
    }    

    public void GoLeaderboard(){
        
        Instance.LeaderboardScreen.SetActive(true);
        ResetLeaderBoard();
        Instance.MenuScreen.SetActive(false);
        manager.GetLeaderboard();
    }
    public void Restart(){
        
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        // Instance.TransparentScreen.SetActive(false);
        // Instance.EndScreen.SetActive(false);
    }

    public void GoMenu(){
        ScreenLoader.firstTime = false;
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        
        
        
    }

    public void StartGame(){
        // SceneManager.LoadScene(0);
        ScreenLoader.loadLevel = true;
    }

    public static void UpdateRankAndScore(){
        Instance.manager.GetYourRankAndScore(ref Instance.rank, ref Instance.score);
    }

    public static void LoadLeaderboard(){
        Instance.ResetLeaderBoard();
        Instance.manager.GetLeaderboard2();

        
    }

    public void Mute(){
        AudioListener.volume =  0;
        if(mutePause != null){
            mutePause.SetActive(true);
        }
        else if(muteMenu != null){
            muteMenu.SetActive(true);
        }
    }
    public void Unmute(){
        AudioListener.volume =  1;
        if(mutePause != null){
            mutePause.SetActive(false);
        }
        else if(muteMenu != null){
            muteMenu.SetActive(false);
        }
    }

     void ResetLeaderBoard(){
        int childCount = boardHolder.transform.childCount;

        // Start from the second child (index 1) and destroy each one
        for (int i = 1; i < childCount; i++)
        {
            Destroy(boardHolder.transform.GetChild(i).gameObject);

        }
    }

    // public void OpenKeyboard(){
    //     Debug.Log("Keyboard Opened");
        
    //     key = TouchScreenKeyboard.Open("ll", TouchScreenKeyboardType.Default); 
        
    // }
    // public void ChangeInput(){
    //     Debug.Log("text is : " + key.text);
    //     NameInput.text = key.text;
    // }



}
