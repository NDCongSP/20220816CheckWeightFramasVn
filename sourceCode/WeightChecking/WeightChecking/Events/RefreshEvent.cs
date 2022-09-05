using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeightChecking
{
    public class RefreshEvent
    {
        private bool refreshStatus = false;

        public bool RefreshStatus
        {
            get => refreshStatus;
            set
            {
                refreshStatus = value;
                if (value)
                {
                    OnRefreshAction();
                }
            }
        }

        private event EventHandler refreshActionevent;
        public event EventHandler RefreshActionevent
        {
            add
            {
                refreshActionevent += value;
            }
            remove
            {
                refreshActionevent -= value;
            }
        }

        void OnRefreshAction()
        {
            refreshActionevent?.Invoke(this, new EventArgs());
        }
    }
}
