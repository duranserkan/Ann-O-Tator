using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Annotator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BrowserBackButton.IsEnabled = BrowserWebView.CanGoBack;
            BrowserForwardButton.IsEnabled = BrowserWebView.CanGoForward;
        }

        private void BrowserBackButton_OnClicked(object sender, EventArgs e)
        {
            BrowserWebView.GoBack(); ;
        }

        private void BrowserForwardButton_OnClicked(object sender, EventArgs e)
        {
            BrowserWebView.GoForward();
        }

        private void BrowserGoButton_OnClicked(object sender, EventArgs e)
        {
            var isURlValid = Uri.TryCreate(BrowserAddressBar.Text, UriKind.Absolute, out var _);
            if (isURlValid)
            {
                BrowserWebView.Source = BrowserAddressBar.Text;
            }
            else
            {
                string searchURL = "http://www.google.com/search?q=";
                BrowserWebView.Source = searchURL + BrowserAddressBar.Text.Replace(' ', '+');
            }
        }

        private void BrowserWebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            string websiteAddress = e.Url;
            BrowserAddressBar.Text = websiteAddress;
        }

        private void BrowserWebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            BrowserBackButton.IsEnabled = BrowserWebView.CanGoBack;
            BrowserForwardButton.IsEnabled = BrowserWebView.CanGoForward;
            var script =
                "var annotatorApihead = document.getElementsByTagName('head')[0];" + Environment.NewLine +
                "var annotatorApiScript = document.createElement('script');" + Environment.NewLine +
                "annotatorApiScript.src = 'http://localhost:54520/js/annotator-api.js';" + Environment.NewLine +
                "annotatorApiScript.type = 'text/javascript';" + Environment.NewLine +
                "annotatorApihead.appendChild(annotatorApiScript);";
            BrowserWebView.Eval(script);
        }
    }
}
