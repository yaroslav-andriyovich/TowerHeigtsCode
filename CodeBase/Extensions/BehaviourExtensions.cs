using UnityEngine;

namespace CodeBase.Extensions
{
    public static class BehaviourExtensions
    {
        public static void EnableComponent(this Behaviour behaviour) => 
            behaviour.enabled = true;
        
        public static void DisableComponent(this Behaviour behaviour) => 
            behaviour.enabled = false;
    }
}