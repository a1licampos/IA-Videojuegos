using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    public float fTimeToRespawn;

    public Animator mAnimatorPlayer;
    public MoveToClick moveToClick;

    public PatrolAgentFSM _sm;

    public bool bIsDeath;

    // Start is called before the first frame update
    void Start()
    {
        _sm = GetComponent<PatrolAgentFSM>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mAnimatorPlayer = collision.gameObject.GetComponentInChildren<Animator>();
            moveToClick = collision.gameObject.GetComponentInParent<MoveToClick>();

            bIsDeath = true;

            moveToClick.enabled = false;
            mAnimatorPlayer.SetBool("Moving", false);
            mAnimatorPlayer.SetBool("Damage", true);

            StartCoroutine(DeathPlayer());

            Debug.LogWarning("PLAYER IS DAMAGED");
        }
    }


    IEnumerator DeathPlayer()
    {
        yield return new WaitForSeconds(fTimeToRespawn);

        mAnimatorPlayer.SetBool("Damage", false);
        moveToClick.enabled = true;
        bIsDeath = false;
    }
}
