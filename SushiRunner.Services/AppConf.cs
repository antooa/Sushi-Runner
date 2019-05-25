using SushiRunner.Services.Interfaces;

namespace SushiRunner.Services
{
    public class AppConf : IAppConf
    {
        public AppConf(string webRootPath)
        {
            WebRootPath = webRootPath;
        }

        public string WebRootPath { get; set; }
    }
}