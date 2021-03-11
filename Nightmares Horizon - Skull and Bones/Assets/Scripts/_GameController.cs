using TMPro;
using UnityEngine;


public enum GameState {
    PAUSE,
    GAMEPLAY

}

public class _GameController : MonoBehaviour
{

    [Header ("Dados gerais do Jogo")]

    public string[] tiposDeDanos;
    
    // Sistemas de resistencias e fraquezas contra determinados objetos.
    public int[] ajusteDano;
    // Sistemas de efeitos por tipo de dano.
    public GameObject[] fxDano;

    public GameState currentState;


    [Header ("Dados do player no Jogo")]

    // Configurações do player
    public int gold;
    // Configurações HUD
    public TextMeshProUGUI textQuantdMoedas;

    [Header("Banco de dados das Armas")]
    
    public Sprite[] spriteArmas0;
    public Sprite[] spriteArmas1;
    public Sprite[] spriteArmas2;

    public int[] danoMinArma;
    public int[] danoMaxArma;
    public int[] tipoDeDano;

    [Header ("Painéis")]
    public GameObject pausePanel;


    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Cancel")) {
            PauseGame();
        }

        textQuantdMoedas.SetText(gold.ToString());
    }




    public void PauseGame(){
        bool pauseState = pausePanel.activeSelf;

        pausePanel.SetActive(!pausePanel.activeSelf);

        switch (pauseState){
            case true:
            Time.timeScale = 1;
             changeState(GameState.GAMEPLAY);
            break;
            case false:
            Time.timeScale = 0;
                changeState(GameState.PAUSE);
            break;
        }
    }

    void changeState (GameState newState){

        currentState = newState;

    }

}
