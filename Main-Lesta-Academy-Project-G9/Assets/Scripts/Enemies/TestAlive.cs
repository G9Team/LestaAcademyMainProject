using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAlive : MonoBehaviour, IAlive
{
    public void ChangeHealth(int health)
    {
    }

    public int GetHealth()
    {
        return 100;
    }

    public void SetHealth(int health)
    {
    }
}
