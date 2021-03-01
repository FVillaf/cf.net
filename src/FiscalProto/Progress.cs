using System;
using System.IO;
using System.Windows.Forms;

namespace FiscalProto
{
    /// <summary>
    /// Form para mostrar el progreso de una operación lenta.
    /// </summary>
    public partial class Progress : Form
    {
        bool saveToFile = false;

        bool showDebugPanel = false;

        public Action OnCancel { get; set; }

        public string Host { get; private set; }

        public bool SaveToFile
        {
            get { return saveToFile; }
            set
            {
                saveToFile = value;
                if (value)
                    File.WriteAllLines(Host + ".log", LogLines);
            }
        }

        public bool ShowDebugPanel
        {
            get { return showDebugPanel; }
            set
            {
                showDebugPanel = value;
                this.Height = showDebugPanel ? 200 : btCanc.Bottom + 4;
            }
        }

        public string Message
        {
            get { return lblMsg.Text; }
            set
            {
                lblMsg.Text = value ?? throw new ArgumentNullException();
                Application.DoEvents();
            }
        }

        public string[] LogLines
        {
            get
            {
                return tbLog.Text.Replace('\r', ' ').Split('\n');
            }
        }

        public void AddSystemLog(string msg)
        {
            tbLog.AppendText(msg);
            if (saveToFile)
            {
                using (var sw = File.AppendText(Host + ".log"))
                    sw.Write(msg);
            }
        }

        public void DoClose()
        {
            if (tbLog.Text.Length > 0)
                System.Threading.Thread.Sleep(3000);
            this.Close();
        }

        public Progress(string host, string msg, bool cancel, Action onCancel = null, bool showDebugPanel = false)
        {
            InitializeComponent();
            this.Host = host;
            ShowDebugPanel = showDebugPanel;

            OnCancel = onCancel;
            Message = msg;
            if (cancel)
            {
                btCanc.Visible = true;
                lblMsg.Top -= 10;
            }
        }

        private void btCanc_Click(object sender, EventArgs e)
        {
            OnCancel?.Invoke();
        }

        public void HandleCancelButton(bool enable)
        {
            btCanc.Enabled = enable;
        }
    }
}
