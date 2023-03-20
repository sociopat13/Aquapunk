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

        [SerializeField]
        private CinemachineVirtualCamera VirtualCamera;

        public void OnBuildMode()
        {
            CameraMode(buildOrthoSize, null);
        }

        public void OnTrevelMode()
        {
            CameraMode(trevelOrthoSize, player.transform);
        }

        private void CameraMode(float orthoSize, Transform trigger)
        {
            VirtualCamera.m_Lens.OrthographicSize = orthoSize;
            VirtualCamera.LookAt = trigger;
            VirtualCamera.Follow = trigger;
        }

        private void Start()
        {
            VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }
}