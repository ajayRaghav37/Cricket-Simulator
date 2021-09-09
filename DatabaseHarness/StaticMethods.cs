using System.Windows.Forms;

namespace DatabaseHarness
{
    public static class StaticMethods
    {
        public static void ResetInputControls(Control ctrl)
        {
            foreach (Control c in ctrl?.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Text = string.Empty;

                if (c is CheckBox)
                    ((CheckBox)c).CheckState = CheckState.Unchecked;

                if (c.Controls.Count > 0)
                    ResetInputControls(c);
            }
        }
    }
}
