using UnityEngine;

public class LoiFireflyMovement : MonoBehaviour
{
    [SerializeField] private float _maxDistance, _minDistance,  _rotationScaler;
    private GameObject _player;
    private Transform _innerFly;
    private float _playerMaxSpeed = 10;

    private void Awake() {
        _player = GameObject.FindGameObjectWithTag("Player");
        _innerFly = transform.GetChild(0);
    }

    void Update()
    {
        MoveMainBody();
        _innerFly.RotateAround(this.transform.position, Vector3.forward, 150 * Time.deltaTime * _rotationScaler);
    }

    private void MoveMainBody()
    {
        float distanceHor = _player.transform.position.x - this.transform.position.x;
        float distanceVer = _player.transform.position.y + 1f - this.transform.position.y;
        float horSpeedBoost = distanceHor / _maxDistance;
        float vertSpeedBoost = distanceVer / _maxDistance;
        if (Mathf.Abs(distanceHor) < _minDistance) horSpeedBoost = 0;
        if (Mathf.Abs(distanceVer) < _minDistance) vertSpeedBoost = 0;

        this.transform.Translate(
            new Vector3(
                _playerMaxSpeed * horSpeedBoost,
                _playerMaxSpeed * vertSpeedBoost,
                0
            )
            * Time.deltaTime);

    }
}
