using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    [SerializeField] private List<NavPoint> points = new List<NavPoint>();
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Animator anim;

    public enum CustomerState
    {
        Entering,
        Browsing,
        Queing,
        Checkout,
        Exiting
    }

    public CustomerState currentState;

    private float waitTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        points.Clear();
        points.AddRange(CustomerManager.instance.GetEntryPoints());

        transform.position = points[0].point.position;

        if (points.Count > 0)
            waitTime = points[0].waitTime;

        //points.AddRange(CustomerManager.instance.GetExitPoints());
    }

    // Update is called once per frame
    void Update()
    {
        //if(points.Count > 0) 
        //    MoveToPoint();

        switch (currentState)
        {
            case CustomerState.Entering:
                if (points.Count > 0)
                    MoveToPoint();
                else StartExiting();
                break;
            case CustomerState.Browsing:
                // Implement browsing behavior here
                break;
            case CustomerState.Queing:
                // Implement queuing behavior here
                break;
            case CustomerState.Checkout:
                // Implement checkout behavior here
                break;
            case CustomerState.Exiting:
                if (points.Count > 0)
                    MoveToPoint();
                else Destroy(gameObject);
                break;
        }
    }
    public void StartExiting()
    {
        currentState = CustomerState.Exiting;

        points.Clear();
        points.AddRange(CustomerManager.instance.GetExitPoints());
    }
    public void MoveToPoint()
    {
        bool isMoving = true;
        Vector3 targetPoint = new Vector3(points[0].point.position.x, transform.position.y, points[0].point.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.LookAt(targetPoint);

        if (Vector3.Distance(transform.position, targetPoint) < 0.25f)
        {
            isMoving = false;
            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                StartNextPoint();
            }
        }

        anim.SetBool("IsMovin'", isMoving);
    }

    public void StartNextPoint()
    {
        if (points.Count > 0)
        {
            points.RemoveAt(0);
            if (points.Count > 0)
            {
                waitTime = points[0].waitTime; // Reset wait time for the next point
            }
        }
    }
}
[System.Serializable]
public class NavPoint
{
    public Transform point;
    public float waitTime;
}
