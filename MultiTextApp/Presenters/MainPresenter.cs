using MultiTextApp.Interfaces;
using MultiTextApp.Services;
using MultiTextApp.Models;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;


namespace MultiTextApp.Presenters
{
    internal class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IDocumentModel _model;
        private readonly IFileService _fileService;

        // DI：両方とも外から注入
        public MainPresenter(IMainView view, IDocumentModel model, IFileService fileService)
        {
            _view = view;
            _model = model;
            _fileService = fileService;

            // イベントハンドラの登録
            _view.NewFileRequested += OnNewFile;
            _view.OpenFileRequested += OnOpenFile;
            _view.SaveFileRequested += OnSaveFile;
            _view.SaveAsFileRequested += OnSaveAsFile;
            _view.ExitRequested += OnExit;
            _view.FormClosingRequested += OnFormClosing;
            _view.TextContentChanged += OnTextContentChanged;

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

            var formats = _model.GetSupportedFormats();
            var result = _view.ShowOpenFileDialog(formats);

            if (result.success)
            {
                try
                {
                    string content = _fileService.ReadFile(result.filePath);
                    _model.LoadContent(content, result.filePath, result.format);
                    UpdateView();
                }
                catch (Exception ex)
                {
                    _view.ShowError($"エラー: {ex.Message}");
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

        private void OnTextContentChanged()
        {
            _model.Content = _view.DocumentText; // Viewから最新テキストを取得
            UpdateView();
        }

        private void UpdateView()
        {
            _view.DocumentText = _model.Content;

            string modifiedMark = _model.IsModified ? "*" : "";
            _view.WindowTitle = $"{_model.GetFileName()}{(_model.IsModified ? "*" : "")} - MultiTextApp";
        }


    }
}