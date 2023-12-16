using CodeBase.Data.Level;
using CodeBase.Gameplay.Services.BlockBind;
using CodeBase.Services.StaticData;
using UnityEngine;
using Zenject;

namespace CodeBase.Gameplay.Services.BlockReleaseTimer
{
    public class ReleaseTimerController : IInitializable, ILateTickable
    {
        public bool Enabled { get; private set; }

        private Timer _model;
        private readonly ReleaseTimerView _view;
        private readonly IStaticDataService _staticDataService;
        private readonly BlockBinder _blockBinder;

        public ReleaseTimerController(
            Timer model,
            ReleaseTimerView view,
            IStaticDataService staticDataService,
            BlockBinder blockBinder
            )
        {
            _model = model;
            _view = view;
            _staticDataService = staticDataService;
            _blockBinder = blockBinder;
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
            _blockBinder.Unbind();
    }
}