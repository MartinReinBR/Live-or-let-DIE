using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Vector3EqualityComparer : IEqualityComparer<Vector3>
{

    public bool Equals(Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Distance(vec1, vec2) < 0.1f;
    }

    public int GetHashCode(Vector3 vec)
    {
        return Mathf.FloorToInt(vec.x) ^ Mathf.FloorToInt(vec.y) << 2 ^ Mathf.FloorToInt(vec.z) >> 2;
    }
}


public class MoveDice : MonoBehaviour
{
    [SerializeField] private float TurnSpeed = 300;
    [SerializeField] private GameObject playerGameObject;
    public bool isRolling { get; private set; }

    [SerializeField] private Renderer rend;

    public void MoveTheDice(Vector3 direction, Action callWhenDone = null, Func<float, float> rateFunction = null)
    {
        float dir = 0;
        if (Vector3.Distance(direction, Vector3.forward) < 0.1f)
            dir = rend.bounds.max.z;
        else if (Vector3.Distance(direction, Vector3.back) < 0.1f)
            dir = rend.bounds.min.z;
        else if (Vector3.Distance(direction, Vector3.right) < 0.1f)
            dir = rend.bounds.max.x;
        else if (Vector3.Distance(direction, Vector3.left) < 0.1f)
            dir = rend.bounds.min.x;
    
        StartCoroutine(RollCube(direction, dir, callWhenDone, rateFunction));
    }

    IEnumerator RollCube(Vector3 direction, float dir, Action callWhenDone, Func<float, float> rateFunction)
    {
        if (!isRolling)
        {
            isRolling = true;
            float remainingAngle = 90;
            Vector3 rotationCenter = rend.bounds.center;

            if (Mathf.Abs(direction.x) > 0.1f)
            {
                rotationCenter = new Vector3(dir, rend.bounds.min.y, 0);
            }

            else if (Mathf.Abs(direction.z) > 0.1f)
            {
                rotationCenter = new Vector3(0, rend.bounds.min.y, dir);
            }

            Vector3 rotationAxis = Vector3.Cross(Vector3.up, direction);


            while (remainingAngle > 0)
            {
                float rate = 1f;
                if(rateFunction != null)
                {
                    rate = rateFunction(remainingAngle / 90f);
                    rate = Mathf.Clamp(rate, 0.1f, 1.0f);
                }

                float rotationAngle = Mathf.Min(Time.deltaTime * TurnSpeed * rate, remainingAngle);
                transform.RotateAround(rotationCenter, rotationAxis, rotationAngle);
                remainingAngle -= rotationAngle;
                yield return null;
            }

            Vector3 temp = transform.position;
            temp.x = Mathf.Round(temp.x);
            temp.y = Mathf.Round(temp.y);
            temp.z = Mathf.Round(temp.z);
            transform.position = temp;

            if(callWhenDone != null)
                callWhenDone();

            isRolling = false;

        }
    }

    public IEnumerator PushCube(Vector3 target)
    {
        float progress = 0.0f;
        while (Vector3.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, target, progress);
            progress += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
    }
}
