using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject changeScene;
    public BoxCollider2D boundScene;

    private PlayerControl player;
    private CameraController theCamera;

    private void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        theCamera = FindObjectOfType<CameraController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            player.transform.position = changeScene.transform.position;

            //Muda os limites do cenario e a posição da camera.
            theCamera.boundBox = boundScene;
            //theCamera.transform.position = changeScene.transform.position;
        }
    }
}
