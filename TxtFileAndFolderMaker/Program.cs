using System;
using System.IO;
using System.Windows;
using System.Linq;

class TxtFileAndFolderMaker
{
    [STAThread]
    static void Main(string[] args)
    {
        if(!Clipboard.ContainsText())
        {
            Console.WriteLine("テキストがありません\r\n何かキーを押してください");
            Console.ReadLine();
            return;
        }

        var str = Clipboard.GetText();
        //1行目を取得
        var title = str.Replace('\r', '_').Replace('\n', '_').Split('_').First();
        //タイトル無効文字列を_に置換
        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (var c in invalidChars)
            title = title.Replace(c, '_');
        //タイトル文字数を100字以内にする
        if (title.Length > 100)
            title = title.Substring(0, 100);

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        Directory.CreateDirectory(desktopPath + "\\" + title);
        string filePath = desktopPath +"\\"+ title + "\\" + title + ".txt";
        using (var sw = new StreamWriter(filePath, false))
            sw.Write(str);
    }
}

