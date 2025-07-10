using QR_Tracker.Model.Service;
using QR_Tracker.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QR_Tracker.ViewModel
{
    public class QrDetectViewModel : BaseViewModel
    {
        public LocalizationManager Loc => LocalizationManager.Instance;

    }
}
