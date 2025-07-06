using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Interfaces
{
    public interface IFileFormat
    {
        string Extension { get; }
        string Description { get; }
        string Filter { get; }

        string Encode(string content);
        string Decode(string content);



    }
}
