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

namespace AutoSign
{
    public partial class AutoSign : Form
    {
        public AutoSign()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        /// <summary>
        /// 中文转网页编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 判断网页是否打开
        /// </summary>
        /// <param name="ie"></param>
        /// <returns></returns>
        private bool OpenSuccess(IEBrowser ie)
        {
            try
            {
                string a = ie.Document.Body.InnerHtml;
                return true;
            }
            catch (NullReferenceException)
            {
                return false;
            }
        }

        /// <summary>
        /// 工作流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_Click(object sender, EventArgs e)
        {
            tips.Text = "准备签到...";

            //起始网址
            string baidu_wapp_favbar = "http://wapp.baidu.com/m?tn=bdFBW";

            //IEBrowser
            IEBrowser ie = new IEBrowser(wb);

            do
            {
                try
                {
                    ie.Navigate(baidu_wapp_favbar);
                    ie.IEFlow.Wait(new UrlCondition("wait", baidu_wapp_favbar, StringCompareMode.StartWith), 10);
                }
                catch (TimeoutException)
                {
                    MessageBox.Show("网页打开超时，请重试");
                }
            } while (OpenSuccess(ie) == false);

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
                tips.Text = "进度：" + num + "/" + fav_bar.Count;

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
                MessageBox.Show("签到完毕");
                Environment.Exit(0);
            }
        }
    }
}
