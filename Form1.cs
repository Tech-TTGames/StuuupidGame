using System.Diagnostics;
using System.Net.Http.Json;

namespace StuuupidGame
{
    public partial class Form1 : Form
    {
        public int size;
        private static readonly HttpClient client = new();
        public static string address = "http://127.0.0.1:5000";
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
            var data = Request(string.Join('/', "boardgen", size.ToString()));

        }
        private static Dictionary<string, dynamic> Request(string endpoint, string? JsonData = null)
        {
            Dictionary<string, dynamic>? result;
            if (JsonData == null)
            {
                var resultTask = client.GetFromJsonAsync<Dictionary<string, dynamic>>(string.Join('/', address, endpoint));
                resultTask.Wait();
                result = resultTask.Result;
            }
            else
            {
                var responseTask = client.PostAsJsonAsync(endpoint, JsonData);
                responseTask.Wait();
                var response = responseTask.Result;
                var resolveResponseTask = response.Content.ReadFromJsonAsync<Dictionary<string, dynamic>>();
                resolveResponseTask.Wait();
                result = resolveResponseTask.Result;
            }
            if (result == null)
            {
                throw new Exception("Request returned null.");
            }
            return result;

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}