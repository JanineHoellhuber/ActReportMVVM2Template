using ActReport.ViewModel;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace ActReport.UI
{
    class MainController : IController
    {
        private Dictionary<BaseViewModel, Window> _windows;

        public MainController()
        {
            _windows = new Dictionary<BaseViewModel, Window>();
        }
        public void ShowWindow(BaseViewModel viewModel)
        {
            Window window = viewModel switch
            {
                null => throw new ArgumentException(nameof(viewModel)),

                EmployeeViewModel _ => new MainWindow(),
                ActivityViewModel _ => new ActivityWindow(),
                ActivityNewAndEditModel _ => new NewAndEditWindow(),

                _ => throw new InvalidOperationException($"Unkonwn ViewModel of type '{viewModel}'"),
            };
            _windows[viewModel] = window;
            window.DataContext = viewModel;

            window.ShowDialog();
        }

        public void CloseWindow(BaseViewModel viewModel)
        {
            if(_windows.ContainsKey(viewModel))
            {
                _windows[viewModel].Close();
                _windows.Remove(viewModel);
            }
        }
    }
}
