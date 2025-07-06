using MultiTextApp.Interfaces;
using System.IO;
using System.Windows.Forms;

namespace MultiTextApp.Services
{
    /// <summary>
    /// ファイル操作を担当するサービス
    /// </summary>
    internal class FileService : IFileService
    {
        public string OpenFile()
        {
            // ブロックを抜ける時に自動的にDispose()が呼ばれる
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*";
                openFileDialog.Title = "ファイルを開く";

                // DialogResult：enum（列挙型）でダイアログの結果を表現
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // File.ReadAllText：静的メソッドでファイル全体を読み込み
                        return File.ReadAllText(openFileDialog.FileName);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"ファイルを開けませんでした。\n{ex.Message}",
                                      "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
                return null;
            }
        }

        public string SaveFile(string content, string filePath = null)
        {
            // C#のnull合体演算子 ??：filePath が null の場合は SaveFileAs を実行
            if (filePath == null)
            {
                return SaveFileAs(content);
            }

            try
            {
                File.WriteAllText(filePath, content);
                return filePath;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"ファイルを保存できませんでした。\n{ex.Message}",
                              "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public string SaveFileAs(string content)
        {
            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*";
                saveFileDialog.Title = "名前を付けて保存";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, content);
                        return saveFileDialog.FileName;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show($"ファイルを保存できませんでした。\n{ex.Message}",
                                      "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                return null;
            }
        }

        public string ReadFile(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"ファイルを読み込めませんでした。\n{ex.Message}",
                              "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
    }
}
