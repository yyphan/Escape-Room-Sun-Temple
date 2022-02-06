using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockPick : MonoBehaviour
{
    public Camera camera;
    public Transform innerLock;
    public Transform pickPosition;

    public float maxAngle = 90f;
    public float lockSpeed = 10f;
    public float lockPickSpeed = 1f;
    
    [Range(1,25)]
    public float lockRange = 10f;

    public float eulerAngle;
    public float unlockAngle;
    // using Vector2 as a container for 2 values
    public Vector2 unlockRange;

    public bool movePick = true;

    public int keyPressFlag = 0;

    public UnityEvent unlockEvent;

    public void Initialize()
    {
        NewLock();
    }

    // Update is called once per frame
    void Update()
    {
        // align the pick with the inner lock
        transform.localPosition = pickPosition.localPosition;

        // rotate the lock pick around with A and D
        if (movePick)
        {
            if (Input.GetKey(KeyCode.D))
            {
                eulerAngle += lockPickSpeed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                eulerAngle -= lockPickSpeed;
            }

            // make sure it is within the max angle range
            eulerAngle = Mathf.Clamp(eulerAngle, -maxAngle, maxAngle);

            // Quaternion is used for rotation to avoid problems like gimbal lock
            Quaternion rotateTo = Quaternion.AngleAxis(eulerAngle, Vector3.forward);
            transform.rotation = rotateTo;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            movePick = false;
            keyPressFlag = 1;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            movePick = true;
            keyPressFlag = 0;
        }

        float angleDiff = Mathf.Abs(eulerAngle - unlockAngle);
        // angleDiff < angleDiff will turn the lock
        // angleDiff == 0 will turn the lock to the max
        float percentage = 100 - angleDiff / maxAngle * 100;
        percentage = Mathf.Clamp(percentage, 0, 100);
        float maxRotation = (percentage / 100) * maxAngle;
        float lockRotation = maxRotation * keyPressFlag;

        // rotate the inner lock gradually
        float lockLerp = Mathf.Lerp(innerLock.eulerAngles.z, lockRotation, lockSpeed * Time.deltaTime);
        innerLock.eulerAngles = new Vector3(0, 0, lockLerp);

        if (lockLerp > maxRotation - 1)
        {
            if (eulerAngle < unlockRange.y && eulerAngle > unlockRange.x)
            {
                unlockEvent.Invoke();
            }
            else
            {
                // shake the lock pick
                float randomRotation = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));
            }
        }
    }

    private void NewLock()
    {
        unlockAngle = Random.Range(lockRange - maxAngle, maxAngle - lockRange);
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }
}
