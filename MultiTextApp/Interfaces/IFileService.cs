using System;

namespace MultiTextApp.Interfaces
{
    /// <summary>
    /// ファイル操作を担当するサービスのInterface
    /// </summary>
    internal interface IFileService
    {
        /// <summary>
        /// ファイルを開く
        /// </summary>
        /// <returns>ファイルの内容。キャンセルされた場合はnull</returns>
        string OpenFile();

        /// <summary>
        /// ファイルに保存
        /// </summary>
        /// <param name="content">保存する内容</param>
        /// <param name="filePath">ファイルパス（nullの場合は名前を付けて保存）</param>
        /// <returns>保存されたファイルパス</returns>
        string SaveFile(string content, string filePath = null);

        /// <summary>
        /// 名前を付けて保存
        /// </summary>
        /// <param name="content">保存する内容</param>
        /// <returns>保存されたファイルパス。キャンセルされた場合はnull</returns>
        string SaveFileAs(string content);
    }
}