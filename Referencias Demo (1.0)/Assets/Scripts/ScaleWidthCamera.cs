using UnityEngine;
using System.Collections;

public class ScaleWidthCamera : MonoBehaviour {

    /* SCRIPT que serve para configuar a resolução do jogo virtual. 
     * O jogo vai poder ser jogado em varias resoluções e 
     * sempre a tela do jogo vai estar com a Scala corretamente, para que não corte nada.*/

    public int targetWidth; //Aqui define qual o tamanho da largura da resolução virtual;
    public float pixelsToUnits; //Aqui define o PPU que esta sendo usado em todos os sprites.

	private void FixedUpdate () {

        //Calcula que descobre a largura da resolução.
        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);

        //Calculo que vai ajustar na camera a resolução virtual que escolhemos.
        Camera.main.orthographicSize = height / pixelsToUnits / 2;
	}
}