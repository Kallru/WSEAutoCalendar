using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//
// ******  중요  ***************************************************************************************
// WebBrowser 객체에서 Javascript를 제대로 지원하려면,
// 레지스트리에 어플리케이션 실행 파일을 IE 버전과 함께 등록해줘야 한다.
// 레지스트리 위치 : HKEY_CURRENT_USER\Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION
// 값 : 어플이름.exe / DWORD / (10진수)11001 (값 의미 - IE v.11)
// ****************************************************************************************************
//

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.DocumentCompleted += WebBrowserDocumentCompletedEventHandler;
            webBrowser1.StatusTextChanged += StatusTextChanged;
            //webBrowser1.ProgressChanged += new WebBrowserProgressChangedEventHandler(_ie);
            //webBrowser1.Navigating += WebBrowser1_Navigating;
            //webBrowser1.Navigated += WebBrowser1_Navigated;
            webBrowser1.Navigate("https://world.wallstreetenglish.com/login");
        }

        private void WebBrowser1_AbsolutelyComplete()
        {
            HtmlDocument doc = webBrowser1.Document;
            
            HtmlElement submit = doc.GetElementById("login-submit");

            WriteInputById(doc, "login-user-name", "sl-kn.32366.kr");
            WriteInputById(doc, "login-password", "Hwijae1234");
            submit.InvokeMember("click");
        }

        public void StatusTextChanged(object sender, EventArgs e)
        {
            if(webBrowser1.ReadyState == WebBrowserReadyState.Complete && webBrowser1.IsBusy == false)
            {
                HtmlElement username = webBrowser1.Document.GetElementById("login-user-name");
                if (username != null)
                {
                    WebBrowser1_AbsolutelyComplete();
                }
            }
        }

        public void WebBrowserDocumentCompletedEventHandler(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            var browser = sender as WebBrowser;

            var a = browser.ActiveXInstance;
            var b = webBrowser1.ActiveXInstance;

            bool aa = e.Url.AbsolutePath != browser.Url.AbsolutePath;
            
            HtmlDocument doc = browser.Document;
            
            HtmlElement username = doc.GetElementById("login-user-name");
            HtmlElement password = doc.GetElementById("login-password");
            HtmlElement submit = doc.GetElementById("login-password");
            //username.SetAttribute("value", "sl-kn.32366.kr");
            //password.SetAttribute("value", "Hwijae1234");
            //submit.InvokeMember("click");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HtmlDocument doc = webBrowser1.Document;

            HtmlElement username = doc.GetElementById("login-user-name");
            HtmlElement password = doc.GetElementById("login-password");
            HtmlElement submit = doc.GetElementById("login-submit");

            WriteInputById(doc, "login-user-name", "sl-kn.32366.kr");
            WriteInputById(doc, "login-password", "Hwijae1234");
            submit.InvokeMember("click");
        }

        private void WriteInputById(HtmlDocument doc, string id, string value)
        {
            string script = string.Format("angular.element(document.querySelector( '#{0}' )).val('{1}').triggerHandler('change')", id, value);
            doc.InvokeScript("eval", new[] { script });
        }
    }
}
