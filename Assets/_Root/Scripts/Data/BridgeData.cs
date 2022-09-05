using Cysharp.Threading.Tasks;
using Gamee_Hiukka.Common;
using Gamee_Hiukka.Data;
using System.Collections.Generic;
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
        public static async UniTask<GameObject> GetLevelCollection(int index)
        {
            if (Config.IsTest && Config.LevelTest != null) return Config.LevelTest.gameObject;
            return await Addressables.LoadAssetAsync<GameObject>(string.Format(Constant.LEVEL_COLLECTION, index));
        }

        public static void CheckLoopLevel()
        {
            if (GameData.LevelIndexCurrent >= GameData.LevelMax - GameData.LevelStartLoop + 1)
            {
                if (GameData.LevelStartLoop != Config.LevelStartLoop)
                {
                    GameData.LevelStartLoop = Config.LevelStartLoop;
                    List<int> temp = new List<int>();
                    for (int i = GameData.LevelStartLoop; i <= GameData.LevelMax; i++)
                    {
                        temp.Add(i);
                    }
                    GameData.LevelList = temp;
                }

                if (GameData.LevelMax != Config.LevelMax)
                {
                    GameData.LevelMax = Config.LevelMax;
                    DataController.LoadLevelList(true);
                }
                UpdateLevelList();
            }

            if (GameData.LevelCollectionIndexCurrent >= GameData.LevelCollectionMax)
            {
                if (GameData.LevelCollectionMax != Config.LevelCollectionMax)
                {
                    GameData.LevelCollectionMax = Config.LevelCollectionMax;
                    DataController.LoadLevelCollectionList(true);
                }
                UpdateLevelCollectionList();
            }
        }
        public static void UpdateLevelList()
        {
            Util.Shuffle(GameData.LevelList);
            GameData.LevelIndexCurrent = 0;
            DataController.SaveLevelList();
            DataController.SaveLevelIndexCurrent();
        }

        public static void UpdateLevelCollectionList()
        {
            Util.Shuffle(GameData.LevelCollectionList);
            GameData.LevelCollectionIndexCurrent = 0;
            DataController.SaveLevelCollectionList();
            DataController.SaveLevelCollectionIndexCurrent();
        }
    }
}

