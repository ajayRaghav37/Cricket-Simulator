using System;
using System.Windows.Forms;
using Repositories;
using Repositories.TableEntities;
using static System.Convert;

namespace DatabaseHarness
{
    public partial class InsertSquads : Form
    {
        int queueLength = 0;
        delegate void SetTitleCallback(int deltaQueue);

        public InsertSquads()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Squads squadItem = new Squads
            {
                TeamId = ToInt32(txtTeamId.Text),
                PlayerId = ToInt32(txtPlayerId.Text)
            };

            AddToQueue(1);

            SquadsRepository.InsertSingle(squadItem).ContinueWith((a) => AddToQueue(-1));

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

        private void txtTeamId_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
