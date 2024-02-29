using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PositionOnLevel
    {
        public Vector3Data Position;
        public string Level;

        public PositionOnLevel(string initialLevel)
        {
            Level = initialLevel;
        }

        public PositionOnLevel(Vector3Data position, string level)
        {
            Position = position;
            Level = level;
        }
    }
}