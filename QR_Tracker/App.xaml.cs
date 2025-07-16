using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Serilog;

namespace QR_Tracker
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.File("log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
            Log.Information("======= Application Start =======");
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Log.Information("======= Application Exit =======");
            Log.CloseAndFlush();
            base.OnExit(e);
        }
    }
}
