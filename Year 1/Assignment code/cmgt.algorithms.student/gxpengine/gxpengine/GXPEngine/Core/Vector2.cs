using System;

namespace GXPEngine.Core
{
    public struct Vector2
    {
        public float x;
        public float y;

        public Vector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public float Length() => Mathf.Sqrt(Mathf.Pow(this.x, 2) + Mathf.Pow(this.y, 2));

        override public string ToString()
        {
            return "[Vector2 " + x + ", " + y + "]";
        }
    }
}