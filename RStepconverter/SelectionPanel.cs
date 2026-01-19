using System;
using System.Linq;
using Rhino;
using Rhino.DocObjects;
using Rhino.UI;
using Eto.Forms;
using Eto.Drawing;

namespace MyRhinoPlugin
{
    // Inherit from Panel so this can be placed inside a Form or Docked Tab
    public class SelectionPanel : Panel
    {
        // UI Controls
        private ListBox _idList;
        private Button _exportButton;
        private Label _statusLabel;

        public SelectionPanel()
        {
            // 1. Initialize Controls
            _idList = new ListBox { Height = 100 };
            _exportButton = new Button { Text = "Export STEP", Enabled = false };
            _statusLabel = new Label { Text = "Select an object..." };

            // 2. Define Layout (Rows and Columns)
            var layout = new DynamicLayout { DefaultSpacing = new Size(5, 5), Padding = new Padding(10) };

            layout.AddRow(_statusLabel);
            layout.AddRow(_idList); // We will hide this if multiple objects are selected
            layout.AddRow(null);
            layout.AddRow(_exportButton);

            Content = layout;

            // 3. Bind Events (Like callback functions in C++)
            _exportButton.Click += OnExportClicked;

            // 4. Hook into Rhino's global selection events for "Live" updates
            RhinoDoc.SelectObjects += (sender, e) => UpdateUI();
            RhinoDoc.DeselectObjects += (sender, e) => UpdateUI();

            // Initial update in case things are already selected
            UpdateUI();
        }

        // --- Logic: Update the ListBox ---
        private void UpdateUI()
        {
            _idList.Items.Clear();
            var doc = RhinoDoc.ActiveDoc;
            var selectedObjects = doc.Objects.GetSelectedObjects(false, false).ToList();
            int count = selectedObjects.Count;

            // 1. Logic: Handle Multiple Selections
            if (count == 0)
            {
                _statusLabel.Text = "No selection.";
                _exportButton.Enabled = false;
                _idList.Visible = false;
            }
            else if (count == 1)
            {
                // Show the specific ID for a single object
                _statusLabel.Text = "Single Object Selected:";
                _idList.Visible = true;
                _idList.Items.Add(selectedObjects[0].Id.ToString());
                _exportButton.Enabled = true;
            }
            else
            {
                // Hide IDs for multiple objects, just show count
                _statusLabel.Text = $"Multiple Objects Selected ({count})";
                _idList.Visible = false;
                _exportButton.Enabled = true;
            }
        }

        // --- Logic: Export to STEP ---
        private void OnExportClicked(object sender, EventArgs e)
        {
            var doc = RhinoDoc.ActiveDoc;

            // Double check we have a selection
            if (doc.Objects.GetSelectedObjects(false, false).Count() == 0) return;

            // Save Dialog
            var saveDialog = new Eto.Forms.SaveFileDialog();
            saveDialog.Filters.Add(new FileFilter("STEP File", ".stp", ".step"));

            if (saveDialog.ShowDialog(this) == DialogResult.Ok)
            {
                string filePath = saveDialog.FileName;
                string script = $"-_Export \"{filePath}\" _Enter _Enter";

                // Run script
                RhinoApp.RunScript(script, false);

                MessageBox.Show($"Exported {filePath}");
            }
        }
    }
}