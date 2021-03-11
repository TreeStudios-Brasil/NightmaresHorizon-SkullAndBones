using System.Collections;
using UnityEngine;

public class Skull : MonoBehaviour {

    private Rigidbody2D rigidBody;
    private Animator animator;
    public EnemyState currentState;
    public float velocidade;
    public float velocidadeBase;
    private Vector2 dir;
    public LayerMask layerMaskObstaculos;
    public LayerMask layerMaskPlayer;
    public bool lookLeft;
    private controleDanoInimigo controleDanoInimigo;
    private playerScript player;
    private bool parado;

    public enum EnemyState {
        Parado,
        Andando,
        Atacando
    }


    // Start is called before the first frame update
    void Start() {

        player = FindObjectOfType(typeof(playerScript)) as playerScript;
        controleDanoInimigo = FindObjectOfType(typeof(controleDanoInimigo)) as controleDanoInimigo;
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dir = Vector2.right;
        velocidadeBase = 1;

    }

    // Update is called once per frame
    void Update() {

        // Debug.DrawRay(transform.position, dir * 0.2f, Color.green);
       // Debug.DrawRay(transform.position, dir * 0.5f, Color.red);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, dir, 0.2f, layerMaskObstaculos);
        RaycastHit2D raycastAtack = Physics2D.Raycast(transform.position, dir, 1, layerMaskPlayer);

        rigidBody.velocity = new Vector2(velocidade, rigidBody.velocity.y);
        float dis = Vector3.Distance(transform.position, player.transform.position);

        //Andando
        if (currentState == EnemyState.Andando) {

            if (velocidade != 0 && controleDanoInimigo.getHit == false)
            {
                animator.SetInteger("idAnimation", 1);
            }

            if (raycastHit2D && !raycastAtack) {
                StartCoroutine(idle());
                //flip();
            }
        }

        //Atacando
        if (raycastAtack) {

            velocidade = velocidadeBase * 1.4f;
            if (dis < 0.66f) {
                changeState(EnemyState.Atacando);

            }


        }


        if (dis > 0.88f && currentState == EnemyState.Atacando)
        {
            changeState(EnemyState.Andando);
            velocidade = velocidadeBase;
        }

        if (controleDanoInimigo.vidaAtual < 0.1f){
            velocidade = 0;
        }

    }


    private void flip() {
        lookLeft = !lookLeft; //inverte o valor da variavel booleana
        float x = transform.localScale.x;
        x *= -1; //inverte o sinal do scale X
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        dir.x = x;
        velocidadeBase *= -1;
        velocidade = velocidadeBase;
        parado = false;

    }

    IEnumerator idle() {

        if(!parado) {
            parado = true;
            changeState(EnemyState.Parado);
            yield return new WaitForSeconds(5f);
            changeState(EnemyState.Andando);
            flip();

        }
    }

    void changeState(EnemyState newState) {

        currentState = newState;

        switch (newState) {

            case EnemyState.Parado:
                animator.SetInteger("idAnimation", 0);
                velocidade = 0;
                break;

            case EnemyState.Andando:
                velocidade = velocidadeBase;
                break;
            case EnemyState.Atacando:
                animator.SetInteger("idAnimation", 4);
                velocidade = 0;
                break;
        }
    }


}
