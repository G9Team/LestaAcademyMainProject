using System;

namespace New
{

    public interface IInteractable
    {
        public bool IsUpgrade();
        public IUpgrade GetUpgrade();
    }

}