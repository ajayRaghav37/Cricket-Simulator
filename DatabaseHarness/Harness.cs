using System;
using System.Windows.Forms;

namespace DatabaseHarness
{
    public partial class Harness : Form
    {
        InsertTeams insertTeams = new InsertTeams();
        InsertPlayers insertPlayers = new InsertPlayers();
        InsertRoles insertRoles = new InsertRoles();
        InsertSquads insertSquads = new InsertSquads();

        public Harness()
        {
            InitializeComponent();
        }

        private void Harness_Load(object sender, EventArgs e)
        {
            insertTeams.MdiParent = this;
            insertTeams.Show();
            insertPlayers.MdiParent = this;
            insertPlayers.Show();
            insertRoles.MdiParent = this;
            insertRoles.Show();
            insertSquads.MdiParent = this;
            insertSquads.Show();
        }
    }
}
