using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class FourthAreaManager : MonoBehaviour
{
    [Header("Dragon Settings")]
    public GameObject dragons;
    public GameObject dragon;

    [Header("Meteor Gimmick Settings")]
    public GameObject meteorPrefab;    // 메테오 프리팹
    public int maxMeteors = 7;         // 메테오 최대 생성 개수
    public float meteorSpawnInterval = 1f; // 메테오 생성 간격 (초 단위)
    public Vector3 spawnAreaCenter = new Vector3(25f, 0f, 25f); // 중심 좌표
    public float spawnAreaRadius = 20f;  // 메테오가 떨어질 범위

    [Header("Player Settings")]
    public Transform player;           // 플레이어 위치 참조

    [Header("Other Settings")]
    public ParticleSystem flame;
    public GameObject inGameCanvas;
    public GameObject playerUI;
    bool isGameClear = false;
    private float time;
    private bool isDrakarisFlyAttackActive = false;  // FlyAttack 중복 방지 플래그
    private bool isDrakarisAttackActive = false;     // Attack 중복 방지 플래그
    private float particlePlayingTime;
    [SerializeField] public PlayableDirector fourthAreaEnterCinematic;
    [SerializeField] public PlayableDirector fourthAreaClearCinematic;
    [SerializeField] PlayerInputs playerInputs;

    private bool isEnterCinematicPlaying; // Enter Cinematic이 실행 중인지 여부

    private void Start()
    {
        PlayEnterCinematic();
        particlePlayingTime = flame.main.duration + flame.main.startLifetime.constant; // 파티클 종료까지 시간
    }

    void Update()
    {
        if (isGameClear || isEnterCinematicPlaying) return; // 시네마틱이 끝나지 않으면 기믹 실행 금지

        time += Time.deltaTime;
        if (time > 5f && !isDrakarisAttackActive)
        {
            StartCoroutine(DrakarisAttack());
        }

        if (time > 8f && !isDrakarisFlyAttackActive)
        {
            StartCoroutine(DrakarisFlyAttack());
        }
    }

    void PlayEnterCinematic()
    {
        if (fourthAreaEnterCinematic != null)
        {
            isEnterCinematicPlaying = true;
            playerInputs.enabled = false; // 시네마틱 시작 시 입력 비활성화
            fourthAreaEnterCinematic.Play(); // 시네마틱 재생
            fourthAreaEnterCinematic.stopped += OnEnterCinematicEnded;
        }
    }

    void PlayClearCinematic()
    {
        if (fourthAreaClearCinematic != null)
        {
            playerInputs.enabled = false; // 시네마틱 시작 시 입력 비활성화
            fourthAreaClearCinematic.Play(); // 시네마틱 재생
            fourthAreaClearCinematic.stopped += OnClearCinematicEnded;
        }
    }

    void OnEnterCinematicEnded(PlayableDirector director)
    {
        playerInputs.enabled = true; // 시네마틱 종료 시 입력 활성화
        isEnterCinematicPlaying = false; // Enter Cinematic이 종료되었음을 표시
        fourthAreaEnterCinematic.stopped -= OnEnterCinematicEnded; // 이벤트 해제
    }

    void OnClearCinematicEnded(PlayableDirector director)
    {
        playerInputs.enabled = true; // 시네마틱 종료 시 입력 활성화
        isGameClear = false; // 게임 클리어 상태 초기화
        fourthAreaClearCinematic.stopped -= OnClearCinematicEnded; // 이벤트 해제
    }

    private IEnumerator DrakarisAttack()
    {
        isDrakarisAttackActive = true;
        dragons.SetActive(true);
        AudioManager.instance.Play("DragonFlameAttack");
        yield return new WaitForSeconds(particlePlayingTime);
        AudioManager.instance.Stop("DragonFlameAttack");

        dragons.SetActive(false);
        isDrakarisAttackActive = false;
        time = 0f;
    }

    private IEnumerator DrakarisFlyAttack()
    {
        isDrakarisFlyAttackActive = true; // 중복 실행 방지
        dragon.SetActive(true);
        yield return StartCoroutine(SpawnMeteors()); // 메테오 생성 코루틴 실행
        dragon.SetActive(false); // 메테오 생성 완료 후 드래곤 비활성화 및 시간 초기화
        time = 0f;
        isDrakarisFlyAttackActive = false;
    }

    private IEnumerator SpawnMeteors()
    {
        int meteorCount = 0;
        while (meteorCount < maxMeteors)
        {
            Instantiate(meteorPrefab, player.position, Quaternion.identity);
            Vector3 randomPosition = GetRandomPosition();
            Instantiate(meteorPrefab, randomPosition, Quaternion.identity);
            meteorCount++;
            yield return new WaitForSeconds(meteorSpawnInterval);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        float randomDistance = Random.Range(0f, spawnAreaRadius);
        float x = Mathf.Cos(randomAngle) * randomDistance;
        float z = Mathf.Sin(randomAngle) * randomDistance;
        return new Vector3(x, 0f, z) + spawnAreaCenter;
    }

    public void OnPrincessKilled()
    {
        isGameClear = true;
        dragon.SetActive(false);
        dragons.SetActive(false);
        AudioManager.instance.Stop("4thAreaBgm"); 
        AudioManager.instance.Stop("DragonFlameAttack");
        PlayClearCinematic();
    }
}
