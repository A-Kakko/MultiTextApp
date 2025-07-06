using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiTextApp.Presenters;
using MultiTextApp.Interfaces;
using MultiTextApp.Models;


namespace MultiTextApp.Views
{
    public partial class MainForm : Form, IMainView
    {
        private readonly MainPresenter _presenter;

        public string DocumentText
        {
            get => textBox1.Text;
            set => textBox1.Text = value;
        }
        public MainForm()
        {
            InitializeComponent();

            DocumentModel model = new DocumentModel();  
            _presenter = new MainPresenter(this, model);

        }
    }
}
