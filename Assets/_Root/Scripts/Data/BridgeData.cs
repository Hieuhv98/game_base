using Cysharp.Threading.Tasks;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Gamee_Hiukka.Control 
{
    public static class BridgeData
    {
        public static async UniTask<GameObject> GetLevel(int index)
        {
            if (Config.IsTest && Config.LevelTest != null) return Config.LevelTest.gameObject;
            return await Addressables.LoadAssetAsync<GameObject>(string.Format(Constant.LEVEL, index));
        }
    }
}

