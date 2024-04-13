using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossTemplate : MonoBehaviour
{
    bool canAttack = true;
    float attackCooldown = 3.0f;
    float bossHealth = 100f;

    public int numberOfPossibleManuevers = 3;


    private IEnumerator ResetBoolAfterDelay(bool toSetTrue, float waitTime)
    {
        // Wait for cooldown second
        yield return new WaitForSeconds(waitTime);

        // Reset the canAttack flag
        toSetTrue = true;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bossHealth <= 0)
        {
            //player wins fight
            //player gets new ability
        }
        //attacks when it can, set canAttack to true via coroutine + cooldown
        if (canAttack)
        {
            canAttack = false;
            attack();
            // Start the coroutine to reset attack flag
            StartCoroutine(ResetBoolAfterDelay(canAttack, attackCooldown));
        }
    }


    void attack()
    {
        //random attack chosen
        int attackChosen = Random.Range(1, numberOfPossibleManuevers);
        switch (attackChosen)
        {
            case 1:
                Debug.Log("boss: first attackChosen");
                //execute attack

                break;

            case 2:
                Debug.Log("boss: second attackChosen");
                //execute attack

                break;

            case 3:
                Debug.Log("boss: third attackChosen");
                //execute attack

                break;

            default:
                Debug.Log("boss: default");
                //shouldn't get here
                break;




        }

    }

}

