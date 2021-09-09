using System;
using System.Windows.Forms;
using Repositories;
using Repositories.TableEntities;
using static System.Convert;

namespace DatabaseHarness
{
    public partial class InsertPlayers : Form
    {
        int queueLength = 0;
        delegate void SetTitleCallback(int deltaQueue);

        public InsertPlayers()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            int runs = ToInt32(txtRuns.Text);
            int fours = ToInt32(txtFours.Text);
            int sixes = ToInt32(txtSixes.Text);

            Players player = new Players
            {
                PlayerName = txtPlayerName.Text,
                NickName = txtNickName.Text,
                ShortName = txtShortName.Text,
                CallingName = txtCallingName.Text,
                BattingAverage = ToDouble(txtBattingAverage.Text),
                BowlingAverage = ToDouble(txtBowlingAverage.Text),
                Role = (byte)((byte)checkBox1.CheckState + 4 * (byte)checkBox2.CheckState + 16 * (byte)checkBox3.CheckState),
                BattingStrikeRate = ToDouble(txtBattingStrikeRate.Text),
                BowlingEconomy = ToDouble(txtBowlingEconomy.Text),
                PlayerId = ToInt32(txtPlayerId.Text),
                PercentRunsInFour = (double)fours * 400 / runs,
                PercentRunsInSix = (double)sixes * 600 / runs,
            };

            AddToQueue(1);

            PlayerRepository.InsertSingle(player).ContinueWith((a) => AddToQueue(-1));

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

        private void txtPlayerId_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }
    }
}