using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayerLight : MonoBehaviour
{
    [SerializeField] private GameObject[] _spotlights;

    private BoxRunner _runnerForDir;
    private GameObject _player;
    private int _costil = 1;

    private void Awake() {
       // _runnerForDir = GetComponent<BoxRunner>();
        _player = GameObject.FindGameObjectWithTag("Player");
       // _runnerForDir.OnDirectionChange += OnDirectionChange;
    }

    private void Update() {
        this.transform.position = _player.transform.position;
    }

    private void OnDirectionChange(){
        if (_costil >0){
            _costil--;
            return;
        }

        foreach (var spotlight in _spotlights)
        {
            Vector3 basePosition = spotlight.transform.localPosition;
            basePosition.x *= -1;
            spotlight.transform.localPosition = basePosition;

            Vector3 baseQuaternion = spotlight.transform.localEulerAngles;
            baseQuaternion.y *= -1;
            spotlight.transform.localEulerAngles = baseQuaternion;
        }
    }
}
