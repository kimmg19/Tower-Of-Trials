using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    [SerializeField] Slider mouseSensitivitySlider_X;
    [SerializeField] Slider mouseSensitivitySlider_Y;
    GameObject obj;
    CinemachineFreeLook freeLook;
    void Awake()
    {
        obj = GameObject.Find("FreeLook Camera");
        freeLook = obj.GetComponent<CinemachineFreeLook>();
        mouseSensitivitySlider_X.onValueChanged.AddListener(SetMouseSensitivity_X);
        mouseSensitivitySlider_Y.onValueChanged.AddListener(SetMouseSensitivity_Y);
    }

    private void SetMouseSensitivity_X(float value)
    {
        freeLook.m_XAxis.m_MaxSpeed=value;
    }
    private void SetMouseSensitivity_Y(float value)
    {
        freeLook.m_YAxis.m_MaxSpeed = value;
    }
}
