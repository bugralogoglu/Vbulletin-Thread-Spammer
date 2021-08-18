using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int sayac = 0;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );
        public Form1()
        {
            InitializeComponent();
            
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.CenterToScreen();
        }

        private void bunifuButton1_Click(object sender, EventArgs e)
        {
            
            if (bunifuButton1.Text == "Start Spam")
            {
                
                if (bunifuTextBox1.Text != "" && bunifuTextBox2.Text != "" && bunifuTextBox3.Text != "" && bunifuTextBox4.Text != "")
                {
                    bunifuButton1.Text = "Stop Spam";
                    timer1.Enabled = true;
                    timer2.Enabled = true;
                    bunifuCircleProgress1.Value = bunifuCircleProgress1.Maximum;
                    bunifuDropdown1.Enabled = false;
                    bunifuTextBox1.Enabled = false;
                    bunifuTextBox2.Enabled = false;
                    bunifuTextBox3.Enabled = false;
                    bunifuTextBox4.Enabled = false;
                }
                else
                {
                    bunifuLabel6.Text = "Lütfen bilgileri eksiksiz girin";
                }

            }
            else
            {
                sayac = 0;
                bunifuButton1.Text = "Start Spam";
                timer1.Enabled = false;
                timer2.Enabled = false;
                bunifuDropdown1.Enabled = true;
                bunifuTextBox1.Enabled = true;
                bunifuTextBox2.Enabled = true;
                bunifuTextBox3.Enabled = true;
                bunifuTextBox4.Enabled = true;
                timer1.Interval = int.Parse(bunifuDropdown1.Text) * 60000;
                bunifuCircleProgress1.Maximum = int.Parse(bunifuDropdown1.Text) * 60;
                bunifuCircleProgress1.Value = int.Parse(bunifuDropdown1.Text) * 60;
            }

        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            sayac += 1;
            bunifuLabel6.Text = "Siteye bağlanılıyor";
            var chromeOptions = new ChromeOptions();
            var driverService = ChromeDriverService.CreateDefaultService();
            driverService.HideCommandPromptWindow = true;
            chromeOptions.AddArgument("--disable-javascript");
            chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
            chromeOptions.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");
            chromeOptions.AddArgument("--remote-debugging-port=9222");
            chromeOptions.AddArgument("--headless");
            var driver = new ChromeDriver(driverService, chromeOptions);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 6));
            driver.Url = bunifuTextBox1.Text;
            driver.FindElement(By.Id("navbar_username")).SendKeys(bunifuTextBox2.Text);
            driver.FindElement(By.Id("navbar_password")).SendKeys(bunifuTextBox3.Text + OpenQA.Selenium.Keys.Enter);
            wait.Until(webDriver => driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td/div/blockquote/p[3]/a")).Displayed);
            bunifuLabel6.Text = "Giriş başarılı";
            driver.FindElement(By.XPath("/html/body/form/table/tbody/tr[2]/td/div/blockquote/p[3]/a")).Click();
            driver.FindElement(By.XPath("/html/body/div[4]/div/div/form/table/tbody[2]/tr/td/div[1]/div/div[2]/div[2]/fieldset/textarea")).SendKeys(bunifuTextBox4.Text);
            driver.FindElement(By.XPath("/html/body/div[4]/div/div/form/table/tbody[2]/tr/td/div[2]/input[10]")).Click();
            bunifuLabel6.Text = sayac + "x Mesaj gönderildi";
            driver.Quit();
            timer1.Interval = int.Parse(bunifuDropdown1.Text) * 60000;
            bunifuCircleProgress1.Maximum = int.Parse(bunifuDropdown1.Text) * 60;
            bunifuCircleProgress1.Value = int.Parse(bunifuDropdown1.Text) * 60;
        }

        private void bunifuLabel6_Click(object sender, EventArgs e)
        {

        }

        private void bunifuPictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuDropdown1_SelectedIndexChanged(object sender, EventArgs e)
        {
            bunifuCircleProgress1.Maximum = int.Parse(bunifuDropdown1.Text) * 60;
            bunifuCircleProgress1.Value = int.Parse(bunifuDropdown1.Text) * 60;

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if(bunifuCircleProgress1.Value > 0)
            bunifuCircleProgress1.Value -= 1;
        }

        private void bunifuPictureBox2_Click(object sender, EventArgs e)
        {
            Hide();
            notifyIcon1.Visible = true;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            Show();
            notifyIcon1.Visible = false;
        }
    }
}
