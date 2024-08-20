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
    float save_mouseXSpeed;
    float save_mouseYSpeed;
    CinemachineFreeLook freeLook;
    [SerializeField] PlayerInputs playerInputs;

    void Awake()
    {
        playerInputs = GameObject.Find("Player").GetComponent<PlayerInputs>();
        obj = GameObject.Find("FreeLook Camera");
        freeLook = obj.GetComponent<CinemachineFreeLook>();
        mouseSensitivitySlider_X.onValueChanged.AddListener(SetMouseSensitivity_X);
        mouseSensitivitySlider_Y.onValueChanged.AddListener(SetMouseSensitivity_Y);
        save_mouseXSpeed = freeLook.m_XAxis.m_MaxSpeed;
        save_mouseYSpeed = freeLook.m_YAxis.m_MaxSpeed;
    }

    void Update()
    {
        if (playerInputs.isInteracting)
        {
            StartUICameraMode();
        } else
        {
            StopUICameraMode();
        }
    }

    void SetMouseSensitivity_X(float value)
    {
        freeLook.m_XAxis.m_MaxSpeed = value;
        if (!playerInputs.isInteracting)
        {
            save_mouseXSpeed = value;
        }
    }

    void SetMouseSensitivity_Y(float value)
    {
        freeLook.m_YAxis.m_MaxSpeed = value;
        if (!playerInputs.isInteracting)
        {
            save_mouseYSpeed = value;
        }
    }

    void StartUICameraMode()
    {
        if (freeLook.m_XAxis.m_MaxSpeed != 0 || freeLook.m_YAxis.m_MaxSpeed != 0)
        {
            save_mouseXSpeed = freeLook.m_XAxis.m_MaxSpeed;
            save_mouseYSpeed = freeLook.m_YAxis.m_MaxSpeed;

            freeLook.m_XAxis.m_MaxSpeed = 0;
            freeLook.m_YAxis.m_MaxSpeed = 0;
        }
    }

    void StopUICameraMode()
    {
        freeLook.m_XAxis.m_MaxSpeed = save_mouseXSpeed;
        freeLook.m_YAxis.m_MaxSpeed = save_mouseYSpeed;
    }
}
