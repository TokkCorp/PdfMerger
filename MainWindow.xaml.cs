using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PdfMerger
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel viewModel;
        public MainWindowViewModel ViewModel
        {
            get
            {
                if(viewModel == null)
                {
                    viewModel = new MainWindowViewModel();
                }
                return viewModel;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddPdf();
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemovePdf();
        }

        private void ButtonMoveUp_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MoveUp();
        }

        private void ButtonMoveDown_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MoveDown();
        }

        private void ButtonCreatePdf_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CreatePdf();
        }
    }
}
