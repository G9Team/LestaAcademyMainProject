using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic interface for player,enimies,companions and etc.
public interface IAlive
{
    public int GetHealth();
    public void SetHealth(int health);
    public void ChangeHealth(int health); //damage must be negative
}
