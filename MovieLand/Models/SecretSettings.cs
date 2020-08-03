using System;
namespace Hydra
{
    public interface ISecretSettings 
    {
        string MapCredantials { get; set; }

        string DbPassword { get; set; }

        string WeatherKey { get; set; }
    }

    public class SecretSettings : ISecretSettings
    {
        public SecretSettings(string mapCredantials, string dbPassword, string weatherKey)
        {
            DbPassword = dbPassword;
            MapCredantials = mapCredantials;
            WeatherKey = weatherKey;
        }

        public string MapCredantials { get; set; }

        public string DbPassword { get; set; }

        public string WeatherKey { get; set; }
    }
}
