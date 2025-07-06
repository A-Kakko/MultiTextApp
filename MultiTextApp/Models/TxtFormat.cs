using MultiTextApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Models
{
    public class TxtFormat : IFileFormat
    {
        public string Extension => ".txt";
        public string Description => "テキストファイル";
        public string Filter => "テキストファイル (*.txt)|*.txt|すべてのファイル (*.*)|*.*";
        public string Encode(string content)
        {
            return content; // テキストファイルはそのまま
        }
        public string Decode(string content)
        {
            return content; // テキストファイルはそのまま
        }
    }
}
