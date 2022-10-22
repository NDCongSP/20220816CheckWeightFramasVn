using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class RefreshEvent
    {
        private bool _refreshStatus = false;

        public bool RefreshStatus
        {
            get => _refreshStatus;
            set
            {
                _refreshStatus = value;
                if (value)
                {
                    OnRefreshAction();
                }
            }
        }

        private event EventHandler _refreshActionevent;
        public event EventHandler RefreshActionevent
        {
            add
            {
                _refreshActionevent += value;
            }
            remove
            {
                _refreshActionevent -= value;
            }
        }

        void OnRefreshAction()
        {
            _refreshActionevent?.Invoke(this, new EventArgs());
        }


        #region Cap nhat giá trị count metal
        private int _countValue = 0;
        public int CountValue
        {
            get => _countValue;
            set
            {
                if (value != _countValue)
                {
                    _countValue = value;
                    OnCountValueAction(value);
                }
            }
        }

        private event EventHandler<CountValueChangedEventArgs> _eventHandlerCount;
        public event EventHandler<CountValueChangedEventArgs> EventHandlerCount
        {
            add
            {
                _eventHandlerCount += value;
            }
            remove
            {
                _eventHandlerCount -= value;
            }
        }

        void OnCountValueAction(int value)
        {
            _eventHandlerCount?.Invoke(this, new CountValueChangedEventArgs(value));
        }
        #endregion
    }

    public class CountValueChangedEventArgs : EventArgs
    {
        private int _countValue = 0;
        public int CountValue { get => _countValue; set => _countValue = value; }

        public CountValueChangedEventArgs(int value)
        {
            _countValue = value;
        }
    }
}
