using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using New;

public class AbyssScript : MonoBehaviour
{
    [SerializeField] private RootCheckBox _checkPoints;
    [SerializeField] private float _xKnockbackForce, _yKnockbackForce;

    private void Awake()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            var child = this.transform.GetChild(i).GetComponent<Killbox>();
            if (child is not null)
            {
                child.OnKillBoxColision += OnPlayerFall;
                continue;
            }
            var spike = this.transform.GetChild(i).GetComponent<Spikes>();
            if (spike is not null)
            {
                spike.OnSpikeDamage += OnSpikeDamaged;
            }
        }
    }
    private void OnPlayerFall(GameObject player)
    {
        IPlayerData data = player.GetComponent<PlayerComponentManager>().GetPlayerData();
        data.ChangeHealth(-1);
        player.GetComponent<PlayerComponentManager>().GetVelocityHandler().Attacked(0,0);
        player.transform.position = _checkPoints.LastCheckpointPosition;

    }

    private void OnSpikeDamaged(GameObject player)
    {
        player.GetComponent<PlayerComponentManager>().DamagePlayer(-1);
    }
}
