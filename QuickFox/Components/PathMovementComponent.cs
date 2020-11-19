using System;
using System.Collections.Generic;
using QuickFox.Rendering;

namespace QuickFox.Components
{
    public class PathMovementComponent : Component
    {
        public List<Point> KeyPoints { get; }

        public bool IsClosed { get; set; }

        public int CurrentTargetIndex { get; set; }

        public Point? CurrentTarget => GetCurrentTarget();

        public float Speed { get; set; }

        public PathMovementComponent()
        {
            KeyPoints = new List<Point>();
        }

        public void Clear()
        {
            KeyPoints.Clear();
        }

        public PathMovementComponent AddMovements(IList<Point> points)
        {
            KeyPoints.AddRange(points);
            return this;
        }

        public PathMovementComponent MoveTo(Point point)
        {
            KeyPoints.Add(point);
            return this;
        }

        public PathMovementComponent MoveTo(float x, float y)
        {
            KeyPoints.Add(new Point { X = x, Y = y});
            return this;
        }

        public void ChooseNext()
        {
            if (CurrentTargetIndex >= KeyPoints.Count - 1)
            {
                CurrentTargetIndex = IsClosed ? 0 : -1;
            } else
            {
                CurrentTargetIndex++;
            }
        }

        private Point? GetCurrentTarget()
        {
            if (CurrentTargetIndex >= 0 && CurrentTargetIndex < KeyPoints.Count)
            {
                return KeyPoints[CurrentTargetIndex];
            }
            else
            {
                return null;
            }
        }
    }
}
