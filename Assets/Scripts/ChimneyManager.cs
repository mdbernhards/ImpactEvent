using UnityEngine;
using System.Collections.Generic;

public class ChimneyManager : MonoBehaviour
{
    public List<ChimneyControl> allHouses;
    public float minDuration = 45f;
    public float maxDuration = 75f;

    private float timer;
    private bool isSmoking;

    void Start()
    {
        // Start by turning everything off
        foreach (var house in allHouses) house.SetSmoke(false);
        timer = Random.Range(minDuration, maxDuration);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isSmoking = !isSmoking;
            UpdateHouses();
            timer = Random.Range(minDuration, maxDuration);
        }
    }

    void UpdateHouses()
    {
        // Shuffle the list and pick a random amount to activate
        int activeCount = isSmoking ? Random.Range(2, 5) : 0;

        // Simple way: Pick random houses each time
        foreach (var house in allHouses) house.SetSmoke(false);

        for (int i = 0; i < activeCount; i++)
        {
            int randomIndex = Random.Range(0, allHouses.Count);
            allHouses[randomIndex].SetSmoke(isSmoking);
        }
    }
}