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
        }

        private void start_Click(object sender, EventArgs e)
        {
            tips.Text = "准备签到...";

            string baidu_wapp = "http://wapp.baidu.com";

            IEBrowser ie = new IEBrowser(this.wb);
            ie.Navigate(baidu_wapp);
            ie.IEFlow.Wait(new UrlCondition("wait", baidu_wapp, StringCompareMode.StartWith), 10);

            ie.InstallJQuery(JQuery.CodeMin);
            ie.ExecuteJQuery(JQuery.Create("'a'"), "__jAs");
            int more_count = ie.ExecuteJQuery<int>(JQuery.Create("__jAs").Length());

            string href = "";
            for (int index = 0; index < more_count; index++)
            {
                ie.ExecuteJQuery(JQuery.Create("__jAs").Eq(index.ToString()), "__jA");
                if (ie.ExecuteJQuery<string>(JQuery.Create("__jA").Text()) == "更多>>")
                {
                    href = ie.ExecuteJQuery<string>(JQuery.Create("__jA").Attr("'href'"));
                }
            }

            if (href.Length > 0)
            {
                string[] more_url_array = href.Split('/');
                string pre_bar = baidu_wapp + "/" + more_url_array[1] + @"/" + more_url_array[2] + @"/";

                string my_favorite = baidu_wapp + href;
                ie.Navigate(my_favorite);
                ie.IEFlow.Wait(new UrlCondition("wait", my_favorite, StringCompareMode.StartWith));

                ie.InstallJQuery(JQuery.CodeMin);
                ie.ExecuteJQuery(JQuery.Create("'a'"), "__jBs");
                int fav_count = ie.ExecuteJQuery<int>(JQuery.Create("__jBs").Length());

                List<string> fav_bar = new List<string>();
                for (int index = 0; index < fav_count; index++)
                {
                    ie.ExecuteJQuery(JQuery.Create("__jBs").Eq(index.ToString()), "__jB");
                    string fav_text = ie.ExecuteJQuery<string>(JQuery.Create("__jB").Text());
                    string fav_url = pre_bar + ie.ExecuteJQuery<string>(JQuery.Create("__jB").Attr("'href'"));
                    if (fav_text != "发言记录" && fav_text != "贴吧" && fav_text != "百度")
                    {
                        fav_bar.Add(fav_url);
                    }
                }
                int num = 0;
                foreach (string bar_url in fav_bar)
                {
                    num++;
                    tips.Text = "进度：" + num + "/" + fav_bar.Count;

                    ie.Navigate(bar_url);
                    ie.IEFlow.Wait(new UrlCondition("wait", bar_url, StringCompareMode.StartWith));

                    ie.InstallJQuery(JQuery.CodeMin);
                    ie.ExecuteJQuery(JQuery.Create("'a'"), "__jCs");
                    int sign_count = ie.ExecuteJQuery<int>(JQuery.Create("__jCs").Length());
                    for (int index = 0; index < sign_count; index++)
                    {
                        ie.ExecuteJQuery(JQuery.Create("__jCs").Eq(index.ToString()), "__jC");
                        string sign_text = ie.ExecuteJQuery<string>(JQuery.Create("__jC").Text());
                        string sign_url = baidu_wapp + ie.ExecuteJQuery<string>(JQuery.Create("__jC").Attr("'href'"));
                        if (sign_text == "签到")
                        {
                            ie.Navigate(sign_url);
                            ie.IEFlow.Wait(new UrlCondition("wait", sign_url, StringCompareMode.StartWith));
                        }
                    }
                }
                tips.Text = fav_bar.Count + "签到完毕！";
            }
        }
    }
}
