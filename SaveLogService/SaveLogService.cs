using System.ServiceProcess;

namespace SaveLogService
{
    public partial class SaveLogService : ServiceBase
    {
        public SaveLogService()
        {
            DataService fd = new();
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        
        }

        protected override void OnStop()
        {
        }
    }
}
