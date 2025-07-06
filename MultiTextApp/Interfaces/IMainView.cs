using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Interfaces
{
    public interface IMainView
    {
        // プロパティ
        string DocumentText { get; set; }
        string WindowTitle { get; set; }

        // イベント
        event Action NewFileRequested;
        event Action OpenFileRequested;
        event Action SaveFileRequested;
        event Action SaveAsFileRequested;
        event Action ExitRequested;
        event Action<System.ComponentModel.CancelEventArgs> FormClosingRequested;

        // メソッド
        void ShowOpenFileDialog(List<IFileFormat> formats, out string filePath, out IFileFormat selectedFormat, out bool result);       // 開くダイアログを表示し、選択されたフォーマットとファイルパスを返す
        void ShowSaveFileDialog(List<IFileFormat> formats, out string filePath, out IFileFormat selectedFormat, out bool result);       // 保存ダイアログを表示し、選択されたフォーマットとファイルパスを返す
        void ShowMessage(string message);    // メッセージを表示
        void ShowError(string message);     // エラーメッセージを表示
        bool ShowConfirmDialog(string message);         // 確認ダイアログを表示し、ユーザーの応答を返す
        void CloseApplication();    // アプリケーションを閉じる

    }
}
