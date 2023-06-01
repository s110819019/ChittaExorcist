using UnityEngine;

namespace ChittaExorcist.CharacterCore
{
    public class CoreComp<T> where T : CoreComponent
    {
        private readonly Core _core;
        private T _comp;

        public T Comp => _comp ? _comp : _core.GetCoreComponent(out _comp);

        public CoreComp(Core core)
        {
            if (core == null)
            {
                Debug.LogWarning($" Core 在設置 {typeof(T)} 類型 CoreComponent 時為 Null ");
            }
            
            _core = core;
        }
    }
}