using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button reiniciarButton;

    private bool gameOverActivo = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
    }
        else{
            Destroy(gameObject);
        }
    }
            
            

    void Start()
    {
        if (gameOverPanel != null) 
            gameOverPanel.SetActive(false);

        if (gameOverText != null)
            reiniciarButton.onClick.AddListener(ReiniciarEscena);

    }


    // Update is called once per frame
    void Update()
    {
        if (!gameOverActivo)
        {
            if (Input.GetKeyUp(KeyCode.R)) {
                ReiniciarEscena();
        }

    }
    }

    public void GameOver()
    {
        if (gameOverActivo) return;

        gameOverActivo = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER";
        }
    }

    public void ReiniciarEscena()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
