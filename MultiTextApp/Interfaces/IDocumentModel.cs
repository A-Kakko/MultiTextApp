using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Interfaces
{
    public interface IDocumentModel
    {
        string Content { get; set; }
        string FilePath { get; set; }
        bool IsModified { get; set; }
        IFileFormat CurrentFormat { get; set; }

        void CreateNew();
        void LoadFromFile(string filePath, IFileFormat format);
        void SaveToFile(string filePath, IFileFormat format);
        string GetFileName();
        List<IFileFormat> GetSupportedFormats();
        void SetDefaultFormat(IFileFormat defaultFormat);
    }
}
