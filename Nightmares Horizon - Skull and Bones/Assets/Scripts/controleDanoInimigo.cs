using System.Collections;
using TMPro;
using UnityEngine;

public class controleDanoInimigo : MonoBehaviour
{

    [Header("Configurações gerais")]
    private _GameController _GameController; //
    private playerScript player; //
    public GameObject inimigoObject;
    public Rigidbody2D inimigoRb; //Objeto que passa posição para o knockback
    public SpriteRenderer spriteRenderer; //
    public bool olhandoParaEsquerda; //
    public bool died;

    [Header("Configurações de Vida")]
    public int vidaInimigo;
    public int vidaAtual;
    public float percVida;
    public bool getHit;
    public GameObject barrasDeVida; //Objeto que contem todas as barras
    public Transform hpBar;         //Indicador da quantidade de vida
    public Color[] characterColor; //Array de cores para o personagem piscar após dano.
    public GameObject textoDanoLevadoPrefab;



    [Header("Configuração de Fraqueza/Resistência")]
    public int[] ajusteDano; //Sistema de resistencia e fraqueza

    [Header("Configurações de Loot")]
    public GameObject[] loots;

    /** Variaveis internas **/

    private Animator inimigoAnimator;


    // Start is called before the first frame update
    void Start()
    {

        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        player = FindObjectOfType(typeof(playerScript)) as playerScript;
        inimigoRb = GetComponent<Rigidbody2D>();
        barrasDeVida.SetActive(false);
        vidaAtual = vidaInimigo;
        hpBar.localScale = new Vector3(7.26f, 10, 1);
        inimigoAnimator = GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        Dead();
    }


    void KnockBack()
    {
        if (player.transform.position.x > transform.position.x)
        {

            inimigoRb.AddForce(new Vector2(-100, 80));

        }
        else if (player.transform.position.x < transform.position.x)
        {

            inimigoRb.AddForce(new Vector2(100, 80));

        }
    }

    void Flip()
    {

        if (died == false)
        {
            if (player.transform.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
                olhandoParaEsquerda = false;
            }
            else
            {
                spriteRenderer.flipX = false;
                olhandoParaEsquerda = true;
            }
        }
    }

    void Dead()
    {
        if (vidaAtual <= 0)
        {
            inimigoAnimator.SetInteger("idAnimation", 3);
            vidaAtual = 0;
            died = true;
            StartCoroutine(loot());
        }
    }
    /**
     void OnTriggerEnter2D(Collider2D col)
     {
         while (!getHit && !died)
         {
             getHit = true;
             switch (col.gameObject.tag)
             {
                 case "Arma":

                     armaInfo armaInfo = col.gameObject.GetComponent<armaInfo>();
                     inimigoAnimator.SetTrigger("hit");

                     int danoArma = Random.Range(armaInfo.danoMin, armaInfo.danoMax);
                     int tipoDeDano = armaInfo.tipoDeDano;
                     //Formula para ajuste de resistencia de dano
                     float danoTomado = danoArma + (danoArma * ajusteDano[tipoDeDano] / 100);
                     vidaAtual -= (int)danoTomado;

                     //print("Tomei " + danoTomado + " de dano do tipo " + _GameController.tiposDeDanos[tipoDeDano]);
                     percVida = (float)vidaAtual + 1 / (float)vidaInimigo;
                     if (percVida <= 0) percVida = 0f;
                     hpBar.localScale = new Vector3(percVida, 10, 1);

                     ExibirDanoTomado(tipoDeDano, danoTomado);
                     KnockBack();
                     Dead();
                     break;
             }

             StartCoroutine(invulnerabilidade());
             
         }
     }
    **/

    void OnTriggerEnter2D(Collider2D col)
     {
         if (!getHit && !died)
         {
            // getHit = true;
             switch (col.gameObject.tag)
             {

                 case "Arma":

                    getHit = true;
                    armaInfo armaInfo = col.gameObject.GetComponent<armaInfo>();
                     inimigoAnimator.SetTrigger("hit");

                     int danoArma = Random.Range(armaInfo.danoMin, armaInfo.danoMax);
                     int tipoDeDano = armaInfo.tipoDeDano;
                     //Formula para ajuste de resistencia de dano
                     float danoTomado = danoArma + (danoArma * ajusteDano[tipoDeDano] / 100);
                     vidaAtual -= (int)danoTomado;

                     //print("Tomei " + danoTomado + " de dano do tipo " + _GameController.tiposDeDanos[tipoDeDano]);
                     percVida = (float)vidaAtual + 1 / (float)vidaInimigo;
                     if (percVida <= 0) percVida = 0f;
                     hpBar.localScale = new Vector3(percVida, 10, 1);

                     ExibirDanoTomado(tipoDeDano, danoTomado);
                     KnockBack();
                     StartCoroutine(invulnerabilidade());
                    break;
             }


             
         }
     }

    IEnumerator loot()
    {
        yield return new WaitForSeconds(1.2f);
        int quantidadeDeMoedas = Random.Range(1,4);
        
        for (int a = 0; a < quantidadeDeMoedas; a++)
        {
            GerarMoeda();
            yield return new WaitForSeconds(0.1f);

        }
       
        Destroy(this.gameObject);
    }

    void GerarMoeda() {
        //int rand = Random.Range(0, 0);
        GameObject lootTemp = Instantiate(loots[0], transform.localPosition, transform.localRotation);
        lootTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-80, 80), 40));
       Destroy(lootTemp.gameObject, 2f);
       
    }

    IEnumerator invulnerabilidade()
    {
        barrasDeVida.SetActive(true);
        //Sistema de brilho após dano
        GetComponent<Collider2D>().enabled = false;
        spriteRenderer.color = characterColor[0];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[1];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[0];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[1];
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = characterColor[0];
        GetComponent<Collider2D>().enabled = true;
        getHit = false;
        barrasDeVida.SetActive(false);
    }




    void ExibirDanoTomado(int tipoDeDano, float danoTomado)
    {
        //Efeito de Dano
        GameObject fxTemp = Instantiate(_GameController.fxDano[tipoDeDano], transform.localPosition, transform.localRotation);
        Destroy(fxTemp.gameObject, 1f);

        //Jogando valor de dano na tela
        GameObject danoTemp = Instantiate(textoDanoLevadoPrefab, transform.position, transform.localRotation);
        danoTemp.GetComponent<TextMeshPro>().text = "-" + danoTomado.ToString();
        if (olhandoParaEsquerda) danoTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(20, 200));
        else danoTemp.GetComponent<Rigidbody2D>().AddForce(new Vector2(-20, 200));
        Destroy(danoTemp.gameObject, 0.50f);
    }
}
