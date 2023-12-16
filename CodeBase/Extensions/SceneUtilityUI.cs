using System.Collections;
using CodeBase.Gameplay.States;
using CodeBase.Infrastructure.States.GameStates;
using UnityEngine;
using Zenject;

namespace CodeBase.Extensions
{
    public class SceneUtilityUI : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;
        private LevelStateMachine _levelStateMachine;
        private float _fpsCount;

        /*private void Awake() => 
            StartCoroutine(GetFPS());*/

        private IEnumerator GetFPS()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(0.5f);
                
                _fpsCount = (int)(1f / Time.unscaledDeltaTime);
            }
        }

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, LevelStateMachine levelStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _levelStateMachine = levelStateMachine;
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / 1920f, Screen.height / 1080f, 1f));
            GUIStyle brnStyle = new GUIStyle(GUI.skin.button);
            brnStyle.fontSize = 30;
            
            if (GUI.Button(new Rect(10f, 170f, 500f, 100f), "Restart", brnStyle))
            {
                _gameStateMachine.Enter<RestartSceneState>();
            }
            
            if (GUI.Button(new Rect(10f, 280f, 500f, 100f), "Pause", brnStyle))
            {
                if (!_levelStateMachine.InState<LevelPauseState>())
                    _levelStateMachine.Enter<LevelPauseState>();
                else
                    _levelStateMachine.Enter<LevelLoopState>();
            }

            /*GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
            GUI.Label(new Rect(10, 220, 300, 100), "FPS: " + _fpsCount.ToString(), labelStyle);*/
        }
    }
}