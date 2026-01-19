using Rhino;
using Rhino.Commands;

namespace MyRhinoPlugin
{
    public class MyCommand : Command
    {
        public MyCommand()
        {
            // Rhino calls this constructor once on startup
            Instance = this;
        }

        public static MyCommand Instance { get; private set; }

        // Command for rhino
        public override string EnglishName => "MyTool";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // Create a floating window (Eto Form) to hold our Panel
            var form = new Eto.Forms.Form
            {
                Title = "My Export Tool",
                ClientSize = new Eto.Drawing.Size(300, 350),
                Topmost = true, // Keep it on top of Rhino
                Padding = new Eto.Drawing.Padding(5),
                Resizable = true
            };

            // Load the logic we wrote in Step 1
            form.Content = new SelectionPanel();

            // Show the window
            form.Show();

            return Result.Success;
        }
    }
}