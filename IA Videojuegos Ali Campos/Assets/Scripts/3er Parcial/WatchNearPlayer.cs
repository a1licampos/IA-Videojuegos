using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchNearPlayer : MonoBehaviour
{
    public PatrolAgentFSM _sm;
    public PlayerDie playerDie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(playerDie.bIsDeath)
            {
                Debug.Log("Changing to Alert State");
                _sm.ChangeState(_sm.alertState);
            }
            else
            {
                Debug.Log("Changing to Attack State");
                _sm.ChangeState(_sm.attackState);
            }
        }
    }
}
