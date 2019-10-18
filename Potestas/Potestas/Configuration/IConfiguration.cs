namespace Potestas.Configuration
{
    public interface IConfiguration
    {
        string GetValue(string key);
    }
}