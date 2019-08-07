using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public GameObject target; //Classe PlayerControl esta sendo estanciada na variavel player;
    public Vector2 focusAreaSize;//Sera definido o tamanhado FocusArea, onde fica em volta do Player.
    public BoxCollider2D boundBox;

    private Camera theCamera;
    private float halfHeight;
    private float halfWidth;

    private Vector3 minBounds;
    private Vector3 maxBounds;

    public bool isFollowing; //Variavel que vai nos dizer se a camera está seguindo o jogador ou não.
    //public float xOffset; 
    //public float yOffset;

    public float verticalOffset; //Variavel que vai configura o quando a camera fica posicionada para cima ou para baixo.
    public float lookAheadDstX; // variavel que vai configura  a visão agora do horizontal. Podemos deixar ela um pouco para frente ou para trás.
    public float lookSmoothTimeX; //E aqui vai guardar o tempo. A velocidade que a camera vai mover quando passar a focus area na horizontal.
    public float verticalSmoothTime; //A mesma coisa que a cima. Vai definir a velocidade em que a camera se move quando passamos a focus area na vertical.

    //As variaveis abaixo, vai guardar as pocições atuais do Player, a pocição da camera nas direções X e Y e velocidade em que elas se movem. (Assim podemos alterar durante o jogo a velocidade e posições)
    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothVelocityY;

    bool lookAheadStopped; //Aqui ira ser definido quando a camera para. Depois que ela se mover até configuramo. Ela para.

    FocusArea focusArea; //Nossa Struct que criamos.
    PlayerControl player; //Nosso player está sendo estaciada nessa classe, para que podemos acessar.

	// Use this for initialization
	void Start () {

        isFollowing = true;

        //Classes sendo estanciadas.
        focusArea = new FocusArea(target.GetComponent<BoxCollider2D>().bounds, focusAreaSize);
        player = target.GetComponent<PlayerControl>();
        theCamera = GetComponent<Camera>();

       /* minBounds = boundBox.bounds.min;
        maxBounds = boundBox.bounds.max;*/

        /*halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;*/

        theCamera.transform.Translate(player.transform.position);
	
	}

    void LateUpdate() //Esta sendo usado o metodo em LateUpdate, pois todo o nosso metodo da camera vai ser executado no fim do Frame. (Esse metodo é executado sempre depois do UPDATE)
    {
        focusArea.Update (target.GetComponent<BoxCollider2D>().bounds);
        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

        minBounds = boundBox.bounds.min;
        maxBounds = boundBox.bounds.max;

        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;

        if (isFollowing)
        {

            if (focusArea.velocity.x != 0)
            {
                lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
                if (Mathf.Sign(player.move) == Mathf.Sign(focusArea.velocity.x) && player.move != 0)
                {
                    lookAheadStopped = false;
                    targetLookAheadX = lookAheadDirX * lookAheadDstX;
                }
                else
                {
                    if (!lookAheadStopped)
                    {
                        lookAheadStopped = true;
                        targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDstX - currentLookAheadX) / 4f;
                    }
                }
            }

            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);
            focusPosition += Vector2.right * currentLookAheadX;

            float clampedX = Mathf.Clamp(focusPosition.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
            float clampedY = Mathf.Clamp(focusPosition.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

            transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        }
    }

    void OnDrawGizmos() //Aqui iremos criar um cubo no centro da tela, onde ira ser a posição da nossa FocusArea. (Esse metodo é executado por frame)
    {
        Gizmos.color = new Color(1, 0, 0, .5f); //Definido com cor Vermelha.
        Gizmos.DrawCube(focusArea.centre, focusAreaSize); //Recebe a posição da nossa FocusArea.
    }
	
	// Update is called once per frame
	void Update () {



        // transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        //Se for verdadeiro, a camera recebe as mesmas posições do jogador, assim fazendo a camera seguilo.
        /* if (isFollowing)
         {
             transform.position = new Vector3(target.transform.position.x + xOffset, target.transform.position.y + yOffset, transform.position.z);
         }*/
         

    }

    struct FocusArea //Aqui sera definido o tamanho da Focus area de acordo com o tamanho da camera e posição do player.
    {
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        public FocusArea (Bounds targetBounds, Vector2 size) //Aqui a FocusArea eh definido de acordo com player. Então ela sera configurada para que ele fique no centro.
        {

            //Pega a posição do Player e define o tamanho da Focusarea.
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;
            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update (Bounds targetBounds) //Metodo que vai atualizar sempre a posição da focus area. Sempre que o player mover, a focusarea vai se mover junto, assim mantendo o focu do Player.
        {
            float shiftX = 0;

            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if(targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            float shiftY = 0;

            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if(targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}
