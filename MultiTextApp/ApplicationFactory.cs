using MultiTextApp.Models;
using MultiTextApp.Presenters;
using MultiTextApp.Services;
using MultiTextApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp
{
    public static class ApplicationFactory
    {
        internal static MainForm CreateApplication()
        {
            var fileService = new FileService();
            var model = new DocumentModel();
            model.SetDefaultFormat(new TxtFormat());

            var view = new MainForm();
            var presenter = new MainPresenter(view, model, fileService);

            // コンストラクタで渡す
            return new MainForm(presenter);
        }
    }
}