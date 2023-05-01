using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    #region DEFINES

    public struct SaveObject
    {
        public int maxHealth;
        public int attackForce;
        public int maxEnergy;
        public int sceneId;
    }

    #endregion

    SaveObject _save;
    string _path = "save.json";
    static SaveManager _instance;



    #region PRIVATE
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _path = Path.Combine(Application.persistentDataPath, _path);
        LoadData();
    }

    private void OnLevelWasLoaded(int level)
    {
        New.PlayerComponentManager manager = FindObjectOfType<New.PlayerComponentManager>();
        if(manager != null)
        {
            ((New.PlayerData)manager.GetPlayerData()).LoadSave(_save);
        }
        SaveData();
    }

    void LoadData()
    {
        if (!File.Exists(_path))
            CreateNew();
        else
            _save = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveObject>(File.ReadAllText(_path));

        UnityEngine.SceneManagement.SceneManager.LoadScene(_save.sceneId);
    }

    void SaveData()
    {
        _save.sceneId = Mathf.Max(1, UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        File.WriteAllText(_path, Newtonsoft.Json.JsonConvert.SerializeObject(_save));
    }

    void CreateNew()
    {
        _save = new SaveObject()
        {
            maxHealth = 3,
            attackForce = 1,
            maxEnergy = 3,
            sceneId = 1
        };
        SaveData();
    }

    private void OnEnable()
    {
        _instance = this;
    }

    private void OnDisable()
    {
        _instance = null;
    }

    #endregion

    #region PUBLIC

    public static SaveObject GetData()
    {
        if (_instance == null)
            return default;
        return _instance._save;
    }

    public static int GetValue(New.UpgradeType type)
    {
        if (_instance == null)
            return 0;
        switch (type)
        {
            case New.UpgradeType.MaxHealth:
                return _instance._save.maxHealth;
            case New.UpgradeType.AttackForce:
                return _instance._save.attackForce;
            case New.UpgradeType.MaxEnergy:
                return _instance._save.maxEnergy;
        }
        return 0;
    }

    public static void AddValue(New.UpgradeType type, int value)
    {
        if (_instance == null)
            return;
        switch (type)
        {
            case New.UpgradeType.MaxHealth:
                _instance._save.maxHealth += value;
                break;
            case New.UpgradeType.AttackForce:
                _instance._save.attackForce += value;
                break;
            case New.UpgradeType.MaxEnergy:
                _instance._save.maxEnergy += value;
                break;
        }
    }

    public static void SetValue(New.UpgradeType type, int value)
    {
        if (_instance == null)
            return;
        switch (type)
        {
            case New.UpgradeType.MaxHealth:
                _instance._save.maxHealth = value;
                break;
            case New.UpgradeType.AttackForce:
                _instance._save.attackForce = value;
                break;
            case New.UpgradeType.MaxEnergy:
                _instance._save.maxEnergy = value;
                break;
        }
    }

    #endregion
}
