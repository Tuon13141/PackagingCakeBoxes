using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField]
    private Vector2 targetPoint;
    public bool reachTargetPoint = false;
    public float moveSpeed = 5f;

    public void SetTargetPoint(Vector2 targetPoint)
    {
        this.targetPoint = targetPoint;
    }

    void FixedUpdate()
    {
        Vector2 thisPos = this.transform.position;
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if(thisPos == targetPoint)
        {
            reachTargetPoint = true;
        }
        else
        {
            reachTargetPoint = false;
        }
    }

    public IEnumerator DestroyObject(GameObject gameObject)
    {
        reachTargetPoint = false;
        yield return new WaitUntil(() => this.reachTargetPoint);

        Destroy(gameObject);
    }
}
