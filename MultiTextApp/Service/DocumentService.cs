using MultiTextApp.Interfaces;
using System;

namespace MultiTextApp.Services
{
    /// <summary>
    /// ドキュメント操作を管理するサービス
    /// </summary>
    internal class DocumentService : IDocumentService
    {
        private readonly IDocumentModel _model;
        private readonly IFileService _fileService;
        private string _currentFilePath;
        private bool _isModified;

        // C#のプロパティ：getter/setterを簡潔に書ける仕組み
        public bool IsModified
        {
            get => _isModified;
            private set
            {
                _isModified = value;
                // プロパティが変更されたら、タイトルバーの更新などをイベントで通知
            }
        }

        public string CurrentFilePath
        {
            get => _currentFilePath;
            private set
            {
                _currentFilePath = value;
                FilePathChanged?.Invoke(this, value); // C#のnull条件演算子 ?. 
            }
        }

        // C#のevent：イベント通知の仕組み
        public event EventHandler<string> ContentChanged;
        public event EventHandler<string> FilePathChanged;

        // コンストラクタ：依存性注入（DI）でサービスを受け取る
        public DocumentService(IDocumentModel model, IFileService fileService)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public void NewDocument()
        {
            _model.CreateNew();
            CurrentFilePath = null;
            IsModified = false;

            // イベント発火：登録されているすべてのイベントハンドラを実行
            ContentChanged?.Invoke(this, _model.Content);
        }

        public bool OpenDocument()
        {
            var content = _fileService.OpenFile();
            if (content != null)
            {
                _model.Content = content;
                // TODO: ファイルパスの取得方法を改善
                IsModified = false;
                ContentChanged?.Invoke(this, content);
                return true;
            }
            return false;
        }

        public bool SaveDocument()
        {
            var savedPath = _fileService.SaveFile(_model.Content, CurrentFilePath);
            if (savedPath != null)
            {
                CurrentFilePath = savedPath;
                IsModified = false;
                return true;
            }
            return false;
        }

        public bool SaveDocumentAs()
        {
            var savedPath = _fileService.SaveFileAs(_model.Content);
            if (savedPath != null)
            {
                CurrentFilePath = savedPath;
                IsModified = false;
                return true;
            }
            return false;
        }

        public void UpdateContent(string content)
        {
            if (_model.Content != content)
            {
                _model.Content = content;
                IsModified = true;
                ContentChanged?.Invoke(this, content);
            }
        }
    }
}