using System;

namespace FruitMatch.Scripts.MapScripts
{
    public class LevelReachedEventArgs : EventArgs
    {
        public int Number
        {
            get; private set;
        }

        public LevelReachedEventArgs(int number)
        {
            Number = number;
        }
    }
}
