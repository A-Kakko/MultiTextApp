using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MultiTextApp.Views;
using MultiTextApp.Models;
using MultiTextApp.Interfaces;
using MultiTextApp.Presenters;

namespace MultiTextApp
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(ApplicationFactory.CreateApplication()); // ← Factory
        }
    }
}