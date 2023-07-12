using System;
using System.Collections.Generic;
using FruitMatch.Scripts.TargetScripts.TargetEditor;
using Malee;
using UnityEngine;
using UnityEngine.Serialization;

namespace FruitMatch.Scripts.TargetScripts.TargetSystem
{
    /// <summary>
    /// target container keeps the object should be collected, its count, sprite, color
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "TargetContainer", menuName = "TargetContainer", order = 1)]
    public class TargetContainer
    {
        public string name = "";
        public CollectingTypes collectAction;
        public int CollectionMaxCount = 0;
        public SetCount setCount;
        [Tooltip("Can switch sublevel if target not complete")]
        public bool CanSwithSublevel;
        [Reorderable]
        public SprArrays defaultSprites;
        public List<GameObject> prefabs = new List<GameObject>();
        public TargetContainer DeepCopy()
        {
            var other = (TargetContainer) MemberwiseClone();

            return other;
        }

        public string GetDescription()
        {
            return "";
            // LocalizationManager.GetText(localization.description.index, localization.description.text);
        }

        public string GetFailedDescription()
        {
            return "";
            //LocalizationManager.GetText(localization.failed.index, localization.failed.text);
        }
    }

    public enum CollectingTypes
    {
        Destroy,
        ReachBottom,
        Spread,
        Clear
    }

    public enum SetCount
    {
        Manually,
        FromLevel
    }

    [Serializable]
    public class SprArrays : ReorderableArray<SprArray>
    {
    }
    
    [Serializable]
    public class SprArray
    {
        [FormerlySerializedAs("sprites0")] [Reorderable]
        public SpriteList sprites;

        public SprArray Clone()
        {
            return (SprArray)MemberwiseClone();
        }
    }
}