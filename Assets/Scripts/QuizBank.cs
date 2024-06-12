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

    string[,] Questions = {
        {"A, B, ... ", "Are you dead ?", "What is the opposite of Yes ?", "C, B, ..."},
        {"C", "Yes", "No", "A"}
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
        int chosen = Random.Range(0,Questions.GetLength(1));
        Debug.Log("The chosen is : "+ Questions.GetLength(1));
        currentAnswer = Questions[1,chosen];
        QuestionBar.text = Questions[0, chosen];
        string[] temp = new string[Questions.GetLength(1)];
        string[] temp2 = new string[4];
        temp2[0] = currentAnswer;
        int random;

        for(int i = 0; i <Questions.GetLength(1); i++){
            temp[i] = Questions[1,i];
        }
        Utilities.Shuffle(temp);
        int j=0;
        for(int i = 1; i <4; i++){
            if(temp[j] == currentAnswer){
                j++;
            }
            temp2[i] = temp[j];
            j++;
        }
        Utilities.Shuffle(temp2);
        for(int i = 0 ; i<4; i++){
            Answers[i].text = temp2[i];
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


