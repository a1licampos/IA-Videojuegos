using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    public float fTimeToRespawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(DeathPlayer(collision.gameObject));
            Debug.LogWarning("CHOCO JUGADOR");
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        StartCoroutine(DeathPlayer(other.gameObject));
    //    }
    //}

    IEnumerator DeathPlayer(GameObject player)
    {
        player.SetActive(false);
        yield return new WaitForSeconds(fTimeToRespawn);

        player.SetActive(true);
    }
}
