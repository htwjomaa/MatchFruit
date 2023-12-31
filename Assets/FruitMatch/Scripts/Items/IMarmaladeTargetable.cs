using UnityEngine;

namespace FruitMatch.Scripts.Items
{
    public interface IMarmaladeTargetable
    {
        GameObject GetMarmaladeTarget { get; set; }
        GameObject GetGameObject { get; }
        Item GetItem { get; }
    }
}