namespace DependencyInjection.Data
{
    public class Demo
    {
        public DateTime StartupTime { get; init; }

        public Demo()
        {
            StartupTime = DateTime.UtcNow;
        }
    }
}
