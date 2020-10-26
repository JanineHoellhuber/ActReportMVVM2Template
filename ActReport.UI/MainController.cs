using ActReport.ViewModel;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace ActReport.UI
{
    class MainController :IController
    {
        public void ShowWindow(BaseViewModel viewModel)
        {
            Window window = viewModel switch
            {
                null => throw new ArgumentException(nameof(viewModel)),

                EmployeeViewModel _ => new MainWindow(),
                ActivityViewModel _ => new ActivityWindow(),

                _ => throw new InvalidOperationException($"Unkonwn ViewModel of type '{viewModel}'"),
            };

            window.DataContext = viewModel;

            window.ShowDialog();
        }
    }
}
