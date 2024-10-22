using System.Collections;
using UnityEngine;

public class FourthAreaManager : MonoBehaviour
{
    public GameObject dragons;
    public GameObject dragon;
    public ParticleSystem flame;
    public float time;
    float particlePlayingTime;
    private void Start()
    {
        particlePlayingTime = flame.main.duration + flame.main.startLifetime.constant;//불쏘는거? 파티클 종료까지 시간
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time > 5f)
        {
            StartCoroutine(DrakarisAttack());
        }
        if (time > 8f)
        {
            //StartCoroutine(DrakarisFlyAttack());
        }
    }

    private IEnumerator DrakarisAttack()
    {
        dragons.SetActive(true);
        yield return new WaitForSeconds(particlePlayingTime);
        dragons.SetActive(false);
        time = 0f;
        
    }

    private IEnumerator DrakarisFlyAttack()
    {
        // 드래곤 활성
        dragon.SetActive(true);
        
        // 드래곤 처음 크기와 위치 설정 (하늘에서 나타나는 효과)
        dragon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // 드래곤을 절반 크기에서 시작

        LeanTween.scale(dragon, new Vector3(1, 1, 1), 2f).setEase(LeanTweenType.easeOutBounce); // 크기를 절반에서 1로 증가


        // 7초 동안 드래곤이 활성화된 상태 유지
        yield return new WaitForSeconds(7f);

        // 드래곤 비활성화 및 시간 초기화
        dragon.SetActive(false);
        time = 0f;
    }

}
