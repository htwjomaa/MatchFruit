using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Items;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.System;
using FruitMatch.Scripts.TargetScripts.TargetSystem;
using UnityEngine;

namespace FruitMatch.Scripts.TargetScripts
{
    /// <summary>
    /// collect items target
    /// </summary>
    public class CollectItems : Target
    {
        public override int CountTarget()
        {
            return amount;
        }

        public override int CountTargetSublevel()
        {
            return amount;
        }

        public override void InitTarget(LevelData levelData)
        {
            foreach (var item in subTargetContainers)
            {
                amount += item.GetCount();
            }

        }

        public override void DestroyEvent(GameObject obj)
        {


        }

        public override void FulfillTarget<T>(T[] _items)
        {
            if (_items.Length>0 && _items[0].GetType().BaseType != typeof(Item)) return;
            var items = _items as Item[];
            foreach (var item in subTargetContainers)
            {
                foreach (var obj in items)
                {
                    if (obj == null) continue;
                    var sprite = obj.GetComponent<IColorableComponent>().directSpriteRenderer.sprite;
                    if ((Sprite)item.extraObject == sprite && item.preCount > 0)
                    {
                        amount--;
                        item.preCount--;
                        CollectionStyle collectionStyle = CollectionStyle.Nothing;
                        var pos = TargetGUI.GetTargetGUIPosition(obj.GetComponent<IColorableComponent>().directSpriteRenderer.sprite.name, ref collectionStyle);
                        
                        Vector3 localScale = obj.transform.localScale;
                        if(collectionStyle== CollectionStyle.Avoid)  localScale = Vector3.zero;
                        var itemAnim = new GameObject();
                        var animComp = itemAnim.AddComponent<AnimateItems>();
                        LevelManager.THIS.animateItems.Add(animComp);
                        animComp.InitAnimation(obj.gameObject, pos, localScale, () => { item.changeCount(-1); });
                    }
                }
            }
        }

        public override int GetDestinationCount()
        {
            return destAmount;
        }

        public override int GetDestinationCountSublevel()
        {
            return destAmount;
        }

        public override bool IsTargetReachedSublevel()
        {
            return amount <= 0;
        }

        public override bool IsTotalTargetReached()
        {
            return amount <= 0;
        }

        public override int GetCount(string spriteName)
        {
            for (var index = 0; index < subTargetContainers.Length; index++)
            {
                var item = subTargetContainers[index];
                if (item.extraObject.name == spriteName)
                    return item.GetCount();
            }

            return CountTarget();
        }
    }
}