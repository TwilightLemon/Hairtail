using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hairtail
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mp.MediaEnded += delegate {
                mp.Close();
            };
        }
        public static string PostWeb(string url, string data)
        {
            byte[] postData = Encoding.UTF8.GetBytes(data);
            WebClient webClient = new WebClient();
            webClient.Headers.Add("Accept", "*/*");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            webClient.Headers.Add("Cookie", "BAIDUID=CF1A84B8FDD881260D93DECF5A8A33C3:FG=1; BIDUPSID=CF1A84B8FDD881260D93DECF5A8A33C3; PSTM=1549463409; MCITY=-41%3A; H_PS_PSSID=1459_21119_28414; BDORZ=FFFB88E999055A3F8A630C64834BD6D0; delPer=0; PSINO=7; BDSFRCVID=v-KsJeCCxG3JyDT9jc-uynbPAZc9k64iSmQ73J; H_BDCLCKID_SF=tR3KB6rtKRTffjrnhPF35CuUKP6-3MJO3b7ZXlbmfRcjKt5wh45mLqISef4qtPcgaGCfohFLK-oj-DDwDjK23J; Hm_lvt_8b973192450250dd85b9011320b455ba=1550309932; Hm_lpvt_8b973192450250dd85b9011320b455ba=1550309932; seccode=0d31296fec64f8e311278ce31edf2b25");
            webClient.Headers.Add("Host", "ai.baidu.com");
            webClient.Headers.Add("Origin", "http://ai.baidu.com");
            webClient.Headers.Add("Referer", "http://ai.baidu.com/tech/speech/tts?track=cp:aipinzhuan|pf:pc|pp:AIpingtai|pu:1-3|ci:|kw:10005801");
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
            byte[] responseData = webClient.UploadData(url, "POST", postData);
            webClient.Dispose();
            return Encoding.UTF8.GetString(responseData);
        }
        MediaPlayer mp = new MediaPlayer();
        string per = "1";//1,0,3,4
        string spd = "5";//语速
        string pit = "5";//音调
        string vol = "5";//音量
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var data = PostWeb("http://ai.baidu.com/aidemo", 
                $"type=tns&spd={spd}&pit={pit}&vol={vol}&per={per}&tex="+ System.Web.HttpUtility.HtmlEncode(tb.Text));
            JObject o = JObject.Parse(data);
            var dt = o["data"].ToString().Replace("data:audio/x-mpeg;base64,", "");
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\qwq.mp3";
            Base64ToOriFile(dt, path);
            mp.Open(new Uri(path, UriKind.Absolute));
            mp.Play();
        }
        public void Base64ToOriFile(string base64Str, string outPath)
        {
            var contents = Convert.FromBase64String(base64Str);
            using (var fs = new FileStream(outPath, FileMode.Create, FileAccess.Write))
            {
                fs.Write(contents, 0, contents.Length);
                fs.Flush();
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as RadioButton;
            if ((bool)cb.IsChecked) {
                if (cb.Content.ToString() == "普通男生")
                    per = "1";
                else if (cb.Content.ToString() == "普通女生")
                    per = "0";
                else if (cb.Content.ToString() == "度逍遥(武侠)")
                    per = "3";
                else if (cb.Content.ToString() == "度丫丫(软萌)")
                    per = "4";
            }
        }

        private void Spd_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            spd = ((int)spd_sd.Value).ToString();
            spd_tb.Text = "x" + spd;
        }

        private void Pit_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            pit= ((int)pit_sd.Value).ToString();
            pit_tb.Text = "x" + pit;
        }

        private void Vol_sd_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            vol= ((int)vol_sd.Value).ToString();
            vol_tb.Text = "x" + vol;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            spd_sd.ValueChanged += Spd_sd_ValueChanged;
            pit_sd.ValueChanged += Pit_sd_ValueChanged;
            vol_sd.ValueChanged += Vol_sd_ValueChanged;
        }
    }
}
