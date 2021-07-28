using UnityEngine;
using Lance.Common;

namespace Lance.Pattern.LevelBase
{
    public class LevelRoot : MonoBehaviour
    {
        #region properties

        [SerializeField, ReadOnly] private int levelIndex; // level index

        [SerializeField, ReadOnly] private LevelMap levelPrefab; // level prefab

        private LevelMap _levelMap; // instance of level

        public int LevelIndex => levelIndex; // level index property

        #endregion

        #region function

        /// <summary>
        /// intialize
        /// </summary>
        /// <param name="level"></param>
        /// <param name="prefab"></param>
        public virtual void Initialized(int level, LevelMap prefab)
        {
            levelIndex = level;
            levelPrefab = prefab;
        }

        /// <summary>
        /// install level map and level root if need
        /// </summary>
        public virtual void Install()
        {
            if (_levelMap != null) DestroyImmediate(_levelMap); // todo fading...

            _levelMap = Instantiate(levelPrefab, transform);
            var t = _levelMap.transform;
            t.localPosition = Vector3.zero;
            t.localEulerAngles = Vector3.zero;
            t.localScale = Vector3.one;
            _levelMap.Root = this;
        }

        /// <summary>
        /// enable
        /// </summary>
        public virtual void DarknessRise() { _levelMap.DarknessRise(); }

        /// <summary>
        /// disable
        /// </summary>
        public virtual void LightReturn() { _levelMap.LightReturn(); }

        /// <summary>
        /// clear level in root
        /// </summary>
        public virtual void Clear()
        {
            if (_levelMap != null) Util.Destroy(_levelMap.gameObject);
        }

        #endregion
    }
}