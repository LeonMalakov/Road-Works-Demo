using UnityEngine;

namespace Combine
{
    public enum CollisionSide
    {
        Front,
        Back,
        Right,
        Left,
        FrontRight,
        FrontLeft,
        BackRight,
        BackLeft
    }

    public static class CollisionSideHelper
    {
        public static CollisionSide Determine(Vector3 orgin, Vector3 target)
        {
            Vector3 vector = target - orgin;

            float angle = Vector3.Angle(vector, Vector3.forward);

            CollisionSide side = CollisionSide.Front;

            // Front
            if (angle <= 22.5f)
                side = CollisionSide.Front;

            // Back.
            else if (angle > 168.75f)
                side = CollisionSide.Back;

            // Sth-Right.
            else if (vector.x > 0)
            {
                // Front-Right.
                if (angle > 22.5f && angle <= 67.5f)
                    side = CollisionSide.FrontRight;

                // Right.
                else if (angle > 67.5f && angle <= 112.5f)
                    side = CollisionSide.Right;

                // Back-Right.
                else if (angle > 112.5f && angle <= 168.75f)
                    side = CollisionSide.BackRight;
            }

            // Sth-Left.
            else
            {
                // Front-Left.
                if (angle > 22.5f && angle <= 67.5f)
                    side = CollisionSide.FrontLeft;

                // Left.
                else if (angle > 67.5f && angle <= 112.5f)
                    side = CollisionSide.Left;

                // Back-Left.
                else if (angle > 112.5f && angle <= 168.75f)
                    side = CollisionSide.BackLeft;
            }

            return side;
        }
    }
}