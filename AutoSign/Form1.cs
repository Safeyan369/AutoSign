using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using zoyobar.shared.panzer;
using zoyobar.shared.panzer.web;
using zoyobar.shared.panzer.web.ib;
using System.Threading;

namespace AutoSign
{
    public partial class AutoSign : Form
    {
        public AutoSign()
        {
            InitializeComponent();
            this.CenterToScreen();
            wb.ScriptErrorsSuppressed = true;
        }

        //起始网址
        string baidu_wapp_favbar = "http://wapp.baidu.com/m?tn=bdFBW";

        //----------------------------------------------------------------------------------------------------------------

        //定义委托
        public delegate string GetInnerHtmlDelegate();

        //返回网页源代码
        private string GetInnerHtml()
        {
            return wb.Document.Body.InnerHtml;
        }

        //判断网页是否打开
        private void OpenIndexPage()
        {
            //IEBrowser
            IEBrowser ie = new IEBrowser(wb);
            try
            {
                ie.Navigate(baidu_wapp_favbar);
                ie.IEFlow.Wait(new UrlCondition("wait", baidu_wapp_favbar, StringCompareMode.StartWith), 10);

                GetInnerHtmlDelegate gih = new GetInnerHtmlDelegate(GetInnerHtml);
                string innerhtml = this.Invoke(gih).ToString();
                Work();
            }
            catch (NullReferenceException)
            {
                OpenIndexPage();
            }
            catch (TimeoutException)
            {
                MessageBox.Show("网页打开超时，请检查网络环境，并重试。");
            }
        }

        //----------------------------------------------------------------------------------------------------------------

        //定义委托
        public delegate void SignAllDelegate();

        //工作流程
        private void SignAll()
        {
            //IEBrowser
            IEBrowser ie = new IEBrowser(wb);

            //JQUERY统计链接数
            ie.InstallJQuery(JQuery.CodeMin);
            ie.ExecuteJQuery(JQuery.Create("'a'"), "__jBs");
            int fav_count = ie.ExecuteJQuery<int>(JQuery.Create("__jBs").Length());

            //将我喜欢的吧放到List
            List<string> fav_bar = new List<string>();
            for (int index = 0; index < fav_count; index++)
            {
                ie.ExecuteJQuery(JQuery.Create("__jBs").Eq(index.ToString()), "__jB");
                string fav_text = ie.ExecuteJQuery<string>(JQuery.Create("__jB").Text());
                if (fav_text != "发言记录" && fav_text != "贴吧" && fav_text != "百度")
                {
                    string fav_url = "http://wapp.baidu.com/m?kw=" + UrlEncode(fav_text);
                    fav_bar.Add(fav_url);
                }
            }

            //历遍每个吧，查找签到的链接URL，打开。
            int num = 0;
            foreach (string bar_url in fav_bar)
            {
                num++;
                tips.Text = "签到进度：" + num + "/" + fav_bar.Count;

                ie.Navigate(bar_url);
                ie.IEFlow.Wait(new UrlCondition("wait", bar_url, StringCompareMode.StartWith), 10);

                ie.InstallJQuery(JQuery.CodeMin);
                ie.ExecuteJQuery(JQuery.Create("'a'"), "__jCs");
                int sign_count = ie.ExecuteJQuery<int>(JQuery.Create("__jCs").Length());
                for (int index = 0; index < sign_count; index++)
                {
                    ie.ExecuteJQuery(JQuery.Create("__jCs").Eq(index.ToString()), "__jC");
                    string sign_text = ie.ExecuteJQuery<string>(JQuery.Create("__jC").Text());
                    string sign_url = "http://wapp.baidu.com" + ie.ExecuteJQuery<string>(JQuery.Create("__jC").Attr("'href'"));

                    if (sign_text == "签到")
                    {
                        ie.Navigate(sign_url);
                        ie.IEFlow.Wait(new UrlCondition("wait", sign_url, StringCompareMode.StartWith));
                    }
                }
            }
            if (num != 0)
            {
                Environment.Exit(0);
            }
        }

        //利用委托进行
        private void Work()
        {
            //IEBrowser
            IEBrowser ie = new IEBrowser(wb);
            try
            {
                SignAllDelegate sa = new SignAllDelegate(SignAll);
                this.Invoke(sa);
            }
            catch (TimeoutException)
            {
                MessageBox.Show("网页打开超时，请检查网络环境，并重试。");
            }
        }

        //----------------------------------------------------------------------------------------------------------------

        //开始运行
        private void AutoSign_Load(object sender, EventArgs e)
        {
            tips.Text = "准备签到...";
            Thread go_therad = new Thread(new ThreadStart(this.OpenIndexPage));
            go_therad.IsBackground = true;
            go_therad.Start();
        }

        //辅助函数
        private static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }
    }
}
