using System.Linq;
using FruitMatch.Scripts.Blocks;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Level;
using FruitMatch.Scripts.System;
using FruitMatch.Scripts.TargetScripts.TargetSystem;
using UnityEngine;

namespace FruitMatch.Scripts.TargetScripts
{
    /// <summary>
    /// Solid blocks target
    /// </summary>
    public class SolidBlock : Target
    {
        public override int CountTarget()
        {
            return GetDestinationCount();
        }

        public override int CountTargetSublevel()
        {
            return GetDestinationCountSublevel();
        }
        public override void InitTarget(LevelData levelData)
        {
            subTargetContainers[0].count = levelData.fields.Sum(x => x.levelSquares.Count(i =>  i.block == SquareTypes.SolidBlock ||  i.obstacle == SquareTypes.SolidBlock));
        }
        public override int GetDestinationCountSublevel()
        {
            var count = 0;
            var field = LevelManager.THIS.field;
            count += field.CountSquaresByType(GetType().Name.ToString());
            return count;
        }

        public override int GetDestinationCount()
        {
            var count = 0;
            var fieldBoards = LevelManager.THIS.fieldBoards;
            foreach (var item in fieldBoards)
            {
                // count += item.CountSquaresByType(this.GetType().ToString());
                count += item.GetTargetObjects().Count();
            }
            return count;
        }

        public override void FulfillTarget<T>(T[] _items)
        {
            if (_items.TryGetElement(0)?.GetType() != typeof(Square)) return;
            var items = _items as Square[];
            var sugarList = items?.Where(i => i.type.ToString() == GetType().Name.ToString());
            CollectionStyle collectionStyle = CollectionStyle.Destroy;
            var pos = TargetGUI.GetTargetGUIPosition(LevelData.THIS.GetFirstTarget(true).name, ref collectionStyle);
            
   
          
            
            foreach (var sugarBlock in sugarList)
            {
                Square sugarBlockSubSquare = sugarBlock.GetSubSquare();
                Vector2 scale = sugarBlockSubSquare.transform.localScale;
                var targetContainer = subTargetContainers.Where(i => sugarBlock.type.ToString().Contains(i.targetPrefab.name)).FirstOrDefault();
                amount++;
                var itemAnim = new GameObject();
                var animComp = itemAnim.AddComponent<AnimateItems>();
                LevelManager.THIS.animateItems.Add(animComp);

                if (collectionStyle == CollectionStyle.Avoid)
                {
                    animComp.InitAnimation(sugarBlockSubSquare.gameObject, Vector3.zero, scale, () => { targetContainer.changeCount(-1); });
                }
                    
                else
                {
                    animComp.InitAnimation(sugarBlockSubSquare.gameObject, pos, scale, () => { targetContainer.changeCount(-1); });
                }
         
                // square.DestroyBlock();
            }
        }

        public override void DestroyEvent(GameObject obj)
        {
            // Debug.Log(obj);
        }

        public override int GetCount(string spriteName)
        {
            // foreach (var item in subTargetContainers)
            // {
            //     if (item.targetPrefab.GetComponent<SpriteRenderer>()?.sprite.name == spriteName)
            //         return item.GetCount();
            // }

            return CountTarget();
        }

        public override bool IsTotalTargetReached()
        {
            return CountTarget() <= 0;
        }

        public override bool IsTargetReachedSublevel()
        {
            return CountTargetSublevel() <= 0;
        }
    }
}