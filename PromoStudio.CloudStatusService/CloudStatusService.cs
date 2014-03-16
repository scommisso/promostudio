using System;
using System.ServiceProcess;
using System.Timers;
using Autofac;
using Nito.AsyncEx;

namespace PromoStudio.CloudStatusService
{
    public partial class CloudStatusService : ServiceBase
    {
        private bool _initialized;
        private Timer _timer;
        private bool _working;

        public CloudStatusService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(1000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DoWork();
        }

        public void DoWork()
        {
            // TODO: Log
            if (_working)
            {
                return;
            } // still busy
            _working = true;

            if (!_initialized)
            {
                IocConfig.RegisterIoc();
                _initialized = true;
            }

            using (ILifetimeScope container = IocConfig.Container.BeginLifetimeScope())
            {
                try
                {
                    var processor = container.Resolve<ICloudStatusProcessor>();
                    AsyncContext.Run(() => { processor.Execute(); });
                }
                catch (Exception ex)
                {
                    // TODO: Log
                }
                finally
                {
                    _working = false;
                }
            }
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }
    }
}