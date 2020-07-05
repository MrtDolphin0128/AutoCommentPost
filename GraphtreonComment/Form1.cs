using System.Windows.Forms;
using System;
using System.IO;
using System.Globalization;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Net;
using Gecko;
using System.Text;
using ThangDC.Core.Entities;
using System.Web.Script.Serialization;
using Gecko.JQuery;
using Gecko.DOM;
using System.Xml.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace GraphtreonComment
{
    using StringList = List<string>;

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class Form1 : Form
    {
        #region static variable
        private string curEmail;
        private string curUserName;

        private bool IsLoggedIn = false;
        private bool IsLoading = false;
        private bool IsRunning = false;
        private bool IsStop = true;
        private TabPage currentTab = null;
        private string curTypeFilter = "";

        //WebBrowser Main
        private WebBrowser wbMain;
        private GeckoHtmlElement htmlElm;

        private string LastScriptFile = "obeygiant.csr";
        public string Version = "1.1.4";

        public string MaxWait = string.Empty;

        private bool IsBreakSleep = false;
        private int countPerPage = 100;
        private int curPage = 1;
        private int curMaxPage = 1;
        private Random rnd = new Random();

        private List<CreatorInfo> creatorInfos = new List<CreatorInfo>();
        private List<int> filteredCreatorIndices = new List<int>();
        private CommentSetting commentSetting = new CommentSetting();

        private Stopwatch swCreators = new Stopwatch();
        private Stopwatch swComments = new Stopwatch();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Int32 SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        #endregion
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnShowAll_Click(object sender, EventArgs e)
        {

        }

        public void tabnew()
        {
            TabPage tab = new TabPage("store.obeygiant.com");
            tabMain.Controls.Add(tab);
            tab.Dock = DockStyle.Fill;
            currentTab = tab;
            tabMain.SelectedTab = currentTab;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CallBackWinAppWebBrowser();
            var path = Application.StartupPath + "\\Firefox";
            Xpcom.Initialize(path);
            LastScriptFile = Application.StartupPath + "\\obeygiant.query";
            LoadScript(null);

            if (System.IO.File.Exists(LastScriptFile))
            {
                sleep(1, false);
            }
            MaxWait = (ConfigurationManager.AppSettings["MaxWait"] != null ? ConfigurationManager.AppSettings["MaxWait"] : string.Empty);

            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Visible = true;
            GetUserInfo();
            loadingForm.Close();

            if (curEmail != "")
                SetLoginState(true);
            else
                SetLoginState(false);

            LoadCommentSettings("settings.dat");

            AddBlankKeyword();
            keywordList.SetSelected(0, true);
        }
        public void LoadScript(string[] args)
        {
            if (args == null) return;
            if (args.Length > 0)
            {
                var path = args[0];
                if (!string.IsNullOrEmpty(path))
                {
                    if (System.IO.File.Exists(path))
                    {
                        sleep(1, false);
                        string code = read(path);
                        RunCode(code);
                    }
                }
            }
        }

        private object GetCurrentWB()
        {
            if (tabMain.SelectedTab != null)
            {
                if (tabMain.SelectedTab.Controls.Count > 0)
                {
                    Control ctr = tabMain.SelectedTab.Controls[0];
                    if (ctr != null)
                    {
                        return ctr as object;
                    }
                }
            }
            return null;
        }

        delegate void StringArgDelegate(string scriptString);
        public void RunCode(string scriptString)
        {
            if(wbMain.InvokeRequired)
            {
                StringArgDelegate d = new StringArgDelegate(RunCode);
                this.Invoke(d, scriptString);
            }
            else
            {
                if (IsStop)
                {
                    IsStop = false;
                    wbMain.Document.InvokeScript("UnAbort");
                    if (!string.IsNullOrEmpty(scriptString))
                    {
                        ExcuteJSCode(scriptString);
                    }
                }
                else
                {
                    IsStop = true;
                    wbMain.Document.InvokeScript("Abort");
                }

                IsStop = true;
            }
        }
        private void ExcuteJSCode(string code)
        {
            ExcuteJSCodeWebBrowser(code);
        }

        #region TextBox and Go Event Functions

        nsIMemory _memoryService = null;
        public void go(string url)
        {
            if (currentTab == null)
            {
                tabnew();
            }

            //WebBrowser
            if (!url.StartsWith("/"))
            {
                if (currentTab.Controls.Count > 0)
                {
                    Control ctr = currentTab.Controls[0];
                    if (ctr != null)
                    {
                        var wb = (GeckoWebBrowser)ctr;
                        wb.Stop();
                        wb.ProgressChanged -= wbBrowser_ProgressChanged;
                        wb.Navigated -= wbBrowser_Navigated;
                        wb.DocumentCompleted -= wbBrowser_DocumentCompleted;
                        wb.CanGoBackChanged -= wbBrowser_CanGoBackChanged;
                        wb.CanGoForwardChanged -= wbBrowser_CanGoForwardChanged;
                        wb.ShowContextMenu -= new EventHandler<GeckoContextMenuEventArgs>(wbBrowser_ShowContextMenu);
                        wb.DomContextMenu -= wbBrowser_DomContextMenu;
                        wb.Dispose();
                        wb = null;

                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        if (_memoryService == null)
                        {
                            _memoryService = Xpcom.GetService<nsIMemory>("@mozilla.org/xpcom/memory-service;1");
                        }
                        _memoryService.HeapMinimize(false);
                    }
                    currentTab.Controls.Clear();
                }
                GoWebBrowser(url);
            }
            else
            {
                GoWebBrowserByXpath(url);
            }
        }

        #endregion
        #region WebBrowser
        public void CallBackWinAppWebBrowser()
        {
            wbMain = new WebBrowser();
            wbMain.ObjectForScripting = this;
            wbMain.ScriptErrorsSuppressed = true;
            wbMain.DocumentText = @"<html>
                                        <head>
                                            <script type='text/javascript'>
                                                var isAborted = false;

                                                function UnAbort() {isAborted = false; window.external.UnAbort();}
                                                function Abort() {isAborted = true; release(); window.external.Abort();}
                                                function CheckAbort() {if(isAborted == true) { window.external.Abort(); throw new Error('Aborted');} }

                                                /*var isAborted = false;
                                                function UnAbort() {isAborted = false;}
                                                function Abort() {isAborted = true;}
                                                function CheckAbort() {if(isAborted == true) throw new Error('Aborted');}*/

                                                function stringtoXML(data){ if (window.ActiveXObject){ var doc = new ActiveXObject('Microsoft.XMLDOM'); doc.async='false'; doc.loadXML(data); } else { var parser = new DOMParser(); var doc = parser.parseFromString(data,'text/xml'); }	return doc; }

                                                /* Open new tab */

                                                function release() { CheckAbort(); window.external.ReleaseMR();  }

                                                function countNodes(xpath) { CheckAbort(); return window.external.countNodes(xpath); } 

                                                function tabnew() { CheckAbort(); window.external.tabnew();}
                                                /* Close current tab  */
                                                function tabclose() { CheckAbort(); window.external.tabclose();}
                                                /* Close all tab  */
                                                function tabcloseall() { CheckAbort(); window.external.tabcloseall();}
                                                /* Go to website by url or xpath  */
                                                function go(a) { CheckAbort(); window.external.go(a);}
                                                
                                                function goWithProxy(url, proxyUrl){ CheckAbort(); window.external.goWithProxy(url, proxyUrl); }

                                                function back() { CheckAbort(); window.external.Back(); }
                                                function next() { CheckAbort(); window.external.Next(); }
                                                function reload() { CheckAbort(); window.external.Reload(); }
                                                function stop() { CheckAbort(); window.external.Stop(); }

                                                /* Sleep with a = miliseconds to sleep, b = true if wait until browser loading finished, b = false wait until timeout miliseconds  */
                                                function sleep(a, b) { CheckAbort(); window.external.sleep(a,b);}
                                                /* Quit application  */
                                                function exit() { CheckAbort(); window.external.exit();}
                                                /* Click by xpath  */
                                                function click(a) { CheckAbort(); window.external.click(a);}
                                                /* write a log to preview, a = content of log  */
                                                function log(a) { CheckAbort(); window.external.log(a);}
                                                /* clear log on the preview  */
                                                function clearlog() { CheckAbort(); window.external.clearlog();}
                                                /* extract data from xpath  */
                                                //function extract(a) {CheckAbort(); return window.external.extract(a);}
                                                function extract(xpath, type) {CheckAbort(); return window.external.extract(xpath, type);}

                                                function extractUntil(xpath, type){ CheckAbort(); return window.external.extractUntil(xpath, type); }

                                                function filliframe(title, value) { CheckAbort(); window.external.filliframe(title, value); }                                                

                                                /* fill xpath by value, a = xpath, b = value  */
                                                function fill(a,b) { CheckAbort(); window.external.fill(a,b);}
                                                /* convert extract string to object  */

                                                /*function filldropdown(a, b) { CheckAbort(); window.external.filldropdown(a, b); }*/
                                                function filldropdown(xpath, value) { CheckAbort(); window.external.filldropdown(xpath, value); }
                                                function toObject(a) {CheckAbort(); var wrapper= document.createElement('div'); wrapper.innerHTML= a; return wrapper;}
                                                function blockFlash(isBlock) { CheckAbort(); window.external.BlockFlash(isBlock); }

                                                /* browser get all link in the area of xpath, it will stop until program go all of link , a = xpath */
                                                function browser(a) {CheckAbort(); window.external.browser(a);}
                                                /* reset list website to unread so program can go back and browser continue */
                                                function resetlistwebsite() {CheckAbort(); window.external.ResetListWebsite();}
                                                /* take a snapshot from current website on current tab, a = location to save a snapshot */

                                                function takesnapshot(a) {CheckAbort(); window.external.TakeSnapshot(a);}
                                                /* reconigze text of image from url, a = url of image  */
                                                function imageToText(xpath, language) { CheckAbort(); return window.external.imgToText(xpath, language);}
                                                /* set value to file upload (not work in ie)  */
                                                function fileupload(a,b){CheckAbort(); window.external.FileUpload(a,b);}

                                                /* create folder, a = location  */
                                                function createfolder(a) { CheckAbort(); window.external.createfolder(a);}
                                                /* download file from url, a = url to download, b = location where file located  */
                                                function download(a,b) {CheckAbort(); window.external.download(a,b);}

                                                function downloadWebsite(url) { CheckAbort(); return window.external.DownloadWebsite(url); } 

                                                function getfiles(a) { CheckAbort(); return window.external.getfiles(a); }
                                                function getfolders(a) { CheckAbort(); return window.external.getfolders(a); }

                                                /* read a file, a = location of file  */
                                                function read(a) { CheckAbort(); return window.external.read(a);}
                                                /* save file, a = content of file, b = location of file to save, c = is file override (true: fill will be override, false: not override)  */
                                                function save(a,b,c) { CheckAbort(); return window.external.save(a,b,c);}
                                                /* remove a file, a = location of file will be removed */
                                                function remove(a) { CheckAbort(); window.external.remove(a);}
                                                function removefolder(a) {CheckAbort(); window.external.removefolder(a);}
                                                
                                                function copyfolder(a,b,c) {CheckAbort(); window.external.copyFolder(a,b,c);}
                                                function copyfile(a,b,c) {CheckAbort(); window.external.copyFile(a,b,c);}

                                                function replacetextinfile(a, b, c) { CheckAbort(); window.external.replaceTextinFile(a,b,c); }

                                                function explorer(a) { CheckAbort(); window.external.explorer(a); }

                                                /* run code from string, a = code to run  */
                                                function excute(a) { CheckAbort(); window.external.excute(a);}

                                                function logoff() { CheckAbort(); window.external.logoff();} 
                                                function lockworkstation() {CheckAbort(); window.external.lockworkstation();} 
                                                function forcelogoff() { CheckAbort(); window.external.forcelogoff();} 
                                                function reboot() { CheckAbort(); window.external.reboot();} 
                                                function shutdown() { CheckAbort(); window.external.shutdown();} 
                                                function hibernate() { CheckAbort(); window.external.hibernate();} 
                                                function standby() { CheckAbort(); window.external.standby();} 


                                                /* run application, a = location of application */
                                                function runcommand(path, parameters) { CheckAbort(); window.external.runcommand(path, parameters); }

                                                function createtask(a,b,c,d,e,f) { CheckAbort(); window.external.createtask(a,b,c,d,e,f); }
                                                function removetask(a) { CheckAbort(); window.external.removetask(a);}

                                                function generatekeys() { CheckAbort(); window.external.generatekeys();}
                                                function encrypt(a, b) { CheckAbort(); return window.external.encrypt(a, b);}
                                                function decrypt(a, b) { CheckAbort(); return window.external.decrypt(a, b);}

                                                function showpicture(a,b) { CheckAbort(); window.external.showimage(a,b); }
                                                function savefilterimage(a) { CheckAbort(); window.external.savefilterimage(a); }

                                                function writetextimage(a, b) {CheckAbort(); window.external.writetextimage(a,b); } 

                                                function getcurrenturl() {CheckAbort(); return window.external.getCurrentUrl();}

                                                function scrollto(a) {CheckAbort(); window.external.scrollto(a); }

                                                function getheight() { CheckAbort(); return window.external.getheight(); }

                                                function gettitle() { CheckAbort(); return window.external.gettitle(); } 

                                                function getlinks(a) { CheckAbort(); return window.external.getlinks(a); } 

                                                function getCurrentContent() { CheckAbort(); return window.external.getCurrentContent(); } 

                                                function getCurrentPath() { CheckAbort(); return window.external.getCurrentPath(); } 

                                                function checkelement(a) { CheckAbort(); return window.external.checkelement(a);}

                                                function readCellExcel(a, b, c, d) { CheckAbort(); return window.external.readCellExcel(a,b,c,d);}

                                                function writeCellExcel(a, b, c, d) { CheckAbort(); window.external.writeCellExcel(a,b,c,d); }

                                                function replaceMsWord(a, b, c, d) { CheckAbort(); window.external.replaceMsWord(a,b,c,d); } 

                                                function loadHTML(a) { CheckAbort(); window.external.loadHTML(a); }" +

                                                "function textToJSON(a) { CheckAbort(); var b = eval(\"(\" + window.external.textToJSON(a) + \")\"); return b; }" +

                                                @"function getCurrentLogin() { return textToJSON(window.external.getCurrentUser());}

                                                function login(a, b) { return window.external.login(a,b); }

                                                function register(a, b, c, d) { return window.external.register(a, b, c, d);}

                                                function getAccount(a) { CheckAbort(); var b = window.external.GetAccount(a); if(b == '') return ''; else return textToJSON(b); }

                                                function captchaborder(a,b) { CheckAbort(); window.external.CaptchaBorder(a,b); } 

                                                function saveImageFromElement(a,b) { CheckAbort(); window.external.SaveImageFromElement(a,b);}

                                                function getControlText(a,b,c) { CheckAbort(); return window.external.GetControlText(a,b,c); }

                                                function setControlText(a,b,c,d) { CheckAbort(); window.external.SetControlText(a,b,c,d); }

                                                function clickControl(a,b,c) { CheckAbort(); window.external.ClickControl(a,b,c); } 

                                                function getCurrentMouseX() { CheckAbort(); return window.external.GetCurrentMouseX(); } 

                                                function getCurrentMouseY() { CheckAbort(); return window.external.GetCurrentMouseY(); } 

                                                function MouseDown(a,b) { CheckAbort(); window.external.Mouse_Down(a,b); }

                                                function MouseUp(a,b) { CheckAbort(); window.external.Mouse_Up(a,b); }

                                                function MouseClick(a,b) { CheckAbort(); window.external.Mouse_Click(a,b); }

                                                function MouseDoubleClick(a,b) { CheckAbort(); window.external.Mouse_Double_Click(a,b); }

                                                function MouseMove(a,b,c,d) {CheckAbort(); window.external.Mouse_Show(a,b,c,d); }

                                                function MouseWheel(a,b) { CheckAbort(); window.external.Mouse_Wheel(a,b); }

                                                function KeyDown(a,b) { CheckAbort(); window.external.Key_Down(a,b); }

                                                function KeyUp(a,b) { CheckAbort(); window.external.Key_Up(a,b); }

                                                function sendText(a) { CheckAbort(); window.external.sendText(a); }

                                                function Reload() { CheckAbort(); window.external.Reload(); }

                                                function sendEmail(name, email, subject, content) { CheckAbort(); return window.external.sendEmail(name, email, subject, content); }" +

                                                "function getAccountBy(name) { CheckAbort(); var a = window.external.GetAccountBy(name); if(a != '') { return eval(\"(\" + a + \")\"); } else { return ''; } }" +

                                                @"function getDatabases(name) { CheckAbort(); return window.external.GetDatabases(name); } 

                                                function getTables(name, dbName) { CheckAbort(); return window.external.GetTables(name, dbName); }

                                                function getColumns(name, dbName, table) { CheckAbort(); return window.external.GetColumns(name, dbName, table); }

                                                function getRows(name, dbName, sql) { CheckAbort(); return window.external.GetRows(name, dbName, sql); }

                                                function excuteQuery(name, dbName, sql) { CheckAbort(); return window.external.ExcuteQuery(name, dbName, sql); } 

                                                function removeStopWords(text) { CheckAbort(); return window.external.RemoveStopWords(text); }

                                                function addElement(path, node1, node2, text) { CheckAbort(); return window.external.AddElement(path, node1, node2, text); }

                                                function checkXmlElement(path, node, text) { CheckAbort(); return window.external.CheckXmlElement(path, node, text); }

                                                function getXmlElement(path, node) { CheckAbort(); return window.external.GetXmlElement(path, node); }

                                                function getParentElement(path, node, text) { CheckAbort(); return window.external.GetParentElement(path, node, text); }
                                                
                                                function extractbyRegularExpression(pattern, groupName) { CheckAbort(); return window.external.ExtractUsingRegularExpression(pattern, groupName); }

                                                function addToDownload(fileName, url, folder) { CheckAbort(); return window.external.AddToDownload(fileName, url, folder); }

                                                function startDownload() { CheckAbort(); return window.external.StartDownload(); }

                                                function hide() { CheckAbort(); return window.external.MinimizeWindow(); }
                                            </script>
                                        </head>
                                        <body>
                                            
                                        </body>
                                    </html>";
            //this.Controls.Add(wbMain);
        }

        void ExcuteJSCodeWebBrowser(string code)
        {
            wbMain.Document.InvokeScript("UnAbort");
            object obj = wbMain.Document.InvokeScript("eval", new object[] { code });
        }

        void GoWebBrowser(string url)
        {
            if (String.IsNullOrEmpty(url)) return;
            if (url.Equals("about:blank")) return;

            GeckoWebBrowser wbBrowser = new GeckoWebBrowser();
            wbBrowser.ProgressChanged += wbBrowser_ProgressChanged;
            wbBrowser.Navigated += wbBrowser_Navigated;
            wbBrowser.DocumentCompleted += wbBrowser_DocumentCompleted;
            wbBrowser.CanGoBackChanged += wbBrowser_CanGoBackChanged;
            wbBrowser.CanGoForwardChanged += wbBrowser_CanGoForwardChanged;
            wbBrowser.ShowContextMenu += new EventHandler<GeckoContextMenuEventArgs>(wbBrowser_ShowContextMenu);
            wbBrowser.DomContextMenu += wbBrowser_DomContextMenu;
            wbBrowser.NoDefaultContextMenu = true;

            currentTab.Controls.Add(wbBrowser);
            wbBrowser.Dock = DockStyle.Fill;
            wbBrowser.Navigate(url);
        }

        void wbBrowser_ProgressChanged(object sender, GeckoProgressEventArgs e)
        {
            Application.DoEvents();
        }

        void wbBrowser_CanGoForwardChanged(object sender, EventArgs e)
        {
        }

        void wbBrowser_CanGoBackChanged(object sender, EventArgs e)
        {
        }

        void wbBrowser_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            /*
            string url = string.Empty;
            url = ((GeckoWebBrowser)sender).Url.ToString();
            if (url != "about:blank")
                tbxAddress.Text = url;
                */
            e = e;
        }

        void wbBrowser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            if (e.Uri.AbsolutePath != (sender as GeckoWebBrowser).Url.AbsolutePath)
                return;

            GeckoWebBrowser wbBrowser = (GeckoWebBrowser)sender;

            string title = wbBrowser.DocumentTitle;
            currentTab.Text = wbBrowser.Url.ToString();
            

            IsBreakSleep = true;
        }

        void wbBrowser_ShowContextMenu(object sender, GeckoContextMenuEventArgs e)
        {
            //contextMenuBrowser.Show(Cursor.Position);

            //CurrentMouseX = Cursor.Position.X;
            //CurrentMouseY = Cursor.Position.Y;

            /*GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                htmlElm = (GeckoHtmlElement)wb.Document.ElementFromPoint(Cursor.Position.X, Cursor.Position.Y);
                if (htmlElm != null)
                {
                    if (htmlElm.GetType().Name == "GeckoIFrameElement")
                    {
                        var iframe = (GeckoIFrameElement)wb.Document.GetElementById(htmlElm.Id);
                        if (iframe != null)
                        {
                            var contentDocument = iframe.ContentWindow.Document;
                            if (contentDocument != null)
                                htmlElm = (GeckoHtmlElement)contentDocument.ElementFromPoint(Cursor.Position.X, Cursor.Position.Y);
                        }
                    }
                }
            }*/
        }

        private int CurrentMouseX = 0;
        private int CurrentMouseY = 0;

        void wbBrowser_DomContextMenu(object sender, DomMouseEventArgs e)
        {
            if (e.Button.ToString().IndexOf("Right") != -1)
            {
                //contextMenuBrowser.Show(Cursor.Position);

                CurrentMouseX = Cursor.Position.X;
                CurrentMouseY = Cursor.Position.Y;

                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    htmlElm = (GeckoHtmlElement)wb.Document.ElementFromPoint(e.ClientX, e.ClientY);
                }
            }
        }

        void GoWebBrowserByXpath(string xpath)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                GeckoHtmlElement elm = GetCompleteElementByXPath(wb, xpath);
                if (elm != null)
                {
                    UpdateUrlAbsolute(wb.Document, elm);
                    string url = extractData(elm, "href");
                    if (!string.IsNullOrEmpty(url))
                        wb.Navigate(url);
                }
            }
        }

        void NextWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoForward();
            }
        }

        void BackWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.GoBack();
            }
        }

        void ReloadWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Refresh();
            }
        }

        void StopWebBrowser()
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                wb.Stop();
            }
        }

        void TabSelectedWebBrowser()
        {
            if (tabMain.TabCount > 0)
            {
                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                if (wb != null)
                {
                    //tbxAddress.Text = wb.Url.ToString();
                    string title = wb.DocumentTitle;
                    currentTab.Text = (title.Length > 10 ? title.Substring(0, 10) + "..." : title);
                    this.Text = title;
                }
            }
        }

        private GeckoHtmlElement GetElementByXpath(GeckoDocument doc, string xpath)
        {
            if (doc == null) return null;

            xpath = xpath.Replace("/html/", "");
            GeckoElementCollection eleColec = doc.GetElementsByTagName("html"); if (eleColec.Length == 0) return null;
            GeckoHtmlElement ele = eleColec[0];
            string[] tagList = xpath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tag in tagList)
            {
                System.Text.RegularExpressions.Match mat = System.Text.RegularExpressions.Regex.Match(tag, "(?<tag>.+)\\[@id='(?<id>.+)'\\]");
                if (mat.Success == true)
                {
                    string id = mat.Groups["id"].Value;
                    GeckoHtmlElement tmpEle = doc.GetHtmlElementById(id);
                    if (tmpEle != null) ele = tmpEle;
                    else
                    {
                        ele = null;
                        break;
                    }
                }
                else
                {
                    mat = System.Text.RegularExpressions.Regex.Match(tag, "(?<tag>.+)\\[(?<ind>[0-9]+)\\]");
                    if (mat.Success == false)
                    {
                        GeckoHtmlElement tmpEle = null;
                        foreach (GeckoNode it in ele.ChildNodes)
                        {
                            if (it.NodeName.ToLower() == tag)
                            {
                                tmpEle = (GeckoHtmlElement)it;
                                break;
                            }
                        }
                        if (tmpEle != null) ele = tmpEle;
                        else
                        {
                            ele = null;
                            break;
                        }
                    }
                    else
                    {
                        string tagName = mat.Groups["tag"].Value;
                        int ind = int.Parse(mat.Groups["ind"].Value);
                        int count = 0;
                        GeckoHtmlElement tmpEle = null;
                        foreach (GeckoNode it in ele.ChildNodes)
                        {
                            if (it.NodeName.ToLower() == tagName)
                            {
                                count++;
                                if (ind == count)
                                {
                                    tmpEle = (GeckoHtmlElement)it;
                                    break;
                                }
                            }
                        }
                        if (tmpEle != null) ele = tmpEle;
                        else
                        {
                            ele = null;
                            break;
                        }
                    }
                }
            }

            return ele;
        }

        private string GetXpath(GeckoNode node)
        {
            if (node == null)
                return string.Empty;

            if (node.NodeType == NodeType.Attribute)
            {
                return String.Format("{0}/@{1}", GetXpath(((GeckoAttribute)node).OwnerDocument), node.LocalName);
            }
            if (node.ParentNode == null)
            {
                return "";
            }
            string elementId = ((GeckoHtmlElement)node).Id;
            if (!String.IsNullOrEmpty(elementId))
            {
                return String.Format("//*[@id='{0}']", elementId);
            }

            int indexInParent = 1;
            GeckoNode siblingNode = node.PreviousSibling;

            while (siblingNode != null)
            {

                if (siblingNode.LocalName == node.LocalName)
                {
                    indexInParent++;
                }
                siblingNode = siblingNode.PreviousSibling;
            }

            return String.Format("{0}/{1}[{2}]", GetXpath(node.ParentNode), node.LocalName, indexInParent);
        }

        private int GetXpathIndex(GeckoHtmlElement ele)
        {
            if (ele.Parent == null) return 0;
            int ind = 0, indEle = 0;
            string tagName = ele.TagName;
            GeckoNodeCollection elecol = ele.Parent.ChildNodes;
            foreach (GeckoNode it in elecol)
            {
                if (it.NodeName == tagName)
                {
                    ind++;
                    if (it.TextContent == ele.TextContent) indEle = ind;
                }
            }
            if (ind > 1) return indEle;
            return 0;
        }

        protected void UpdateUrlAbsolute(GeckoDocument doc, GeckoHtmlElement ele)
        {
            string link = doc.Url.GetLeftPart(UriPartial.Authority);

            var eleColec = ele.GetElementsByTagName("IMG");
            foreach (GeckoHtmlElement it in eleColec)
            {
                if (!it.GetAttribute("src").StartsWith("http://"))
                    it.SetAttribute("src", link + it.GetAttribute("src"));
            }
            eleColec = ele.GetElementsByTagName("A");
            foreach (GeckoHtmlElement it in eleColec)
            {
                if (!it.GetAttribute("href").StartsWith("http://"))
                    it.SetAttribute("href", link + it.GetAttribute("href"));
            }
        }

        private GeckoHtmlElement GetCompleteElementByXPath(GeckoWebBrowser wb, string xpath)
        {
            GeckoHtmlElement elm = GetElement(wb, xpath);

            int waitUntil = 0;
            int count = 0;

            int.TryParse(MaxWait, out waitUntil);

            while (elm == null)
            {
                //Stop when click Stop button
                if (IsStop) break;

                //It will stop when get the limit configuration
                if (count > waitUntil) break;

                elm = GetElement(wb, xpath);
                sleep(1, false);
                count++;
            }

            return elm;
        }

        private GeckoHtmlElement GetElement(GeckoWebBrowser wb, string xpath)
        {
            GeckoHtmlElement elm = null;
            if (xpath.StartsWith("/"))
            {
                if (xpath.Contains("@class") || xpath.Contains("@data-type"))
                {
                    var html = GetHtmlFromGeckoDocument(wb.Document);
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(html);

                    var node = doc.DocumentNode.SelectSingleNode(xpath);
                    if (node != null)
                    {
                        var currentXpath = "/" + node.XPath;
                        elm = (GeckoHtmlElement)wb.Document.EvaluateXPath(currentXpath).GetNodes().FirstOrDefault();
                    }
                }
                else
                {
                    elm = (GeckoHtmlElement)wb.Document.EvaluateXPath(xpath).GetNodes().FirstOrDefault();
                }
            }
            else
            {
                elm = (GeckoHtmlElement)wb.Document.GetElementById(xpath);
            }
            return elm;
        }

        private string GetHtmlFromGeckoDocument(GeckoDocument doc)
        {
            var result = string.Empty;

            GeckoHtmlElement element = null;
            var geckoDomElement = doc.DocumentElement;
            if (geckoDomElement is GeckoHtmlElement)
            {
                element = (GeckoHtmlElement)geckoDomElement;
                result = element.InnerHtml;
            }

            return result;
        }

        #endregion  

        #region functions
        public string extract(string xpath, string type)
        {
            string result = string.Empty;
            GeckoHtmlElement elm = null;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                elm = GetElement(wb, xpath);
                if (elm != null)
                    UpdateUrlAbsolute(wb.Document, elm);

                if (elm != null)
                {
                    switch (type)
                    {
                        case "html":
                            result = elm.OuterHtml;
                            break;
                        case "text":
                            if (elm.GetType().Name == "GeckoTextAreaElement")
                            {
                                result = ((GeckoTextAreaElement)elm).Value;
                            }
                            else
                            {
                                result = elm.TextContent.Trim();
                            }
                            break;
                        case "value":
                            result = ((GeckoInputElement)elm).Value;
                            break;
                        default:
                            result = extractData(elm, type);
                            break;
                    }
                }
            }

            return result;
        }

        public string extractUntil(string xpath, string type)
        {
            var result = string.Empty;

            GeckoHtmlElement elm = null;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                elm = GetCompleteElementByXPath(wb, xpath);
                if (elm != null)
                    UpdateUrlAbsolute(wb.Document, elm);

                if (elm != null)
                {
                    switch (type)
                    {
                        case "html":
                            result = elm.OuterHtml;
                            break;
                        case "text":
                            if (elm.GetType().Name == "GeckoTextAreaElement")
                            {
                                result = ((GeckoTextAreaElement)elm).Value;
                            }
                            else
                            {
                                result = elm.TextContent.Trim();
                            }
                            break;
                        default:
                            result = extractData(elm, type);
                            break;
                    }
                }
            }

            return result;
        }

        public void filliframe(string title, string value)
        {
            /*GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                foreach (GeckoWindow ifr in wb.Window.Frames)
                {
                    if (ifr.Document.Title == title)
                    {
                        foreach (var item in ifr.Document.ChildNodes)
                        {
                            if (item.NodeName == "HTML")
                            {
                                foreach (var it in item.ChildNodes)
                                {
                                    if (it.NodeName == "BODY")
                                    {
                                        GeckoBodyElement elem = (GeckoBodyElement)it;
                                        elem.InnerHtml = value;
                                        elem.Focus();
                                    }
                                }                                
                                break;
                            }
                        }
                        break;
                    }
                }
            }*/
        }

        public void fill(string xpath, string value)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        switch (elm.TagName)
                        {
                            case "IFRAME":
                                /*foreach (GeckoWindow ifr in wb.Window.Frames)
                                {
                                    if (ifr.Document == elm.DOMElement)
                                    {
                                        ifr.Document.TextContent = value;
                                        break;
                                    }
                                }*/
                                break;
                            case "INPUT":
                                GeckoInputElement input = (GeckoInputElement)elm;
                                input.Value = value;
                                input.Focus();
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Byte[] bytes = Encoding.UTF32.GetBytes(value);
                    StringBuilder asAscii = new StringBuilder();
                    for (int idx = 0; idx < bytes.Length; idx += 4)
                    {
                        uint codepoint = BitConverter.ToUInt32(bytes, idx);
                        if (codepoint <= 127)
                            asAscii.Append(Convert.ToChar(codepoint));
                        else
                            asAscii.AppendFormat("\\u{0:x4}", codepoint);
                    }
                    /*var id = xpath;
                    using (AutoJSContext context = new AutoJSContext(wb.Window.JSContext))
                    {
                        context.EvaluateScript("document.getElementById('" + id + "').value = '" + asAscii.ToString() + "';");
                        context.EvaluateScript("document.getElementById('" + id + "').scrollIntoView();");
                    }*/
                }
            }

        }

        public void filldropdown(string xpath, string value)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        var dropdown = elm as GeckoSelectElement;
                        var length = dropdown.Options.Length;
                        var items = dropdown.Options;
                        for (var i = 0; i < length; i++)
                        {
                            var item = dropdown.Options.item((uint)i);
                            if (item.Text.ToUpper() == value.ToUpper())
                            {
                                item.SetAttribute("selected", "selected");
                            }
                            else
                            {
                                item.RemoveAttribute("selected");
                            }
                        }
                        elm.Focus();
                    }
                }
                else
                {
                }
            }
        }

        public void click(string xpath)
        {
            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                if (xpath.StartsWith("/"))
                {
                    GeckoHtmlElement elm = GetElement(wb, xpath);
                    if (elm != null)
                    {
                        elm.Click();
                        elm.Focus();
                    }
                }
                else
                {
                }
            }
        }

        public void sleep(double seconds, bool isBreakWhenWBCompleted)
        {
            IsBreakSleep = false;
            for (int i = 0; i < seconds * 10; i++)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);

                if (isBreakWhenWBCompleted && IsBreakSleep)
                {
                    break;
                }
            }
        }
        public string read(string path)
        {
            string result = "";
            try
            {
                string[] list = System.IO.File.ReadAllLines(path);
                foreach (string l in list)
                {
                    result += l + "\n";
                }
            }
            catch { }
            return result;
        }
        public void runcommand(string path, string parameters)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = getCurrentPath();
                startInfo.FileName = path;
                startInfo.Arguments = parameters;

                try
                {
                    Process p = Process.Start(startInfo);
                    p.WaitForExit();
                }
                catch { }
            }
            catch { }
        }

        public void save(string content, string path, bool isOverride)
        {
            try
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, isOverride))
                {
                    file.WriteLine(content);
                }
            }
            catch { }
        }
        public void excute(string script)
        {
            ExcuteJSCodeWebBrowser(script);
        }
        public string encrypt(string publicKey, string plainText)
        {
            System.Security.Cryptography.CspParameters cspParams = null;
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = null;
            byte[] plainBytes = null;
            byte[] encryptedBytes = null;

            string result = "";
            try
            {
                cspParams = new System.Security.Cryptography.CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(publicKey);

                plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                encryptedBytes = rsaProvider.Encrypt(plainBytes, false);
                result = Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception ex) { }
            return result;
        }

        public string decrypt(string privateKey, string encrypted)
        {
            System.Security.Cryptography.CspParameters cspParams = null;
            System.Security.Cryptography.RSACryptoServiceProvider rsaProvider = null;
            byte[] encryptedBytes = null;
            byte[] plainBytes = null;

            string result = "";
            try
            {
                cspParams = new System.Security.Cryptography.CspParameters();
                cspParams.ProviderType = 1;
                rsaProvider = new System.Security.Cryptography.RSACryptoServiceProvider(cspParams);

                rsaProvider.FromXmlString(privateKey);

                encryptedBytes = Convert.FromBase64String(encrypted);
                plainBytes = rsaProvider.Decrypt(encryptedBytes, false);

                result = System.Text.Encoding.UTF8.GetString(plainBytes);
            }
            catch (Exception ex) { }
            return result;
        }
        public void TakeSnapshot(string location)
        {
            try
            {
                GeckoWebBrowser wbBrowser = (GeckoWebBrowser)GetCurrentWB();
                ImageCreator creator = new ImageCreator(wbBrowser);
                byte[] rs = creator.CanvasGetPngImage((uint)wbBrowser.Document.ActiveElement.ScrollWidth, (uint)wbBrowser.Document.ActiveElement.ScrollHeight);


                MemoryStream ms = new MemoryStream(rs);
                Image returnImage = Image.FromStream(ms);

                returnImage.Save(location);

            }
            catch { }
        }

        public string imgToText(string xpath, string language)
        {
            string data = string.Empty;
            string path = string.Empty;
            path = Application.StartupPath + "\\captcha\\image.png";
            bool isSaveSuccess = saveImage(xpath, path);

            if (isSaveSuccess)
            {
                string text = Application.StartupPath + "\\captcha\\output.txt";

                string param = "";
                if (language == "vie")
                {
                    param = "\"" + path + "\" \"" + Application.StartupPath + "\\captcha\\output" + "\" -l vie";
                }
                else
                {
                    param = "\"" + path + "\" \"" + Application.StartupPath + "\\captcha\\output" + "\" -l eng";
                }


                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = Application.StartupPath + "\\tesseract.exe";
                process.StartInfo.Arguments = param;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();

                data = read(text).Replace("\n", "");

            }

            return data;
        }

        private bool saveImage(string xpath, string location)
        {
            bool result = false;
            try
            {
                GeckoWebBrowser wbBrowser = (GeckoWebBrowser)GetCurrentWB();
                if (wbBrowser != null)
                {
                    GeckoImageElement element = null;
                    if (xpath.StartsWith("/"))
                        element = (GeckoImageElement)wbBrowser.Document.EvaluateXPath(xpath).GetNodes().FirstOrDefault();
                    else
                        element = (GeckoImageElement)wbBrowser.Document.GetElementById(xpath);
                    GeckoSelection selection = wbBrowser.Window.Selection;
                    selection.SelectAllChildren(element);
                    wbBrowser.CopyImageContents();
                    if (Clipboard.ContainsImage())
                    {
                        Image img = Clipboard.GetImage();
                        img.Save(location, System.Drawing.Imaging.ImageFormat.Png);
                        result = true;
                    }
                }
            }
            catch { result = false; }

            return result;
        }
        public void explorer(string path)
        {
            string argument = "/select, \"" + path + "\"";
            System.Diagnostics.Process.Start("explorer.exe", argument);
        }
        public void loadHTML(string path)
        {
            go(path);
        }

        public string textToJSON(string text)
        {
            return text;
        }

        public string GetAccountBy(string name)
        {
            string result = "";

            if (User.Current != null)
            {
                Account account = new Account();
                result = account.GetByJSON(name);
            }
            return result;
        }

        public string sendEmail(string name, string email, string subject, string content)
        {
            string result = "False";

            if (User.Current != null && !string.IsNullOrEmpty(name))
            {
                MailServer mailserver = new MailServer();
                var item = mailserver.GetBy(name);
                if (item != null)
                {
                    mailserver.Username = item.Username;
                    mailserver.Password = item.Password;
                    mailserver.Server = item.Server;
                    mailserver.Port = item.Port;

                    result = mailserver.SendEmail(email, subject, content).ToString();
                }
            }
            return result;
        }

        public void CaptchaBorder(string xpath, string style)
        {

        }

        [ComImport, InterfaceType((short)1), Guid("3050F669-98B5-11CF-BB82-00AA00BDCE0B")]
        private interface IHTMLElementRenderFixed
        {
            void DrawToDC(IntPtr hdc);
            void SetDocumentPrinter(string bstrPrinterName, IntPtr hdc);
        }

        public void SaveImageFromElement(string xpath, string path)
        {
            saveImage(xpath, path);
        }
        public void Abort()
        {
            IsStop = true;
            Stop();
        }

        public void UnAbort()
        {
            IsStop = false;
        }
        public void Stop()
        {
        }
        public string GetDatabases(string name)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetDatabases(name);

            return result;
        }

        public string GetTables(string name, string dbName)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetTables(name, dbName);

            return result;
        }

        public string GetColumns(string name, string dbName, string table)
        {
            string result = "";

            Connection conn = new Connection();
            result = conn.GetColumns(name, dbName, table);

            return result;
        }

        public string GetRows(string name, string dbName, string sql)
        {
            string result = "";

            Connection conn = new Connection();

            result = conn.GetRows(name, dbName, sql);

            return result;
        }

        public int ExcuteQuery(string name, string dbName, string sql)
        {
            int result = -1;

            Connection conn = new Connection();
            try
            {
                result = conn.ExcuteQuery(name, dbName, sql);
            }
            catch { }
            return result;
        }
        public void AddElement(string path, string node1, string node2, string text)
        {
            XDocument xdoc = XDocument.Load(path);
            var item = xdoc.Element(node1).Element(node2);
            if (item != null)
                item.Add(new XElement("w", text));
            else
                xdoc.Element(node1).Add(new XElement(node2, new XElement("w", text)));

            xdoc.Save(path);
        }

        public bool CheckXmlElement(string path, string node, string text)
        {
            bool result = false;

            XDocument xdoc = XDocument.Load(path);

            var rs = (from w in xdoc.Descendants(node) where w.Value == text select w).FirstOrDefault();
            if (rs != null) result = true;

            return result;
        }

        public string GetParentElement(string path, string node, string text)
        {
            string result = string.Empty;

            XDocument xdoc = XDocument.Load(path);

            var rs = (from w in xdoc.Descendants(node) where w.Value == text select w).FirstOrDefault();
            if (rs != null) result = rs.Parent.Name.LocalName;

            return result;
        }

        public string GetXmlElement(string path, string node)
        {
            string result = string.Empty;
            List<string> data = new List<string>();
            XDocument xdoc = XDocument.Load(path);
            var list = xdoc.Root.Nodes();
            foreach (XElement elem in list)
            {
                data.Add(elem.Name.LocalName);
            }
            result = string.Join(",", data);
            return result;
        }

        public string ExtractUsingRegularExpression(string pattern, string groupName)
        {
            string result = string.Empty;

            GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
            if (wb != null)
            {
                string doc = wb.Document.Body.TextContent;
                Match m = Regex.Match(doc, pattern);
                if (m.Success)
                {
                    if (m.Groups.Count > 0)
                    {
                        result = m.Groups[groupName].Value;
                    }
                }
            }

            return result;
        }


        private string extractData(GeckoHtmlElement ele, string attribute)
        {
            var result = string.Empty;

            if (ele != null)
            {
                var tmp = ele.GetAttribute(attribute);
                /*if (tmp == null)
                {
                    tmp = extractData(ele.Parent, attribute);
                }*/
                if (tmp != null)
                    result = tmp.Trim();
            }

            return result;
        }
        public string getCurrentPath()
        {
            string result = "";
            try
            {
                result = Application.StartupPath;
            }
            catch { }
            return result;
        }
        #endregion
        #region Excel

        public string readCellExcel(string filePath, string isheetname, int irow, int icolumn)
        {
            string result = "";
            try
            {
                using (StreamReader input = new StreamReader(filePath))
                {
                    NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(new NPOI.POIFS.FileSystem.POIFSFileSystem(input.BaseStream));
                    if (null == workbook)
                    {
                        result = "";
                    }

                    NPOI.HSSF.UserModel.HSSFFormulaEvaluator formulaEvaluator = new NPOI.HSSF.UserModel.HSSFFormulaEvaluator(workbook);
                    NPOI.HSSF.UserModel.HSSFDataFormatter dataFormatter = new NPOI.HSSF.UserModel.HSSFDataFormatter(new CultureInfo("vi-VN"));

                    NPOI.SS.UserModel.ISheet sheet = workbook.GetSheet(isheetname);
                    NPOI.SS.UserModel.IRow row = sheet.GetRow(irow);

                    if (row != null)
                    {
                        short minColIndex = row.FirstCellNum;
                        short maxColIndex = row.LastCellNum;

                        if (icolumn >= minColIndex || icolumn <= maxColIndex)
                        {
                            NPOI.SS.UserModel.ICell cell = row.GetCell(icolumn);
                            if (cell != null)
                            {
                                if (cell.CellType == NPOI.SS.UserModel.CellType.FORMULA)
                                {
                                    result = cell.StringCellValue;
                                }
                                else
                                {
                                    result = dataFormatter.FormatCellValue(cell, formulaEvaluator);
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                string test = ex.Message;
            }

            return result;
        }

        public void writeCellExcel(string filePath, string sheetname, string cellName, string value)
        {
            NPOI.HSSF.UserModel.HSSFWorkbook workbook;
            if (!File.Exists(filePath))
            {
                workbook = NPOI.HSSF.UserModel.HSSFWorkbook.Create(NPOI.HSSF.Model.InternalWorkbook.CreateWorkbook());
                var sheet = workbook.CreateSheet(sheetname);
                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }
            using (var input = new StreamReader(filePath))
            {
                workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(new NPOI.POIFS.FileSystem.POIFSFileSystem(input.BaseStream));
                if (workbook != null)
                {
                    var sheet = workbook.GetSheet(sheetname);
                    NPOI.SS.Util.CellReference celRef = new NPOI.SS.Util.CellReference(cellName);
                    var row = sheet.GetRow(celRef.Row);
                    if (row == null)
                        row = sheet.CreateRow(celRef.Row);

                    var cell = row.GetCell(celRef.Col);
                    if (cell == null)
                        cell = row.CreateCell(celRef.Col);

                    cell.SetCellValue(value);
                }
            }

            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Write))
            {
                workbook.Write(file);
                file.Close();
            }
        }

        #endregion

        delegate void VoidArgDelegate();
        delegate void LoginDelegate(string email, string password);
        private void BeginLoading()
        {
            if(creatorList.InvokeRequired)
            {
                VoidArgDelegate d = new VoidArgDelegate(BeginLoading);
                this.Invoke(d);
            }
            else
            {
                creatorList.Items.Clear();
                creatorList.Enabled = false;
                loadingBar.Visible = true;
                BtnReloadList.Enabled = false;
                BtnStartStop.Enabled = false;

                BtnPrevPage.Visible = false;
                BtnNextPage.Visible = false;
                txtCurPage.Visible = false;
                labelMaxPage.Visible = false;

                IsLoading = true;
            }
        }

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);
        private void LoadByRequest()
        {
            string data = string.Empty;
            string url = @"https://graphtreon.com/api/creators";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                data = reader.ReadToEnd();
                data = data.Replace("\\/", " per ");

                int startPos = 8;
                int endPos = data.Length - 1;

                string jsonContent = data.Substring(startPos, endPos - startPos);
                if (jsonContent.Length == 0)
                    return;

                dynamic infoArray = JsonConvert.DeserializeObject(jsonContent);
                creatorInfos.Clear();

                for (int i = 0; i < infoArray.Count; i++)
                {
                    string link = infoArray[i].link;
                    string seperator = "_&_";
                    string nsfwString = "_adlt_";

                    int pos1 = link.IndexOf(seperator, 0);
                    int pos2 = pos1 + seperator.Length;
                    int pos3 = link.IndexOf(seperator, pos2);
                    int pos4 = pos3 + seperator.Length;

                    if (pos1 == -1 || pos3 == -1)
                        continue;

                    string name = link.Substring(pos2, pos3 - pos2);
                    string linkname = link.Substring(0, pos1);
                    string desc = link.Substring(pos4);
                    string type = "sfw";

                    if (desc.Contains(nsfwString))
                    {
                        desc = desc.Substring(0, desc.Length - nsfwString.Length);
                        type = "nsfw";
                    }

                    CreatorInfo creatorInfo = new CreatorInfo();
                    creatorInfo.rank = infoArray[i].rank;
                    creatorInfo.name = name + " is creating " + desc;
                    creatorInfo.patrons = infoArray[i].patrons;

                    if (infoArray[i].earnings == null)
                        creatorInfo.earnings = "";
                    else
                        creatorInfo.earnings = "$" + infoArray[i].earnings;

                    if (infoArray[i].supportPerPatron == null)
                        creatorInfo.support = "";
                    else
                        creatorInfo.support = "$" + infoArray[i].supportPerPatron;

                    creatorInfo.linkUrl = "https://www.graphtreon.com/creator/" + linkname;
                    creatorInfo.type = type;
                    creatorInfo.status = "";

                    creatorInfos.Add(creatorInfo);
                }
            }
        }

        private void LoadFromBrowser()
        {
            //Open Website
            go("https://graphtreon.com/all-patreon-creators");

            // Wait while loading
            var elem = "";
            while (elem == "")
            {
                sleep(1, false);
                elem = extract("//*[@id='allCreatorsTable']/tbody[1]/tr[1]/td[1]", "text");
            }

            // Write the list of creators to the excel file
            int i = 1;
            int num_columns = 5;

            creatorInfos.Clear();

            while (IsLoading)
            {
                string[] row = new string[num_columns + 2];

                elem = extract("//*[@id='allCreatorsTable']/tbody[1]/tr[" + i + "]/td[1]", "text");
                if (elem == "")
                {
                    var button_class = extract("//*[@id='allCreatorsTable_next']", "class");
                    if (button_class == "next paginate_button")
                    {
                        click("//*[@id='allCreatorsTable_next']");
                        sleep(0.3, false);
                        i = 1;
                        continue;
                    }
                    else
                        break;
                }

                for (int j = 1; j <= num_columns + 2; j++)
                {
                    if (j == num_columns + 2)
                        row[j - 1] = extract("//*[@id='allCreatorsTable']/tbody[1]/tr[" + i + "]/td[2]/a[1]/span[2]", "text");
                    else if (j == num_columns + 1)
                        row[j - 1] = extract("//*[@id='allCreatorsTable']/tbody[1]/tr[" + i + "]/td[2]/a[1]", "href");
                    else
                        row[j - 1] = extract("//*[@id='allCreatorsTable']/tbody[1]/tr[" + i + "]/td[" + j + "]", "text");
                }

                CreatorInfo creatorInfo = new CreatorInfo();
                creatorInfo.rank = row[0];
                creatorInfo.name = row[1];
                creatorInfo.patrons = row[2];
                creatorInfo.earnings = row[3];
                creatorInfo.support = row[4];
                creatorInfo.linkUrl = row[5];
                creatorInfo.type = row[6];

                creatorInfos.Add(creatorInfo);
                i++;
            }
        }

        private void FinishLoading()
        {
            if (creatorList.InvokeRequired)
            {
                VoidArgDelegate d = new VoidArgDelegate(FinishLoading);
                this.Invoke(d);
            }
            else
            {
                loadingBar.Visible = false;
                creatorList.Enabled = true;

                BtnReloadList.Enabled = true;
                BtnStartStop.Enabled = IsLoggedIn;

                BtnPrevPage.Visible = true;
                BtnNextPage.Visible = true;
                txtCurPage.Visible = true;
                labelMaxPage.Visible = true;

                UpdateFilter();
                txtCurPage.Text = "1";
                GotoPage(1);

                IsLoading = false;
            }
        }

        private void ShowFilteredList()
        {
            if (creatorList.InvokeRequired)
            {
                VoidArgDelegate d = new VoidArgDelegate(ShowFilteredList);
                this.Invoke(d);
            }
            else
            {
                creatorList.Items.Clear();

                if (curPage < 1)
                    curPage = 1;

                int startIndex = (curPage - 1) * countPerPage;

                if (startIndex < filteredCreatorIndices.Count)
                {
                    for(int i = startIndex; i < filteredCreatorIndices.Count && i < curPage * countPerPage; i++)
                    {
                        string[] elems;
                        CreatorInfo info = creatorInfos[filteredCreatorIndices[i]];
                        info.ToStringArray(out elems);

                        ListViewItem viewItem = new ListViewItem(elems);
                        creatorList.Items.Add(viewItem);
                    }
                }
            }
        }

        private void UpdateFilter()
        {
            string filterText = txtFilter.Text;
            int maxPatron;

            if (int.TryParse(txtPatronNum.Text, out maxPatron) == false)
                maxPatron = int.MaxValue;

            filteredCreatorIndices.Clear();

            for(int i = 0; i < creatorInfos.Count; i++)
            {
                CreatorInfo info = creatorInfos[i];

                int num_patrons;
                if (int.TryParse(info.patrons, NumberStyles.Number, CultureInfo.InvariantCulture, out num_patrons) == false)
                    num_patrons = 0;

                string[] elems;
                info.ToStringArray(out elems);
                bool hasFilterText = false;

                if ((curTypeFilter == "" || info.type == curTypeFilter) && num_patrons <= maxPatron)
                {
                    foreach (string elem in elems)
                    {
                        if (filterText == "" || elem.Contains(filterText))
                        {
                            hasFilterText = true;
                            break;
                        }
                    }

                    if (hasFilterText)
                        filteredCreatorIndices.Add(i);
                }
            }

            curMaxPage = (int)Math.Ceiling((double)filteredCreatorIndices.Count / countPerPage);
            if (curMaxPage == 0)
                curMaxPage = 1;

            labelMaxPage.Text = "of " + curMaxPage.ToString();
        }

        private void PostComments()
        {
            if (tabMain.InvokeRequired)
            {
                VoidArgDelegate d = new VoidArgDelegate(PostComments);
                this.Invoke(d);
            }
            else
            {
                int maxComments = int.MaxValue, commentDelay = 0, creatorDelay = 0;
                bool bCheckPrevious = chkPreviousCheck.Checked;

                RegistryKey curUser = Registry.CurrentUser.OpenSubKey("SOFTWARE\\PatreonCommentor");
                if (curUser == null)
                    Registry.CurrentUser.CreateSubKey("SOFTWARE\\PatreonCommentor");

                var binaryTime = (long)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\PatreonCommentor", "CommentedDate", DateTime.Now.ToBinary());
                DateTime savedTime = DateTime.FromBinary(binaryTime);
                int curCommentedNumber = (int)Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\PatreonCommentor", "CommentedNumber", 0);

                if (savedTime.Date != DateTime.Now.Date)
                    curCommentedNumber = 0;

                if (int.TryParse(txtMaxComments.Text, out maxComments) == false) maxComments = int.MaxValue;
                if (int.TryParse(txtCommentDelay.Text, out commentDelay) == false) commentDelay = 0;
                if (int.TryParse(txtCreatorDelay.Text, out creatorDelay) == false) creatorDelay = 0;

                if (txtMaxComments.Text == "")
                    maxComments = int.MaxValue;

                foreach(int creatorIndex in filteredCreatorIndices)
                {
                    if (IsRunning == false)
                        return;

                    CreatorInfo creatorInfo = creatorInfos[creatorIndex];

                    if(swCreators.IsRunning )
                    {
                        swCreators.Stop();

                        if (swCreators.Elapsed.TotalSeconds < creatorDelay)
                            sleep((double)(creatorDelay - swCreators.Elapsed.TotalSeconds), false);
                    }

                    swCreators.Start();
                    creatorInfo.status = "Processing...";
                    ShowFilteredList();

                    go(creatorInfo.linkUrl);
                    sleep(100, true);

                    if (IsRunning == false)
                        return;

                    click("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/section[1]/div[1]/div[1]/div[1]/div[1]/div[1]/a[1]/img[1]");
                    sleep(100, true);

                    if (IsRunning == false)
                        return;

                    GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                    wb.Focus();

                    string htmltext = "";

                    GeckoHtmlElement element = null;
                    var geckoDomElement = wb.Document.DocumentElement;
                    if (geckoDomElement is GeckoHtmlElement)
                    {
                        element = (GeckoHtmlElement)geckoDomElement;
                        htmltext = element.InnerHtml;
                    }

                    #region Checks the last post
                    {
                        int postStart_cur = htmltext.IndexOf("div class=\"_2q0-components-Post-PostHeader--fullName\"", 0);
                        int postStart_next = htmltext.IndexOf("div class=\"_2q0-components-Post-PostHeader--fullName\"", postStart_cur + 1);

                        if (postStart_cur == -1 || postStart_next == -1)
                        {
                            creatorInfo.status = "Finished";
                            continue;
                        }

                        GeckoNode creatorNameNode = wb.Document.EvaluateXPath("//div[@class='_2q0-components-Post-PostHeader--fullName']").GetSingleNodeValue();
                        if (creatorNameNode != null && ((creatorNameNode.TextContent == curUserName) || (commentSetting.blackListItems.Find(x => x == creatorNameNode.TextContent) != null)))
                        {
                            continue;
                        }

                        int lockedPost = htmltext.IndexOf("Unlock it now", 0);

                        if (lockedPost != -1 && lockedPost > postStart_cur && lockedPost < postStart_next)
                        {
                            creatorInfo.status = "Finished";
                            continue;
                        }
                    }
                    #endregion

                    #region Get User Id
                    string strUserid = "";
                    {
                        string strStart = "var _user_id = \"";
                        string strEnd = "\"";
                        if (htmltext.Contains(strStart) && htmltext.Contains(strEnd))
                        {
                            int Start = htmltext.IndexOf(strStart, 0) + strStart.Length;
                            int End = htmltext.IndexOf(strEnd, Start);

                            if (End == -1)
                                continue;

                            strUserid = htmltext.Substring(Start, End - Start);
                        }
                    }
                    #endregion

                    #region Loads all comments
                    bool hasLoadedAll = true;
                    {
                        GeckoTextAreaElement commentTextArea = (GeckoTextAreaElement)wb.Document.EvaluateXPath("//textarea[@placeholder='Write a comment ...']").GetSingleNodeValue();
                        if (commentTextArea == null)
                        {
                            creatorInfo.status = "Finished";
                            continue;
                        }

                        string xpath_comment = GetXpath(commentTextArea);
                        string xpath_loadMore = GetUpperPath(xpath_comment, 4) + "/div[1]/a[1]";
                        string xpath_curComments = GetUpperPath(xpath_comment, 4) + "/div[1]/span[1]";

                        GeckoAnchorElement btn_loadMore = (GeckoAnchorElement)wb.Document.EvaluateXPath(xpath_loadMore).GetSingleNodeValue();
                        GeckoElement span_curComments = (GeckoElement)wb.Document.EvaluateXPath(xpath_curComments).GetSingleNodeValue();
                        string curCommentsText = "";

                        while (btn_loadMore == null && span_curComments != null && span_curComments.TextContent.Contains("0 of"))
                        {
                            sleep(0.2f, false);
                            btn_loadMore = (GeckoAnchorElement)wb.Document.EvaluateXPath(xpath_loadMore).GetSingleNodeValue();
                            span_curComments = (GeckoElement)wb.Document.EvaluateXPath(xpath_curComments).GetSingleNodeValue();
                        }

                        if (bCheckPrevious)
                        {
                            while (btn_loadMore != null && btn_loadMore.TextContent == "Load more comments")
                            {
                                if (span_curComments == null || curCommentsText == span_curComments.TextContent)
                                {
                                    hasLoadedAll = false;
                                    break;
                                }

                                if (IsRunning == false)
                                    return;

                                curCommentsText = span_curComments.TextContent;
                                btn_loadMore.ScrollIntoView(false);
                                btn_loadMore.Click();
                                sleep(1.5f, false);
                                btn_loadMore = (GeckoAnchorElement)wb.Document.EvaluateXPath(xpath_loadMore).GetSingleNodeValue();
                            }
                        }
                        else
                        {
                            if (btn_loadMore != null && btn_loadMore.TextContent == "Load more comments")
                                hasLoadedAll = false;
                            else
                                hasLoadedAll = true;
                        }
                    }
                    #endregion

                    {
                        GeckoTextAreaElement commentTextArea = (GeckoTextAreaElement)wb.Document.EvaluateXPath("//textarea[@placeholder='Write a comment ...']").GetSingleNodeValue();
                        if (commentTextArea == null)
                        {
                            creatorInfo.status = "Finished";
                            continue;
                        }

                        if (IsRunning == false)
                            return;

                        string xpath_comment = GetXpath(commentTextArea);
                        int commentorIndex = 1;
                        bool isAlreadyCommented = false;
                        string postTitle = "";

                        if (hasLoadedAll == false)
                            commentorIndex = 2;

                        #region Checks all comments
                        {
                            while (true)
                            {
                                string xpath_commentor = GetUpperPath(xpath_comment, 4) + "/div[" + commentorIndex + "]/div[1]/div[1]/div[1]/div[2]/div[1]/a[1]";

                                string commentorLink = extract(xpath_commentor, "href");
                                string myLink = "/user?u=" + strUserid;

                                if (commentorLink == "" )
                                    break;
                                if (IsRunning == false)
                                    return;

                                if (commentorLink == myLink)
                                {
                                    isAlreadyCommented = true;
                                    break;
                                }

                                commentorIndex++;
                            }

                            if (isAlreadyCommented)
                            {
                                creatorInfo.status = "Finished";
                                continue;
                            }
                        }
                        #endregion

                        #region Get the post content and determine the comment
                        string commentText = "";

                        {
                            int[] tagPos = new int[4];
                            string[] tagName = new string[4];
                            string postContent = "";

                            int commentPos = htmltext.IndexOf(commentTextArea.OuterHtml, 0);
                            int postStartPos = htmltext.LastIndexOf("_1VS-components-Post-PostHeader--authorInfoContainer", commentPos);

                            if (postStartPos == -1)
                                postStartPos = 0;

                            tagPos[0] = htmltext.IndexOf("span class=\"_3jG-components-Post--title\"", postStartPos);
                            tagPos[1] = htmltext.IndexOf("div class=\"_1V5-components-Post--postContentWrapper text\"", postStartPos);
                            tagPos[2] = htmltext.IndexOf("div class=\"_3vM-components-GenericEmbed--embedSubject\"", postStartPos);
                            tagPos[3] = htmltext.IndexOf("div class=\"_2Jc-components-GenericEmbed--embedDescription\"", postStartPos);

                            tagName[0] = "span";
                            tagName[1] = "div";
                            tagName[2] = "div";
                            tagName[3] = "div";

                            for (int j = 0; j < 4; j++)
                            {
                                if (tagPos[j] != -1 && tagPos[j] < commentPos)
                                {
                                    string data_reactid = "";

                                    string strStart = "data-reactid=\"";
                                    string strEnd = "\"";
                                    if (htmltext.Contains(strStart) && htmltext.Contains(strEnd))
                                    {
                                        int Start = htmltext.IndexOf(strStart, tagPos[j]);
                                        if (Start == -1)
                                            continue;

                                        Start += strStart.Length;
                                        int End = htmltext.IndexOf(strEnd, Start);
                                        if (End == -1)
                                            continue;

                                        data_reactid = htmltext.Substring(Start, End - Start);
                                    }

                                    string tagXPath = "//" + tagName[j] + "[@data-reactid='" + data_reactid + "']";
                                    GeckoElement elem = (GeckoElement)wb.Document.EvaluateXPath(tagXPath).GetSingleNodeValue();

                                    if (elem == null)
                                        continue;

                                    postContent += elem.TextContent;

                                    if (j == 0 || j == 2)       // If the element is post title
                                        postTitle = elem.TextContent;
                                }
                            }

                            StringList corrComments = new StringList();

                            foreach (string keyword in commentSetting.keywordMap.Keys)
                            {
                                if (postContent.Contains(keyword))
                                {
                                    int numberInList = commentSetting.keywordMap[keyword];
                                    StringList comments = commentSetting.settingRecords[numberInList].GetComments();
                                    corrComments.AddRange(comments);
                                }
                            }

                            if (corrComments.Count == 0)
                            {
                                if (commentSetting.genericComments.Count == 0)
                                {
                                    MessageBox.Show("No suitable comments found. Please add some generic comments.");
                                    creatorInfo.status = "Finished";
                                    continue;
                                }
                                else
                                    commentText = SelectRandomTextInList(commentSetting.genericComments);
                            }
                            else
                                commentText = SelectRandomTextInList(corrComments);
                        }

                        #endregion

                        #region Checks the elapsed time between comments.
                        {
                            if (swComments.IsRunning)
                            {
                                swComments.Stop();

                                if (swComments.Elapsed.TotalSeconds < commentDelay)
                                    sleep((double)(commentDelay - swComments.Elapsed.TotalSeconds), false);
                            }
                            swComments.Start();
                        }
                        #endregion

                        #region Post the comment
                        {
                            if (curCommentedNumber >= maxComments)
                            {
                                MessageBox.Show("Already posted max number of comments today. Please try again tomorrow.");
                                return;
                            }

                            commentTextArea.Focus();
                            commentTextArea.ScrollIntoView(false);

                            IntPtr child1handle = FindWindowEx(wb.Handle, IntPtr.Zero, "MozillaWindowClass", "");
                            IntPtr child2handle = FindWindowEx(child1handle, IntPtr.Zero, "MozillaWindowClass", "");

                            StartTypingThread(child2handle, commentText);
                            sleep(3, false);
                            
                            // Add item to the comment history list.

                            string[] elems = new string[3];

                            elems[0] = creatorInfo.name;
                            elems[1] = postTitle;
                            elems[2] = commentText;

                            ListViewItem item = new ListViewItem(elems);
                            commentHistoryList.Items.Add(item);

                            // Save the number of today's commented posts.
                            curCommentedNumber++;
                            Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\PatreonCommentor", "CommentedDate", DateTime.Now.ToBinary(), RegistryValueKind.QWord);
                            Registry.SetValue("HKEY_CURRENT_USER\\SOFTWARE\\PatreonCommentor", "CommentedNumber", curCommentedNumber);

                            #region Mark Like
                            {
                                XPathResult like_result = wb.Document.EvaluateXPath("//div[@class='_24n-components-LikesCounter--container sc-htoDjs hfMUI']");
                                if(like_result != null)
                                {
                                    GeckoHtmlElement likeNode = (GeckoHtmlElement)like_result.GetSingleNodeValue();
                                    if(likeNode != null)
                                    {
                                        likeNode.Click();
                                        sleep(1, false);
                                    }
                                }  
                            } 
                            #endregion Mark Like

                        }
                        #endregion                       

                    }
                    // Set the posted status in the list
                    creatorInfo.status = "Finished";
                    ShowFilteredList();
                }
                swCreators.Stop();
            }
        }

        private string SelectRandomTextInList(StringList stringList)
        {
            int index = rnd.Next(0, stringList.Count);

            return stringList[index];
        }

        private void LoadCreators()
        {
            BeginLoading();
            //LoadFromBrowser();
            LoadByRequest();
            FinishLoading();
        }

        private void StartProcess()
        {
            for (int i = 0; i < filteredCreatorIndices.Count; i++)
                creatorInfos[filteredCreatorIndices[i]].status = "Waiting";
            txtCurPage.Text = "1";
            ShowFilteredList();

            BtnStartStop.Text = "Stop";
            IsRunning = true;

            PostComments();

            BtnStartStop.Text = "Start";
            BtnStartStop.Enabled = true;
            IsRunning = false;
            for (int i = 0; i < filteredCreatorIndices.Count; i++)
                creatorInfos[filteredCreatorIndices[i]].status = "";
            ShowFilteredList();

            //MessageBox.Show("Successfully Finished!");
        }

        private void TypingThreadProc(IntPtr hWnd, string text)
        {
            //MessageBox.Show(Thread.CurrentThread.Name);
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                IntPtr val = new IntPtr((Int32)c);
                SendMessage((IntPtr)hWnd, WM_CHAR, val, 1);
                Thread.Sleep(30);
            }

            Thread.Sleep(200);
            SendMessage((IntPtr)hWnd, WM_KEYDOWN, (IntPtr)VK_RETURN, 1);
            SendMessage((IntPtr)hWnd, WM_KEYUP, (IntPtr)VK_RETURN, 1);
        }

        private void StartTypingThread(IntPtr hWnd, string text)
        {
            /*ThreadStart starter = delegate { TypingThreadProc(hWnd, text); };
            var thread = new Thread(starter);
            thread.IsBackground = true;
            thread.Start();*/
            TypingThreadProc(hWnd, text);
        }

        private void LoginThreadProc(string email, string password)
        {
            if (tabMain.InvokeRequired)
            {
                LoginDelegate d = new LoginDelegate(LoginThreadProc);
                this.Invoke(d, email, password);
            }
            else
            {
                go("https://www.patreon.com/login");
                sleep(100, true);

                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                string curUrl = wb.Url.AbsolutePath;
                wb.Focus();

                if (curUrl == "/login")
                {
                    IntPtr child1handle = FindWindowEx(wb.Handle, IntPtr.Zero, "MozillaWindowClass", "");
                    IntPtr child2handle = FindWindowEx(child1handle, IntPtr.Zero, "MozillaWindowClass", "");

                    wb.ExecuteJQuery("function getElementByXpath(path) {  return document.evaluate(path, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;        } ");

                    var element = (GeckoInputElement)wb.GetElementByJQuery("getElementByXpath(\"//input[@name='email']\");");
                    element.Focus();
                    StartTypingThread(child2handle, email);

                    element = (GeckoInputElement)wb.GetElementByJQuery("getElementByXpath(\"//input[@name='password']\");");
                    element.Focus();
                    StartTypingThread(child2handle, password);
                    sleep(20, true);

                    curUrl = wb.Url.AbsolutePath;
                    if (curUrl != "/login")
                        curEmail = email;
                    else
                        curEmail = "";
                }
            }
        }

        private void SetLoginState(bool bLoggedIn)
        {
            IsLoggedIn = bLoggedIn;

            if (bLoggedIn)
            {
                BtnLogInOut.Text = "Log Out";
                txtEmail.Text = curEmail;
                txtEmail.Enabled = false;
                txtPassword.Enabled = false;
                BtnStartStop.Enabled = true;
            }
            else
            {
                BtnLogInOut.Text = "Log In";
                txtEmail.Enabled = true;
                txtPassword.Enabled = true;
                BtnStartStop.Enabled = false;
                BtnStartStop.Enabled = false;
            }
        }

        private void Login(string email, string password)
        {
            /*ThreadStart starter = delegate { LoginThreadProc(email, password); };
            Thread loginThread = new Thread(starter);
            loginThread.Name = "LoginThread";
            loginThread.Start();*/
            LoginThreadProc(email, password);
        }

        private void Logout()
        {
            go("https://www.patreon.com/logout");
            sleep(100, true);
            //go("https://www.patreon.com/login");
            //sleep(100, true);
        }

        private void GetUserInfo()
        {
            if(tabMain.InvokeRequired)
            {
                VoidArgDelegate d = new VoidArgDelegate(GetUserInfo);
                this.Invoke(d);
            }
            else
            {
                go("https://www.patreon.com/settings/profile");
                sleep(100, true);

                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                string curUrl = wb.Url.AbsolutePath;

                if (curUrl == "/settings/profile")
                {
                    curUserName = extract("//*[@id='name']", "value");
                    curEmail = extract("//*[@id='email']", "value");
                }
                else
                {
                    curUserName = curEmail = "";
                }

                txtEmail.Text = curEmail;
            }
        }

        private string GetUpperPath(string xPath, int nGoUpperTimes = 1)
        {
            string res = xPath;

            for(int i = 0; i < nGoUpperTimes; i++)
            {
                int pos = res.LastIndexOf("/");

                if (pos == -1)
                {
                    res = "";
                    break;
                }
                else
                    res = res.Substring(0, pos);
            }

            return res;
        }

        public void AddBlankKeyword()
        {
            SettingRecord settingRecord = new SettingRecord();
            commentSetting.settingRecords.Add(settingRecord);

            keywordList.Items.Add("");
        }

        const int WM_KEYDOWN = 0x0100;
        const int WM_KEYUP = 0x0101;
        const int WM_CHAR = 0x102;
        const int VK_RETURN = 0x0D;

        private void Type(string text, IntPtr handle)
        {
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                IntPtr val = new IntPtr((Int32)c);
                SendMessage((IntPtr)handle, WM_KEYDOWN, val, 1);
                SendMessage((IntPtr)handle, WM_CHAR, val, 1);
                SendMessage((IntPtr)handle, WM_KEYUP, val, 1);
            }
        }
        private void PressEnter(IntPtr handle)
        {
            SendMessage((IntPtr)handle, WM_KEYDOWN, (IntPtr)VK_RETURN, 1);
            SendMessage((IntPtr)handle, WM_KEYUP, (IntPtr)VK_RETURN, 1);
        }

        public void LoadCommentSettings(string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Open))
            {
                if (stream == null)
                    return;

                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                commentSetting = (CommentSetting)bformatter.Deserialize(stream);
            }

            if (commentSetting.keywordMap.Count == 0)
                return;

            int keywordCnt = commentSetting.keywordMap.Values.Max() + 1;

            for(int i = 0; i < keywordCnt; i++)
                keywordList.Items.Add("");
            
            foreach(string keyword in commentSetting.keywordMap.Keys)
            {
                int index = commentSetting.keywordMap[keyword];
                keywordList.Items[index] = commentSetting.settingRecords[index].GetKeywordsInString();
            }

            foreach (string comment in commentSetting.genericComments)
                genericList.Items.Add(comment);

            foreach (string blackitem in commentSetting.blackListItems)
                blackList.Items.Add(blackitem);

            txtMaxComments.Text = commentSetting.maxComments;
            txtCommentDelay.Text = commentSetting.commentDelay;
            txtCreatorDelay.Text = commentSetting.creatorDelay;
        }

        public void SaveCommentSettings(string fileName)
        {
            commentSetting.maxComments = txtMaxComments.Text;
            commentSetting.commentDelay = txtCommentDelay.Text;
            commentSetting.creatorDelay = txtCreatorDelay.Text;

            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bformatter.Serialize(stream, commentSetting);
            }
        }

        #region Control Message Handlers

        private void BtnLogInOut_Click(object sender, EventArgs e)
        {
            if (IsLoading)
            {
                MessageBox.Show("Can not log in/out while loading the list.");
                return;
            }

            if (IsLoggedIn)
            {
                if (IsRunning)
                {
                    MessageBox.Show("Can not log out while posting comments.");
                    return;
                }

                BtnLogInOut.Enabled = false;

                Logout();

                GeckoWebBrowser wb = (GeckoWebBrowser)GetCurrentWB();
                string curUrl = wb.Url.AbsolutePath;

                if (curUrl == "/")
                    SetLoginState(false);

                BtnLogInOut.Enabled = true;
            }
            else
            {
                BtnLogInOut.Enabled = false;

                Login(txtEmail.Text, txtPassword.Text);

                if(curEmail == "")
                {
                    MessageBox.Show("Login failed! Please check your email and password.");
                    SetLoginState(false);
                    BtnLogInOut.Enabled = true;
                    return;
                }

                SetLoginState(true);
                BtnLogInOut.Enabled = true;
            }
        }

        private void BtnReloadList_Click(object sender, EventArgs e)
        {
            if (BtnLogInOut.Enabled == false)
            {
                MessageBox.Show("Can not reload the list while logging in/out.");
                return;
            }
            if (IsRunning)
            {
                MessageBox.Show("Can not reload the list while posting comment.");
                return;
            }

            Thread loadingThread = new Thread(LoadCreators);
            loadingThread.Start();
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if(IsRunning)
            {
                IsStop = false;
                BtnStartStop.Enabled = false;
                IsRunning = false;
            }
            else
            {
                BtnStartStop.Text = "Stop";
                IsRunning = true;
                //Thread newThread = new Thread(StartProcess);
                //newThread.Start();
                StartProcess();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveCommentSettings("settings.dat");

            Process[] targetProcesses = Process.GetProcessesByName("plugin-container");
            if (targetProcesses != null)
            {
                foreach (Process targetProcess in targetProcesses)
                    targetProcess.Kill();
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Thread loadingThread = new Thread(LoadCreators);
            loadingThread.Start();
        }

        private void BtnShowAll_CheckedChanged(object sender, EventArgs e)
        {
            if (BtnShowAll.Checked)
            {
                curTypeFilter = "";
                UpdateFilter();
                txtCurPage.Text = "1";
                GotoPage(1);
            }
        }

        private void BtnShowNSFW_CheckedChanged(object sender, EventArgs e)
        {
            if (BtnShowNSFW.Checked)
            {
                curTypeFilter = "nsfw";
                UpdateFilter();
                txtCurPage.Text = "1";
                GotoPage(1);
            }
        }

        private void BtnShowSFW_CheckedChanged(object sender, EventArgs e)
        {
            if (BtnShowSFW.Checked)
            {
                curTypeFilter = "sfw";
                UpdateFilter();
                txtCurPage.Text = "1";
                GotoPage(1);
            }
        }

        private void txtPatronNum_TextChanged(object sender, EventArgs e)
        {
            UpdateFilter();
            txtCurPage.Text = "1";
            GotoPage(1);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            UpdateFilter();
            txtCurPage.Text = "1";
            GotoPage(1);
        }

        private void txtPatronNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BtnAddKeyword_Click(object sender, EventArgs e)
        {
            AddTextForm addTextForm = new AddTextForm();
            addTextForm.SetCaption("New Keyword");
            addTextForm.SetDescription("Please enter the new keyword.");

            if(addTextForm.ShowDialog() == DialogResult.OK)
            {
                if (addTextForm.GetNewText().Length > 0)
                {
                    int selectedIndex = keywordList.SelectedIndex;
                    string newText = addTextForm.GetNewText();

                    if(commentSetting.keywordMap.ContainsKey(newText))
                    {
                        MessageBox.Show("The keyword was already added.");
                        return;
                    }

                    SettingRecord settingRecord = commentSetting.settingRecords[selectedIndex];
                    settingRecord.AddKeyword(newText);
                    keywordList.Items[selectedIndex] = settingRecord.GetKeywordsInString();
                    commentSetting.keywordMap.Add(newText, selectedIndex);

                    if (selectedIndex == keywordList.Items.Count - 1)
                    {
                        AddBlankKeyword();
                        keywordList.SetSelected(selectedIndex, false);
                        keywordList.SetSelected(keywordList.Items.Count - 1, true);
                    }
                }
            }
        }

        private void BtnRemoveKeyword_Click(object sender, EventArgs e)
        {
            int index = keywordList.SelectedIndex;
            StringList keywords = commentSetting.settingRecords[index].GetKeywords();
            StringList comments = commentSetting.settingRecords[index].GetComments();

            if (keywords.Count > 1)
            {
                RemoveKeywordForm form = new RemoveKeywordForm(keywords);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    List<int> subIndices = form.selectedIndices;
                    for (int i = subIndices.Count - 1; i >= 0; i--)
                    {
                        int subIndex = subIndices[i];
                        string selectedKeyword = keywords[subIndex];

                        keywords.RemoveAt(subIndex);
                        commentSetting.keywordMap.Remove(selectedKeyword);
                    }

                    if (keywords.Count == 0)
                    {
                        comments.Clear();
                        commentSetting.settingRecords.RemoveAt(index);
                        keywordList.Items.RemoveAt(index);
                    }
                    else
                        keywordList.Items[index] = commentSetting.settingRecords[index].GetKeywordsInString();
                }
            }
            else
            {
                keywords.Clear();
                comments.Clear();
                commentSetting.settingRecords.RemoveAt(index);
                keywordList.Items.RemoveAt(index);
            }
        }

        private void keywordList_SelectedIndexChanged(object sender, EventArgs e)
        {
            commentList.Items.Clear();

            int index = keywordList.SelectedIndex;
            if (index == -1)
                return;

            if(index == keywordList.Items.Count - 1)
            {
                BtnAddComment.Enabled = false;
                BtnRemoveComment.Enabled = false;
            }
            else
            {
                BtnAddComment.Enabled = true;
                BtnRemoveComment.Enabled = true;
            }

            StringList newComments = commentSetting.settingRecords[index].GetComments();
            foreach (string comment in newComments)
                commentList.Items.Add(comment);
        }

        private void BtnAddComment_Click(object sender, EventArgs e)
        {
            AddTextForm addTextForm = new AddTextForm();
            addTextForm.SetCaption("New Comment");
            addTextForm.SetDescription("Please enter the new comment.");

            if (addTextForm.ShowDialog() == DialogResult.OK)
            {
                if (addTextForm.GetNewText().Length > 0)
                {
                    int selectedKeywordIndex = keywordList.SelectedIndex;
                    string newText = addTextForm.GetNewText();
                    StringList curComments = commentSetting.settingRecords[selectedKeywordIndex].GetComments();

                    if (curComments.Find(x => x == newText) != null)
                    {
                        MessageBox.Show("The comment was already added.");
                        return;
                    }

                    SettingRecord settingRecord = commentSetting.settingRecords[selectedKeywordIndex];
                    settingRecord.AddComment(newText);
                    commentList.Items.Add(newText);
                }
            }
        }

        private void BtnRemoveComment_Click(object sender, EventArgs e)
        {
            int selectedKeywordIndex = keywordList.SelectedIndex;
            StringList curComments = commentSetting.settingRecords[selectedKeywordIndex].GetComments();

            ListBox.SelectedIndexCollection indices = commentList.SelectedIndices;

            for (int i = indices.Count - 1; i >= 0; i--)
            {
                curComments.RemoveAt(indices[i]);
                commentList.Items.RemoveAt(indices[i]);
            }
        }

        private void BtnAddGeneric_Click(object sender, EventArgs e)
        {
            AddTextForm addTextForm = new AddTextForm();
            addTextForm.SetCaption("New Generic Comment");
            addTextForm.SetDescription("Please enter new generic comment.");

            if (addTextForm.ShowDialog() == DialogResult.OK)
            {
                if (addTextForm.GetNewText().Length > 0)
                {
                    string newText = addTextForm.GetNewText();

                    if (commentSetting.genericComments.Find(x => x == newText) != null)
                    {
                        MessageBox.Show("The comment was already added as generic.");
                        return;
                    }

                    commentSetting.genericComments.Add(newText);
                    genericList.Items.Add(newText);
                }
            }
        }

        private void BtnRemoveGeneric_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection indices = genericList.SelectedIndices;

            for (int i = indices.Count - 1; i >= 0; i--)
            {
                commentSetting.genericComments.RemoveAt(indices[i]);
                genericList.Items.RemoveAt(indices[i]);
            }
        }
        #endregion

        private void tabMain_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void BtnClearHistory_Click(object sender, EventArgs e)
        {
            commentHistoryList.Items.Clear();
        }

        private void txtCurPage_TextChanged(object sender, EventArgs e)
        {
            string curPageText = txtCurPage.Text;
            int page;

            if (int.TryParse(curPageText, out page) == false)
                page = 1;

            if (page > curMaxPage)
            {
                txtCurPage.Text = curMaxPage.ToString();
                return;
            }

            GotoPage(page);
        }

        private void BtnPrevPage_Click(object sender, EventArgs e)
        {
            if (curPage > 1)
            {
                curPage--;
                txtCurPage.Text = curPage.ToString();
            }
        }

        private void BtnNextPage_Click(object sender, EventArgs e)
        {
            if (curPage < curMaxPage)
            {
                curPage++;
                txtCurPage.Text = curPage.ToString();
            }
        }

        private void GotoPage(int pageNumber)
        {
            if (pageNumber > curMaxPage)
                pageNumber = curMaxPage;
            else if (pageNumber < 1)
                pageNumber = 1;

            if (pageNumber == curMaxPage)
                BtnNextPage.Enabled = false;
            else
                BtnNextPage.Enabled = true;

            if (pageNumber == 1)
                BtnPrevPage.Enabled = false;
            else
                BtnPrevPage.Enabled = true;

            curPage = pageNumber;
            ShowFilteredList();
        }

        private void BtnAddBlack_Click(object sender, EventArgs e)
        {
            AddTextForm addTextForm = new AddTextForm();
            addTextForm.SetCaption("New Blacklist Item");
            addTextForm.SetDescription("Please enter new blacklist item.");

            if (addTextForm.ShowDialog() == DialogResult.OK)
            {
                if (addTextForm.GetNewText().Length > 0)
                {
                    string newText = addTextForm.GetNewText();

                    if(commentSetting.blackListItems == null)
                    {
                        commentSetting.blackListItems = new StringList();

                    }else if (commentSetting.blackListItems.Find(x => x == newText) != null){
                        MessageBox.Show("This item was already existing in blacklist .");
                        return;
                    }

                    commentSetting.blackListItems.Add(newText);
                    blackList.Items.Add(newText);
                }
            }
        }

        private void BtnRemoveBlack_Click(object sender, EventArgs e)
        {
            ListBox.SelectedIndexCollection indices = blackList.SelectedIndices;

            for (int i = indices.Count - 1; i >= 0; i--)
            {
                commentSetting.blackListItems.RemoveAt(indices[i]);
                blackList.Items.RemoveAt(indices[i]);
            }
        }
    }

    class CreatorInfo
    {
        public string rank;
        public string name;
        public string patrons;
        public string earnings;
        public string support;
        public string type;
        public string linkUrl;
        public string status;

        public void ToStringArray(out string[] info)
        {
            info = new string[6];
            info[0] = rank;
            info[1] = name;
            info[2] = patrons;
            info[3] = earnings;
            info[4] = support;
            info[5] = status;
        }
    }

    [Serializable]
    class SettingRecord
    {
        private StringList keywords;
        private StringList comments;

        public SettingRecord()
        {
            keywords = new StringList();
            comments = new StringList();
        }

        public void AddKeyword(string keyword)
        {
            keywords.Add(keyword);
        }

        public void AddComment(string comment)
        {
            comments.Add(comment);
        }

        public string GetKeywordsInString()
        {
            string res = "";

            for(int i = 0; i < keywords.Count; i++)
            {
                if (i > 0)
                    res += ", ";
                res += keywords[i];
            }

            return res;
        }

        public StringList GetKeywords()
        {
            return keywords;
        }

        public StringList GetComments()
        {
            return comments;
        }
    }

    [Serializable]
    class CommentSetting
    {
        public List<SettingRecord> settingRecords = new List<SettingRecord>();
        public Dictionary<string, int> keywordMap = new Dictionary<string, int>();

        public StringList genericComments = new StringList();
        public StringList blackListItems = new StringList();

        public string maxComments = "";
        public string commentDelay = "";
        public string creatorDelay = "";

        public CommentSetting()
        {

        }
    }
}
