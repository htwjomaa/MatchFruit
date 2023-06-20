using FruitMatch.Scripts.Items;
using UnityEngine;

namespace FruitMatch.Scripts
{
    public class ItemAnimEvents : MonoBehaviour {


        public Item item;

        public void SetAnimationDestroyingFinished()
        {
            item.SetAnimationDestroyingFinished();
        }
    }
}
