using Avalonia.Controls;
using Task1.ViewModels;

namespace Task1;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }
}