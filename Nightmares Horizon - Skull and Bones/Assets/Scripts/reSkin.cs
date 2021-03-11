using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class reSkin : MonoBehaviour
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


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        idPersonagemSelecionado = PlayerPrefs.GetInt("idPersonagem");
        LoadSpriteSheet();

    }

    // Update is called once per frame
    void Update()
    {
        LateUpdate();
    }


    private void LateUpdate()
    {
        if (idPersonagemAtual != idPersonagemSelecionado)
        {
            LoadSpriteSheet();
        }

        spriteRenderer.sprite = spriteSheet[spriteRenderer.sprite.name];
    }


    private void LoadSpriteSheet() {

        if (idPersonagemSelecionado == 1) {
        spriteSheetName = "rutabaga";
        sprites = Resources.LoadAll<Sprite>(spriteSheetName);
        spriteSheet = sprites.ToDictionary(x => x.name, x => x);
        loadedSpriteSheetName = spriteSheetName;
            idPersonagemAtual = 1;
            trocarPersonagem = false;     
        }

        if (idPersonagemSelecionado == 0) {
            spriteSheetName = "pink-the-non-human";
            sprites = Resources.LoadAll<Sprite>(spriteSheetName);
            spriteSheet = sprites.ToDictionary(x => x.name, x => x);
            loadedSpriteSheetName = spriteSheetName;
            idPersonagemAtual = 0;
            trocarPersonagem = false;
        }
    }
}
