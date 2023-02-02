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
    private bool _canAtack = true, _combo = false;
    private Vector3 _hitboxPosition;
    private int _comboIterator = 3;
    [SerializeField] private Animator _animator;
    private string _lastAnimatorStateName;
    private void Awake()
    {
        _hitboxPosition = _hitbox.transform.localPosition;
        GetComponent<BoxRunner>().OnDirectionChange += OnDirectionChange;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_animator.GetBool("combo") && !_animator.GetCurrentAnimatorStateInfo(0).IsName("atac 1"))
            {
                _lastAnimatorStateName = "atac 1";
                _animator.SetTrigger("atack");
                print("E");
                //_hitbox.SetActive(true);
                //StartCoroutine(AtackDelay());
                //_canAtack = false;
            }
            else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("atac 1") || _animator.GetCurrentAnimatorStateInfo(0).IsName("Armature|hit 2")){
                _animator.SetBool("combo", true);
                //_lastAnimatorStateName = _animator.GetCurrentAnimatorClipInfo(0)
            }
            
            /*
            else if (_combo && _comboIterator > 0)
            {
                _combo = false;
                StopCoroutine(AtackDelay());
                _hitbox.SetActive(true);
                StartCoroutine(AtackDelay()); 
                _comboIterator--;
                print (_comboIterator);               
            }
            */
        }
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_lastAnimatorStateName) && _animator.GetBool("combo")){
            _animator.SetBool("combo", false);
        }
    }
    private IEnumerator AtackDelay()
    {
        yield return new WaitForSeconds(0.2f);
        //_hitbox.SetActive(false);
        //_hitbox.transform.localPosition = _hitboxPosition;
        _combo = true;
        yield return new WaitForSeconds(_atackDelay);
        _canAtack = true;
        _combo = false;
        _comboIterator = 3;
        // print("now can atack once more");
    }
    private void OnDirectionChange()
    {
        if (_hitbox.activeSelf)
        {
            _hitbox.transform.localPosition = new Vector3(_hitbox.transform.localPosition.x * -1, _hitboxPosition.y, _hitboxPosition.z);
        }
    }
    public void GetDamage(int damage) => print("damage to player: " + damage);
    private void OnDisable()
    {
        GetComponent<BoxRunner>().OnDirectionChange -= OnDirectionChange;

    }

}