using MultiTextApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Models
{
    internal class DocumentModel : IDocumentModel
    {
        public string Content { get; set; } = "";

        public void CreateNew()
        {
            Content = "";
        }
    }
}
