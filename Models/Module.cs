using Quick.Models.Enum;
using Caliburn.Micro;

namespace Quick.Models
{
    public class Module : PropertyChangedBase
    {
        private ModuleStatus _status = ModuleStatus.waiting;
        public ModuleStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    NotifyOfPropertyChange(() => Status); 
                }
            }
        }
    }
}
