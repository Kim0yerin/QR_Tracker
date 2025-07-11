using QR_Tracker.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QR_Tracker.Services;

namespace QR_Tracker.ViewModel
{
    public class QrDetectViewModel : BaseViewModel
    {
        public LocalizationManager Loc => LocalizationManager.Instance;

    }
}
