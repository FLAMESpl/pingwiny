using Microsoft.Xna.Framework;
using System;

namespace DlaGrzesia.Objects
{
    public struct ObjectOrientation
    {
        public readonly ObjectOrientationName Name;

        public ObjectOrientation(ObjectOrientationName name)
        {
            Name = name;
        }

        public ObjectOrientation(ObjectHorizontalVector horizontal, ObjectVerticalVector vertical)
        {
            Name = (horizontal, vertical) switch
            {
                (ObjectHorizontalVector.Left, ObjectVerticalVector.Down) => ObjectOrientationName.DownLeft,
                (ObjectHorizontalVector.Left, ObjectVerticalVector.Neutral) => ObjectOrientationName.Left,
                (ObjectHorizontalVector.Left, ObjectVerticalVector.Up) => ObjectOrientationName.UpLeft,
                (ObjectHorizontalVector.Neutral, ObjectVerticalVector.Down) => ObjectOrientationName.Down,
                (ObjectHorizontalVector.Neutral, ObjectVerticalVector.Up) => ObjectOrientationName.Up,
                (ObjectHorizontalVector.Right, ObjectVerticalVector.Down) => ObjectOrientationName.DownRight,
                (ObjectHorizontalVector.Right, ObjectVerticalVector.Neutral) => ObjectOrientationName.Right,
                (ObjectHorizontalVector.Right, ObjectVerticalVector.Up) => ObjectOrientationName.UpRight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public Point PointingDirection => new Point((int)HorizontalVector, (int)VerticalVector);

        public ObjectVerticalVector VerticalVector => Name switch
        {
            ObjectOrientationName.Down or
            ObjectOrientationName.DownRight or
            ObjectOrientationName.DownLeft => ObjectVerticalVector.Down,

            ObjectOrientationName.Up or
            ObjectOrientationName.UpRight or
            ObjectOrientationName.UpLeft => ObjectVerticalVector.Up,

            _ => ObjectVerticalVector.Neutral
        };

        public ObjectHorizontalVector HorizontalVector => Name switch
        {
            ObjectOrientationName.Right or
            ObjectOrientationName.DownRight or
            ObjectOrientationName.UpRight => ObjectHorizontalVector.Right,

            ObjectOrientationName.Left or
            ObjectOrientationName.UpLeft or
            ObjectOrientationName.DownLeft => ObjectHorizontalVector.Left,

            _ => ObjectHorizontalVector.Neutral
        };

        public static ObjectOrientation Random(Random random)
        {
            var orientation = random.Next(0, 7);
            return new ObjectOrientation((ObjectOrientationName)orientation);
        }

        public static ObjectOrientation RandomInDirectionOfOrNeutral(Random random, ObjectHorizontalVector vector)
        {
            var verticalVector = (ObjectVerticalVector)random.Next(-1, 2);
            var horizontalVector = verticalVector == ObjectVerticalVector.Neutral
                ? vector
                : random.Next(0, 2) == 0
                    ? vector
                    : ObjectHorizontalVector.Neutral;

            return new ObjectOrientation(horizontalVector, verticalVector);
        }
    }

    public enum ObjectOrientationName
    {
        Down = 0,
        DownRight = 1,
        Right = 2,
        UpRight = 3,
        Up = 4,
        UpLeft = 5,
        Left = 6,
        DownLeft = 7
    }

    public enum ObjectVerticalVector
    {
        Down = 1,
        Neutral = 0,
        Up = -1
    }

    public enum ObjectHorizontalVector
    {
        Right = 1,
        Neutral = 0,
        Left = -1
    }
}
