using System.Threading;
using CodeBase.Data.Level;
using CodeBase.Extensions;
using CodeBase.Services.StaticData;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.HoistingRopeLogic
{
    public class RopeMovement : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        
        [Header("Relocation")]
        [SerializeField] private Vector3 _upperPosition;
        [SerializeField] private Vector3 _lowerPosition;
        [SerializeField, Min(0f)] private float _showingTime;
        [SerializeField] private Ease _relocationEase = Ease.OutQuad;

        [Header("Moving")]
        [SerializeField] private float _frequency;
        [SerializeField] private float _horizontalAmplitude;
        [SerializeField] private float _verticalAmplitude;
        [SerializeField] private float _rotationAmplitude;

        private IStaticDataService _staticDataService;
        private Vector3 _startPosition;
        private float _customTime;
        private CancellationToken _cancellationToken;

        private void Awake() => 
            _cancellationToken = gameObject.GetCancellationTokenOnDestroy();

        private void Start()
        {
            HoistingRopeData config = _staticDataService.ForCurrentMode().HoistingRopeData;

            _frequency = config.frequency;
            _horizontalAmplitude = config.horizontalAmplitude;
            _verticalAmplitude = config.verticalAmplitude;
            _rotationAmplitude = config.rotationAmplitude;
            
            _startPosition = _pivot.localPosition;
        }

        private void Update()
        {
            if (Time.timeScale == 0f)
                return;
            
            Move(Time.deltaTime);
        }

        [Inject]
        public void Construct(IStaticDataService staticDataService) => 
            _staticDataService = staticDataService;
        
        public void ResetLocation()
        {
            transform.position = _upperPosition;
            this.DisableComponent();
        }

        public async UniTask Drop()
        {
            this.EnableComponent();
            await RelocateTo(_lowerPosition);
        }

        public async UniTask Raise()
        {
            await RelocateTo(_upperPosition);
            this.DisableComponent();
        }

        private async UniTask RelocateTo(Vector3 position)
        {
            await transform
                .DOMove(position, _showingTime)
                .SetEase(_relocationEase)
                .AwaitForComplete(cancellationToken: _cancellationToken);
        }

        private void Move(float deltaTime)
        {
            _customTime += deltaTime;
            
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            float x = Mathf.Cos(_customTime * _frequency) * _horizontalAmplitude;
            float y = Mathf.Sin(_customTime * _frequency) * _verticalAmplitude;

            _pivot.localPosition = _startPosition + new Vector3(x, y, 0f);
        }

        private void UpdateRotation()
        {
            float z = Mathf.Cos(_customTime * _frequency) * _rotationAmplitude;

            _pivot.localEulerAngles = new Vector3(0f, 0f, z);
        }
    }
}