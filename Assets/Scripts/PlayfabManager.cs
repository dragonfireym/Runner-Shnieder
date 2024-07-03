using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayfabManager : MonoBehaviour
{
    // public static PlayfabManager Instance{get; private set;}
    // Start is called before the first frame update
    [SerializeField] GameObject row, firstRow;
    [SerializeField] Transform rowParent;

    string id = "dd" , rank = null , score= null, storedID = null;

    void Start()
    {
        // Instance = this;
        Login();
    }

    // Update is called once per frame
    void Login()
    {
        
        var request = new LoginWithCustomIDRequest{
            CustomId= SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams  {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnError);
    }

    void OnLogin(LoginResult result){
        Debug.Log("Suceesfully logged in");
        GetLeaderboard2();
        storedID = result.PlayFabId;
        string name = null;
        if(result.InfoResultPayload.PlayerProfile!= null){
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        // if(name == null){
        //     GameManager.ShowMenuScreen();
        // }

    }
    void OnError(PlayFabError error){
        Debug.Log("ERROR");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score){
        var request = new UpdatePlayerStatisticsRequest{
            Statistics = new List<StatisticUpdate>{
                new StatisticUpdate{
                    StatisticName = "LeaderboardScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result){
        Debug.Log("Successfully updated");
    }

    public void GetLeaderboard(){
        var request = new GetLeaderboardRequest{
            StatisticName = "LeaderboardScore",
            StartPosition = 0 ,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }
    public void GetLeaderboard2(){
        var request = new GetLeaderboardRequest{
            StatisticName = "LeaderboardScore",
            StartPosition = 0 ,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, UpdateData, OnError);
    }
    void UpdateData(GetLeaderboardResult result){
        foreach(var item in result.Leaderboard){
            item.Position ++;
            Debug.Log("I am in leaderboard");
            if(storedID == item.PlayFabId){
                Debug.Log("I updated leaderboard");
                rank = item.Position.ToString();
                score = item.StatValue.ToString();
            }
        }
    }

    void OnLeaderboardGet(GetLeaderboardResult result){
        int count =1, counter = 0;
        
        foreach(var item in result.Leaderboard){
            item.Position ++;
            Debug.Log("I am in leaderboard");
            if(storedID == item.PlayFabId){
                
                rank = item.Position.ToString();
                score = item.StatValue.ToString();
            }
            string temp = item.DisplayName;
            if((counter <=2) && (temp != null)){
                GameObject newObj;
                if(count == 1){
                    newObj = firstRow;
                    count ++;
                }
                else{
                    newObj = Instantiate(row, rowParent);
                }
                
                newObj.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = item.Position.ToString();
                newObj.transform.GetChild(3).gameObject.GetComponent<TMP_Text>().text = item.DisplayName;
                newObj.transform.GetChild(4).gameObject.GetComponent<TMP_Text>().text = item.StatValue.ToString();
                counter ++;                
            }

        }
    }

    public void SubmitNameButton(){

        CheckDisplayName(GameManager.GetNameInput().text);
        if(!Validate()){
            return;
        }
        SaveInfo(GameManager.GetEmail().text, GameManager.GetNumber().text);
        var request = new UpdateUserTitleDisplayNameRequest{
            DisplayName = GameManager.GetNameInput().text
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate , OnError);

    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result){
        Debug.Log("Successfully updated name");
        GameManager.ShowMenuScreen();
    }
    void CheckDisplayName(string displayName)
    {
        var request = new GetAccountInfoRequest { TitleDisplayName = displayName };
        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        // Display name exists
        Debug.Log("Display name already exists.");
    }

    private void OnGetAccountInfoFailure(PlayFabError error){
         if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
            Debug.Log("Display name is available.");
        }
        else
        {
            // Handle other possible errors
            Debug.LogError("Error occurred: " + error.GenerateErrorReport());
        }
    }
    bool Validate(){
        if(GameManager.GetNameInput().text.Length < 1){
            Debug.Log("Put your name");
            return false;
        }
        string temp = GameManager.GetEmail().text;
        if( (temp.Length< 1 )|| (!temp.Contains("@")) ){
            return false;
        }
        temp = GameManager.GetNumber().text;
        int i;
        if((temp.Length< 11 || temp.Length > 11 )|| (!int.TryParse(temp, out i))){
            Debug.Log("Put your number correctly, " + temp.Length + ": Invalid Length");
            return false;
        }
        return true;
    }


    public void GetYourRankAndScore(ref TMP_Text r , ref TMP_Text s ){
        r.text = rank;
        s.text = score;

        Debug.Log("The id is "+ rank +" and main id is "+ score);
        
    }


    void SaveInfo(string email, string number){
        var request = new UpdateUserDataRequest{
            Data = new Dictionary<string,string>{
                {"Email", email},
                {"Number",number}
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    void OnDataSend(UpdateUserDataResult request){
        Debug.Log("Successfully sent");
    }
}
