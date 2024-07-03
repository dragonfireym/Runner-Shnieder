using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class QuizBank : MonoBehaviour
{
    [SerializeField] TMP_Text QuestionBar;
    [SerializeField] List<TMP_Text> Answers;

    // string[,] Questions = {
    //     {"A, B, ... ", "Are you dead ?", "What is the opposite of Yes ?", "C, B, ..."},
    //     {"C", "Yes", "No", "A"}
    // };
    string[,] Questions = {
        {"When was the first SE contactor invented?", "1924", "1928", "1938", "1934"},
        {"What is the name of the upgraded version of TVS?", "Easy TeSys", "TeSys TVS", "ProTeSys", "Ultra TVS"},
        {"TeSys Island provides what percentage savings?", "40%", "30%", "50%", "60%"},
        {"What does the 7 indicate in LC1E0901M7?", "50/60 Hz", "50 Hz", "60 Hz", "60/50 Hz"},
        {"What is the Current rating range of TeSys Deca?", "9 to 150 A", "0 to 16 A", "115 to 800 A", "10 to 100 A"},
        {"Name of Schneider Electric contactors family?", "TeSys", "Deca", "MagneFix", "PowerContact"}
    };

    string currentAnswer;
    // Start is called before the first frame update
    void Awake()
    {
        StartQuiz();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartQuiz(){
        int chosen = Random.Range(0,Questions.GetLength(0));
        currentAnswer = Questions[chosen,1];
        QuestionBar.text = Questions[chosen, 0];
        // string[] temp = new string[Questions.GetLength(1)];
        string[] temp = new string[4];
        // temp2[0] = currentAnswer;
        int random;

        for(int i = 0; i <4; i++){
            temp[i] = Questions[chosen,i+1];
        }
        Utilities.Shuffle(temp);
        // int j=0;
        // for(int i = 1; i <4; i++){
        //     if(temp[j] == currentAnswer){
        //         j++;
        //     }
        //     temp2[i] = temp[j];
        //     j++;
        // }
        // Utilities.Shuffle(temp2);
        for(int i = 0 ; i<4; i++){
            Answers[i].text = temp[i];
        }

    }

    public void CheckAnswer(){
        TMP_Text clicked = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TMP_Text>();
        if(clicked.text == currentAnswer){

            GameManager.QuestionOff();
            Time.timeScale = 1f;
            Powers.powerTimer = true;
            Powers.powerDuration = 3f;
            StartQuiz();
        }
        else{
            GameManager.GameOver();
        }
    }


}


