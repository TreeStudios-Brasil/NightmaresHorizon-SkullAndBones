using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class selecaoPersonagem : MonoBehaviour
{

    //teste
    public bool trocarPersonagem;
    //teste
    private int idPersonagemAtual;
    public int idPersonagemSelecionado;


    public Sprite[] sprites;
    public string spriteSheetName;
    public string loadedSpriteSheetName;

    private SpriteRenderer spriteRenderer;
    private Dictionary<string, Sprite> spriteSheet;
    public TextMeshProUGUI textoNome;


    private float h;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idPersonagemAtual = 0;
        LoadSpriteSheet();


    }

    // Update is called once per frame
    void Update()
    {
        h =Input.GetAxis("Horizontal");
        LateUpdate();
    }


    private void LateUpdate()
    {
        if (idPersonagemAtual != idPersonagemSelecionado)
        {
            LoadSpriteSheet();
        }

        if(h < 0){
            SelecionandoPersonagem(0);
        }
        if (h > 0){
            SelecionandoPersonagem(1);
        }

        spriteRenderer.sprite = spriteSheet[spriteRenderer.sprite.name];
    }


    private void LoadSpriteSheet() {

        if (idPersonagemSelecionado == 1 ) {
            spriteSheetName = "rutabaga";
            sprites = Resources.LoadAll<Sprite>(spriteSheetName);
            spriteSheet = sprites.ToDictionary(x => x.name, x => x);
            loadedSpriteSheetName = spriteSheetName;
            idPersonagemAtual = 1;
            trocarPersonagem = false;
        }

        if (idPersonagemSelecionado == 0 ) {
            spriteSheetName = "pink-the-non-human";
            sprites = Resources.LoadAll<Sprite>(spriteSheetName);
            spriteSheet = sprites.ToDictionary(x => x.name, x => x);
            loadedSpriteSheetName = spriteSheetName;
            idPersonagemAtual = 0;
            trocarPersonagem = false;
        }
    }



    public  void SelecionandoPersonagem(int id){

        PlayerPrefs.SetInt("idPersonagem", id);
        idPersonagemSelecionado = id;
        switch (id){
            case 0:
                textoNome.SetText("PINK THE NON-HUMAN");

                break;

            case 1:
                textoNome.SetText("RUTABAGA");
                break;
        }
    }

    public void StartGame(){

        SceneManager.LoadScene("MainScene");

    }
}
