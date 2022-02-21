using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Windows.Forms;

namespace StavangerTideRange
{
    public partial class Form1 : Form
    {
        HtmlWeb web = new HtmlWeb();
        HtmlAgilityPack.HtmlDocument document;
        FirefoxOptions options;
        FirefoxDriver firefox;
        string temp;
        Thread thread;

        public Form1()
        {
            InitializeComponent();
            web.UseCookies = true;
            SetUpBrowser();
            temp = "1";
            thread = new Thread(GetData);
            thread.Start();
        }

        void GetData()
        {
            document = new HtmlAgilityPack.HtmlDocument();
            firefox.Navigate().GoToUrl("https://www.tide-forecast.com/locations/Stavanger-Norway/tides/latest");
            document.LoadHtml(firefox.PageSource);

            string Time = document.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/section[1]/div/div[3]/div[3]/div[3]/div[2]/span/span")[0].InnerText;
            string Now = document.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/section[1]/div/div[3]/div[2]/div[4]/div/div[1]/span/span")[0].InnerText;
            string NextLowTide = document.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/section[1]/div/div[3]/div[3]/div[2]/div/div[2]/div/b")[0].InnerText;
            string NextHighTide = document.DocumentNode.SelectNodes("/html/body/main/div[3]/div[1]/section[1]/div/div[3]/div[3]/div[2]/div/div[1]/div/b")[0].InnerText;
            string LastUpdate = "Last Update: " + DateTime.Now.ToString();
            temp = "Time: " + Time + Environment.NewLine + "In this Moment: " + Now + Environment.NewLine + "Next low Tide: " + NextLowTide + Environment.NewLine + "Next high Tide: " + NextHighTide + Environment.NewLine + LastUpdate;
        }

        void SetUpBrowser()
        {
            options = new FirefoxOptions
            {
                BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe"
            };

            options.AddArguments("--headless");

            firefox = new FirefoxDriver(options);
        }

        public string GetHtml()
        {
            firefox.Navigate().GoToUrl("https://www.tide-forecast.com/locations/Stavanger-Norway/tides/latest");

            return firefox.PageSource;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = temp;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (thread.IsAlive == false)
            {
                thread = new Thread(GetData);
            thread.Start();
            }
        }
    }
}
