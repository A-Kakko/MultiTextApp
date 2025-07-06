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
        public string Content { get; set; } = "";
        public string FilePath { get; set; } = "";
        public bool IsModified { get; set; } = false;
        public IFileFormat CurrentFormat { get; set; } = null;
        private IFileFormat _defaultFormat;

        public DocumentModel()
        {
            CurrentFormat = new TxtFormat(); // デフォルトのフォーマットを設定
        }

        public void SetDefaultFormat(IFileFormat defaultFormat)
        {
            // デフォルトのフォーマットを設定
            _defaultFormat = defaultFormat;
            if(CurrentFormat == null)
            {
                CurrentFormat = defaultFormat; // 初期状態ではデフォルトフォーマットを使用
            }
        }
        // 新規作成
        public void CreateNew()
        {
            Content = "";
            FilePath = "";
            IsModified = false;
            CurrentFormat = _defaultFormat; // 新規作成時はデフォルトのフォーマットを使用
        }
        // ファイルから読み込み
        public void LoadFromFile(string filePath, IFileFormat format)
        {
            // ファイルからコンテンツを読み込み、プロパティに設定
            string fileContent = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
            Content = format.Decode(System.IO.File.ReadAllText(filePath));
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
            IsModified = false;
        }

        public string GetFileName()
        {
            // ファイルパスからファイル名を取得
            return System.IO.Path.GetFileName(FilePath);
        }

        public List<IFileFormat> GetSupportedFormats()
        {
            // サポートされているフォーマットのリストを返す
            return new List<IFileFormat>
            {
                new TxtFormat(),
                // 他のフォーマットを追加することも可能
            };
        }
    }
}
