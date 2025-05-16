using Caliburn.Micro;
using System.Windows;

namespace Quick.Models
{
    public class CavityUnit : PropertyChangedBase
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Carrier ParentCarrier { get; set; }

        private bool _isProcessed = false;
        public bool IsProcessed
        {
            get => _isProcessed;
            set
            {
                _isProcessed = value;
                NotifyOfPropertyChange(() => IsProcessed);
                ParentCarrier?.UpdateSn();
            }


        }


        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                _isEnabled = value;
                NotifyOfPropertyChange(() => IsEnabled);
            }
        }

        private Rect _clipRect = new Rect(0, 0, 30, 20);
        public Rect ClipRect
        {
            get => _clipRect;
            set
            {
                _clipRect = value;
                NotifyOfPropertyChange(() => ClipRect);
            }
        }

        private double _cavLeft;
        public double CavLeft
        {
            get => _cavLeft;
            set
            {
                _cavLeft = value;
                NotifyOfPropertyChange(() => CavLeft);
            }
        }

        private double _cavTop;
        public double CavTop
        {
            get => _cavTop;
            set
            {
                _cavTop = value;
                NotifyOfPropertyChange(() => CavTop);
            }
        }
    }
}
