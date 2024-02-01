using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;


namespace InfiniteCraftAutomation
{
    public partial class InfiniteCraftAutomationForm : Form
    {
        WebView2 webBrowser = new WebView2();
        StatusStrip statusStrip = new StatusStrip();
        ToolStripStatusLabel toolStripStatusLabel = new ToolStripStatusLabel();

        Thread automationThread = null;

        int totalElements;
        int firstElement = 0;
        int secondElement = 0;
        Dictionary<int, int> maxElementReachedForElement = new Dictionary<int, int>();

        public InfiniteCraftAutomationForm()
        {
            InitializeComponent();
            SetupBrowser();
        }

        private void SetupBrowser()
        {
            toolStripStatusLabel.Text = "Starting New Game...";
            statusStrip.Items.Add(toolStripStatusLabel);
            this.Controls.Add(statusStrip);

            webBrowser.CoreWebView2InitializationCompleted += WebBrowser_CoreWebView2InitializationCompleted;
            webBrowser.NavigationCompleted += WebBrowser_NavigationCompleted;            

            webBrowser.Dock = DockStyle.Fill;
            webBrowser.Source = new Uri("https://neal.fun/infinite-craft/");
            this.Controls.Add(webBrowser);

            this.FormClosing += InfiniteCraftAutomationForm_FormClosing;
        }

        private void InfiniteCraftAutomationForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            automationThread?.Interrupt();
        }

        private void WebBrowser_CoreWebView2InitializationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2InitializationCompletedEventArgs e)
        {
            webBrowser.CoreWebView2.IsMuted = true;

            webBrowser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(
                "function countElementsWithClass(className) {" +
                "    return document.getElementsByClassName(className).length;" +
                "}");
        }

        private void WebBrowser_NavigationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            toolStripStatusLabel.Text = "Page Loaded, Setting up Automation...";

            automationThread = new Thread(() =>
            {
                try
                {
                    for (int i = 3; i > 0; i--)
                    {
                        toolStripStatusLabel.Text = "Automation Setup completed, Playing Game in " + i + "...";
                        Thread.Sleep(1000);
                    }

                    PlayGame();
                }
                catch { }
                
            });

            automationThread.Start();
        }

        private void PlayGame()
        {
            Exception exceptionThrown = null;
            while (exceptionThrown == null)
            {
                UpdateElementCount()
                .ContinueWith(x =>
                {
                    if(x.Exception == null)
                    {
                        Invoke(() => { toolStripStatusLabel.Text = totalElements + " element(s) found."; });
                    }                    
                    else
                    {
                        exceptionThrown = x.Exception;
                    }
                })
                .ContinueWith(x =>
                {
                    if (x.Exception == null)
                    {
                        Invoke(() => { toolStripStatusLabel.Text += "Trying out " + firstElement + " with " + secondElement; });                        
                        Combine(firstElement, secondElement);
                        Thread.Sleep(30);
                    }
                    else
                    {
                        exceptionThrown = x.Exception;
                    }
                }).ContinueWith(x =>
                {
                    if (x.Exception == null)
                    {
                        secondElement = (secondElement + 1) % totalElements;

                        if (secondElement == 0)
                        {
                            if(maxElementReachedForElement.ContainsKey(firstElement))
                            {
                                maxElementReachedForElement[firstElement] = totalElements;
                            }
                            else
                            {
                                maxElementReachedForElement.Add(firstElement, totalElements);
                            }                            

                            if(maxElementReachedForElement.Keys.Any(item => maxElementReachedForElement[item] < totalElements))
                            {
                                var prevStart = maxElementReachedForElement.Keys.First(item => maxElementReachedForElement[item] < totalElements);
                                firstElement = prevStart;
                                secondElement = maxElementReachedForElement[prevStart]; // Start from previous end
                            }
                            else
                            {
                                firstElement = (firstElement + 1) % totalElements;
                                secondElement = firstElement; // No need to repeat the previous combinations.
                            }                                                        
                        }
                    }
                    else
                    {
                        exceptionThrown = x.Exception;
                    }
                }).Wait();
            }

            MessageBox.Show(exceptionThrown.Message);
        }

        private Task UpdateElementCount()
        {                        
            return Invoke(() => 
            webBrowser.CoreWebView2.ExecuteScriptAsync("countElementsWithClass('mobile-item')")
            .ContinueWith(x =>
            {                
                totalElements = JsonSerializer.Deserialize<Int32>(x.Result); 
            }));
        }

        private Task Combine(int elementIdx1, int elementIdx2)
        {
            return Invoke(() =>
            webBrowser.CoreWebView2.ExecuteScriptAsync(
                "document.getElementsByClassName('mobile-item')[" + elementIdx1 + "].getElementsByClassName('item')[0].click();" +
                "document.getElementsByClassName('mobile-item')[" + elementIdx2 + "].getElementsByClassName('item')[0].click();" 
            ));
        }
    }
}
