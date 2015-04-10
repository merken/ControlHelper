using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Merken.WinRT.Util
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder builder = new StringBuilder();
            var element = sender as FrameworkElement;
            var parentPage = ControlHelper.GetParentPage(element);
            var resultsHubSection = ControlHelper.FindChild<HubSection>(parentPage, hs => hs.Header.ToString() == "Results section");
            var results = ControlHelper.FindChild<TextBlock>(resultsHubSection, tb => tb.Height == 500);

            //Resize the tjardis
            var hub = ControlHelper.FindSibling<Hub>(element, h => h.Visibility == Windows.UI.Xaml.Visibility.Visible);
            var image = ControlHelper.FindChild<Image>(hub);
            var allImages = ControlHelper.FindChildren<Image>(parentPage);
            image.Stretch = Stretch.None;
            builder.AppendFormat("Tardis altered, found one other image : {0}", allImages.FirstOrDefault(i => i != image).Name);

            //Sibling test
            var siblingTest = ControlHelper.FindChild<TextBlock>(parentPage, hs => hs.Name == "SiblingTest");
            var siblings = ControlHelper.FindSiblings<TextBlock>(siblingTest);
            foreach (var sibling in siblings)
                sibling.Text = "Sibling!";
            builder.AppendFormat("\nAltered siblings : {0}", siblings.Count());

            //Child test
            var layoutRoot = ControlHelper.FindParent<Grid>(element, g => g.Name == "LayoutRoot");
            var firstHub = ControlHelper.Child<Hub>(layoutRoot);
            builder.AppendFormat("\nThe first hub on the LayoutRoot is called : {0}", firstHub.Header);

            results.Text = builder.ToString();
        }
    }
}
