using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

using ClipboardHelper;
using ClipboardMonitor.Properties;

namespace ClipboardMonitor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyCustomApplicationContext());
        }
        
        public class MyCustomApplicationContext : ApplicationContext
        {

            static readonly string[] Formats = Enum.GetNames(typeof(ClipboardFormat));
            private readonly NotifyIcon _trayIcon;

            public MyCustomApplicationContext()
            {
                var assist = new ClipboardHelper.ClipboardHelper();
                assist.ClipboardChanged += AssistOnClipboardChanged;
                _trayIcon = new NotifyIcon()
                {
                    Icon = Resources.AppIcon,
                    ContextMenu = new ContextMenu(new MenuItem[]
                    {
                        //new MenuItem("Show current clipboard contents", ShowCurrent()),
                        new MenuItem("Exit", Exit)
                    }),
                    Text = "ClipMon",
                    Visible = true
                };
            }

            private void ShowCurrent(object data, ClipboardFormat format)
            {
                // TODO: Make pop-in agnostic to ClipboardFormat.
                // TODO: Show quick, fancy pop-up.

                var pasted = Clipboard.GetText();
                if (String.IsNullOrWhiteSpace(pasted))
                    return;

                _trayIcon.BalloonTipText = pasted;
                _trayIcon.BalloonTipTitle = "Clipboard contents";
                _trayIcon.ShowBalloonTip(2000);
            }

            private void AssistOnClipboardChanged(object sender, ClipboardChangedEventArgs clipboardChangedEventArgs)
            {
                IDataObject iData = clipboardChangedEventArgs.DataObject;

                ClipboardFormat? format = null;

                foreach (var f in Formats)
                {
                    if (iData.GetDataPresent(f))
                    {
                        format = (ClipboardFormat)Enum.Parse(typeof(ClipboardFormat), f);
                        break;
                    }
                }

                object data = iData.GetData(format.ToString());

                if (data == null || format == null)
                    return;

                ShowCurrent(data, (ClipboardFormat)format);
            }

            void Exit(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                _trayIcon.Visible = false;

                Application.Exit();
            }
        }

        public enum ClipboardFormat : byte
        {
            /// <summary>Specifies the standard ANSI text format. This static field is read-only.
            /// </summary>
            /// <filterpriority>1</filterpriority>
            Text,
            /// <summary>Specifies the standard Windows Unicode text format. This static field
            /// is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            UnicodeText,
            /// <summary>Specifies the Windows device-independent bitmap (DIB) format. This static
            /// field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Dib,
            /// <summary>Specifies a Windows bitmap format. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Bitmap,
            /// <summary>Specifies the Windows enhanced metafile format. This static field is
            /// read-only.</summary>
            /// <filterpriority>1</filterpriority>
            EnhancedMetafile,
            /// <summary>Specifies the Windows metafile format, which Windows Forms does not
            /// directly use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            MetafilePict,
            /// <summary>Specifies the Windows symbolic link format, which Windows Forms does
            /// not directly use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            SymbolicLink,
            /// <summary>Specifies the Windows Data Interchange Format (DIF), which Windows Forms
            /// does not directly use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Dif,
            /// <summary>Specifies the Tagged Image File Format (TIFF), which Windows Forms does
            /// not directly use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Tiff,
            /// <summary>Specifies the standard Windows original equipment manufacturer (OEM)
            /// text format. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            OemText,
            /// <summary>Specifies the Windows palette format. This static field is read-only.
            /// </summary>
            /// <filterpriority>1</filterpriority>
            Palette,
            /// <summary>Specifies the Windows pen data format, which consists of pen strokes
            /// for handwriting software, Windows Forms does not use this format. This static
            /// field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            PenData,
            /// <summary>Specifies the Resource Interchange File Format (RIFF) audio format,
            /// which Windows Forms does not directly use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Riff,
            /// <summary>Specifies the wave audio format, which Windows Forms does not directly
            /// use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            WaveAudio,
            /// <summary>Specifies the Windows file drop format, which Windows Forms does not
            /// directly use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            FileDrop,
            /// <summary>Specifies the Windows culture format, which Windows Forms does not directly
            /// use. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Locale,
            /// <summary>Specifies text consisting of HTML data. This static field is read-only.
            /// </summary>
            /// <filterpriority>1</filterpriority>
            Html,
            /// <summary>Specifies text consisting of Rich Text Format (RTF) data. This static
            /// field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Rtf,
            /// <summary>Specifies a comma-separated value (CSV) format, which is a common interchange
            /// format used by spreadsheets. This format is not used directly by Windows Forms.
            /// This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            CommaSeparatedValue,
            /// <summary>Specifies the Windows Forms string class format, which Windows Forms
            /// uses to store string objects. This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            StringFormat,
            /// <summary>Specifies a format that encapsulates any type of Windows Forms object.
            /// This static field is read-only.</summary>
            /// <filterpriority>1</filterpriority>
            Serializable,
        }
    }
}