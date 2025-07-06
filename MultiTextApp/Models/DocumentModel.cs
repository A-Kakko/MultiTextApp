using MultiTextApp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Models
{
    internal class DocumentModel : IDocumentModel
    {

        private IFileFormat _defaultFormat;

        private string _originalContent = "";
        private string _content = "";

        // コンテンツのプロパティ
        public string Content
        {
            get => _content;
            set
            {
                if (_content != value)
                {
                    _content = value;
                    IsModified = true; // コンテンツが変更された場合はIsModifiedをtrueに設定
                }
            }
        }

        public string FilePath { get; set; } = "";
        public bool IsModified { get; private set; } = false;
        public IFileFormat CurrentFormat { get; set; } = null;


        //変更状態を更新
        private void UpdateModificationState()
        {
            if (CurrentFormat != null)
            {
                IsModified = _content != _originalContent; // 現在のコンテンツと元のコンテンツを比較
            }
        }

        public DocumentModel()
        {
            CurrentFormat = new TxtFormat(); // デフォルトのフォーマットを設定
        }

        // 新規作成
        public void CreateNew()
        {
            Content = "";
            FilePath = "";
            IsModified = false;
            CurrentFormat = _defaultFormat; // 新規作成時はデフォルトのフォーマットを使用
        }

        //変更状態を更新

        // ファイルから読み込み
        public void LoadFromFile(string filePath, IFileFormat format)
        {
            // ファイルからコンテンツを読み込み、プロパティに設定
            string fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
            _content = format.Decode(System.IO.File.ReadAllText(filePath));
            _originalContent = _content; // 元のコンテンツを保存
            FilePath = filePath;
            IsModified = false;
            CurrentFormat = format;
        }

        // ファイルに保存
        public void SaveToFile(string filePath, IFileFormat format)
        {
            string encodedContent = format.Encode(Content);  // フォーマット固有の変換
            File.WriteAllText(filePath, encodedContent, Encoding.UTF8);
            FilePath = filePath;
            CurrentFormat = format;
            _originalContent = _content; // 保存後に元のコンテンツを更新
            IsModified = false;
        }

        // ファイル名を取得
        public string GetFileName()
        {
            return string.IsNullOrEmpty(FilePath) ? "無題" : Path.GetFileName(FilePath);
        }

        // サポートされているフォーマットのリストを取得
        public List<IFileFormat> GetSupportedFormats()
        {
            return new List<IFileFormat>
            {
                new TxtFormat(),
                // 他のフォーマットを追加することも可能
            };
        }

        // デフォルトのフォーマットを設定
        public void SetDefaultFormat(IFileFormat defaultFormat)
        {
            _defaultFormat = defaultFormat;
            if (CurrentFormat == null)
            {
                CurrentFormat = defaultFormat; // 初期状態ではデフォルトフォーマットを使用
            }
        }
    }
}
