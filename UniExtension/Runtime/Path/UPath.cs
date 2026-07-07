using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniExtension.Path
{
    [Serializable]
    public class UPath : IEquatable<UPath>, IEnumerable<Vector3>
    {
        [SerializeField] private List<Vector3> points = new List<Vector3>();

        public List<Vector3> Points
        {
            get
            {
                Ensure();
                return points;
            }
            set
            {
                points = value ?? new List<Vector3>();
            }
        }

        public int Count
        {
            get { return Points.Count; }
        }

        public Vector3 this[int index]
        {
            get { return Points[index]; }
            set { Points[index] = value; }
        }

        public float Length
        {
            get
            {
                float length = 0f;

                for (int i = 1; i < Points.Count; i++)
                {
                    length += Vector3.Distance(Points[i - 1], Points[i]);
                }

                return length;
            }
        }

        public void Ensure()
        {
            if (points == null)
                points = new List<Vector3>();
        }

        public void AddPoint(Vector3 point)
        {
            Points.Add(point);
        }

        public void RemovePoint(Vector3 point)
        {
            Points.Remove(point);
        }

        public void RemoveAt(int index)
        {
            Points.RemoveAt(index);
        }

        public void InsertPoint(int index, Vector3 point)
        {
            Points.Insert(index, point);
        }

        public void Clear()
        {
            Points.Clear();
        }

        public Vector3 GetPoint(float distance)
        {
            if (Points.Count == 0)
                throw new InvalidOperationException("Path has no points.");

            if (distance <= 0f)
                return Points[0];

            float accumulatedDistance = 0f;

            for (int i = 1; i < Points.Count; i++)
            {
                Vector3 from = Points[i - 1];
                Vector3 to = Points[i];

                float segmentLength = Vector3.Distance(from, to);

                if (Mathf.Approximately(segmentLength, 0f))
                    continue;

                if (accumulatedDistance + segmentLength >= distance)
                {
                    float t = (distance - accumulatedDistance) / segmentLength;
                    return Vector3.Lerp(from, to, t);
                }

                accumulatedDistance += segmentLength;
            }

            return Points[Points.Count - 1];
        }

        public float GetClosestPointDistance(Vector3 position)
        {
            if (Points.Count == 0)
                return float.MaxValue;

            float closestDistance = float.MaxValue;

            for (int i = 0; i < Points.Count; i++)
            {
                float distance = Vector3.Distance(position, Points[i]);

                if (distance < closestDistance)
                    closestDistance = distance;
            }

            return closestDistance;
        }

        public IEnumerator<Vector3> GetEnumerator()
        {
            return Points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                for (int i = 0; i < Points.Count; i++)
                {
                    hash = hash * 31 + Points[i].GetHashCode();
                }

                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as UPath);
        }

        public bool Equals(UPath other)
        {
            if (ReferenceEquals(other, null))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Points.Count != other.Points.Count)
                return false;

            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i] != other.Points[i])
                    return false;
            }

            return true;
        }

        public static bool operator ==(UPath left, UPath right)
        {
            if (ReferenceEquals(left, null))
                return ReferenceEquals(right, null);

            return left.Equals(right);
        }

        public static bool operator !=(UPath left, UPath right)
        {
            return !(left == right);
        }
    }
}