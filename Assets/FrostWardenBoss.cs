using System;
using System.Collections;
using UnityEngine;

public class FrostWardenBoss : Boss
{
    [Header("Platforming")]
    [SerializeField] private float gravity = 75f;
    [SerializeField] private float jumpVelocity = 15f;
    [SerializeField] private float jumpHoldTime = .15f;
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpBufferTime = .1f;
    [SerializeField] private float jumpRaycastDist = .8f;

    [Header("Attack Stuff")]
    [SerializeField] float coolDownTime = .5f;

    enum ActionTypes { Move, Jump, Attack };
    ActionTypes chosenAction;

    [Header("Frost")]
    [SerializeField] private FrostAOE frostAOEprefab;
    [SerializeField] private float frostTime = 2f;
    [SerializeField] private float frostCooldown = 4f;
    [SerializeField] private float slowDownTime = 2f;

    private GameManager gameManager;

    bool grounded = true;
    bool attackCooldown = false;
    bool canSpawnFrost = true;
    bool slowed = false;
    bool canAttack = true;

    private void Action()
    {
        chosenAction = (ActionTypes)UnityEngine.Random.Range(0, Enum.GetNames(typeof(ActionTypes)).Length);

        switch (chosenAction)
        {
            case ActionTypes.Move:
                Move();
                break;
            case ActionTypes.Jump:
                Jump();
                break;
            case ActionTypes.Attack:
                Swing();
                break;
            default:
                break;
        }

        StartCoroutine(AttackCooldown());
    }

    private void Move()
    {

    }

    private void Jump()
    {

    }

    private void Swing()
    {

    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(coolDownTime);
        canAttack = true;
    }
}
