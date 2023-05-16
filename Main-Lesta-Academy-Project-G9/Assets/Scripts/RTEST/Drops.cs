using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    private GameObject _nucleus;
    private bool _fire = true;
    private float _nucleusSpeed = 4f;
    [SerializeField] private GameObject _nucPrefab;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_fire) {

            _nucleus = Instantiate(_nucPrefab);
            _nucleus.transform.SetPositionAndRotation(transform.position, new Quaternion(0, 0, 0, 0));
            

            _fire = false;
        }

        if (_nucleus != null) {
                _nucleus.transform.Translate(0, _nucleusSpeed * Time.deltaTime * -1, 0);
        }

        if (_nucleus && _nucleus.transform.position.y < 40) {
            StartCoroutine(RemoveNucleus());
            ;
        }
        
    }



    


    private IEnumerator RemoveNucleus()
{
         if (_nucleus) {
            Destroy(_nucleus); //remove Object for memory cleaning  
        }
        

             yield return new WaitForSeconds(1);

    
        _fire = true;
}


}
