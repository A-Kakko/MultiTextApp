using MultiTextApp.Interfaces;
using MultiTextApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTextApp.Presenters
{
    internal class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IDocumentModel _model;

        // DI：両方とも外から注入
        public MainPresenter(IMainView view, IDocumentModel model)
        {
            _view = view;
            _model = model;  

            UpdateView();
        }

        private void UpdateView()
        {
            _view.DocumentText = _model.Content;
        }
    }
}