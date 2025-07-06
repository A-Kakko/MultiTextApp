using MultiTextApp.Interfaces;
using MultiTextApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiTextApp.Presenters
{
    internal class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IDocumentModel _model;

        // DI：両方とも外から注入
        public MainPresenter(IMainView view, IDocumentModel model)
        {
            _view = view;
            _model = model;


            // イベントハンドラの登録
            _view.NewFileRequested += OnNewFile;
            _view.OpenFileRequested += OnOpenFile;
            _view.SaveFileRequested += OnSaveFile;
            _view.SaveAsFileRequested += OnSaveAsFile;
            _view.ExitRequested += OnExit;
            _view.FormClosingRequested += OnFormClosing;

            UpdateView();
        }
        // 新規ファイル
        private void OnNewFile()
        {
            if(CheckUnsavedChanges())
            {
                _model.CreateNew();
                UpdateView();
            }
        }
        // ファイルを開く
        private void OnOpenFile()
        {
            if (!CheckUnsavedChanges()) return;
            
            _view.ShowOpenFileDialog(_model.GetSupportedFormats(), out string filePath, out IFileFormat selectedFormat, out bool result);
            if (result)
            {
                try
                {
                    _model.LoadFromFile(filePath, selectedFormat);
                    UpdateView();
                }
                catch (Exception ex)
                {
                    _view.ShowError($"ファイルの読み込みに失敗しました: {ex.Message}");
                }
            }
        }

        // 上書き保存
        private void OnSaveFile()
        {
            if (string.IsNullOrEmpty(_model.FilePath))
            {
                OnSaveAsFile();
            }
            else
            {
                SaveFile(_model.FilePath, _model.CurrentFormat);
            }
        }

        // 名前を付けて保存
        private void OnSaveAsFile()
        {
            var formats = _model.GetSupportedFormats();
            _view.ShowSaveFileDialog(formats, out string filePath, out IFileFormat selectedFormat, out bool result);

            if(result)
            {
                SaveFile(filePath, selectedFormat);
            }
        }

        //保存処理
        private void SaveFile(string filePath, IFileFormat selectedFormat)
        {
            try
            {
                _model.Content = _view.DocumentText; // Viewから最新テキストを取得
                _model.SaveToFile(filePath, selectedFormat);
                UpdateView();
                _view.ShowMessage("保存しました");
            }
            catch (Exception ex)
            {
                _view.ShowError($"保存できませんでした: {ex.Message}");
            }
        }

        //終了要求
        private void OnExit()
        {
            if (CheckUnsavedChanges())
            {
                _view.CloseApplication();
            }
        }

        //　フォーム終了時
        private void OnFormClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (!CheckUnsavedChanges())
            {
                e.Cancel = true; // フォームの閉じる処理をキャンセル
            }
        }

        private bool CheckUnsavedChanges()
        {
            _model.Content = _view.DocumentText; // 最新内容を反映
            _model.IsModified = true; // とりあえず変更ありとする（後で改善）

            if (_model.IsModified)
            {
                bool saveChanges = _view.ShowConfirmDialog(
                    $"'{_model.GetFileName()}'への変更を保存しますか？");

                if (saveChanges)
                {
                    OnSaveFile();
                    return !_model.IsModified; // 保存成功時のみtrue
                }
            }

            return true; // 変更なし、または破棄
        }



        private void UpdateView()
        {
            _view.DocumentText = _model.Content;
            _view.WindowTitle = $"{_model.GetFileName()}{(_model.IsModified ? "*" : "")} - MultiTextApp";
        }
    }
}