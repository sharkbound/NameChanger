using Rocket.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameChanger
{
    public class NameChanger : RocketPlugin
    {
        protected override void Load()
        {
            Log("NameChanger has loaded!");
        }

        protected override void Unload()
        {
            Log("NameChanger has unloaded!");
        }

        public static void Log(string message)
        {
            Rocket.Core.Logging.Logger.Log(message);
        }
    }
}
