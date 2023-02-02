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

    [HideInInspector] public IAlive _enemy;
    

    public abstract string GetName();
    public abstract ENEMY_STATE GetState();
    public abstract void ForceState(ENEMY_STATE state);

    
#if UNITY_EDITOR
    private void OnGUI()
    {
        Vector2 guiPoint = HandleUtility.WorldToGUIPoint(transform.position);
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.black;
        style.alignment = TextAnchor.UpperCenter;
        GUI.Label(new Rect(guiPoint.x - 50, guiPoint.y - 60, 100f, 30f), GetName()+"\n"+GetState().ToString(), style);
    }
#endif

    public void InitAI()
    {
    }

    public void UpdateAI()
    {
        
    }

    public enum ENEMY_STATE
    {
        INACTIVE = 0,
        IDLE = 1,
        PATROL = 2,
        ATTACK = 3
    }

    public Vector3 GetEnemyPosition()
    {
        return ((MonoBehaviour)_enemy).transform.position;
    }
}
