using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    /// <summary>
    /// Determina a velocidade maxima que o Player pode ir.
    /// </summary>
    public float maxSpeedRun;
    /// <summary>
    /// Determina a velocidade maxima que o player vai nadar.
    /// </summary>
    public float maxSpeedSwim;
    /// <summary>
    /// Determina a velocidade minima do pulo do Player quando estiver nadando.
    /// </summary>
    public float minSpeedJumpSwim;
    /// <summary>
    /// Determina a velocidade minima do pulo do Player.
    /// </summary>
    public float minSpeedJump;
    /// <summary>
    /// Variavel que determina quanto tempo o Player vai Ficar subindo no pulo, até alcançar o limite maximo.
    /// </summary>
    public float jumpTime;
    /// <summary>
    /// Variavel que vai VERIFICAR se o Player esta no chão.
    /// </summary>
    public Transform groundCheck;
    /// <summary>
    /// Variavel que vai VERIFICAR se o Player esta na agua.
    /// </summary>
    public Transform waterCheck;
    /// <summary>
    /// Isso ira determinar ao Player o que é um chão.
    /// </summary>
    public LayerMask whatIsGround;
    /// <summary>
    /// Isso ira determinar ao Player o que é a agua.
    /// </summary>
    public LayerMask whatIsWater;
    /// <summary>
    /// Botões mobile que irão controlar o personagem
    /// </summary>
    public GameObject mobileInput;

    /// <summary>
    /// Essa variavel é responsavel de dizer, qual Layer vai ser considerada um chão para o Player.
    /// </summary>
    private float groundRadius = 0.1f;
    /// <summary>
    /// Variavel que diz se o Player está virado para DIREITA ou não.
    /// </summary>
    private bool facingRight = false;
    /// <summary>
    /// Variavel que vai me DIZER se o Player esta no chão.
    /// </summary>
    private bool grounded = false;
    /// <summary>
    /// Variavel que vai me DIZER se o Player esta na agua.
    /// </summary>
    private bool inThewater = false;

    private float jumpTimeCount; //Variavel que vai ser o contador do tempo. E vai contar do 0.5 segundos até ZERO.

    /// <summary>
    /// Move que recebe a direção que o jogador escolhe para controlar o player.
    /// </summary>
    [HideInInspector]
    public float move;

    /// <summary>
    /// Variavel que irá ser estanciada a Classe Rigidbody2d, para que possamos mover o Player.
    /// </summary>
    private Rigidbody2D body;
    /// <summary>
    /// Variavel que irá ser estanciada a Classe Animator, onde iremos poder acessar as animações do Player.
    /// </summary>
    private Animator anim;

    

    private void Start ()
    {
        body = GetComponent<Rigidbody2D> (); //Variavel recebendo a Classe Rigidbody2d, assim que o jogo começar.
		anim = GetComponent<Animator> (); //Variavel recebendo a Classe Animator, assim que o jogo começar.

        jumpTimeCount = jumpTime; //O contador recebe o valor do JumpTime, que é o tempo do Player em seu pulo.        
	}

    /// <summary>
    /// FixedUpdate está sendo usado, pois ele é usado para controlara velocidade e a fisica do jogador com mais precisão.
    /// </summary>
    private void FixedUpdate ()
    {
        //Sempre irá verificar se o Player esta no chão.

        //Variavel que vai dizer se o Player está no Chão. Se sim ele retorna um TRUE se Não um FALSE.
        grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
        //Variavel que vai dizer se o Player está na Agua. Se sim ele retorna um TRUE se Não um FALSE.
        inThewater = Physics2D.OverlapCircle(waterCheck.position, groundRadius, whatIsWater);
        
        //O valor do GROUND (Criado em Animator) recebe o valor do grounded.
        anim.SetBool("Ground", grounded);
        //O valor do WATER (Criado em Animator) recebe o valor do inThewater.
        anim.SetBool("Water", inThewater); 

        //Variavel recebe a direção em que o jogador escolher mover.
        move = MoveButton();

        /*Speed(Criado em Animator) recebe o valor da variavel Move. 
         * Onde irá comparar sempre o valor do MOVE ao SPEED. 
         * (Speed determina que o personagem faça animação de correr).*/
        anim.SetFloat ("Speed", Mathf.Abs(move));

        if (inThewater)
        {
            body.velocity = new Vector2(move * maxSpeedSwim, body.velocity.y);
        }
        else
        {
            body.velocity = new Vector2(move * maxSpeedRun, body.velocity.y); //Calculo que determina a velocidade em que o Player se move.
        }

		if (move > 0 && !facingRight) //Se o Move for maior que 0 e o Player não estiver virado para DIREITA ele da um FLIP.
			Flip ();
		else if(move < 0 && facingRight) //Se o Move for menor que 0 e o player estiver virado para DIRETA ele da um FLIP.
			Flip();

        /*Lembrando que o Move recebe o valor X, que indica o movimento do personagem em Horizontal. 
         * Então, se o jogador mover para esquerda e ele não estiver virado para esquerda ele vai vira e vice-versa.*/
    }

    /// <summary>
    /// Aqui esta sendo usado o Update, pois esse metodo funciona com mais precição os botões apertados pelo jogador.
    /// No FixedUpdate, pode não funcionar corretamente.
    /// </summary>
    private void Update()
    {
        //Setando os valores para os Parametros do Animator.
        anim.SetFloat("vSpeed", body.velocity.y);
        anim.SetBool("jumpButton", JumpButtonDown());

        if (inThewater)
        {
            if (JumpButtonDown())
            {
                body.velocity = new Vector2(body.velocity.x, minSpeedJumpSwim);
            }
        }
        else
        {
            if (grounded && JumpButtonDown())
            {
                body.velocity = new Vector2(body.velocity.x, minSpeedJump);
            }
            else if (grounded == false && body.velocity.y > 0.0f)
            {
                if (JumpButton() == false)
                {
                    body.velocity = new Vector2(body.velocity.x, 0.0f);
                }
            }
        }
        
        //Pisou do no chão o Tempo é resetado.
        if (grounded)
        {
            jumpTimeCount = jumpTime;
        }
    }

    private bool JumpButton()
    {
        bool value;

        /*Se o objeto estiver ativo, o personagem vai passar 
         * a ser controlado somente por toque na tela*/
        if (mobileInput.activeInHierarchy)
        {
            value = getButton;
        }
        else
        {
            value = Input.GetButton("Jump");
        }

        return value;
    }

    private bool JumpButtonDown()
    {
        bool value;

        /*Se o objeto estiver ativo, o personagem vai passar 
         * a ser controlado somente por toque na tela*/
        if (mobileInput.activeInHierarchy)
        {
            value = getButtonDown;
        }
        else
        {
            value = Input.GetButtonDown("Jump");
        }

        return value;
    }

    private float MoveButton()
    {
        float value;

        /*Se o objeto estiver ativo, o personagem vai passar 
         * a ser controlado somente por toque na tela*/
        if (mobileInput.activeInHierarchy)
        {
            value = getAxisRaw;
        }
        else
        {
            value = Input.GetAxisRaw("Horizontal");
        }

        return value;
    }

    #region Mobile Inputs (Used in Mobile_UI)
    //Mobile Buttons
    private float getAxisRaw = 0f;
    private bool getButtonDown = false;
    private bool getButton = false;

    //Mobile Methods
    public void GetAxisRaw(float value)
    {
        getAxisRaw = value;
    }

    public void GetButtonDown(bool value)
    {
        getButtonDown = value; 
    }

    public void GetButton(bool value)
    {
        getButton = value;
    }
    #endregion

    /// <summary>
    /// Metodo que inverte o Sprite do Player para direção em que o jogador for.
    /// </summary>
    private void Flip()
    {
		facingRight = !facingRight; //Atualiza o atual lado do Player. Se ele estiver no Esquerdo recebe FALSE, se estiver virado para o lado Direito recebe TRUE;

		Vector3 theScale = transform.localScale; //Recebe a posição atual do Player.
		theScale.x *= -1; //Da um Flip da na posição X.
		transform.localScale = theScale; //Aplica o flip na posição atual, assim alterando a posição, invertando o lado.
	}
}