using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using SynApi;

namespace Crystal
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			this.InitializeComponent();
			this.ExtraLoads();
			base.Closed += this.MainWindow_Closed;
		}

		public WebView2 ScriptExecutionControl
		{
			get
			{
				return this.ScriptExecution;
			}
		}

		public void ExtraLoads()
		{
			this.InitializeScripts();
			this.MonacoInitiliaze();
			base.Topmost = true;
			this.InjectionStatus.Fill = new SolidColorBrush(Colors.Red);
			this.CrystalName.Text = "Crystal";
			this.CrystalDiscord.Text = ".gg/getcrystal";
			this.InjectionStatusTXT.Text = "Injection Status:";
		}

		public void InitializeScripts()
		{
			this.LoadScripts();
		}

		private void LoadScripts()
		{
			this.ScriptList.Items.Clear();
			foreach (FileInfo fileInfo in new DirectoryInfo("./Scripts").GetFiles("*.txt"))
			{
				this.ScriptList.Items.Add(fileInfo.Name);
			}
			foreach (FileInfo fileInfo2 in new DirectoryInfo("./Scripts").GetFiles("*.lua"))
			{
				this.ScriptList.Items.Add(fileInfo2.Name);
			}
			foreach (FileInfo fileInfo3 in new DirectoryInfo("./Scripts").GetFiles("*.rbxl"))
			{
				this.ScriptList.Items.Add(fileInfo3.Name);
			}
		}

		private void RefreshScripts(object sender, RoutedEventArgs e)
		{
			this.LoadScripts();
			this.ScriptExecution.ExecuteScriptAsync("window.setEditorText('hi');");
		}

		public void MonacoInitiliaze()
		{
			this.ScriptExecution.Source = new Uri(global::System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Monaco\\index.html"));
		}

		private void CloseGUI(object sender, MouseButtonEventArgs e)
		{
			bool flag = this.ScriptHub != null;
			if (flag)
			{
				ScriptHub.CloseScriptHub();
			}
			base.Close();
		}

		private void MinimizeGUI(object sender, MouseButtonEventArgs e)
		{
			try
			{
				base.WindowState = WindowState.Minimized;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void NotClickable(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("FUCK OFF IM NOT CLICKABLE \ud83d\ude2d", this.CrystalName.Text);
		}

		private async void ExecuteScript(object sender, RoutedEventArgs e)
		{
			string text = await this.ScriptExecution.ExecuteScriptAsync("editor.getValue()");
			string editorvalue = text;
			text = null;
			editorvalue = JsonConvert.DeserializeObject<string>(editorvalue);
			bool flag = await ExploitApi.CheckInjectionStatus();
			bool IsInjected = flag;
			if (!IsInjected)
			{
				MessageBox.Show("Not Injected");
			}
			else
			{
				await ExploitApi.Execute(editorvalue);
			}
		}

		private void ClearEditor(object sender, MouseButtonEventArgs e)
		{
			this.ScriptExecution.ExecuteScriptAsync("clearEditor();");
		}

		private async void LoadScript(object sender, RoutedEventArgs e)
		{
			try
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Text files (*.txt)|*.txt|Lua files (*.lua)|*.lua";
				bool? flag = openFileDialog.ShowDialog();
				bool flag2 = true;
				bool flag3 = (flag.GetValueOrDefault() == flag2) & (flag != null);
				if (flag3)
				{
					string filePath = openFileDialog.FileName;
					string fileExtension = global::System.IO.Path.GetExtension(filePath).ToLower();
					bool flag4 = fileExtension == ".txt" || fileExtension == ".lua";
					if (flag4)
					{
						string fileContent = File.ReadAllText(filePath);
						string script = "editor.setValue(`" + fileContent.Replace("`", "\\`") + "`);";
						await this.ScriptExecution.ExecuteScriptAsync(script);
						fileContent = null;
						script = null;
					}
					else
					{
						MessageBox.Show("Please select a file with a .txt or .lua extension.", "Invalid File Type", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					}
					filePath = null;
					fileExtension = null;
				}
				openFileDialog = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error loading file: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}

		private async void Attach(object sender, MouseButtonEventArgs e)
		{
			bool flag = await ExploitApi.CheckInjectionStatus();
			bool IsInjected = flag;
			if (!IsInjected)
			{
				this.InjectionStatus.Fill = new SolidColorBrush(Colors.Red);
				await ExploitApi.Inject();
				await Task.Delay(TimeSpan.FromSeconds(4.0));
				this.AutoExec();
				this.InjectionStatus.Fill = new SolidColorBrush(Colors.LightGreen);
			}
			else
			{
				MessageBox.Show("Already Injected");
			}
		}

		private void AutoExec()
		{
			ExploitApi.Execute("if getgenv and getgenv().MoreUNC then return end\r\n\r\n-- Definitions\r\nlocal table = table.clone(table) -- Prevent modifications from other scripts\r\nlocal debug = table.clone(debug) -- ^^^^\r\nlocal bit32 = table.clone(bit32)\r\nlocal bit = bit32\r\nlocal os = table.clone(os)\r\nlocal math = table.clone(math)\r\nlocal utf8 = table.clone(utf8)\r\nlocal string = table.clone(string)\r\nlocal task = table.clone(task)\r\n\r\nlocal game = game -- game is game\r\nlocal oldGame = game\r\n\r\nlocal ExecutorVersion = '1.2.8'\r\n\r\nlocal isDragging = false -- rconsole\r\nlocal dragStartPos = nil -- rconsole\r\nlocal frameStartPos = nil -- rconsole\r\n\r\nlocal Data = game:GetService(\"TeleportService\"):GetLocalPlayerTeleportData()\r\nlocal TeleportData\r\nif Data and Data.MOREUNCSCRIPTQUEUE then\r\n\tTeleportData = Data.MOREUNCSCRIPTQUEUE\r\nend\r\nif TeleportData then\r\n\tlocal func = loadstring(TeleportData)\r\n\tlocal s, e = pcall(func)\r\n\tif not s then task.spawn(error, e) end\r\nend\r\n\r\n\r\nprint = print\r\nwarn = warn\r\nerror = error\r\npcall = pcall\r\nprintidentity = printidentity\r\nipairs = ipairs\r\npairs = pairs\r\ntostring = tostring\r\ntonumber = tonumber\r\nsetmetatable = setmetatable\r\nrawget = rawget\r\nrawset = rawset\r\ngetmetatable = getmetatable\r\ntype = type\r\nExecutorVersion = ExecutorVersion\r\n\r\n-- Services / Instances\r\nlocal HttpService = game:GetService('HttpService');\r\nlocal Log = game:GetService('LogService');\r\n\r\nlocal vim = Instance.new(\"VirtualInputManager\");\r\n\r\nlocal DrawingDict = Instance.new(\"ScreenGui\") -- For drawing.new\r\n\r\nlocal ClipboardUI = Instance.new(\"ScreenGui\") -- For setclipboard\r\n\r\nlocal hui = Instance.new(\"Folder\") -- For gethui\r\nhui.Name = '\\0'\r\n\r\nlocal ClipboardBox = Instance.new('TextBox', ClipboardUI) -- For setclipboard\r\nClipboardBox.Position = UDim2.new(100, 0, 100, 0) -- VERY off screen\r\n\r\n-- All the following are for rconsole\r\nlocal Console = Instance.new(\"ScreenGui\")\r\nlocal ConsoleFrame = Instance.new(\"Frame\")\r\nlocal Topbar = Instance.new(\"Frame\")\r\nlocal _CORNER = Instance.new(\"UICorner\")\r\nlocal ConsoleCorner = Instance.new(\"UICorner\")\r\nlocal CornerHide = Instance.new(\"Frame\")\r\nlocal DontModify = Instance.new(\"Frame\")\r\nlocal UICorner = Instance.new(\"UICorner\")\r\nlocal CornerHide2 = Instance.new(\"Frame\")\r\nlocal Title = Instance.new(\"TextLabel\")\r\nlocal UIPadding = Instance.new(\"UIPadding\")\r\nlocal ConsoleIcon = Instance.new(\"ImageLabel\")\r\nlocal Holder = Instance.new(\"ScrollingFrame\")\r\nlocal MessageTemplate = Instance.new(\"TextLabel\")\r\nlocal InputTemplate = Instance.new(\"TextBox\")\r\nlocal UIListLayout = Instance.new(\"UIListLayout\")\r\nlocal HolderPadding = Instance.new(\"UIPadding\")\r\n\r\nConsole.Name = \"Console\"\r\nConsole.Parent = nil\r\nConsole.ZIndexBehavior = Enum.ZIndexBehavior.Sibling\r\n\r\nConsoleFrame.Name = \"ConsoleFrame\"\r\nConsoleFrame.Parent = Console\r\nConsoleFrame.BackgroundColor3 = Color3.fromRGB(0, 0, 0)\r\nConsoleFrame.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nConsoleFrame.BorderSizePixel = 0\r\nConsoleFrame.Position = UDim2.new(0.0963890627, 0, 0.220791712, 0)\r\nConsoleFrame.Size = UDim2.new(0, 888, 0, 577)\r\n\r\nTopbar.Name = \"Topbar\"\r\nTopbar.Parent = ConsoleFrame\r\nTopbar.BackgroundColor3 = Color3.fromRGB(20, 20, 20)\r\nTopbar.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nTopbar.BorderSizePixel = 0\r\nTopbar.Position = UDim2.new(0, 0, -0.000463640812, 0)\r\nTopbar.Size = UDim2.new(1, 0, 0, 32)\r\n\r\n_CORNER.Name = \"_CORNER\"\r\n_CORNER.Parent = Topbar\r\n\r\nConsoleCorner.Name = \"ConsoleCorner\"\r\nConsoleCorner.Parent = ConsoleFrame\r\n\r\nCornerHide.Name = \"CornerHide\"\r\nCornerHide.Parent = ConsoleFrame\r\nCornerHide.BackgroundColor3 = Color3.fromRGB(20, 20, 20)\r\nCornerHide.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nCornerHide.BorderSizePixel = 0\r\nCornerHide.Position = UDim2.new(0, 0, 0.0280000009, 0)\r\nCornerHide.Size = UDim2.new(1, 0, 0, 12)\r\n\r\nDontModify.Name = \"DontModify\"\r\nDontModify.Parent = ConsoleFrame\r\nDontModify.BackgroundColor3 = Color3.fromRGB(20, 20, 20)\r\nDontModify.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nDontModify.BorderSizePixel = 0\r\nDontModify.Position = UDim2.new(0.98169291, 0, 0.0278581586, 0)\r\nDontModify.Size = UDim2.new(-0.00675675692, 21, 0.972141862, 0)\r\n\r\nUICorner.Parent = DontModify\r\n\r\nCornerHide2.Name = \"CornerHide2\"\r\nCornerHide2.Parent = ConsoleFrame\r\nCornerHide2.AnchorPoint = Vector2.new(1, 0)\r\nCornerHide2.BackgroundColor3 = Color3.fromRGB(20, 20, 20)\r\nCornerHide2.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nCornerHide2.BorderSizePixel = 0\r\nCornerHide2.Position = UDim2.new(1, 0, 0.0450000018, 0)\r\nCornerHide2.Size = UDim2.new(0, 9, 0.955023408, 0)\r\n\r\nTitle.Name = \"Title\"\r\nTitle.Parent = ConsoleFrame\r\nTitle.BackgroundColor3 = Color3.fromRGB(255, 255, 255)\r\nTitle.BackgroundTransparency = 1.000\r\nTitle.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nTitle.BorderSizePixel = 0\r\nTitle.Position = UDim2.new(0.0440017432, 0, 0, 0)\r\nTitle.Size = UDim2.new(0, 164, 0, 30)\r\nTitle.Font = Enum.Font.GothamMedium\r\nTitle.Text = \"rconsole title\"\r\nTitle.TextColor3 = Color3.fromRGB(255, 255, 255)\r\nTitle.TextSize = 17.000\r\nTitle.TextXAlignment = Enum.TextXAlignment.Left\r\n\r\nUIPadding.Parent = Title\r\nUIPadding.PaddingTop = UDim.new(0, 5)\r\n\r\nConsoleIcon.Name = \"ConsoleIcon\"\r\nConsoleIcon.Parent = ConsoleFrame\r\nConsoleIcon.BackgroundColor3 = Color3.fromRGB(255, 255, 255)\r\nConsoleIcon.BackgroundTransparency = 1.000\r\nConsoleIcon.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nConsoleIcon.BorderSizePixel = 0\r\nConsoleIcon.Position = UDim2.new(0.00979213417, 0, 0.000874322082, 0)\r\nConsoleIcon.Size = UDim2.new(0, 31, 0, 31)\r\nConsoleIcon.Image = \"http://www.roblox.com/asset/?id=11843683545\"\r\n\r\nHolder.Name = \"Holder\"\r\nHolder.Parent = ConsoleFrame\r\nHolder.Active = true\r\nHolder.BackgroundColor3 = Color3.fromRGB(20, 20, 20)\r\nHolder.BackgroundTransparency = 1.000\r\nHolder.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nHolder.BorderSizePixel = 0\r\nHolder.Position = UDim2.new(0, 0, 0.054600548, 0)\r\nHolder.Size = UDim2.new(1, 0, 0.945399463, 0)\r\nHolder.ScrollBarThickness = 8\r\nHolder.CanvasSize = UDim2.new(0,0,0,0)\r\nHolder.AutomaticCanvasSize = Enum.AutomaticSize.XY\r\n\r\nMessageTemplate.Name = \"MessageTemplate\"\r\nMessageTemplate.Parent = Holder\r\nMessageTemplate.BackgroundColor3 = Color3.fromRGB(255, 255, 255)\r\nMessageTemplate.BackgroundTransparency = 1.000\r\nMessageTemplate.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nMessageTemplate.BorderSizePixel = 0\r\nMessageTemplate.Size = UDim2.new(0.9745, 0, 0.030000001, 0)\r\nMessageTemplate.Visible = false\r\nMessageTemplate.Font = Enum.Font.RobotoMono\r\nMessageTemplate.Text = \"TEMPLATE\"\r\nMessageTemplate.TextColor3 = Color3.fromRGB(255, 255, 255)\r\nMessageTemplate.TextSize = 20.000\r\nMessageTemplate.TextXAlignment = Enum.TextXAlignment.Left\r\nMessageTemplate.TextYAlignment = Enum.TextYAlignment.Top\r\nMessageTemplate.RichText = true\r\n\r\nUIListLayout.Parent = Holder\r\nUIListLayout.SortOrder = Enum.SortOrder.LayoutOrder\r\nUIListLayout.Padding = UDim.new(0, 4)\r\n\r\nHolderPadding.Name = \"HolderPadding\"\r\nHolderPadding.Parent = Holder\r\nHolderPadding.PaddingLeft = UDim.new(0, 15)\r\nHolderPadding.PaddingTop = UDim.new(0, 15)\r\n\r\nInputTemplate.Name = \"InputTemplate\"\r\nInputTemplate.Parent = nil\r\nInputTemplate.BackgroundColor3 = Color3.fromRGB(255, 255, 255)\r\nInputTemplate.BackgroundTransparency = 1.000\r\nInputTemplate.BorderColor3 = Color3.fromRGB(0, 0, 0)\r\nInputTemplate.BorderSizePixel = 0\r\nInputTemplate.Size = UDim2.new(0.9745, 0, 0.030000001, 0)\r\nInputTemplate.Visible = false\r\nInputTemplate.RichText = true\r\nInputTemplate.Font = Enum.Font.RobotoMono\r\nInputTemplate.Text = \"\"\r\nInputTemplate.PlaceholderText = ''\r\nInputTemplate.TextColor3 = Color3.fromRGB(255, 255, 255)\r\nInputTemplate.TextSize = 20.000\r\nInputTemplate.TextXAlignment = Enum.TextXAlignment.Left\r\nInputTemplate.TextYAlignment = Enum.TextYAlignment.Top\r\n\r\n-- Variables\r\nlocal Identity = -1\r\nlocal active = true\r\n-- Others\r\nlocal oldLoader = loadstring\r\n-- Empty Tables\r\nlocal clonerefs = {}\r\nlocal protecteduis = {}\r\nlocal gc = {}\r\nlocal Instances = {} -- for nil instances\r\nlocal funcs = {} -- main table\r\nlocal names = {} -- protected gui names\r\nlocal Cache = {} -- for cached instances\r\nlocal Drawings = {} -- for cleardrawcache\r\n-- Non empty tables\r\nlocal colors = {\r\n\tBLACK = Color3.fromRGB(50, 50, 50),\r\n\tBLUE = Color3.fromRGB(0, 0, 204),\r\n\tGREEN = Color3.fromRGB(0, 255, 0),\r\n\tCYAN = Color3.fromRGB(0, 255, 255),\r\n\tRED = Color3.fromHex('#5A0101'),\r\n\tMAGENTA = Color3.fromRGB(255, 0, 255),\r\n\tBROWN = Color3.fromRGB(165, 42, 42),\r\n\tLIGHT_GRAY = Color3.fromRGB(211, 211, 211),\r\n\tDARK_GRAY = Color3.fromRGB(169, 169, 169),\r\n\tLIGHT_BLUE = Color3.fromRGB(173, 216, 230),\r\n\tLIGHT_GREEN = Color3.fromRGB(144, 238, 144),\r\n\tLIGHT_CYAN = Color3.fromRGB(224, 255, 255),\r\n\tLIGHT_RED = Color3.fromRGB(255, 204, 203),\r\n\tLIGHT_MAGENTA = Color3.fromRGB(255, 182, 193),\r\n\tYELLOW = Color3.fromRGB(255, 255, 0),\r\n\tWHITE = Color3.fromRGB(255, 255, 255),\r\n\tORANGE = Color3.fromRGB(255, 186, 12)\r\n}\r\nlocal patterns = {\r\n\t{ pattern = '(%w+)%s*%+=%s*(%w+)', format = \"%s = %s + %s\" },\r\n\t{ pattern = '(%w+)%s*%-=%s*(%w+)', format = \"%s = %s - %s\" },\r\n\t{ pattern = '(%w+)%s*%*=%s*(%w+)', format = \"%s = %s * %s\" },\r\n\t{ pattern = '(%w+)%s*/=%s*(%w+)', format = \"%s = %s / %s\" }\r\n}\r\nlocal patterns2 = {\r\n\t{ pattern = 'for%s+(%w+)%s*,%s*(%w+)%s*in%s*(%w+)%s*do', format = \"for %s, %s in pairs(%s) do\" }\r\n}\r\nlocal renv = {\r\n\tprint, warn, error, assert, collectgarbage, load, require, select, tonumber, tostring, type, xpcall, pairs, next, ipairs,\r\n\tnewproxy, rawequal, rawget, rawset, rawlen, setmetatable, PluginManager,\r\n\tcoroutine.create, coroutine.resume, coroutine.running, coroutine.status, coroutine.wrap, coroutine.yield,\r\n\tbit32.arshift, bit32.band, bit32.bnot, bit32.bor, bit32.btest, bit32.extract, bit32.lshift, bit32.replace, bit32.rshift, bit32.xor,\r\n\tmath.abs, math.acos, math.asin, math.atan, math.atan2, math.ceil, math.cos, math.cosh, math.deg, math.exp, math.floor, math.fmod, math.frexp, math.ldexp, math.log, math.log10, math.max, math.min, math.modf, math.pow, math.rad, math.random, math.randomseed, math.sin, math.sinh, math.sqrt, math.tan, math.tanh,\r\n\tstring.byte, string.char, string.find, string.format, string.gmatch, string.gsub, string.len, string.lower, string.match, string.pack, string.packsize, string.rep, string.reverse, string.sub, string.unpack, string.upper,\r\n\ttable.concat, table.insert, table.pack, table.remove, table.sort, table.unpack,\r\n\tutf8.char, utf8.charpattern, utf8.codepoint, utf8.codes, utf8.len, utf8.nfdnormalize, utf8.nfcnormalize,\r\n\tos.clock, os.date, os.difftime, os.time,\r\n\tdelay, elapsedTime, require, spawn, tick, time, typeof, UserSettings, ExecutorVersion, wait,\r\n\ttask.defer, task.delay, task.spawn, task.wait,\r\n\tdebug.traceback, debug.profilebegin, debug.profileend\r\n}\r\nlocal keys={[0x08]=Enum.KeyCode.Backspace,[0x09]=Enum.KeyCode.Tab,[0x0C]=Enum.KeyCode.Clear,[0x0D]=Enum.KeyCode.Return,[0x10]=Enum.KeyCode.LeftShift,[0x11]=Enum.KeyCode.LeftControl,[0x12]=Enum.KeyCode.LeftAlt,[0x13]=Enum.KeyCode.Pause,[0x14]=Enum.KeyCode.CapsLock,[0x1B]=Enum.KeyCode.Escape,[0x20]=Enum.KeyCode.Space,[0x21]=Enum.KeyCode.PageUp,[0x22]=Enum.KeyCode.PageDown,[0x23]=Enum.KeyCode.End,[0x24]=Enum.KeyCode.Home,[0x2D]=Enum.KeyCode.Insert,[0x2E]=Enum.KeyCode.Delete,[0x30]=Enum.KeyCode.Zero,[0x31]=Enum.KeyCode.One,[0x32]=Enum.KeyCode.Two,[0x33]=Enum.KeyCode.Three,[0x34]=Enum.KeyCode.Four,[0x35]=Enum.KeyCode.Five,[0x36]=Enum.KeyCode.Six,[0x37]=Enum.KeyCode.Seven,[0x38]=Enum.KeyCode.Eight,[0x39]=Enum.KeyCode.Nine,[0x41]=Enum.KeyCode.A,[0x42]=Enum.KeyCode.B,[0x43]=Enum.KeyCode.C,[0x44]=Enum.KeyCode.D,[0x45]=Enum.KeyCode.E,[0x46]=Enum.KeyCode.F,[0x47]=Enum.KeyCode.G,[0x48]=Enum.KeyCode.H,[0x49]=Enum.KeyCode.I,[0x4A]=Enum.KeyCode.J,[0x4B]=Enum.KeyCode.K,[0x4C]=Enum.KeyCode.L,[0x4D]=Enum.KeyCode.M,[0x4E]=Enum.KeyCode.N,[0x4F]=Enum.KeyCode.O,[0x50]=Enum.KeyCode.P,[0x51]=Enum.KeyCode.Q,[0x52]=Enum.KeyCode.R,[0x53]=Enum.KeyCode.S,[0x54]=Enum.KeyCode.T,[0x55]=Enum.KeyCode.U,[0x56]=Enum.KeyCode.V,[0x57]=Enum.KeyCode.W,[0x58]=Enum.KeyCode.X,[0x59]=Enum.KeyCode.Y,[0x5A]=Enum.KeyCode.Z,[0x5D]=Enum.KeyCode.Menu,[0x60]=Enum.KeyCode.KeypadZero,[0x61]=Enum.KeyCode.KeypadOne,[0x62]=Enum.KeyCode.KeypadTwo,[0x63]=Enum.KeyCode.KeypadThree,[0x64]=Enum.KeyCode.KeypadFour,[0x65]=Enum.KeyCode.KeypadFive,[0x66]=Enum.KeyCode.KeypadSix,[0x67]=Enum.KeyCode.KeypadSeven,[0x68]=Enum.KeyCode.KeypadEight,[0x69]=Enum.KeyCode.KeypadNine,[0x6A]=Enum.KeyCode.KeypadMultiply,[0x6B]=Enum.KeyCode.KeypadPlus,[0x6D]=Enum.KeyCode.KeypadMinus,[0x6E]=Enum.KeyCode.KeypadPeriod,[0x6F]=Enum.KeyCode.KeypadDivide,[0x70]=Enum.KeyCode.F1,[0x71]=Enum.KeyCode.F2,[0x72]=Enum.KeyCode.F3,[0x73]=Enum.KeyCode.F4,[0x74]=Enum.KeyCode.F5,[0x75]=Enum.KeyCode.F6,[0x76]=Enum.KeyCode.F7,[0x77]=Enum.KeyCode.F8,[0x78]=Enum.KeyCode.F9,[0x79]=Enum.KeyCode.F10,[0x7A]=Enum.KeyCode.F11,[0x7B]=Enum.KeyCode.F12,[0x90]=Enum.KeyCode.NumLock,[0x91]=Enum.KeyCode.ScrollLock,[0xBA]=Enum.KeyCode.Semicolon,[0xBB]=Enum.KeyCode.Equals,[0xBC]=Enum.KeyCode.Comma,[0xBD]=Enum.KeyCode.Minus,[0xBE]=Enum.KeyCode.Period,[0xBF]=Enum.KeyCode.Slash,[0xC0]=Enum.KeyCode.Backquote,[0xDB]=Enum.KeyCode.LeftBracket,[0xDD]=Enum.KeyCode.RightBracket,[0xDE]=Enum.KeyCode.Quote} -- for keypress\r\nlocal Fonts = { -- Drawing.Fonts\r\n\t[0] = Enum.Font.Arial,\r\n\t[1] = Enum.Font.BuilderSans,\r\n\t[2] = Enum.Font.Gotham,\r\n\t[3] = Enum.Font.RobotoMono\r\n}\r\n-- rconsole\r\nlocal MessageColor = colors['WHITE']\r\nlocal ConsoleClone = nil\r\n-- functions\r\nlocal function Descendants(tbl)\r\n\tlocal descendants = {}\r\n\r\n\tlocal function process_table(subtbl, prefix)\r\n\t\tfor k, v in pairs(subtbl) do\r\n\t\t\tlocal index = prefix and (prefix .. \".\" .. tostring(k)) or tostring(k)\r\n\t\t\tdescendants[index] = v\r\n\t\t\tif type(v) == 'table' then\r\n\t\t\t\tprocess_table(v, index)\r\n\t\t\telse\r\n\t\t\t\tdescendants[index] = v\r\n\t\t\tend\r\n\t\tend\r\n\tend\r\n\r\n\tif type(tbl) ~= 'table' then\r\n\t\tdescendants[tostring(1)] = tbl\r\n\telse\r\n\t\tprocess_table(tbl, nil)\r\n\tend\r\n\r\n\treturn descendants\r\nend\r\n\r\nlocal function rawlength(tbl)\r\n\tlocal a = 0\r\n\tfor i, v in pairs(tbl) do\r\n\t\ta = a + 1\r\n\tend\r\n\treturn a\r\nend\r\n\r\nlocal function ToPairsLoop(code)\r\n\tfor _, p in ipairs(patterns2) do\r\n\t\tcode = code:gsub(p.pattern, function(var1, var2, tbl)\r\n\t\t\treturn p.format:format(var1, var2, tbl)\r\n\t\tend)\r\n\tend\r\n\treturn code\r\nend\r\n\r\nlocal function SafeOverride(a, b, c) --[[ Index, Data, Should override ]]\r\n\tif getgenv()[a] and not c then return 1 end\r\n\tgetgenv()[a] = b\r\n\r\n\treturn 2\r\nend\r\n\r\nlocal function toluau(code)\r\n\tfor _, p in ipairs(patterns) do\r\n\t\tcode = code:gsub(p.pattern, function(var, value)\r\n\t\t\treturn p.format:format(var, var, value)\r\n\t\tend)\r\n\tend\r\n\tcode = ToPairsLoop(code)\r\n\treturn code\r\nend\r\n\r\nlocal function handleInput(input, Object)\r\n\tif isDragging then\r\n\t\tlocal delta = input.Position - dragStartPos\r\n\t\tObject.Position = UDim2.new(\r\n\t\t\tframeStartPos.X.Scale, \r\n\t\t\tframeStartPos.X.Offset + delta.X, \r\n\t\t\tframeStartPos.Y.Scale, \r\n\t\t\tframeStartPos.Y.Offset + delta.Y\r\n\t\t)\r\n\tend\r\nend\r\n\r\nlocal function startDrag(input, Object)\r\n\tisDragging = true\r\n\tdragStartPos = input.Position\r\n\tframeStartPos = Object.Position\r\n\tinput.UserInputState = Enum.UserInputState.Begin\r\nend\r\n\r\nlocal function stopDrag(input)\r\n\tisDragging = false\r\n\tinput.UserInputState = Enum.UserInputState.End\r\nend\r\n\r\n-- Main Functions\r\nfunction QueueGetIdentity()\r\n\tprintidentity()\r\n\ttask.wait(.1)\r\n\tlocal messages = Log:GetLogHistory()\r\n\tlocal message;\r\n\tif not messages[#messages].message:match(\"Current identity is\") then\r\n\t\tfor i = #messages, 1, -1 do\r\n\t\t\tif messages[i].message:match(\"Current identity is %d\") then\r\n\t\t\t\tmessage = messages[i].message\r\n\t\t\t\tbreak\r\n\t\t\tend\r\n\t\tend\r\n\telse\r\n\t\tmessage = messages[#messages].message:match('Current identity is %d'):gsub(\"Current identity is \", '')\r\n\tend\r\n\tIdentity = tonumber(message)\r\nend\r\nlocal Queue = {}\r\nQueue.__index = Queue\r\nfunction Queue.new()\r\n\tlocal self = setmetatable({}, Queue)\r\n\tself.elements = {}\r\n\treturn self\r\nend\r\n\r\nfunction Queue:Queue(element)\r\n\ttable.insert(self.elements, element)\r\nend\r\n\r\nfunction Queue:Update()\r\n\tif #self.elements == 0 then\r\n\t\treturn nil\r\n\tend\r\n\treturn table.remove(self.elements, 1)\r\nend\r\n\r\nfunction Queue:IsEmpty()\r\n\treturn #self.elements == 0\r\nend\r\nfunction Queue:Current()\r\n\treturn self.elements\r\nend\r\n\r\n-- Events\r\ngame.DescendantRemoving:Connect(function(des)\r\n\ttable.insert(Instances, des)\r\n\tCache[des] = 'REMOVE'\r\nend)\r\ngame.DescendantAdded:Connect(function(des)\r\n\tCache[des] = true\r\nend)\r\ngame:GetService(\"UserInputService\").WindowFocused:Connect(function()\r\n\tactive = true\r\nend)\r\n\r\ngame:GetService(\"UserInputService\").WindowFocusReleased:Connect(function()\r\n\tactive = false\r\nend)\r\n\r\ngame:GetService(\"UserInputService\").InputChanged:Connect(function(input)\r\n\tif not input then return end\r\n\tif isDragging and input.UserInputType == Enum.UserInputType.MouseMovement and ConsoleClone then\r\n\t\thandleInput(input, ConsoleClone.ConsoleFrame)\r\n\tend\r\nend)\r\n\r\ngame:GetService(\"UserInputService\").InputEnded:Connect(function(input)\r\n\tif not input then return end\r\n\tif input.UserInputType == Enum.UserInputType.MouseButton1 then\r\n\t\tstopDrag(input)\r\n\tend\r\nend)\r\n-- Libraries\r\nfuncs.base64 = {}\r\nfuncs.crypt = {hex={},url={}}\r\nfuncs.syn = {}\r\nfuncs.syn_backup = {}\r\nfuncs.http = {}\r\nfuncs.Drawing = {}\r\nfuncs.cache = {}\r\nfuncs.string = string\r\nfuncs.debug = debug\r\nfuncs.debug.getinfo = function(t)\r\n\tlocal CurrentLine = tonumber(debug.info(t, 'l'))\r\n\tlocal Source = debug.info(t, 's')\r\n\tlocal name = debug.info(t, 'n')\r\n\tlocal numparams, isvrg = debug.info(t, 'a')\r\n\tif #name == 0 then name = nil end\r\n\tlocal a, b = debug.info(t, 'a')\r\n\treturn {\r\n\t\t['currentline'] = CurrentLine,\r\n\t\t['source'] = Source,\r\n\t\t['name'] = tostring(name),\r\n\t\t['numparams'] = tonumber(numparams),\r\n\t\t['is_vararg'] = isvrg and 1 or 0,\r\n\t\t['short_src'] = tostring(Source:sub(1, 60)),\r\n\t\t['what'] = Source == '[C]' and 'C' or 'Lua',\r\n\t\t['func'] = t,\r\n\t\t['nups'] = 0 -- i CANNOT make an upvalue thingy\r\n\t}\r\nend\r\n\r\nfuncs.Drawing.Fonts = {\r\n\t['UI'] = 0,\r\n\t['System'] = 1,\r\n\t['Plex'] = 2,\r\n\t['Monospace'] = 3\r\n}\r\n\r\n\r\nlocal ClipboardQueue = Queue.new()\r\nlocal ConsoleQueue = Queue.new()\r\nlocal getgenv = getgenv or function() return getfenv(1) end\r\ngetgenv().getgenv = getgenv\r\n\r\n-- [[ Functions ]]\r\n\r\n--[[funcs.cloneref = function(a)\r\n    if not clonerefs[a] then clonerefs[a] = {} end\r\n    local Clone = {}\r\n\r\n    local mt = {__type='Instance'} -- idk if this works ;(\r\n\r\n    mt.__tostring = function()\r\n        return a.Name\r\n    end\r\n\r\n    mt.__index = function(_, key)\r\n        local thing = funcs.debug.getmetatable(a)[key]\r\n        if type(thing) == 'function' then\r\n            return function(...)\r\n                return thing(a, ...)\r\n            end\r\n        else\r\n            return thing\r\n        end\r\n    end\r\n    mt.__newindex = function(_, key, value)\r\n     a[key] = value\r\n    end\r\n    mt.__metatable = getmetatable(a)\r\n    mt.__len = function(_)\r\n     return error('attempt to get length of a userdata value')\r\n    end\r\n\r\n    setmetatable(Clone, mt)\r\n\r\n    table.insert(clonerefs[a], Clone)\r\n\r\n    return Clone\r\nend\r\nTEMPORARY REMOVED UNTIL WE FIND A FIX\r\n]]\r\n-- // The rest is made by me.\r\n\r\nfuncs.compareinstances = function(a, b)\r\n\tif not clonerefs[a] then\r\n\t\treturn a == b\r\n\telse\r\n\t\tif table.find(clonerefs[a], b) then return true end\r\n\tend\r\n\treturn false\r\nend\r\n\r\nfuncs.clonefunction = function(a)\r\n\tassert(type(a)=='function', 'Invalid parameter 1 to \\'clonefunction\\', function expected got ' .. typeof(a))\r\n\r\n\treturn function(...)\r\n\t\tlocal Copy = Sandbox(a, {}, {}, {}, 0, {...})\r\n\t\treturn Copy.return_value\r\n\tend\r\nend\r\n\r\nfuncs.cache.iscached = function(thing)\r\n\treturn Cache[thing] ~= 'REMOVE' and thing:IsDescendantOf(game) or false -- If it's cache isnt 'REMOVE' and its a des of game (Usually always true) or if its cache is 'REMOVE' then its false.\r\nend\r\nfuncs.cache.invalidate = function(thing)\r\n\tCache[thing] = 'REMOVE'\r\n\tthing.Parent = nil\r\nend\r\nfuncs.cache.replace = function(a, b)\r\n\tif Cache[a] then\r\n\t\tCache[a] = b\r\n\tend\r\n\tlocal n, p = a.Name, a.Parent -- name, parent\r\n\tb.Parent = p\r\n\tb.Name = n\r\n\ta.Parent = nil\r\nend\r\nfuncs.deepclone = function(a)\r\n\tlocal Result = {}\r\n\tfor i, v in pairs(a) do\r\n\t\tif type(v) == 'table' then\r\n\t\t\tResult[i] = funcs.deepclone(v)\r\n\t\tend\r\n\t\tResult[i] = v\r\n\t[...string is too long...]");
		}

		private void OpenScriptHub(object sender, RoutedEventArgs e)
		{
			bool flag = this.ScriptHub == null;
			if (flag)
			{
				this.ScriptHub = new ScriptHub();
				this.ScriptHub.Closed += delegate(object s, EventArgs args)
				{
					this.ScriptHub = null;
				};
				this.ScriptHub.Show();
			}
			else
			{
				this.ScriptHub.Focus();
			}
		}

		private void Drag(object sender, MouseButtonEventArgs e)
		{
			try
			{
				base.DragMove();
			}
			catch (Exception)
			{
				MessageBox.Show("An error has occured!!", this.CrystalName.Text);
			}
		}

		private void ScriptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = this.ScriptList.SelectedIndex != -1;
			if (flag)
			{
				try
				{
					string text = File.ReadAllText("Scripts\\" + this.ScriptList.SelectedItem.ToString());
					string text2 = JsonConvert.SerializeObject(text);
					this.ScriptExecution.ExecuteScriptAsync("editor.setValue(" + text2 + ");");
				}
				catch (Exception)
				{
					MessageBox.Show("An error has occured!!", this.CrystalName.Text);
				}
			}
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			bool flag = this.ScriptHub != null;
			if (flag)
			{
				ScriptHub.CloseScriptHub();
			}
		}

		private ExploitApi exploitApi = new ExploitApi();

		private ScriptHub ScriptHub;
	}
}
