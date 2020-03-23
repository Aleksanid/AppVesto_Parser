using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Parsing {
    internal class Program {
        public static void Main(string[] args) {
            Uri uri = new Uri("https://www.udemy.com/course/learn-flutter-dart-to-build-ios-android-apps");
            Regex myReg = new Regex(@"<div class=""title"">.<a [^\>]*>.([^\<]*).<\/a>.*?<span class=""content-summary"">.([^<]*)",RegexOptions.Singleline);
            WebClient webClient = new WebClient();
            string userAgentString = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36";
            
            webClient.Headers.Add("user-agent", userAgentString);
            
            string html = webClient.DownloadString(uri);
            
            Dictionary<string,TimeSpan> videos = new Dictionary<string, TimeSpan>();

            foreach (Match match in myReg.Matches(html)) {
                string videoName = match.Groups[1].ToString();
                string videoTime = match.Groups[2].ToString();
                TimeSpan videoLength;
                if(videoTime.Split(':').Length>2){
                    videoLength = TimeSpan.Parse(videoTime);
                } else {
                    videoLength = TimeSpan.Parse("00:" + videoTime);
                }
                videos.Add(videoName,videoLength);
            }
            Console.WriteLine("================== List of videos by length ==================");
            foreach (KeyValuePair<string, TimeSpan> course in videos.OrderBy(key => key.Value))  
            {  
                Console.WriteLine("Name: {0}, Time: {1}", course.Key, course.Value);  
            }  
        }
    }
}