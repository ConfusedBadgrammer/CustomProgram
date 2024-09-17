using Microsoft.Xna.Framework;
using System;

namespace SpaceDefenders
{
    public static class CollisionDetection
    {
        //Axil alligned boundary box collision detection
        public static bool IsOverlapping(Entity entityOne, Entity entityTwo)
        {
            if (entityOne.Position.X <= entityTwo.Position.X + entityTwo.Width &&
                entityOne.Position.X + entityOne.Width >= entityTwo.Position.X &&
                entityOne.Position.Y <= entityTwo.Position.Y + entityTwo.Height &&
                entityOne.Position.Y + entityOne.Height >= entityTwo.Position.Y)
            {
                return true;
            }
            return false;
        }
        // Calculate the closest point between a circle entity and another entity's boundary box and compare to the radius
        public static bool IsOverlappingCircle(Entity entityOne, Entity entityTwo)
        {
            float circleX = entityOne.Position.X + entityOne.Width / 2;
            float circleY = entityOne.Position.Y + entityOne.Height / 2;
            Vector2 circleCenter = new Vector2(circleX, circleY);

            float circleRadius = Math.Min(entityOne.Width / 2, entityOne.Height / 2);

            // Keep the values within the entityTwo's dimensions
            float closestX = MathHelper.Clamp(circleCenter.X, entityTwo.Position.X, entityTwo.Position.X + entityTwo.Width);
            float closestY = MathHelper.Clamp(circleCenter.Y, entityTwo.Position.Y, entityTwo.Position.Y + entityTwo.Height);

            float distance = Vector2.Distance(circleCenter, new Vector2(closestX, closestY));

            return distance <= circleRadius;
        }
        //PURE FUNCTION - static classes only have pure functions, no variables are changed
        public static bool IsAtEdge(Vector2 vector, int width, int height)
        {
            if (vector.X <= 0 ||
                vector.X + width >= GameConstants.ScreenWidth ||
                vector.Y <= 0 ||
                vector.Y + height >= GameConstants.ScreenHeight
                )
            {
                return true;
            }
            return false;
        }
    }
}
