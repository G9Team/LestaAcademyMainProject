using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using New;

public class UITest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private PlayerComponentManager _player;

    private void Update() {
        _text.text = _player.GetPlayerData().GetCollectableCount(CollectableType.Wheet).ToString();
    }
}
