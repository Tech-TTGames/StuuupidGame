
namespace StuuupidGame
{
    public partial class Summary : Form
    {
        public bool restart;
        public Summary(int score, bool win = false)
        {
            InitializeComponent();
            label3.Text = score.ToString();
            if (win)
            {
                label1.Text = "YOU WIN";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            restart = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            restart = false;
            Close();
        }
    }
}
