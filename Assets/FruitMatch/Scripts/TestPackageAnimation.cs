using UnityEngine;

namespace FruitMatch.Scripts
{
    public class TestPackageAnimation : UnityEngine.MonoBehaviour
    {
        public GameObject expl;
        [SerializeField] private int count;

        private void OnEnable()
        {
            for (int i = 0; i < count; i++)
            {
                Instantiate(expl);
            }
        }
    }
}