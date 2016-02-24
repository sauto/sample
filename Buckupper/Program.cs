using System;
using System.IO;

//今日の日付で特定ファイルをバックアップ
class Program
{
    static void Main()
    {
        //バックアップしたいファイルのファイルパスを設定
        var copyfilepath = @"C:\Users\sauto\OneDrive\Word\新しいテキスト ドキュメント.txt";
        //バックアップしたいファイル名を取得
        var filename = Path.GetFileName(copyfilepath);
        //日付を取得
        var d = DateTime.Today.Date.Month.ToString("D2") + DateTime.Today.Date.Day.ToString("D2");
        //日付を先頭にしてバックアップファイルのパスを作る
        var buckupfilepath = string.Format(@"C:\Users\sauto\OneDrive\Word\buckup\{0}{1}", d, filename);
        //本日のバックアップファイルが作成済みなら削除
        if (File.Exists(buckupfilepath))
            File.Delete(buckupfilepath);
        //ファイルをコピーしてバックアップ
        File.Copy(copyfilepath, buckupfilepath);

        System.Media.SystemSounds.Beep.Play();
    }
}
    

