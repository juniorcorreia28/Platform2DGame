using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject currentCheckPoint;

    private PlayerControl player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerControl>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RespawnPlayer()
    {
        Debug.Log("Player Respawn");
        player.transform.position = currentCheckPoint.transform.position;
    }
}
