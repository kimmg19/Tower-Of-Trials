using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState : StateMachineBehaviour
{
    protected Transform player;
    protected PlayerStatus playerStatus;
    protected NavMeshAgent agent;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
            playerStatus = player.GetComponent<PlayerStatus>();
            if (playerStatus == null)
            {
                Debug.LogError("PlayerStatus component not found on Player.");
            }
        }

        if (agent == null)
        {            
            agent = animator.GetComponentInParent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("NavMeshAgent not found on the animator or its parent objects.");
            }
        }

        OnStateEnterCustom(animator, stateInfo, layerIndex);
    }

    // 각 몬스터가 고유한 동작을 정의할 수 있는 추상 메서드
    protected abstract void OnStateEnterCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);
    protected abstract void OnStateUpdateCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex);

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        OnStateUpdateCustom(animator, stateInfo, layerIndex);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        OnStateExitCustom(animator, stateInfo, layerIndex);
    }

    // 필요 시 오버라이드할 수 있는 메서드
    protected virtual void OnStateExitCustom(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
}
