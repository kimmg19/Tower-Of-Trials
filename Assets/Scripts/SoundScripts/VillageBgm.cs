using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBgm : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.Play("VillageBgm");
    }


}
