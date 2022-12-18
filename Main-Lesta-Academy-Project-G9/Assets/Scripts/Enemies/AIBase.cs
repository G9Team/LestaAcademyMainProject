using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public abstract class AIBase : MonoBehaviour
{
    public float health = 100f;

    [HideInInspector] public float detectDistance = 10f;
    [HideInInspector] public bool canDetectThroughWalls = true;
    [HideInInspector] public PlayerController _player;
    

    public abstract string GetEnemyName();
    public abstract ENEMY_STATE GetState();
    public abstract void ForceState(ENEMY_STATE state);

    
#if UNITY_EDITOR
    private void OnGUI()
    {
        Vector2 guiPoint = HandleUtility.WorldToGUIPoint(transform.position);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        style.alignment = TextAnchor.UpperCenter;
        GUI.Label(new Rect(guiPoint.x - 50, guiPoint.y - 60, 100f, 30f), GetEnemyName()+"\n"+GetState().ToString(), style);
    }
#endif

    public void InitAI()
    {
        _player = FindObjectOfType<PlayerController>();
    }

    public void UpdateAI()
    {
        if(Vector3.Distance(_player.transform.position, this.transform.position) <= detectDistance && GetState() != ENEMY_STATE.ATTACK)
        {
            if (canDetectThroughWalls)
                ForceState(ENEMY_STATE.ATTACK);
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, (_player.transform.position - this.transform.position), out hit))
                {
                    Debug.Log(hit.transform.name);
                    if (hit.transform.GetComponent<PlayerController>())
                        ForceState(ENEMY_STATE.ATTACK);
                }
            }
        }
        else if(Vector3.Distance(_player.transform.position, this.transform.position) > detectDistance && GetState() == ENEMY_STATE.ATTACK)
        {
            ForceState(ENEMY_STATE.IDLE);
        }
    }

    public enum ENEMY_STATE
    {
        INACTIVE = 0,
        IDLE = 1,
        PATROL = 2,
        ATTACK = 3
    }
}
