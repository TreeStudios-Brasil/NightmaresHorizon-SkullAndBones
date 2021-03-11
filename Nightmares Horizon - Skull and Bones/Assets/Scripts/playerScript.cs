using UnityEngine;

public class playerScript : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    private _GameController _GameController;

    private Animator playerAnimator;
    private Rigidbody2D playerRb;

    [Header("Configurações de Chão")]
    public Transform groundCheck; //Objeto responsável por detectar se o personagem está sobre alguma superficie
    public bool grounded; // Indica se o personagem está no chão

    [Header("Configurações do Personagem")]
    public bool attacking; // Indica se o personagem está atacando 
    public int idAnimation; //Indica o Id da animação 
    public float speed; //Velocidade do personagem 
    public float jumpForce;
    public bool lookLeft;
    public Collider2D standing, crounching; //colisor em pé e sentado
    private Vector2 atualPosition;
    private float h, v;  //Indica a movimentação horizontal e vertical 
    private Vector3 horizontalDirection;
    public Transform hand;
    public LayerMask interacao;
    public bool pulando;
    public bool doubleJump;

    //Sistema de armas 
    public int idArma, idArmaAtual;
    public GameObject[] armas;


    // Start is called before the first frame update
    void Start() {
        _GameController = FindObjectOfType(typeof(_GameController)) as _GameController;
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        atualPosition = transform.position;
        horizontalDirection = transform.right;
        //garantindo que os gameobjects da arma esteja desativado (para nao aparecer todas)
        desativandoSpritesArma();
        TrocarArmas(idArma);

    }

    void FixedUpdate() {

        if (_GameController.currentState != GameState.GAMEPLAY){
            return;
        }

        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.01f);
        playerRb.velocity = new Vector2(h * speed, playerRb.velocity.y);
        interagir();
        if (attacking == false) {
            desativandoSpritesArma();
        }

    }


    // Update is called once per frame
    void Update() {

        if (_GameController.currentState != GameState.GAMEPLAY){
            return;
        }

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        bool crounched = Input.GetButtonDown("Fire1");



        //sistema para flipar jogador
        if (h > 0 && lookLeft == true)
        {
            flip();
        }
        else if (h < 0 && lookLeft == false)
        {
            flip();
        }

        //abaixado
        if (v < 0 && grounded && h == 0)
        {
            idAnimation = 3;
        }

            //olhando pra cima
        else if (v > 0 && grounded && h == 0) {
            idAnimation = 4;
        }
            //andando
        else if (h != 0 && crounched == false)
        {
            idAnimation = 1;

        }
            //parado
        else
        {

            idAnimation = 0;
        }





        playerAnimator.SetBool("grounded", grounded);
        playerAnimator.SetBool("doubleJump", doubleJump);
        playerAnimator.SetInteger("idAnimation", idAnimation);
        playerAnimator.SetFloat("speedY", playerRb.velocity.y);



        //Double Jump
        if (Input.GetButtonDown("Jump") && !grounded && pulando && !doubleJump)
        {
            doubleJump = true;
            Vector2 vector = new Vector2();
            vector.y = 0f;
            playerRb.velocity = vector;
            playerRb.AddForce(new Vector2(0, 350));

        }

        //Pulando
        if (Input.GetButtonDown("Jump") && grounded == true)
        {

            playerRb.AddForce(new Vector2(0, jumpForce));
            grounded = false;
            doubleJump = false;
            pulando = true;


        }

        if (grounded){
            doubleJump = false;
        }


        //controle se o usuario esta abaixado ou em pé
        if (idAnimation == 3 ) {
            standing.enabled = false;
            crounching.enabled = true;
        }
        else if ( idAnimation != 3)
        {
            standing.enabled = true;
            crounching.enabled = false;
        }

        //Reiniciar posição

        if (Input.GetButtonDown("Fire1")) {

            transform.position = atualPosition;

        }

        //Attack
        //if(Input.GetButtonDown("Fire2") && grounded == true && v >=0 )
        if (Input.GetButtonDown("Fire2") && !attacking)

        {
            playerAnimator.SetTrigger("attack");

        }

        if (attacking && grounded)
        {
            h = 0;
        }


    }


    private void flip() {
        lookLeft = !lookLeft; //inverte o valor da variavel booleana
        float x = transform.localScale.x;
        x *= -1; //inverte o sinal do scale X
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        horizontalDirection.x = x;

    }

    //Campo de visão para interação com objetos
    private void interagir() {
        Debug.DrawRay(hand.position, horizontalDirection * 0.2f, Color.green);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(hand.position, horizontalDirection, 0.2f, interacao);
        if (raycastHit2D) {
            print(raycastHit2D.collider.gameObject.name);
        }
    }


    // controlando evento de attack
    // Método via parametros do Animator
    // No primeiro frame ativamos o 1, no ultimo desativamos
    void attack(int atk) {
        switch (atk) {
            case 0:
                attacking = false;
                break;
            case 1:
                attacking = true;
                break;
        }
    }


    //Método via parametros do Animator
    //Tu passa o ID e ele ativa arma.
    void controleArma(int id) {
        desativandoSpritesArma();
        armas[id].SetActive(true);
    }


    //garantindo que os gameObjects da arma estejam desativados (para nao aparecer todas juntas)
    //foreach = para cada
    void desativandoSpritesArma() {
        foreach (GameObject gameObject in armas) {
            gameObject.SetActive(false);
        }
    }


    //Colisão com objetos
    void OnTriggerEnter2D(Collider2D collider) {

        switch (collider.gameObject.tag) {

            case "coletavel":
                collider.gameObject.SendMessage("Coletar", SendMessageOptions.DontRequireReceiver);
                break;
        }

    }




    /** SISTEMA PARA MUDAR DE ARMAS **/

    void ChangeMaterial(Material novoMaterial) {

        spriteRenderer.material = novoMaterial;
        foreach (GameObject o in armas) {
            o.GetComponent<SpriteRenderer>().material = novoMaterial;
        }

    }

    public void TrocarArmas(int id) {
        idArma = id;

        //configurando sprites e danos das armas

        armas[0].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas0[idArma];
        armaInfo tempArmaInfo = armas[0].GetComponent<armaInfo>();
        tempArmaInfo.danoMax = _GameController.danoMaxArma[idArma];
        tempArmaInfo.danoMin = _GameController.danoMinArma[idArma];


        armas[1].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas1[idArma];
        tempArmaInfo = armas[1].GetComponent<armaInfo>();
        tempArmaInfo.danoMax = _GameController.danoMaxArma[idArma];
        tempArmaInfo.danoMin = _GameController.danoMinArma[idArma];

        armas[2].GetComponent<SpriteRenderer>().sprite = _GameController.spriteArmas2[idArma];
        tempArmaInfo = armas[2].GetComponent<armaInfo>();
        tempArmaInfo.danoMax = _GameController.danoMaxArma[idArma];
        tempArmaInfo.danoMin = _GameController.danoMinArma[idArma];


        idArmaAtual = idArma;
    }


}
