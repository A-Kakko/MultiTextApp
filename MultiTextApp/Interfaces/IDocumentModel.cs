using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Interfaces
{
    internal interface IDocumentModel
    {
        string Content { get; set; }
        void CreateNew();
    }
}
