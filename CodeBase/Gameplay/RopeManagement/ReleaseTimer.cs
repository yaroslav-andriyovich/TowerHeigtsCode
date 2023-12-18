using CodeBase.Data.Level;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.RopeManagement
{
    public class ReleaseTimer : IInitializable, ILateTickable
    {
        public bool Enabled { get; private set; }

        private Timer _model;
        private readonly ReleaseTimerView _view;
        private readonly Rope _rope;
        private readonly IStaticDataService _staticDataService;

        public ReleaseTimer(
            Timer model,
            ReleaseTimerView view,
            Rope rope,
            IStaticDataService staticDataService
            )
        {
            _model = model;
            _view = view;
            _rope = rope;
            _staticDataService = staticDataService;
        }

        public void Initialize()
        {
            ReleaseTimerData config = _staticDataService.ForCurrentMode().ReleaseTimerData;
            
            _model.ChangeDuration(config.duration);
            _view.SetProgress(_model.CurrentTime, _model.Duration);
        }

        public void LateTick()
        {
            if (!Enabled || Time.timeScale == 0f)
                return;
            
            _model.Update();
            _view.SetProgress(_model.CurrentTime, _model.Duration);

            if (_model.IsElapsed)
            {
                Stop();
                Perform();
            }
        }

        public void Start()
        {
            _model.Reset();
            Enabled = true;
        }

        public void Stop() => 
            Enabled = false;

        private void Perform() =>
            _rope.ReleaseBlock();
    }
}