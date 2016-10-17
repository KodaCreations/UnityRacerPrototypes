using UnityEngine;
using System.Collections;
using System;

public class BezierSpline : MonoBehaviour {

    [SerializeField]
    private Vector3[] points;

    [SerializeField]
    private float[] angles;

    [SerializeField]
    private BezierControlPointMode[] modes;

    [SerializeField]
    private bool loop;

    public BezierSpline parent;
    public BezierSpline child;

    public bool Loop
    {
        get { return loop; }
        set
        {
            loop = value;
            if (value == true)
            {
                modes[modes.Length - 1] = modes[0];
                SetControlPoint(0, points[0]);
            }                
        }
    }
    public enum BezierControlPointMode
    {
        Free,
        Aligned,
        Mirrored
    }
    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }
    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        int modeIndex = (index + 1) / 3;
        modes[modeIndex] = mode;
        if(loop)
        {
            if(modeIndex == 0)
            {
                modes[modes.Length - 1] = mode;
            }
            else if(modeIndex == modes.Length - 1)
            {
                modes[0] = mode;
            }
        }
        EnforceMode(index);
    }
    public void SetControlPoint(int index, Vector3 point)
    {
        if(index % 3 == 0)
        {
            Vector3 delta = point - points[index];
            if(loop)
            {
                if(index == 0)
                {
                    points[1] += delta;
                    points[points.Length - 2] += delta;
                    points[points.Length - 1] = point;
                }
                else if(index == points.Length - 1)
                {
                    points[0] = point;
                    points[1] += delta;
                    points[index - 1] += delta;
                }
                else
                {
                    points[index - 1] += delta;
                    points[index + 1] += delta;
                }
            }
            else
            {
                if (index > 0)
                {
                    points[index - 1] += delta;
                }
                if (index + 1 < points.Length)
                {
                    points[index + 1] += delta;
                }
            }
        }
        points[index] = point;
        EnforceMode(index);
    }
    public void SetControlRotation(int index, float angle)
    {
        angles[index] = angle;
    }
    public Quaternion GetRotationAt(float t)
    {
        if (t > CurveCount)
        {
            if (parent != null)
            {
                float time = t - CurveCount;
                return parent.GetRotationAt(time);
            }
        }
        
        int i = (int)t * 3;
        
        float procent = t - (int)t;

        float angle = (angles[i] * (1 - procent)) + (angles[i + 3] * procent);

        Vector3 currentRotation = GetDirection2(t);
        Quaternion rotation = Quaternion.LookRotation(currentRotation) * Quaternion.Euler(0, 0, angle);
        //Quaternion rotation = new Quaternion(currentRotation.x, currentRotation.y, currentRotation.z, 0);
        return rotation;
    }
    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if(mode == BezierControlPointMode.Free || !loop && (modeIndex == 0 || modeIndex == modes.Length - 1))
            return;

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;
        if(index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            if(fixedIndex < 0)
            {
                fixedIndex = points.Length - 2;
            }
            enforcedIndex = middleIndex + 1;
            if(enforcedIndex >= points.Length)
            {
                enforcedIndex = 1;
            }
        }
        else
        {
            fixedIndex = middleIndex + 1;
            if(fixedIndex >= points.Length)
            {
                fixedIndex = 1;
            }
            enforcedIndex = middleIndex - 1;
            if(enforcedIndex < 0)
            {
                enforcedIndex = points.Length - 2;
            }
        }
        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];
        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }
        points[enforcedIndex] = middle + enforcedTangent;

    }
    public int ControlPointCount
    {
        get { return points.Length; }
    }
    public Vector3 GetControlPoint(int index)
    {
        return points[index];
    }
    public float GetControlRotation(int index)
    {
        return angles[index];
    }
    public Vector3 GetPoint(float t)
    {
        int i;
        if(t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
    }
    public Vector3 GetPointConstantSpeed(ref float t, float length)
    {
        if (t > CurveCount)
        {
            if (parent != null)
            {
                float time = t - CurveCount;
                Vector3 newPos = parent.GetPointConstantSpeed(ref time, length);
                t = time + CurveCount;
                return newPos;
            }
        }
        
        int i = (int)t * 3;
        
        //Brytt upp T och GetPoint i olika funtioner
        return transform.TransformPoint(Bezier.GetPointConstantSpeed(points[i], points[i + 1], points[i + 2], points[i + 3], ref t, length));
    }
    public int CurveCount
    {
        get { return (points.Length - 1) / 3; }
    }

    public Vector3 GetVelocity(float t)
    {
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount;
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
    }

    public Vector3 GetDirection(float t)
    {
        return GetVelocity(t).normalized;
    }
    public Vector3 GetVelocity2(float t)
    {
        float time = t % 1;
        int i = (int)t * 3;
        return transform.TransformPoint(Bezier.GetFirstDerivative(points[i], points[i + 1], points[i + 2], points[i + 3], time)) - transform.position;
    }
    public Vector3 GetDirection2(float t)
    {
        if (t > CurveCount)
        {
            if (parent != null)
            {
                float time = t - CurveCount;
                return parent.GetDirection2(time);
            }
        }
        return GetVelocity2(t).normalized;
    }
    public void AddCurve()
    {
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        Array.Resize(ref angles, angles.Length + 3);

        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;

        angles[angles.Length - 3] = 0;
        angles[angles.Length - 2] = 0;
        angles[angles.Length - 1] = 0;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
        EnforceMode(points.Length - 4);
        if(loop)
        {
            points[points.Length - 1] = points[0];
            modes[modes.Length - 1] = modes[0];
            EnforceMode(0);
        }
    }

    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };

        angles = new float[]
        {
            0,
            0,
            0,
            0
        };

        modes = new BezierControlPointMode[]
        {
            BezierControlPointMode.Mirrored,
            BezierControlPointMode.Mirrored
        };
    }
}
