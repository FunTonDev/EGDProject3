using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossVulnarbleState : StateMachineBehaviour
{
    BossEnemy Boss;
    //bool BossVulnarable;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Boss = animator.GetComponent<BossEnemy>();
        Boss.BecomeVulnarable();
        animator.SetBool("Vulnarable", true);

        Debug.Log("Platformer Boss is now Vulnarable");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If Boss is no longer vulnarable, exit
        if(!Boss.GetVulnarablity())
        {
            animator.SetBool("Vulnarable", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Platformer Boss is no longer Vulnarable");
        animator.ResetTrigger("Vulnarable");
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
