using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiTextApp.Presenters;
using MultiTextApp.Interfaces;
using MultiTextApp.Models;


namespace MultiTextApp.Views
{
    public partial class MainForm : Form, IMainView
    {
        private readonly MainPresenter _presenter;

        // テキストボックスの内容を取得・設定するプロパティ
        public string DocumentText
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
        // ウィンドウのタイトル
        public string WindowTitle
        {
            get => this.Text;
            set => this.Text = value;
        }
        // IMainView イベントの実装
        public event Action NewFileRequested;
        public event Action OpenFileRequested;
        public event Action SaveFileRequested;
        public event Action SaveAsFileRequested;    
        public event Action ExitRequested;
        public event Action<CancelEventArgs> FormClosingRequested;
        public event Action TextContentChanged;

        public MainForm()
        {
            InitializeComponent();

            textBox1.TextChanged += (s, e) => TextContentChanged?.Invoke();

            // DI: MainPresenter を初期化
            DocumentModel model = new DocumentModel();  
            model.SetDefaultFormat(new TxtFormat()); // デフォルトのフォーマットを設定
            _presenter = new MainPresenter(this, model);

            // フォーム終了イベントの設定
            this.FormClosing += MainForm_FormClosing;
        }

        public void ShowOpenFileDialog(List<IFileFormat> formats, out string filePath, out IFileFormat selectedFormat, out bool result)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = CreateFileFilter(formats);
                result = dialog.ShowDialog() == DialogResult.OK;

                if (result)
                {
                    filePath = dialog.FileName;
                    selectedFormat = GetFormatFromFilterIndex(formats, dialog.FilterIndex - 1);
                }
                else
                {
                    filePath = "";
                    selectedFormat = null;
                }
            }
        }

        public void ShowSaveFileDialog(List<IFileFormat> formats, out string filePath, out IFileFormat selectedFormat, out bool result)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = CreateFileFilter(formats);
                result = dialog.ShowDialog() == DialogResult.OK;

                if (result)
                {
                    filePath = dialog.FileName;
                    selectedFormat = GetFormatFromFilterIndex(formats, dialog.FilterIndex - 1);
                }
                else
                {
                    filePath = "";
                    selectedFormat = null;
                }
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "MultiTextApp", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public bool ShowConfirmDialog(string message)
        {
            DialogResult result = MessageBox.Show(message, "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return result == DialogResult.Yes;
        }

        public void CloseApplication()
        {
            Application.Exit();
        }

        // ヘルパーメソッド
        private string CreateFileFilter(List<IFileFormat> formats)
        {
            var filters = formats.Select(f => f.Filter).ToArray();
            return string.Join("|", filters) + "|すべてのファイル(*.*)|*.*";
        }

        private IFileFormat GetFormatFromFilterIndex(List<IFileFormat> formats, int index)
        {
            if (index >= 0 && index < formats.Count)
                return formats[index];
            return formats.FirstOrDefault(); // デフォルト
        }

        // イベントハンドラー（ボタンクリック等）
        private void buttonNew_Click(object sender, EventArgs e)
        {
            NewFileRequested?.Invoke();
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileRequested?.Invoke();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileRequested?.Invoke();
        }

        private void buttonSaveAs_Click(object sender, EventArgs e)
        {
            SaveAsFileRequested?.Invoke();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            ExitRequested?.Invoke();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FormClosingRequested?.Invoke(e);
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            TextContentChanged?.Invoke();
        }

    }
}
