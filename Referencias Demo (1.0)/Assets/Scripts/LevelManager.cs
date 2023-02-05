using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public GameObject currentCheckPoint;

    private PlayerControl player;

	private void Start () 
    {
        player = FindObjectOfType<PlayerControl>();
	}

    public void RespawnPlayer()
    {
#if UNITY_EDITOR
		Debug.Log("Player Respawn");
#endif

		player.transform.position = currentCheckPoint.transform.position;
    }
}
