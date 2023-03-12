using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aquapunk
{
    public class CameraModifier : MonoBehaviour
    {
        public Player player;
        public float buildOrthoSize, trevelOrthoSize;

        public Action buildMode;
        public Action trevelMode;

        [SerializeField]
        private CinemachineVirtualCamera VirtualCamera;

        public void OnBuildMode()
        {
            buildMode?.Invoke();
        }

        public void OnTrevelMode()
        {
            trevelMode?.Invoke();
        }

        private void CameraMode(float orthoSize, Transform trigger)
        {
            VirtualCamera.m_Lens.OrthographicSize = orthoSize;
            VirtualCamera.LookAt = trigger;
            VirtualCamera.Follow = trigger;
        }

        private void Start()
        {
            VirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            buildMode += () => CameraMode(buildOrthoSize, null);
            trevelMode += () => CameraMode(trevelOrthoSize, player.transform);
        }
    }
}