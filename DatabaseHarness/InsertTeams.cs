using System;
using System.Windows.Forms;
using Repositories;
using Repositories.TableEntities;
using static System.Convert;

namespace DatabaseHarness
{
    public partial class InsertTeams : Form
    {
        int queueLength = 0;
        delegate void SetTitleCallback(int deltaQueue);

        public InsertTeams()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Teams team = new Teams
            {
                TeamId = ToInt32(txtTeamId.Text),
                TeamName = txtTeamName.Text,
                ShortName = txtShortName.Text,
                Captain = ToInt32(txtCaptain.Text),
                ViceCaptain = ToInt32(txtViceCaptain.Text),
                LogoUri = new Uri(txtLogoUri.Text),
                FlagUri = new Uri(txtFlagUri.Text),
                Color1 = btnColor1.BackColor,
                Color2 = btnColor2.BackColor,
                IsCounty = ToBoolean(txtIsCounty.Text)
            };

            AddToQueue(1);

            TeamsRepository.InsertSingle(team).ContinueWith((a) => AddToQueue(-1));

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

        private void btnColor1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btnColor1.BackColor = colorDialog1.Color;
        }

        private void btnColor2_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                btnColor2.BackColor = colorDialog1.Color;
        }

        private void txtTeamId_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}
