using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace New
{

    [RequireComponent(typeof(SphereCollider))]
    [RequireComponent(typeof(Rigidbody))]

    public class CollectableObject : MonoBehaviour, ICollectable
    {
        [SerializeField] private UpgradeType _typeOfUpgrade;
        public UpgradeType Type
        {
            get => _typeOfUpgrade;
            private set => _typeOfUpgrade = value;
        }
        public CollectableType TypeOfCollectable { get => (CollectableType)_typeOfUpgrade; }

        [SerializeField] private float _valueOfUpgrade;
        public float UpgradeValue
        {
            get => _valueOfUpgrade;
            private set => _valueOfUpgrade = value;
        }


        private void Awake()
        {
            gameObject.layer = LayerMask.NameToLayer("Interactable");
            GetComponent<Rigidbody>().isKinematic = true;
            SphereCollider collider = GetComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = 1f;
        }
        public void Collect()
        {
            Destroy(this.gameObject);
        }

        public ICollectable GetCollectable()
        {
            return this;
        }

        public IUpgrade GetUpgrade()
        {
            return this;
        }

        //probable UI call logick
    }
}
