using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager instance;

    [SerializeField] private List<Customer> customersToSpawn = new List<Customer>();

    [SerializeField] private float timeBetweenSpawns;
    private float spawnCounter;

    public List<NavPoint> navPoints = new List<NavPoint>();
    public List<NavPoint> entryPointsLeft, entryPointsRight;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if(spawnCounter <= 0)
        {
            SpawnCustomer();
        }
    }

    public void SpawnCustomer()
    {
        Instantiate(customersToSpawn[Random.Range(0, customersToSpawn.Count)]);
        spawnCounter = timeBetweenSpawns*Random.Range(.75f, 1.25f);
    }

    public List<NavPoint> GetEntryPoints()
    {
        List<NavPoint> points = new List<NavPoint>();

        if(Random.value < 0.5f) 
        {
            points.AddRange(entryPointsLeft);
        }
        else
        {
            points.AddRange(entryPointsRight);
        }

        return points;
    }

    public List<NavPoint> GetExitPoints()
    {
        List<NavPoint> points = new List<NavPoint>();

        List<NavPoint> temp = new List<NavPoint>();


        if (Random.value < 0.5f)
        {
            temp.AddRange(entryPointsLeft);
        }
        else
        {
            temp.AddRange(entryPointsRight);
        }

        for (int i = temp.Count - 1; i >= 0; i--)
        {
                points.Add(temp[i]);
        }

        return points;
    }
}
