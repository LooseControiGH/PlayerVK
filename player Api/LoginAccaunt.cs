using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace player_Api
{
    public partial class LoginAccaunt : Form
    {
        Form1 fr;
        public LoginAccaunt( Form1 fr)
        {
            this.fr = fr;
            InitializeComponent();
            
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().IndexOf("access_token") != -1)
            {
                
               
                Regex myReg = new Regex(@"(?<name>[\w\d\x5f]+)=(?<value>[^\x26\s]+)", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                foreach (Match m in myReg.Matches(e.Url.ToString()))
                {
                    if (m.Groups["name"].Value == "access_token")
                    {
                        
                        Form1.accessToken = m.Groups["value"].Value;
                     
                    }
                    else if (m.Groups["name"].Value == "user_id")
                    {
                       Form1.userId = Convert.ToInt32(m.Groups["value"].Value);
                      
                    }
                    
                    Close();
                }
                fr.loadAndParseXml();

            }
        }

        private void LoginAccaunt_Load(object sender, EventArgs e)
        {

        }
    }
}
