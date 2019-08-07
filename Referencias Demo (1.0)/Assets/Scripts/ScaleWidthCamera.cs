using UnityEngine;
using System.Collections;

public class ScaleWidthCamera : MonoBehaviour {

    //SCRIPTque serve para configuar a resolução do jogo virtual. O jogo vai poder ser jogado em varias resoluções e sempre a tela do jogo vai estar com a Scala corretamente, para que não corte nada.

    public int targetWidth = 640; //Aqui define qual o tamanho da largura da resolução virtual;
    public float pixelsToUnits = 64; //Aqui define o PPU que esta sendo usado em todos os sprites.

	// Update is called once per frame
	void FixedUpdate () {

        //Calcula que descobre a largura da resolução.
        int height = Mathf.RoundToInt(targetWidth / (float)Screen.width * Screen.height);

        //Calculo que vai ajustar na camera a resolução virtual que escolhemos.
        Camera.main.orthographicSize = height / pixelsToUnits / 2;
	}

}