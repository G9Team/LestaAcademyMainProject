using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace New
{

    public class IngameUI : MonoBehaviour
    {
        private const int INDEX_REDUSE = 3,  MAX_COINS =10; 
        private int _maxWheet, _maxPirojki;
        [SerializeField]
        private GameObject[] _allVoidHearts, _allFullHearts, _allVoidEnergy,
            _allFullEnergy, _healthFrames, _energyFrames;

        [SerializeField]
        private GameObject _coins, _pirojki, _wheet;

        [SerializeField] 
        private TextMeshProUGUI _coinsText, _progkiText, _wheetText;

        private int _previousHP = 100, _previousMaxHP = 100, _previousEnergy = 100, 
            _previousMaxEnergy = 100, _previousCoins = 100, _previousPirogki = 100, 
            _previousWheet = 100;

        private IPlayerData _playerData;

        private BossHealth _bossHealth;
        private void Start()
        {
            _playerData = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponentManager>().GetPlayerData();
            _playerData.OnUIUpdate += UpdateUI;
            _bossHealth = FindObjectOfType<BossHealth>();
            if (_bossHealth is not null){
                _bossHealth.Damaged += UpdateBossHP;
                _bossHealth.Death += BossDeath;
            }
            CollectableObject[] c = FindObjectsOfType<CollectableObject>();
            foreach (var item in c)
            {
                if(item.Type == UpgradeType.Pirojki) _maxPirojki++;
                if(item.Type == UpgradeType.Wheet) _maxWheet++;
            }
            print(_maxPirojki);
            _maxPirojki += GetPreviousValue(UpgradeType.Pirojki);
            _maxWheet += GetPreviousValue(UpgradeType.Wheet);
            UpdateUI();
        }

        private int GetPreviousValue(UpgradeType type){
            if(SceneManager.GetActiveScene().buildIndex < 5) return 0;
            if(type == UpgradeType.Wheet){
                if(SceneManager.GetActiveScene().buildIndex == 5){
                    return 6;
                }
                else{
                    return 10;
                }
            }
            else{
                if (SceneManager.GetActiveScene().buildIndex == 5){
                    return 0;
                }
                else if(SceneManager.GetActiveScene().buildIndex == 6){
                    return 4;
                }
                else if(SceneManager.GetActiveScene().buildIndex == 7){
                    return 4;
                }
                else return 10;

            }
        }

        private void BossDeath(){

        }

        private void UpdateBossHP(){

        }

        void UpdateUI()
        {
            if(_previousHP != _playerData.CurrentHealth){
                _previousHP = _playerData.CurrentHealth;
                RefreshCurrentValues(ref _allFullHearts, _previousHP);
            }
            if (_previousMaxHP != _playerData.MaxHealth){
                _previousMaxHP = _playerData.MaxHealth;
                RefreshCurrentValues(ref _allVoidHearts, _previousMaxHP);  
                print(_previousMaxHP - INDEX_REDUSE);
                ChangeFrame(ref _healthFrames, _previousMaxHP > 0 ? _previousMaxHP - INDEX_REDUSE : 0) ;
            }
            if (_previousEnergy != _playerData.CurrentEnergy){
                _previousEnergy = _playerData.CurrentEnergy;
                RefreshCurrentValues(ref _allFullEnergy, _previousEnergy);   
            }
            if (_previousMaxEnergy != _playerData.MaxEnergy){
                _previousMaxEnergy = _playerData.MaxEnergy;
                RefreshCurrentValues(ref _allVoidEnergy, _previousMaxEnergy); 
                ChangeFrame(ref _energyFrames, _previousMaxEnergy > 0 ? _previousMaxEnergy - INDEX_REDUSE : 0) ;
            }
            if (_previousCoins != _playerData.Coins){
                _previousCoins = _playerData.Coins;
                _coinsText.text =  _previousCoins + "/" + MAX_COINS;  
            }
            if (_previousWheet != _playerData.Wheet){
                _previousWheet = _playerData.Wheet;
                _wheetText.text =  _previousWheet + "/" + _maxWheet;  
            }
            if (_previousPirogki != _playerData.Pirojki){
                _previousPirogki = _playerData.Pirojki;
                _progkiText.text =  _previousPirogki + "/" + _maxPirojki;  
            }
            print("UI Updated");
        }


        private void RefreshCurrentValues(ref GameObject[] objectsToRefresh, int index){
            for (int i = 0; i < objectsToRefresh.Length; i++)
            {
                bool isActive = false;
                if(i < index){
                    isActive = true;
                }
                objectsToRefresh[i].SetActive(isActive);
            }
        }
        private void ChangeFrame(ref GameObject[] allFrames, int index){
            RefreshCurrentValues(ref allFrames, 0);
            allFrames[index].SetActive(true);
        }


    }
}
