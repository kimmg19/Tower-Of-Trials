using System.Collections;
using UnityEngine;

public class FirstFloor : MonoBehaviour
{
    public GameObject floorCanvas; // 1st Floor 캔버스에 대한 레퍼런스

    void Start()
    {
        StartCoroutine(DisplayFloorCanvas());
    }

    IEnumerator DisplayFloorCanvas()
    {
        // 캔버스를 활성화하여 보여줍니다.
        floorCanvas.SetActive(true);

        // 2초 후에 캔버스를 비활성화하여 숨깁니다.
        yield return new WaitForSeconds(5f);

        floorCanvas.SetActive(false);
    }
}
