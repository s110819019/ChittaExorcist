using ChittaExorcist.Common.Module;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ChittaExorcist.GameCore
{
    public class GameManager : MonoSingleton<GameManager>
    {
        #region w/ Variables

        private const string GameManagerKey = "GameManager";

        #endregion
        
        // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // private static void InstantiateGameManager()
        // {
        //     Addressables.InstantiateAsync(GameManagerKey).Completed += OnInstantiate;
        // }

        // #region w/ On Instantiate
        //
        // private static void OnInstantiate(AsyncOperationHandle<GameObject> obj)
        // {
        //     // obj.Result 等於 GameManager
        //     DontDestroyOnLoad(obj.Result);
        //     obj.Result.transform.SetAsFirstSibling();
        // }        
        //
        // #endregion
    }
}