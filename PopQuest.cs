using System.Text.Json;
using System.Text.Json.Serialization;

namespace StuuupidGame
{
    public partial class PopQuest : Form
    {
        public int size;
        public PopQuest()
        {
            InitializeComponent();

        }

        private void PopQuest_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            size = (int)numericUpDown1.Value;
            this.Close();
        }
    }
}