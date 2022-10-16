using System;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace YourDeath
{
    public partial class YourForm : Form
    {
        public YourForm()
        {
            InitializeComponent();
            Death_Timer.Text = TimeSpan.FromMinutes(4).ToString();
        }

        private void RulesButton_Click(object sender, EventArgs e)
        {
            var RulesForm = new Rules();
            RulesForm.ShowDialog();
        }

        private void YourForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            /* Anti-close / Anti-kill */
            e.Cancel = true;
        }

        private void YourForm_Load(object sender, EventArgs e)
        {
            // Time
            ClockTimer();

            // Play the music
            Stream someSTR = Properties.Resources.some_music;
            SoundPlayer someSND = new SoundPlayer(someSTR);
            someSND.Play();
        }

        public void ClockTimer()
        {
            var startTime = DateTime.Now;
            var timer = new Timer() { Interval = 1000 };
            timer.Tick += (obj, args) =>
            Death_Timer.Text =
            (TimeSpan.FromMinutes(4) - (DateTime.Now - startTime))
            .ToString("mm\\:ss");
            timer.Enabled = true;
        }
    }
}
