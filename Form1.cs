using System.Diagnostics;
using System.Net.Http.Json;

namespace StuuupidGame
{
    public partial class Form1 : Form
    {
        public int size;
        public Form1()
        {
            InitializeComponent();
            var PopQuestActive = new PopQuest();
            PopQuestActive.ShowDialog();
            size = PopQuestActive.size;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}