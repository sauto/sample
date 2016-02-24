using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;

namespace NicoAnimeScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            GetNicoAnimeList();

            Console.ReadLine();
        }

        const string HEAD = "http://ch.nicovideo.jp/";
        const int YEAR = 2016;
        enum Season { spring, summer, fall, winter }
        readonly static string FILEPATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ニコニコアニメデータ.txt";

        //http://ch.nicovideo.jp/yyyySeason_animeが各クールのアニメリストのURL
        //2014年冬季以前はアドレスめちゃくちゃなので無理
        readonly static string ADDRESS = HEAD + YEAR + Season.winter.ToString() + "_anime";

        //staticにしてるのはMainから呼び出したかったからで特に意味はない
        static async void GetNicoAnimeList()
        {
            string[] titles = default(string[]);
            string[] dayTimes = default(string[]);
            //スクレイピング 
            using (var client = new HttpClient())
            {
                var html = new HtmlAgilityPack.HtmlDocument();
                //ページを取得してhtmlパース
                html.LoadHtml(await client.GetStringAsync(ADDRESS));

                //アニメタイトルの取得
                //h3タグの
                titles = html.DocumentNode.Descendants("h3")
                    //タイトルのタグの
                    .Where(node => node.GetAttributeValue("class", string.Empty).Contains("program_title"))
                    //中身を取得
                    .Select(node => node.InnerText)
                    .ToArray();

                //チャンネルでの放送日時を取得　生放送は別途やってね
                //liタグの
                dayTimes = html.DocumentNode.Descendants("li")
                    //放送日時のタグの //2015年以前はclass="channel_time" スクレ対策？ただのミス？
                    .Where(node => node.GetAttributeValue("class", string.Empty).Contains("ch_time"))
                    //中身を取得
                    .Select(node => node.InnerText)
                    .ToArray();
            }

            //取得データのリスト作成
            var mergeList = new List<string>();
            for (int i = 0; i < dayTimes.Count(); i++)
            {
                mergeList.Add(titles[i].Replace("\n", "") + "\t放送日時:"
                 + dayTimes[i].Replace("\n", "").Replace("チャンネル", "")
                 .Replace("WEB最速！", "").Replace("TV同時配信！", ""));
            }
            //txtに書き出し
            using (var sw = new StreamWriter(FILEPATH, true))
                mergeList.ForEach(s => sw.WriteLine(s));
            //コンソールに表示
            mergeList.ForEach(s => Console.WriteLine(s));
        }
    }
}

