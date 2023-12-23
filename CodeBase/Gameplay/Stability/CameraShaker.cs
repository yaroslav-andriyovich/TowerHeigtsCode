using System;
using CodeBase.Data.Level;
using CodeBase.Services.StaticData;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Stability
{
    public class CameraShaker : IInitializable, IDisposable
    {
        private readonly IStaticDataService _staticDataService;
        
        private CameraShakerData _config;
        private Transform _cameraTransform;

        public CameraShaker(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;

        public void Initialize()
        {
            _cameraTransform = Camera.main.transform;
            _config = _staticDataService.ForCurrentMode().CameraShakerData;
        }

        public void Dispose() => 
            _cameraTransform.DOKill();

        public void Shake() =>
            _cameraTransform.DOShakePosition(_config.duration, _config.strength);
    }
}