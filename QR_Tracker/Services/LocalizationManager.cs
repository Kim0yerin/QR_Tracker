using QR_Tracker.Properties;
using QR_Tracker.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QR_Tracker.Services
{
    public class LocalizationManager : BaseViewModel
    {
        // 싱글톤 = 이 클래스의 인스턴스를 1개만 생성되는 것을 보장 (메모리 낭비 x)
        public static LocalizationManager Instance { get; } = new LocalizationManager();
        private ResourceManager _resourceManager = StringResources.ResourceManager;

        // 바인딩에서 Loc["QrCreate"]처럼 사용할 수 있도록 인덱서 제공
        public string this[string key] => _resourceManager.GetString(key, CultureInfo.CurrentUICulture);

        // 언어 전환 메소드
        public void ChangeCulture(string culture)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            OnPropertyChanged(string.Empty); // 모든 바인딩 속성 업데이트
        }
    }
}
