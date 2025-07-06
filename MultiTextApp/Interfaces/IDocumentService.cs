using System;

namespace MultiTextApp.Interfaces
{
    /// <summary>
    /// ドキュメント操作を担当するサービスのInterface
    /// </summary>
    internal interface IDocumentService
    {
        /// <summary>
        /// 文書の変更状態
        /// </summary>
        bool IsModified { get; }

        /// <summary>
        /// 現在のファイルパス
        /// </summary>
        string CurrentFilePath { get; }

        /// <summary>
        /// 新規文書を作成
        /// </summary>
        void NewDocument();

        /// <summary>
        /// 文書を開く
        /// </summary>
        /// <returns>成功した場合true</returns>
        bool OpenDocument();

        /// <summary>
        /// 文書を保存
        /// </summary>
        /// <returns>成功した場合true</returns>
        bool SaveDocument();

        /// <summary>
        /// 名前を付けて保存
        /// </summary>
        /// <returns>成功した場合true</returns>
        bool SaveDocumentAs();

        /// <summary>
        /// 文書の内容を更新
        /// </summary>
        /// <param name="content">新しい内容</param>
        void UpdateContent(string content);

        /// <summary>
        /// 文書の内容変更イベント
        /// </summary>
        event EventHandler<string> ContentChanged;

        /// <summary>
        /// ファイルパス変更イベント
        /// </summary>
        event EventHandler<string> FilePathChanged;
    }
}
