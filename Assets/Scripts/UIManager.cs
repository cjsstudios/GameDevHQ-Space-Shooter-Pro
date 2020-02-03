using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to Text
    [SerializeField]
    private Text _scoreText;        //<Text> ref for score
    [SerializeField]
    private Image _livesImg;        //<Image> ref for lives
    [SerializeField]
    private Sprite[] _livesSprite;  //<Sprite> array ref for lives

    [SerializeField]
    private Text _gameOverText;     //<Text> ref for gameover text

    [SerializeField]
    private Text _restartText;      //<Text> ref for restart game text

    [SerializeField]
    private GameManager _gameManager;
    //[SerializeField]
    //private bool _canRestart = false; //**NOT IN USE

    //START
    //Hide GameOver & GameRestart text
    //Set <GameManager> ref
    void Start()
    {
        //Player player = GameObject.Find("Player").GetComponent<Player>();
        //assign text component to the handle
        _restartText.gameObject.SetActive(false);                                   //Set restart text inactive
        _gameOverText.gameObject.SetActive(false);                                  //Set gameOver text inactive
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>(); //Set <GameManager> ref
        if (_gameManager == null)                                                   //Null-check <GameManager>
        {
            Debug.LogError("Game_Manager is NULL");
        }
        _scoreText.text = "Score: " + 0;                                            //SET score to 0 on new game
    }

    //UPDATE SCORE TEXT
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    //UPDATE LIVES IMG WITH NUMBER OF LIVES
    public void UpdateLives(int currentLives)
    {
        //display img sprite
        //give it a new sprite based on current lives index
        _livesImg.sprite = _livesSprite[currentLives];      //SET lives image to current sprite array[lives]
        /*
         * NOT IN USE (followed tutorial)
        if (currentLives == 0)
        {
            _gameOverText.gameObject.SetActive(true);
        }
        */
    }

    //DISPLAY GAME OVER TEXT
    //CB: <Player> @Damage()
    public void DisplayGameOver()
    {
        StartCoroutine(GameOverFXRoutine());
    }

    //CB: DisplayGameOver()
    IEnumerator GameOverFXRoutine()
    {
        _restartText.gameObject.SetActive(true);        //Display restart text
        //_canRestart = true;
        _gameManager.GameOver();                        //Set GameOver toggle

        //Make GameOver Text Blink
        while (true)                                    //Infinate loop until player Quits or Restarts
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.75f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.75f);
        } 
    }
    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (_canRestart == true)
            {
                SceneManager.LoadScene("Game")
            }
        }
    }
    */
}
