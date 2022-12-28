using System.Diagnostics;
using System.Net.Http.Json;

namespace StuuupidGame
{
    public partial class Form1 : Form
    {
        public int size;
        public Form1()
        {
            Process process = new();
            ProcessStartInfo startInfo = new()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                FileName = "cmd.exe",
                Arguments = "/C python D:\\vsrepo\\StuuupidGame\\engine-apified.pyw"
            };
            process.StartInfo = startInfo;
            process.Start();
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