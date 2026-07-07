using System;
using UnityEngine;

namespace UniExtension.Path
{
    public enum UPathEditPlane
    {
        XZ,
        XY,
        YZ,
        View
    }

    public class UPathBuilder : MonoBehaviour
    {
        [SerializeField] private UPath path = new UPath();

        [Header("Final Result")]
        [SerializeField] private bool applyBuilderTransform = false;
        [SerializeField] private Vector3 offset = Vector3.zero;

        [Header("Scene Edit")]
        [SerializeField] private UPathEditPlane addPointPlane = UPathEditPlane.XZ;
        [SerializeField] private bool drawLines = true;
        [SerializeField] private bool drawLabels = true;
        [SerializeField, Min(0.01f)] private float handleSize = 0.08f;

        public UPath Path
        {
            get
            {
                EnsurePath();
                return path;
            }
            set
            {
                path = value ?? new UPath();
                EnsurePath();
            }
        }

        public bool ApplyBuilderTransform
        {
            get { return applyBuilderTransform; }
            set { applyBuilderTransform = value; }
        }

        public Vector3 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public UPathEditPlane AddPointPlane
        {
            get { return addPointPlane; }
        }

        public bool DrawLines
        {
            get { return drawLines; }
        }

        public bool DrawLabels
        {
            get { return drawLabels; }
        }

        public float HandleSize
        {
            get { return Mathf.Max(0.01f, handleSize); }
        }

        public int Count
        {
            get { return Path.Count; }
        }

        public float FinalLength
        {
            get
            {
                EnsurePath();

                float length = 0f;

                for (int i = 1; i < Path.Count; i++)
                {
                    length += Vector3.Distance(GetFinalPoint(i - 1), GetFinalPoint(i));
                }

                return length;
            }
        }

        public void EnsurePath()
        {
            if (path == null)
                path = new UPath();

            path.Ensure();
        }

        public Vector3 GetRawPoint(int index)
        {
            return Path[index];
        }

        public Vector3 GetOffsetPoint(int index)
        {
            return Path[index] + offset;
        }

        public Vector3 GetFinalPoint(int index)
        {
            return ToFinalPoint(Path[index]);
        }

        public Vector3 GetFinalPoint(float distance)
        {
            EnsurePath();

            if (Path.Count == 0)
                throw new InvalidOperationException("Path has no points.");

            if (distance <= 0f)
                return GetFinalPoint(0);

            float accumulatedDistance = 0f;

            for (int i = 1; i < Path.Count; i++)
            {
                Vector3 from = GetFinalPoint(i - 1);
                Vector3 to = GetFinalPoint(i);

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

            return GetFinalPoint(Path.Count - 1);
        }

        public Vector3[] GetFinalPoints()
        {
            EnsurePath();

            Vector3[] result = new Vector3[Path.Count];

            for (int i = 0; i < Path.Count; i++)
            {
                result[i] = GetFinalPoint(i);
            }

            return result;
        }

        public Vector3 ToFinalPoint(Vector3 rawPoint)
        {
            Vector3 offsetPoint = rawPoint + offset;

            if (applyBuilderTransform)
                return transform.TransformPoint(offsetPoint);

            return offsetPoint;
        }

        public Vector3 ToRawPoint(Vector3 finalPoint)
        {
            Vector3 offsetPoint;

            if (applyBuilderTransform)
                offsetPoint = transform.InverseTransformPoint(finalPoint);
            else
                offsetPoint = finalPoint;

            return offsetPoint - offset;
        }

        public void ApplyOffsetToPoints()
        {
            EnsurePath();

            if (offset == Vector3.zero)
                return;

            for (int i = 0; i < Path.Count; i++)
            {
                Path[i] += offset;
            }

            offset = Vector3.zero;
        }

        private void Reset()
        {
            EnsurePath();
        }

        private void OnValidate()
        {
            EnsurePath();
        }
    }
}