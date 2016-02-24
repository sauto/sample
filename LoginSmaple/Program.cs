using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace LoginSample
{    
    class Program
    {
        const string ID = "hugahuga@gmail.com";//ログインID
        const string PASSWORD = "piyopiyo";//ログインパスワード
        const string LOGIN_ADDRESS = "https://www.secure.pixiv.net/login.php";

        static void Main(string[] args)
        {
            var p = new Program();
            var temp = p.LoginAsync().Result;            
            Console.ReadLine();
        }

        public async Task<CookieContainer> LoginAsync()
        {
            CookieContainer cc;
            using (var handler = new HttpClientHandler())
            {
                using (var client = new HttpClient(handler))
                {
                    //ログイン用のPOSTデータ生成
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        {"mode","login"},
                        { "pixiv_id", ID },
                        { "pass", PASSWORD },
                        {"skip","1"}
                    });
                    //ログイン
                    await client.PostAsync(LOGIN_ADDRESS, content);

                    //クッキー保存
                    cc = handler.CookieContainer;
                }
            }
            
            CookieCollection cookies = cc.GetCookies(new Uri(LOGIN_ADDRESS));

            foreach (Cookie c in cookies)
            {
                Console.WriteLine("クッキー名:" + c.Name.ToString());
                Console.WriteLine("クッキーを使うサイトのドメイン名:" + c.Domain.ToString());
                Console.WriteLine("クッキー発行日時:" + c.TimeStamp.ToString() + Environment.NewLine);
            }

            Console.WriteLine("ログイン処理完了！");

            return cc;
        }
    }
}
