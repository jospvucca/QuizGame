using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Question[] _questions;

    public Text _questionText;
    public Text _answerA;
    public Text _answerB;
    public Text _answerC;

    private int _correctAnswerID;

    public GameObject _responsePanel;
    public Text _responseText;

    private static List<Question> _unansweredQuestions;

    private static int _score = 0;
    private static int _numberOfAnsweredQuestions = 0;
    private static float _time = 0.0f;

    public GameObject _gameOverPanel;
    public Text _gameOverScore;
    public Text _timeScore;
    private bool _timerSetActive = false;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverPanel.SetActive(false);
        if(_unansweredQuestions != null && _unansweredQuestions.Count == 0)
        {
            _gameOverPanel.SetActive(true);
            _gameOverScore.text = "Score: " + _score.ToString() + "/" + _numberOfAnsweredQuestions.ToString();
            _timeScore.text = "Time: " + _time.ToString("0.00");
            _timerSetActive = false;
            StartCoroutine(GameOverPause());            
        }
        if(_unansweredQuestions == null)
        {
            _unansweredQuestions = _questions.ToList();
        }

        _responsePanel.SetActive(false);
        SetCurrentQuestion();
        _timerSetActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerSetActive == true)
            Timer();
    }

    void SetCurrentQuestion()
    {
        Question _question = PickRandomQuestion();
        _questionText.text = _question._questionText;
        List<Answer> answers = new List<Answer>();
        List<Answer> shuffledAnswers = new List<Answer>();

        answers.Add(_question._answer1);
        answers.Add(_question._answer2);
        answers.Add(_question._answer3);

        for (int i = 0; i < 3; i++)
        {
            int id = Random.Range(0, answers.Count);
            if(answers[id]._isCorrect == true)
            {
                _correctAnswerID = i;
            }
            shuffledAnswers.Add(answers[id]);
            answers.RemoveAt(id);
        }

        _answerA.text = shuffledAnswers[0]._text;
        _answerB.text = shuffledAnswers[1]._text;
        _answerC.text = shuffledAnswers[2]._text;
    }

    public void GetAnswer(int buttonID)
    {
        _numberOfAnsweredQuestions++;
        _responsePanel.SetActive(true);
        if(buttonID == _correctAnswerID)
        {
            _responseText.text = "Correct!";
            _responsePanel.GetComponent<Image>().color = new Color32(0, 255, 255, 255);
            _score++;
        }
        else
        {
            _responseText.text = "Wrong!";
            _responsePanel.GetComponent<Image>().color = new Color32(200, 50, 0, 255);
        }
        StartCoroutine(TransitionToNextQuestion());
    }

    IEnumerator TransitionToNextQuestion()
    {
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    Question PickRandomQuestion()
    {
        Question theQuestion;
        int id = Random.Range(0, _unansweredQuestions.Count);
        theQuestion = _unansweredQuestions[id];
        _unansweredQuestions.Remove(theQuestion);

        return theQuestion;
    }

    void Timer()
    {
        _time += Time.deltaTime;
        Debug.Log("Time: " + _time.ToString());
    }

    IEnumerator GameOverPause()
    {
        yield return new WaitForSeconds(5);
    }

    public void StopTimerOnClick()
    {
        _timerSetActive = false;
    }
}
