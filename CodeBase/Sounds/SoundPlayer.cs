using UnityEngine;

namespace CodeBase.Sounds
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _combo;
        [SerializeField] private AudioSource _miss;
        [SerializeField] private AudioSource _gameOver;
        
        private void Awake() => 
            DontDestroyOnLoad(this);


        public void PlayComboHit() => 
            _combo.Play();

        public void PlayMiss() => 
            _miss.Play();

        public void PlayGameOver() => 
            _gameOver.Play();
    }
}