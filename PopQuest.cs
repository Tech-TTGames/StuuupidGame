namespace StuuupidGame
{
    public partial class PopQuest : Form
    {
        public int size;
        public int speed;
        public PopQuest()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            size = (int)numericUpDown1.Value;
            speed = (int)numericUpDown2.Value;
            Close();
        }
    }
}