using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.PlugIns;

namespace MyRhinoPlugin
{
    public class MyPlugin : PlugIn
    {
        public static MyPlugin Instance { get; private set; }

        public MyPlugin()
        {
            Instance = this;
        }

        // This runs when Rhino loads your plugin
        protected override LoadReturnCode OnLoad(ref string errorMessage)
        {
            return LoadReturnCode.Success;
        }
    }
}
