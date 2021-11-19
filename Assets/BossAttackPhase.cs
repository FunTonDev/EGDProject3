using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackPhase : StateMachineBehaviour
{    
    BossPlatformerMovement BossMovement;
    BossPlatformerAttack BossAtks;

    bool exiting;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        BossMovement = animator.GetComponent<BossPlatformerMovement>();
        BossAtks = animator.GetComponent<BossPlatformerAttack>();
        exiting = false;

        //Performed all Atks, move onto Vulnarable phase
        if (BossAtks.GetAtkIndex() == 3)
        {
            BossAtks.SetAtkIndex(0);
            BossAtks.SetAttacking(false);
            BossAtks.SetNewAtkPhase(true);

            animator.SetTrigger("Vulnarable");

            Debug.Log("Atk Phase finished");
        }


        //Set values for new Atk Phase
        else if(BossAtks.GetNewAtkPhase())
        {
            Debug.Log("New Atk Phase, setting values");
            BossAtks.ResetAtkforPhase();

            //Create a list of random atacks for phase
            for (int i = 0; i < 3; i++)
            {
                if (i != 0)
                {
                    bool AtkSet = false;
                    while (!AtkSet)
                    {
                        int atk = Random.Range(1, 4);
                        if (atk != BossAtks.GetAtkForPhase(i - 1))
                        {
                            BossAtks.AddAtkforPhase(atk);
                            AtkSet = true;
                        }
                    }
                }

                else
                {
                    BossAtks.AddAtkforPhase(Random.Range(1, 4));
                }

                BossAtks.SetAtkIndex(0);
                BossAtks.SetAttacking(false);
                BossAtks.SetNewAtkPhase(false);
            }
        }
                        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(BossAtks.GetAtkIndex() == 3 && !BossAtks.GetAttacking() && !exiting)
        {
            Debug.Log("Atk Phase finished");

            BossAtks.SetAttacking(false);
            exiting = true;

            animator.SetTrigger("Vulnarable");
        }

        //Perform next attack in list
        else if (!BossAtks.GetAttacking() && !exiting)
        {
            int index = BossAtks.GetAtkIndex();
            PerformAtk(BossAtks.GetAtkForPhase(index));
            BossAtks.SetAttacking(true);
            BossAtks.SetAtkIndex(index + 1);
        } 

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Vulnarable");

        if (BossAtks.GetAtkIndex() == 3)
        { BossAtks.SetAtkIndex(0); }
    }

    void PerformAtk(int i)
    {
        Debug.Log("Boss Performing Atk3");
        BossAtks.Attack3();
        
        /*
        Debug.Log("Boss Performing Atk" + i);

        if (i == 1)
        {
            BossAtks.Attack1();
        }

        else if (i == 2)
        {
            BossAtks.Attack1();
        }

        else if (i == 3)
        {
            BossAtks.Attack1();
        }
        */
    }




    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
