using UnityEngine;
using System.Collections;


public class PlayerBattleData : MonoBehaviour
{
    [SerializeField] private GameObject _hitbox;
    [SerializeField] private float _atackDelay, _knockbackForce;
    public int AtackForce { get; private set; }

    public Vector3 KnockbackForce
    {
        get
        {
            return Vector3.right * _knockbackForce * Mathf.Sign(this.GetComponent<BoxRunner>().Direction);
        }
    }
    private bool _combo = false, _isAttacking, _canSetCombo;
    [SerializeField] private Animator _animator;
    private AttackAnimationController _controller;

    private void Awake()
    {
        _controller = GetComponent<AttackAnimationController>();

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)){
            if (_isAttacking == false){
                _animator.SetTrigger("atack");
                _isAttacking = true;
            }
            else if(_canSetCombo == true){
                _animator.SetBool("combo", true);
                _canSetCombo = false;
            }

            Debug.Log($"is attacking - {_isAttacking}, canGetCombo - {_canSetCombo}");
        }
       
    }
    public void ResetAttack() => _isAttacking = false;
    public void ResetCombo() => _animator.SetBool("combo", false);
    public void StartAcceptCombo() => _canSetCombo = true;
    public void StopAcceptCombo() => _canSetCombo = false;
    
    /*
    private IEnumerator ComboHandler()
    {
        if(_animator.GetBool("combo") == false)
            _animator.SetBool("combo", _combo);
        yield return new WaitForSeconds(_atackDelay);
        _animator.SetBool("combo", _combo);
    }
    */
    public void HitEnded() => _hitbox.SetActive(false);

    public void GetDamage(int damage) => print("damage to player: " + damage);

}