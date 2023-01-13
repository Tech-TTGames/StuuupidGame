using System.Diagnostics;
using System.Net.Http.Json;
using System.Reflection;

namespace StuuupidGame
{
    public partial class Form1 : Form
    {
        public const string address = "http://127.0.0.1:23500";
        private static readonly HttpClient client = new();

        public List<SolidBrush> drawingTools = new();
        private readonly Process srv = new();

        public int size;
        public int ppg;
        public GameData gameData = new();
        public string? direction;
        public Form1()
        {
            StartServer();
            InitializeComponent();
            PopQuest PopQuestActive = new();
            PopQuestActive.ShowDialog();
            size = PopQuestActive.size;
            timer1.Interval = 1000 / PopQuestActive.speed;
            Type colorType = typeof(Color);
            // We take only static property to avoid properties like Name, IsSystemColor ...
            PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo propInfo in propInfos)
            {
                if (propInfo.Name == "Green")
                {
                    continue;
                }
                drawingTools.Add(new SolidBrush(Color.FromName(propInfo.Name)));
            }
            var drawingToolsTMP = drawingTools;
            drawingTools = drawingToolsTMP.OrderBy(a => Guid.NewGuid()).ToList();
            ppg = (int)pictureBox1.Size.Width / size;
        }

        public class GameData
        {
            public int score { get; set; } = 0;
            public List<List<int>> board { get; set; } = new();
            public List<List<int>> snake { get; set; } = new();
            public List<int> snake_data { get; set; } = new();
            public int status { get; set; } = 0;

        }

        private void StartServer()
        {
            string exe_directory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            string program_str = string.Concat("/C python ", exe_directory, "\\engine-apified.pyw");
            ProcessStartInfo startInfo = new()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                FileName = "cmd.exe",
                Arguments = program_str
            };
            srv.StartInfo = startInfo;
            srv.Start();
        }

        private static GameData Request(string endpoint, GameData? PostData = null)
        {
            GameData? result;
            if (PostData == null)
            {
                var resultTask = client.GetFromJsonAsync<GameData>(string.Join('/', address, endpoint));
                resultTask.Wait();
                result = resultTask.Result;
            }
            else
            {
                var responseTask = client.PostAsJsonAsync(string.Join('/', address, endpoint), PostData);
                responseTask.Wait();
                var response = responseTask.Result;
                var resolveResponseTask = response.Content.ReadFromJsonAsync<GameData>();
                resolveResponseTask.Wait();
                result = resolveResponseTask.Result;
            }
            if (result == null)
            {
                throw new Exception("Request returned null.");
            }
            return result;

        }

        private void Field_Draw(Graphics g, (int, int) location, int colorid, bool border = false)
        {
            Rectangle rect = new(location.Item2 * ppg, location.Item1 * ppg, ppg, ppg);
            g.FillRectangle(drawingTools[colorid], rect);
            if (border)
            {
                g.DrawRectangle(new Pen(drawingTools[colorid + 1], ppg / 10), rect);
            }
        }

        private void Draw_Board(Graphics g)
        {
            foreach (var row in gameData.board.Select((Value, Index) => new { Value, Index }))
            {
                foreach (var box in row.Value.Select((Value, Index) => new { Value, Index }))
                {
                    if (box.Value != -1)
                    {
                        Field_Draw(g, (row.Index, box.Index), box.Value, true);
                    }
                }
            }
            foreach (var seg in gameData.snake.Select((Value, Index) => new { Value, Index }))
            {
                Field_Draw(g, (seg.Value[0], seg.Value[1]), gameData.snake_data[seg.Index]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            direction = null;
            gameData = Request(string.Join('/', "boardgen", size.ToString()));
            timer1.Start();
            pictureBox1.Invalidate();

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Draw_Board(e.Graphics);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            var k_v = e.KeyCode;
            switch (k_v)
            {
                case Keys.W or Keys.Up:
                    direction = "U";
                    break;
                case Keys.S or Keys.Down:
                    direction = "D";
                    break;
                case Keys.A or Keys.Left:
                    direction = "L";
                    break;
                case Keys.D or Keys.Right:
                    direction = "R";
                    break;
                default:
                    break;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (direction != null)
            {
                gameData = Request(string.Join('/', "move", direction), gameData);
                if (gameData.status != 0)
                {
                    bool win = false;
                    if (gameData.status == 1){
                        win = true;
                        pictureBox1.Invalidate();
                    }
                    timer1.Stop();
                    Summary alph = new(gameData.score, win);
                    alph.ShowDialog();
                    if (alph.restart)
                    {
                        Form1_Load(timer1, new EventArgs());
                        return;
                    }
                    srv.Kill(true);
                    Close();
                    Environment.Exit(0);

                }
                pictureBox1.Invalidate();
            }
        }
    }
}
