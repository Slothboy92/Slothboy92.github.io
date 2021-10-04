using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoop : MonoBehaviour
{
    public struct Question
    {
        public string questionText;
        public string[] answers;
        public int correctAnswerIndex;

        public Question(string cur_questionText, string[] cur_answers, int cur_correctAnswerIndex)
        {
            this.questionText = cur_questionText;
            this.answers = cur_answers;
            this.correctAnswerIndex = cur_correctAnswerIndex;
        }
    }

    public Question currentQuestion = new Question("What is your favorite color?", new string[] { "Red", "Green", "Blue", "Yellow", "Magenta" }, 2);

    public Button[] answerButtons;
    public Text questionDisplayText;
    public GameObject[] triviaPanels;
    public GameObject resultsPanel;
    public Text resultsText;
    public GameObject feedbackText;

    private Question[] questionBank = new Question[10];
    private int currentQuestionIndex;
    private int[] questionNumbersChosen = new int[5];
    private int questionsFinished;
    private int numberOfCorrectAnswers = 0;
    private bool allowSelection = true;
  
    void AssignQuestion(int questionNum)
    {
        currentQuestion = questionBank[questionNum];
        questionDisplayText.text = currentQuestion.questionText;
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].GetComponentInChildren<Text>().text = currentQuestion.answers[i];

        }
    }

    public void CheckAnswer(int buttonNum)
    {
        if (allowSelection)
        {
            if (buttonNum == currentQuestion.correctAnswerIndex)
            {
                print("Correct");
                feedbackText.GetComponent<Text>().text = "Correct";
                feedbackText.GetComponent<Text>().color = Color.green;
                numberOfCorrectAnswers++;
            }
            else
            {
                print("incorrect");
                feedbackText.GetComponent<Text>().text = "Incorrect";
                feedbackText.GetComponent<Text>().color = Color.red;
            }

            StartCoroutine(WaitForFeedback());
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {

        questionBank[0] = new Question("What is the answer?", new string[] { "yes", "no", "nope", "not this one", "yeah... no" }, 1);
        questionBank[1] = new Question("What is the capital of antartica?", new string[] { "polar bear peak", "frosty lane", "peinguin cove", "germany", "yeah... no" }, 2);
        questionBank[2] = new Question("What is the porpoise of life?", new string[] { "steve", "42", "cheese curds", "to repopulate this rock we are stuck on until we fixes that for us", "chocolate" }, 3);
        questionBank[3] = new Question("alabama is in which country?", new string[] { "murica", "germany", "canada", "france", "drusselstein" }, 0);
        questionBank[4] = new Question("when was the office produced?", new string[] { "yesterday", "5/31/05", "3/24/05", "3/7/04", "banana" }, 3);
        questionBank[5] = new Question("Arby has what?", new string[] { "nuggets", "the meats", "child labor", "fries", "Betty white" }, 4);
        questionBank[6] = new Question("When will i get a job?", new string[] { "yes", "no", "never", "trick question", "yesterday" }, 1);
        questionBank[7] = new Question("who is bill murry?", new string[] { "Martin Heiss", "boss", "Bob", "Baloo", "Steve Jobs" }, 3);
        questionBank[8] = new Question("where does canada live?", new string[] { "up", "north", "left", "by alaska", "by alabama" }, 1);
        questionBank[9] = new Question("what color is our tooth brush", new string[] { "blue", "white", "pink", "yellow", "gray" }, 1);

        ChooseQuestions();
        AssignQuestion(questionNumbersChosen[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void ChooseQuestions()
    {
        for (int i = 0; i < questionNumbersChosen.Length; i++)
        {
            int questionNum = Random.Range(0, questionBank.Length);
            if (NumberNotContained(questionNumbersChosen, questionNum))
            {
                questionNumbersChosen[i] = questionNum;
            }
            else
            {
                i--;
            }
        }

        currentQuestionIndex = Random.Range(0, questionBank.Length);
    }

    bool NumberNotContained(int[] numbers, int num)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if (num == numbers[i])
            {
                return false;
            }
        }
        return true;
    }

    public void MoveToNextQuestion()
    {
        AssignQuestion(questionNumbersChosen[questionNumbersChosen.Length - 1 - questionsFinished]);
    }

    void DisplayResults()
    {

        switch(numberOfCorrectAnswers)
        {

            case 5:
                resultsText.text = "5 of 5 Correct. Great Job";
                break;
            case 4:
                resultsText.text = "4 of 5 Correct. Very Good";
                break;
            case 3:
                resultsText.text = "3 of 5 Correct. Well Done";
                break;
            case 2:
                resultsText.text = "2 of 5 Correct. Better Luck Next Time";
                break;
            case 1:
                resultsText.text = "1 of 5 Correct. You Suck!";
                break;
            case 0:
                resultsText.text = "0 of 5 Correct. You are the definition of trash";
                break;
            default:
                print("Not A number");
                break;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator WaitForFeedback()
    {
        allowSelection = false;
        feedbackText.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        if (questionsFinished < questionNumbersChosen.Length - 1)
        {
            MoveToNextQuestion();
            questionsFinished++;
        }
        else
        {
            foreach (GameObject panel in triviaPanels)
            {
                panel.SetActive(false);
            }
            resultsPanel.SetActive(true);
            DisplayResults();
        }
        feedbackText.SetActive(false);
        allowSelection = true;

    }
}
