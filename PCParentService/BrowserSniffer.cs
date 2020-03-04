using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("PCParentTestProject")]
namespace PCParentServiceApp
{
    class BrowserSniffer : IBrowserSniffer
    {
        [DllImport("user32.dll")]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public List<string> PrintBrowserTabName()
        {
            List<string> retList = new List<string>();
            var browsersList = new List<string>
            {
                "chrome",
                "firefox",
                "iexplore",
                "edge"
            };

            var restrictedList = new List<string>()
            {
                "youtube", "pandora", "wwe", "roman", "reigns", "banks", "sasha", "raw", "smackdown"
            };

            foreach (var singleBrowser in browsersList)
            {
                var process = Process.GetProcessesByName(singleBrowser);
                if (process.Length > 0)
                {
                    foreach (var proc in process)
                    {
                        var recText = string.Empty;
                        IntPtr hWnd = proc.MainWindowHandle;
                        int length = GetWindowTextLength(hWnd);
                        StringBuilder text = new StringBuilder(length + 1);
                        GetWindowText(hWnd, text, text.Capacity);
                        if (text.Length > 0)
                        {
                            int inStr = 0;
                            foreach(string str in restrictedList)
                            {
                                inStr = text.ToString().ToLower().IndexOf(str);
                                if (inStr > 0)
                                {
                                    retList.Add(text.ToString().ToLower());
                                    proc.Kill();
                                    return retList;
                                }
                            }
                        }
                    }
                }
            }

            return retList;
        }

    }
}
