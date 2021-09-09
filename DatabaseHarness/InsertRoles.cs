using System;
using System.Windows.Forms;
using Repositories;
using Repositories.TableEntities;

namespace DatabaseHarness
{
    public partial class InsertRoles : Form
    {
        int queueLength = 0;
        delegate void SetTitleCallback(int deltaQueue);

        public InsertRoles()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Roles role = new Roles
            {
                RoleId = Convert.ToInt32(txtRoleId.Text),
                RoleDesc = txtRoleDesc.Text
            };

            AddToQueue(1);

            RolesRepository.InsertSingle(role).ContinueWith((a) => AddToQueue(-1));

            StaticMethods.ResetInputControls(this);

            button1.Enabled = false;
        }

        private void AddToQueue(int deltaQueue)
        {
            if (button1.InvokeRequired)
            {
                SetTitleCallback d = new SetTitleCallback(AddToQueue);
                Invoke(d, new object[] { deltaQueue });
            }
            else
            {
                queueLength += deltaQueue;
                if (queueLength > 0)
                    Text = Name + " (Pending Inserts: " + queueLength + ")";
                else
                    Text = Name;
            }
        }
        
        private void txtRoleId_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
