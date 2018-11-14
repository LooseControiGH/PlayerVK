using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using WMPLib;
using player_Api.Properties;

namespace player_Api
{

    public partial class Form1 : Form
    {
        Rectangle myBounds;
        dataAccaunt dAcc;
        LoginAccaunt fLoginAccaunt;
        WindowsMediaPlayer wplayer = new WindowsMediaPlayer();

        List<string> durationSounds = new List<string>();
        List<string> urlSounds = new List<string>();
        List<string> titleSounds = new List<string>();
        List<string> playList = new List<string>();
        List<string> artist = new List<string>();

        public int status;
        public int number;
        public double duration;
        public int time;
        public int lastTime;

        public bool repetition;

        #region
        private enum Vklist
        {
            /// <summary>
            /// Пользователь разрешил отправлять ему уведомления. 
            /// </summary>
            notify = 1,
            /// <summary>
            /// Доступ к друзьям.
            /// </summary>
            friends = 2,
            /// <summary>
            /// Доступ к фотографиям. 
            /// </summary>
            photos = 4,
            /// <summary>
            /// Доступ к аудиозаписям. 
            /// </summary>
            audio = 8,
            /// <summary>
            /// Доступ к видеозаписям. 
            /// </summary>
            video = 16,
            /// <summary>
            /// Доступ к предложениям (устаревшие методы). 
            /// </summary>
            offers = 32,
            /// <summary>
            /// Доступ к вопросам (устаревшие методы). 
            /// </summary>
            questions = 64,
            /// <summary>
            /// Доступ к wiki-страницам. 
            /// </summary>
            pages = 128,
            /// <summary>
            /// Добавление ссылки на приложение в меню слева.
            /// </summary>
            link = 256,
            /// <summary>
            /// Доступ заметкам пользователя. 
            /// </summary>
            notes = 2048,
            /// <summary>
            /// (для Standalone-приложений) Доступ к расширенным методам работы с сообщениями. 
            /// </summary>
            messages = 4096,
            /// <summary>
            /// Доступ к обычным и расширенным методам работы со стеной. 
            /// </summary>
            wall = 8192,
            /// <summary>
            /// Доступ к документам пользователя.
            /// </summary>
            docs = 131072

        }
        static public int userId = 0;
        static public string accessToken = "";
        #endregion
        private int scope;
        public Form1()
        {
            InitializeComponent();


            scope = (int)(Vklist.audio | Vklist.docs | Vklist.friends | Vklist.link | Vklist.messages
               | Vklist.notes | Vklist.notify | Vklist.offers | Vklist.pages | Vklist.photos | Vklist.questions
               | Vklist.video | Vklist.wall);
            fLoginAccaunt = new LoginAccaunt(this);
            dAcc = new dataAccaunt();

            myBounds.Width = 20;
            number = 0;
            duration = 0.0;
            userId = 0;

            accessToken = "false";
            myScrollBar1.Value = 0;
            myScrollBar2.Maximum = 100;
            myScrollBar2.Value = 20;
            panel1.Height = 502;
            panel1.Width = 387;


            panel1.BackgroundImage = Resources.backgraund;
            setButtonsOption();
        }

        public void loadAndParseXml()
        {
            // Получение количеста музыки
            int count = 10;
            WebRequest _requestCount = WebRequest.Create(string.Format("https://api.vk.com/method/audio.getCount.xml?oid={0}&access_token={1}", userId, accessToken));
            WebResponse _responseCount = _requestCount.GetResponse();
            Stream streamCount = _responseCount.GetResponseStream();
            StreamReader _streamreaderCount = new StreamReader(streamCount);

            using (StreamWriter writerCount = new StreamWriter("Count.xml", false))
            {
                string lineCount;
                while ((lineCount = _streamreaderCount.ReadLine()) != null)
                {
                    writerCount.WriteLine(lineCount);
                }
                writerCount.Close();
                XmlDocument loadXml = new XmlDocument();
                loadXml.Load("Count.xml");
               count = Convert.ToInt32 (loadXml.DocumentElement.FirstChild.Value);
               




            }

            // загрузка файла xml со списком музыки

            WebRequest _request = WebRequest.Create(string.Format("https://api.vk.com/method/audio.get.xml?uid={0}&count={1}&access_token={2}", userId, count, accessToken));
            WebResponse _response = _request.GetResponse();
            Stream stream = _response.GetResponseStream();
            StreamReader _streamreader = new StreamReader(stream);

            using (StreamWriter writer = new StreamWriter("Sounds.xml", false))
            {
                string line;
                while ((line = _streamreader.ReadLine()) != null)
                {
                    writer.WriteLine(line);
                }
                writer.Close();

            }

            backgroundWorker1.RunWorkerAsync();
        }

        private void VolumeInvers(int value)
        {



        }
        private void setButtonsOption()
        {
            // 0 - stop
            // 1 - player
            // 2 - pause
            status = 1;

            //play

            button2.Height = 32;
            button2.Width = 32;
            button2.FlatAppearance.BorderSize = 0;
            button2.FlatStyle = FlatStyle.Flat;
            button2.BackColor = Color.Transparent;
            button2.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button2.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button2.Text = "";
            button2.BackgroundImage = Resources.play;
            //volume
            button3.Height = 14;
            button3.Width = 15;
            button3.BackColor = Color.Transparent;
            button3.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button3.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button3.FlatAppearance.BorderSize = 0;
            button3.FlatStyle = FlatStyle.Flat;
            button3.Text = "";
            button3.BackgroundImage = Resources.refresh;



            //stop
            button4.Height = 47;
            button4.Width = 51;
            button4.FlatAppearance.BorderSize = 0;
            button4.FlatStyle = FlatStyle.Flat;
            button4.Text = "";
            button4.BackgroundImage = Resources.replay;

            //previus
            button5.Height = 32;
            button5.Width = 32;
            button5.BackColor = Color.Transparent;
            button5.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button5.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Text = "";
            button5.BackgroundImage = Resources.rewind;

            //next
            button6.Height = 32;
            button6.Width = 32;
            button6.FlatAppearance.BorderSize = 0;
            button6.BackColor = Color.Transparent;
            button6.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button6.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Text = "";
            button6.BackgroundImage = Resources.forward;


            //shuffle
            button7.Height = 14;
            button7.Width = 15;
            button7.BackColor = Color.Transparent;
            button7.FlatAppearance.MouseDownBackColor = Color.Transparent;
            button7.FlatAppearance.MouseOverBackColor = Color.Transparent;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;
            button7.Text = "";
            button7.BackgroundImage = Resources.shuffle;

        }
        public void trackBarControlDuration(double duration)
        {

            myScrollBar1.Value = 0;

            myScrollBar1.Maximum = Convert.ToInt32(durationSounds[number]) + 1;

        }

        private void setPositionTrackbar()
        {
            wplayer.URL = urlSounds[number];
            trackBarControlDuration(duration);
            myScrollBar1.Value = 0;
            myScrollBar1.Maximum = Convert.ToInt32(durationSounds[number]) + 1;
            label2.Text = artist[number];
            label3.Text = titleSounds[number];
            lastTime = Convert.ToInt32(durationSounds[number]) % 60;
            time = Convert.ToInt32(durationSounds[number]) / 60;
            label1.Text = time.ToString() + ":" + lastTime.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }


        private void button1_Click_1(object sender, EventArgs e)
        {

            int appID = 5099950;
            fLoginAccaunt.Show();

            fLoginAccaunt.webBrowser1.Navigate(string.Format("http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope=audio&display=popup&redirect_uri=https://oauth.vk.com/blank.html&response_type=token", appID));

        }




        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            //функции пауза и плей
            if (status == 1)
            {
                wplayer.controls.pause();
                button2.BackgroundImage = Resources.play_on;
                status = 2;
            }
            else if (status == 2)
            {
                //тип данных должен быть double
                status = 1;
                trackBarControlDuration(duration);
                timer1.Enabled = true;
                wplayer.controls.play();
                button2.BackgroundImage = Resources.pause_on;
            }

        }



        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            XmlDocument loadXml = new XmlDocument();
            loadXml.Load("Sounds.xml");

            this.Invoke((MethodInvoker)delegate
            {

                foreach (XmlNode node in loadXml.DocumentElement)
                {

                    titleSounds.Add(node.SelectSingleNode("title").InnerText);
                    durationSounds.Add((node.SelectSingleNode("duration").InnerText));
                    urlSounds.Add((node.SelectSingleNode("url").InnerText));
                    wplayer.mediaCollection.add(node.SelectSingleNode("url").InnerText);
                    artist.Add(node.SelectSingleNode("artist").InnerText);
                    listBox1.Items.Add(node.SelectSingleNode("artist").InnerText + " - " + node.SelectSingleNode("title").InnerText);
                    listBox1.BackColor = Color.AntiqueWhite;
                    listBox1.Update();

                }


            });

        }

        private void button4_Click(object sender, EventArgs e)
        {


        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            myScrollBar1.Value += 1;
            if (myScrollBar1.Value == Convert.ToInt32(durationSounds[number]) + 1)
            {
                number++;
                setPositionTrackbar();
                wplayer.controls.play();
            }


        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            number = listBox1.SelectedIndex;


            if (number >= 0)
            {

                duration = Convert.ToDouble(durationSounds[number]);
                setPositionTrackbar();
                timer1.Enabled = true;
                wplayer.controls.play();
                button2.BackgroundImage = Resources.pause_on;
                status = 2;
            }
            else { number = 0; }

        }

        private void listBox1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            button4.BackgroundImage = Resources.stop;
            button2.BackgroundImage = Resources.play;
            wplayer.controls.stop();
            timer1.Enabled = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            number -= 1;
            wplayer.controls.previous();
            setPositionTrackbar();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            number++;
            setPositionTrackbar();
            wplayer.controls.play();
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {


        }

        private void progressBar1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBarControl1_Properties_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void trackBarControl1_Modified(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {

        }

        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void trackBarControl1_Click(object sender, EventArgs e)
        {
            wplayer.controls.currentPosition = (double)myScrollBar1.Value;
            wplayer.controls.play();
        }

        private void trackBarControl2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void myScrollBar1_Click(object sender, EventArgs e)
        {
            wplayer.controls.currentPosition = (double)myScrollBar1.Value;
            wplayer.controls.play();
        }

        private void myScrollBar2_Click(object sender, EventArgs e)
        {
            wplayer.settings.volume = (int)myScrollBar2.Value;

        }

        private void myScrollBar2_Click_1(object sender, EventArgs e)
        {

        }

        private void myScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            VolumeInvers(myScrollBar2.Value);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
}
