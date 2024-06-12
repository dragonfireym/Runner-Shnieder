using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get;  set; }
      [SerializeField] TMP_Text scoreText;
    [SerializeField] List<TMP_Text> islandText, gigaText, decaText, easyText ;
    int score = 0, island= 0, giga= 0, deca= 0, easy= 0;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeInHierarchy){
            UpdatePowersText();
        }
    }
    public static void UpdateScore(int x){
        Instance.score+= x;
        Instance.scoreText.text = Instance.score.ToString();
    }

    public static void UpdatePowerScore(char x){
        if(x == 'd'){
            Instance.deca++;
        }
        else if (x == 'g'){
            Instance.giga++;
        }
        else if ( x== 'e'){
            Instance.easy++;
        }
        else{
            Instance.island++;
        }
    }
    public static int GetScore(){
        return Instance.score;
    }
    void UpdatePowersText(){
        // /islandText, gigaText, decaText, easyText
        foreach(TMP_Text temp in islandText){
            temp.text = island.ToString();
        }
        foreach(TMP_Text temp in gigaText){
            temp.text = giga.ToString();
        }
        foreach(TMP_Text temp in decaText){
            temp.text = deca.ToString();
        }
        foreach(TMP_Text temp in easyText){
            temp.text = easy.ToString();
        }
    }
}
