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
using Injection;

namespace Crystal
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			ExtraLoads();
			base.Closed += MainWindow_Closed;
		}

		public WebView2 ScriptExecutionControl
		{
			get
			{
				return ScriptExecution;
			}
		}

		public void ExtraLoads()
		{
			InitializeScripts();
			MonacoInitiliaze();
			base.Topmost = true;
			InjectionStatus.Fill = new SolidColorBrush(Colors.Red);
			CrystalName.Text = "Crystal - Chromatic";
			CrystalDiscord.Text = ".gg/ESzxG5S4Y2";
			InjectionStatusTXT.Text = "Injection Status:";
		}

		public void InitializeScripts()
		{
			LoadScripts();
		}

		private void LoadScripts()
		{
			ScriptList.Items.Clear();
			foreach (FileInfo fileInfo in new DirectoryInfo("./Scripts").GetFiles("*.txt"))
			{
				ScriptList.Items.Add(fileInfo.Name);
			}
			foreach (FileInfo fileInfo2 in new DirectoryInfo("./Scripts").GetFiles("*.lua"))
			{
				ScriptList.Items.Add(fileInfo2.Name);
			}
			foreach (FileInfo fileInfo3 in new DirectoryInfo("./Scripts").GetFiles("*.rbxl"))
			{
				ScriptList.Items.Add(fileInfo3.Name);
			}
		}

		private void RefreshScripts(object sender, RoutedEventArgs e)
		{
			LoadScripts();
			ScriptExecution.ExecuteScriptAsync("window.setEditorText('hi');");
		}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScriptExecution.Source = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Monaco\\index.html"));
        }

        public async void MonacoInitiliaze()
		{
            try
            {
                await ScriptExecution.EnsureCoreWebView2Async(null);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("WebView2 initialization failed: " + ex.Message, "Initialization Error", System.Windows.MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

		private void CloseGUI(object sender, MouseButtonEventArgs e)
		{
			bool flag = ScriptHub != null;
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
			MessageBox.Show("FUCK OFF IM NOT CLICKABLE \ud83d\ude2d", CrystalName.Text);
		}

		private async void ExecuteScript(object sender, RoutedEventArgs e)
		{
			string text = await ScriptExecution.ExecuteScriptAsync("editor.getValue()");
			string editorvalue = text;
			text = null;
			editorvalue = JsonConvert.DeserializeObject<string>(editorvalue);
			if (!IsInjected)
			{
				MessageBox.Show("Not Injected");
			}
			else
			{
				IsInjected = true;
                Injector.Execute(editorvalue, false);
			}
		}

		private void ClearEditor(object sender, MouseButtonEventArgs e)
		{
			ScriptExecution.ExecuteScriptAsync("clearEditor();");
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
						await ScriptExecution.ExecuteScriptAsync(script);
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

		public static bool IsInjected = false;


        private void Attach(object sender, MouseButtonEventArgs e)
		{
            foreach (Util.ProcInfo procInfo in Util.openProcessesByName("RobloxPlayerBeta.exe"))
            {
                InjectionStatus injectionStatuss = Injector.Inject(procInfo);
                try
                {
                    IsInjected = true;
                    InjectionStatus.Fill = new SolidColorBrush(Colors.LightGreen);
                    injectionStatuss = Injection.InjectionStatus.SUCCESS;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed To Inject {ex}");
                    injectionStatuss = Injection.InjectionStatus.FAILED;
                }
            }
        }

		private void OpenScriptHub(object sender, RoutedEventArgs e)
		{
			bool flag = ScriptHub == null;
			if (flag)
			{
				ScriptHub = new ScriptHub();
				ScriptHub.Closed += delegate(object s, EventArgs args)
				{
					ScriptHub = null;
				};
				ScriptHub.Show();
			}
			else
			{
				ScriptHub.Focus();
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
				MessageBox.Show("An error has occured!!", CrystalName.Text);
			}
		}

		private void ScriptList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bool flag = ScriptList.SelectedIndex != -1;
			if (flag)
			{
				try
				{
					string text = File.ReadAllText("Scripts\\" + ScriptList.SelectedItem.ToString());
					string text2 = JsonConvert.SerializeObject(text);
					ScriptExecution.ExecuteScriptAsync("editor.setValue(" + text2 + ");");
				}
				catch (Exception)
				{
					MessageBox.Show("An error has occured!!", CrystalName.Text);
				}
			}
		}

		private void MainWindow_Closed(object sender, EventArgs e)
		{
			bool flag = ScriptHub != null;
			if (flag)
			{
				ScriptHub.CloseScriptHub();
			}
		}

		private ScriptHub ScriptHub;
	}
}
