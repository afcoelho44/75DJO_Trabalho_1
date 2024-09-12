using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstadoCorrendoInimigoBoss : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("podeAndar", true);

        // Adiciona uma chance de grunhir ao entrar no estado
        // Você pode ajustar essa lógica conforme a necessidade (por exemplo, tocar em cada entrada ou aleatoriamente)
        Grunhir(animator);
    }
    // Função para acionar a animação de grunhido
    private void Grunhir(Animator animator)
    {
        // Define uma chance para o grunhido ou grunhido garantido dependendo da lógica do jogo
        // Por exemplo, uma chance de 30% para grunhir quando começa a correr
        float chanceDeGrunhir = 0.3f;
        if (Random.value <= chanceDeGrunhir)
        {
            animator.SetTrigger("grunhir");
            animator.SetBool("podeAndar", false);
        }
    }


    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
