using System;
using System.Collections.Generic;
using CodeBase.Data.Level;
using CodeBase.Services.StaticData;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.TransformDescend
{
    public class TransformDescender : IInitializable, IDisposable
    {
        private readonly TransformsToDescendProvider _transformsToDescendProvider;
        private readonly IStaticDataService _staticDataService;

        private List<Tween> _moveAnimations;
        private List<Transform> _transforms;
        private TransformDescenderData _config;

        public TransformDescender(
            TransformsToDescendProvider transformsToDescendProvider, 
            IStaticDataService staticDataService
            )
        {
            _transformsToDescendProvider = transformsToDescendProvider;
            _staticDataService = staticDataService;
            
            _transforms = new List<Transform>();
            _moveAnimations = new List<Tween>();
        }

        public void Initialize() => 
            _config = _staticDataService.ForCurrentMode().TransformDescenderData;

        public void Dispose()
        {
            foreach (Tween moveAnimation in _moveAnimations)
                moveAnimation.Kill();
            
            _moveAnimations.Clear();
        }

        public void Move(float distance)
        {
            if (_moveAnimations.Count != 0)
                throw new InvalidOperationException("The movement is not complete!");

            foreach (Transform transform in _transforms)
            {
                Tween tween = RunMovement(transform, distance);

                SaveMovementTween(tween);
            }
        }

        public void CompleteMovement()
        {
            foreach (Tween moveAnimation in _moveAnimations)
                moveAnimation.Complete();
            
            _moveAnimations.Clear();
        }

        public void DescendCustomTransforms(List<Transform> transforms) => 
            ChangeMovingParts(transforms);

        public void DescendDefaultTransforms() => 
            ChangeMovingParts(_transformsToDescendProvider.transforms);

        private Tween RunMovement(Transform transform, float distance) =>
            transform.
                DOLocalMove(new Vector3(0f, -distance, 0f), _config.descendTime).
                SetRelative().
                SetEase(Ease.OutFlash);

        private void SaveMovementTween(Tween tween) => 
            _moveAnimations.Add(tween);

        private void ChangeMovingParts(List<Transform> transforms)
        {
            if (_moveAnimations.Count != 0)
                throw new InvalidOperationException("The movement is not complete!");

            _transforms = transforms;
        }
    }
}