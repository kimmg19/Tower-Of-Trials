using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    [SerializeField]ParticleSystem fireEmber;
    [SerializeField] ParticleSystem fireFlame;


    // Start is called before the first frame update
    void OnEnable ()
    {
        fireEmber.Play();
        animator = GetComponent<Animator>();
        LookAtPlayerXYZ(player.transform);
        animator.SetTrigger("Drakaris");
    }
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
    }
    void LookAtPlayerXYZ(Transform player)
    {
        // 플레이어와의 방향 벡터 계산 (Y축 고정)
        Vector3 direction = player.position - gameObject.transform.position;
        direction.y = direction.y - 0.5f;
        // 방향 벡터를 기준으로 회전 설정
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            gameObject.transform.rotation = targetRotation;
        }
    }
    public void FireEmber()
    {

        fireEmber.Play();

    }
    public void FireFlame()
    {
        
        fireFlame.Play();
        
    }
}
