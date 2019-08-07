using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float maxSpeedRun = 3.0f ; //Determina a velocidade maxima que o Player pode ir.
	public float minSpeedJump = 6.0f; //Determina a velocidade minima do pulo do Player.
	public Transform groundCheck; //Variavel que vai VERIFICAR se o Player esta no chão.
	public LayerMask whatIsGround; //Isso ira determinar ao Player o que é um chão.
	float groundRadius = 0.1f; //Essa variavel é responsavel de dizer, qual Layer vai ser considerada um chão para o Player.
	bool facingRight = false; //Variavel que diz se o Player está virado para DIREITA ou não.
	bool grounded = false; //Variavel que vai me DIZER se o Player esta no chão.

    public float jumpTime = 0.5f; //Variavel que determina quanto tempo o Player vai Ficar subindo no pulo, até alcançar o limite maximo.
    private float jumpTimeCount; //Variavel que vai ser o contador do tempo. E vai contar do 0.5 segundos até ZERO.
    
    [HideInInspector]
    public float move; //Move que recebe a direção que o jogador escolhe para controlar o player.


    Rigidbody2D body; //Variavel que irá ser estanciada a Classe Rigidbody2d, para que possamos mover o Player.
	Animator anim; //Variavel que irá ser estanciada a Classe Animator, onde iremos poder acessar as animações do Player.
		
	// Use this for initialization
	void Start ()
    {

        body = GetComponent<Rigidbody2D> (); //Variavel recebendo a Classe Rigidbody2d, assim que o jogo começar.
		anim = GetComponent<Animator> (); //Variavel recebendo a Classe Animator, assim que o jogo começar.

        jumpTimeCount = jumpTime; //O contador recebe o valor do JumpTime, que é o tempo do Player em seu pulo.
        
	}

	//FixedUpdate está sendo usado, pois ele é usado para controlara velocidade e a fisica do jogador com mais precisão.
	void FixedUpdate ()
    {

        //Sempre irá verificar se o Player esta no chão.
        grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround); //Variavel que vai dizer se o Player está no Chão. Se sim ele retorna um TRUE se Não um FALSE.
		anim.SetBool("Ground", grounded); //O valor do GROUND(Criado em Animator) recebe o valor do grounded.

		move = Input.GetAxisRaw ("Horizontal"); //Variavel recebe a direção em que o jogador escolher mover.

		anim.SetFloat ("Speed", Mathf.Abs(move)); //Speed(Criado em Animator) recebe o valor da variavel Move. Onde irá comparar sempre o valor do MOVE ao SPEED. (Speed determina que o personagem faça animação de correr).

		body.velocity = new Vector2 (move * maxSpeedRun, body.velocity.y); //Calculo que determina a velocidade em que o Player se move.

		if (move > 0 && !facingRight) //Se o Move for maior que 0 e o Player não estiver virado para DIREITA ele da um FLIP.
			Flip ();
		else if(move < 0 && facingRight) //Se o Move for menor que 0 e o player estiver virado para DIRETA ele da um FLIP.
			Flip();
        //Lembrando que o Move recebe o valor X, que indica o movimento do personagem em Horizontal. Então, se o jogador mover para esquerda e ele não estiver virado para esquerda ele vai vira e vice-versa.

    }

	 //Aqui esta sendo usado o Update, pois esse metodo funciona com mais precição os botões apertador pelo jogador. No FixedUpdate, pode não funcionar corretamente.
	void Update()
    {

        //Setando os valores para os Parametros do Animator.
        anim.SetFloat("vSpeed", body.velocity.y);
        anim.SetBool("jumpButton", Input.GetButtonDown("Jump"));

        //Aqui ele esta verificando, se o Player esta no CHÃO e o jogador aperta o botão de pular, ele faz essa animação.
        /*if (grounded && Input.GetButtonDown("Jump"))
        {
            body.velocity = new Vector2 (body.velocity.x, minSpeedJump); //Executando a força onde irá fazer o Player pular
            
        }

        if (grounded && Input.GetButton("Jump"))//Se o Botão manter-se pressionado, o Player vai pular mais alto.
        {
            if(jumpTimeCount > 0) //Enquanto o contador não chegar a ZERO, Ele continua subindo.
            {
                body.velocity = new Vector2 (body.velocity.x, minSpeedJump); //Adicionado a altura do Pulo.
                jumpTimeCount -= Time.deltaTime; //Contador que Diminui o tempo do pulo.
            }
        }

        //Assim que o Botão soltar, o Player começa a cair, e assim resetando o contador do tempo do pulo.
        if (Input.GetButtonUp("Jump"))
        {
            jumpTimeCount = 0;
        }*/

        if (grounded && Input.GetButtonDown("Jump"))
        {
            body.velocity = new Vector2(body.velocity.x, minSpeedJump);
        }
        else if(grounded == false && body.velocity.y > 0.0f)
        {
            if (Input.GetButton("Jump") == false)
            {
                body.velocity = new Vector2(body.velocity.x, 0.0f);
            }
        }

        //Pisou do no chão o Tempo é resetado.
        if (grounded)
        {
            jumpTimeCount = jumpTime;
        }
    }

	//Metodo que inverte o Sprite do Player para direção em que o jogador for.
	void Flip()
    {
		facingRight = !facingRight; //Atualiza o atual lado do Player. Se ele estiver no Esquerdo recebe FALSE, se estiver virado para o lado Direito recebe TRUE;
		Vector3 theScale = transform.localScale; //Recebe a posição atual do Player.
		theScale.x *= -1; //Da um Flip da na posição X.
		transform.localScale = theScale; //Aplica o flip na posição atual, assim alterando a posição, invertando o lado.
	}
}