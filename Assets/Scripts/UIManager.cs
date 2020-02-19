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

    [SerializeField]
    private Slider _thrusterFuel_UI;  //Thruster fuel UI Slider ref @inspector

    [SerializeField]
    private Slider _ammoCount_UI;   //Ammo UI Slider ref @inspector

    [SerializeField]
    private Text _textStatusFuel_UI;    //Status Text ref @inspector
    public static string _statusTextFuel;   //Status text string

    [SerializeField]
    private Text _textStatusAmmo_UI;    //Status Text ref @inspector
    public static string _statusTextAmmo;   //Status text string

    [SerializeField]
    private Text _textStatusMines_UI;    //Status Text ref @inspector
    public static string _statusTextMines;   //Status text string

    private float _currentFuel;
    private int _currentAmmo, _currentMines;

    //START
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
    
    //Update UI: FUEL
    public void Update_ThrusterFuel(float fuel)
    {
        _thrusterFuel_UI.value = fuel;
       
    }

    //Update UI: AMMO
    public void Update_AmmoCount(int ammo)
    {
        _ammoCount_UI.value = ammo;
        
    }

    public void UpdateStatusText(int ammo, float fuel, int mines, int ammoMax, bool isRefuel, bool isReload)
    {
        _currentMines = mines;
        _currentAmmo = ammo;
        _currentFuel = fuel;
        float fuelPercent = Mathf.Floor(_currentFuel * 100);
        //_statusText = ">STATUS<\nFuel: " + fuelPercent.ToString() + "\nAmmo: " + _currentAmmo.ToString() +  "%\nMines: " + _currentMines.ToString();
        if (isRefuel) { _statusTextFuel = "RE-\nFUEL"; _textStatusFuel_UI.color = new Color(Random.value, Random.value, Random.value); } else {_statusTextFuel = "FUEL\n" + fuelPercent.ToString() + "%"; _textStatusFuel_UI.color = Color.red; }
        if (isReload) { _statusTextAmmo = "RE-\nLOAD (" + _currentAmmo + ")"; _textStatusAmmo_UI.color = new Color(Random.value, Random.value, Random.value); } else { _statusTextAmmo = "AMMO\n " + _currentAmmo.ToString() + " (" + ammoMax + ")"; _textStatusAmmo_UI.color = Color.grey; }
        _statusTextMines = "MINES\n " + _currentMines.ToString();

        //_textStatus_UI.color = new Color(Random.value, Random.value, Random.value);
    }

    private void Update()
    {
        _textStatusFuel_UI.text = _statusTextFuel;
        _textStatusAmmo_UI.text = _statusTextAmmo;
        _textStatusMines_UI.text = _statusTextMines;
        
    }
}
