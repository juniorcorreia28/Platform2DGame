using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour {

    private LevelManager levelManager;

	private void Start () 
    {
        levelManager = FindObjectOfType<LevelManager>();
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Player")
        {
            levelManager.RespawnPlayer();
        }
    }
}
