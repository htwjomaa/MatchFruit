using System.Collections;
using System.Collections.Generic;
using FruitMatch.Scripts.Core;
using FruitMatch.Scripts.Items;
using UnityEngine;

namespace FruitMatch.Scripts.Blocks
{
    /// <summary>
    /// block expands constantly until you explode one
    /// </summary>
    public class ThrivingBlock : Square
    {
        static bool blockCreated;
        int lastMoveID = -1;

        void OnEnable()
        {
            LevelManager.OnTurnEnd += OnTurnEnd;
        }

        void OnDisable()
        {
            LevelManager.OnTurnEnd -= OnTurnEnd;
            LevelManager.THIS.thrivingBlockDestroyed = true;
        }

        private void OnTurnEnd()
        {
            if (LevelManager.THIS.moveID == lastMoveID) return;
            lastMoveID = LevelManager.THIS.moveID;
            if (LevelManager.THIS.thrivingBlockDestroyed || blockCreated || field != LevelManager.THIS.field) return;
            LevelManager.THIS.thrivingBlockDestroyed = false;
            List<Square> sqList = new List<Square>();
             sqList = mainSquare.GetAllNeghborsCross();
            foreach (var sq in sqList)
            {
                if (!sq.CanGoInto() || Random.Range(0, 1) != 0 ||
                    sq.type != SquareTypes.EmptySquare || sq.Item?.currentType != ItemsTypes.NONE) continue;
                if (sq.Item == null) continue;
                if (sq.Item.currentType != ItemsTypes.NONE) continue;
                sq.CreateObstacle(SquareTypes.ThrivingBlock, 1);
                blockCreated = true;
                StartCoroutine(blockCreatedCD());
                Destroy(sq.Item.gameObject);
                break;
            }
        }

        IEnumerator blockCreatedCD()
        {
            yield return new WaitForSeconds(1);
            blockCreated = false;
        }
    }
}