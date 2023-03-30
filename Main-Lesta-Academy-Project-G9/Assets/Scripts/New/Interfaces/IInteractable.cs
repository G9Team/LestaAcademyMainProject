using System;

namespace New
{

    public interface IInteractable : IUpgrade
    {
        public bool IsUpgrade(){
            if ((int)this.Type < 20){
                return true;
            }
            else return false;
        }
        public ICollectable GetCollectable();
        public IUpgrade GetUpgrade();

    }

    public interface ICollectable : IInteractable
    {
        public CollectableType TypeOfCollectable {get;}
        public void Collect();

    }
}