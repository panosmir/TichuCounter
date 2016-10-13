using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TichuCounter.Model;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TichuCounter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstTimeView : Page
    {
        public FirstTimeView()
        {
            this.InitializeComponent();
            this.Loaded += Page_Loaded;

            hintsTextBlock.Text = "\n\n1)You can now enter the points of your team only and then it will auto complete for both teams.\n" +
                "2)If your team wins with 1-2 and tichu you can check it only, you don't have to enter points manually.\n" +
                "3)If your team wins with tichu or grand then you can enter the points of your team and check the tichu (or grant) and it will auto complete the correct points for both teams.";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var s = ApplicationView.GetForCurrentView();
            s.TryResizeView(new Size { Width = 450, Height = 450 });

        }

        private void closeTips(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
