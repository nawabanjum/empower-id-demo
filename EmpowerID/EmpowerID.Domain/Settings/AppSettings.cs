namespace EmpowerID.Domain.Settings
{
    public class AppSettings
    {
        public string LogFolder { get; set; } = string.Empty;
        public string LogFilePath { get; set; } = string.Empty;
        public int ServiceSleep { get; set; }
    }
}
