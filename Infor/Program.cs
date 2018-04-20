using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Infor
{
    class Program
    {
        // 初始化
        public static List<student> studentsList;// 學生清單
        static string data;// 資料位置
        static int mode = 0;// 目前視窗

        static void Main(string[] args)
        {
            // 得到JSON檔並轉成List
            StreamReader rw = new StreamReader("D:\\VSProject\\Infor\\Infor\\Data.txt", Encoding.Default);
            data = rw.ReadToEnd();
            studentsList = JsonConvert.DeserializeObject<List<student>>(data);

            while (true)
            {
                // Title
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("╔════════════════════════╗");
                Console.WriteLine("║                        ║");
                Console.WriteLine("║       DMA學生資料      ║");
                Console.WriteLine("║                        ║");
                Console.WriteLine("╚════════════════════════╝");

                // 根據mode變換資料
                switch (mode)
                {
                    case 0:
                        Menu();
                        break;
                    case 1:
                        Gender();
                        break;
                    case 2:
                        BloodType();
                        break;
                    case 3:
                        Height();
                        break;
                    case 4:
                        Find();
                        break;
                }

                // 左翻頁右翻頁+清空畫面
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.LeftArrow)
                {
                    if (mode > 0)
                    {
                        mode--;
                    }
                }
                else if (key == ConsoleKey.RightArrow)
                {
                    if (mode < 4)
                    {
                        mode++;
                    }
                }
                Console.Clear();
            }
        }

        // 開頭標題
        static void Menu()
        {
            Console.WriteLine("\r\n\r\n\r\n\r\n   歡迎來到DMA學生資料查詢系統\r\n   請用左右方向鍵來翻頁");
        }

        // 性別總數&比例
        static void Gender()
        {
            // 宣告儲存變數
            double Boy = 0, Girl = 0;
            double BoyP, GirlP;

            // 從List中搜尋(if)
            foreach (student s in studentsList)
            {
                if (s.gender == "男")
                {
                    Boy++;
                }
                else
                {
                    Girl++;
                }
            }
            
            Console.WriteLine("\r\n男生人數：" + Boy.ToString());
            Console.WriteLine("\r\n女生人數：" + Girl.ToString());

            // 取得男女比例
            BoyP = Math.Round((double)(Boy / (Boy + Girl)), 2, MidpointRounding.AwayFromZero) * 100;
            GirlP = 100 - BoyP;

            Console.WriteLine("\r\n\r\n");

            // 輸出並繪製長條圖
            Console.Write("男生比例 ");
            Console.BackgroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < BoyP / 2; i++)
            {
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" " + BoyP + "%");
            Console.WriteLine("\r\n\r\n");

            Console.Write("女生比例 ");
            Console.BackgroundColor = ConsoleColor.Red;
            for (int i = 0; i < GirlP / 2; i++)
            {
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(" " + GirlP + "%");
            Console.WriteLine("\r\n\r\n");
        }

        // 血型
        static void BloodType()
        {
            // 宣告儲存變數
            int o = 0, a = 0, b = 0, ab = 0, other = 0;
            int total;

            // 從List中搜尋(switch)
            foreach (student s in studentsList)
            {
                switch (s.bloodtype)
                {
                    case "O":
                        o++;
                        break;
                    case "A":
                        a++;
                        break;
                    case "B":
                        b++;
                        break;
                    case "AB":
                        ab++;
                        break;
                    default:
                        other++;
                        break;
                }
            }

            total = studentsList.Count();

            // 函數繪製長條圖
            Console.WriteLine("O型人數：" + o);
            double oP = Math.Round((double)(o * 100 / total), 2, MidpointRounding.AwayFromZero);
            DrawBar(oP / 2, ConsoleColor.Cyan);
            Console.WriteLine(oP + "%" + "\r\n");

            Console.WriteLine("A型人數：" + a);
            double aP = Math.Round((double)(a * 100 / total), 2, MidpointRounding.AwayFromZero);
            DrawBar(aP / 2, ConsoleColor.Red);
            Console.WriteLine(aP + "%" + "\r\n");

            Console.WriteLine("B型人數：" + b);
            double bP = Math.Round((double)(b * 100 / total), 2, MidpointRounding.AwayFromZero);
            DrawBar(bP / 2, ConsoleColor.Blue);
            Console.WriteLine(bP + "%" + "\r\n");

            Console.WriteLine("AB型人數：" + ab);
            double abP = Math.Round((double)(ab * 100 / total), 2, MidpointRounding.AwayFromZero);
            DrawBar(abP / 2, ConsoleColor.Magenta);
            Console.WriteLine(abP + "%" + "\r\n");

            Console.WriteLine("其他血型人數：" + other);
            double otherP = Math.Round((double)(other * 100 / total), 2, MidpointRounding.AwayFromZero);
            DrawBar(otherP / 2, ConsoleColor.Gray);
            Console.WriteLine(otherP + "%" + "\r\n");
        }

        // 身高
        static void Height()
        {
            // 宣告儲存變數
            float boyAvr = 0, girlAvr = 0, allAvr;
            int boyCount = 0, girlCount = 0;
            float boyHighest = 0, boyShortest = 0, girlHighest = 0, girlShortest = 0;

            // List搜尋並剔除過高身高  但並沒有管過矮的 順便登記最高的身高 *其實好像能用Sort
            foreach (student s in studentsList)
            {
                // 判斷性別
                if (s.gender == "男")
                {
                    // 身高過高
                    if (s.height >= 272)
                    {
                        continue;
                    }

                    // 將身高加入總和(最後除)
                    boyAvr += s.height;

                    // 該次身高是否超越紀錄
                    if (s.height > boyHighest)
                    {
                        boyHighest = s.height;
                    }
                    if (s.height < boyShortest)
                    {
                        boyShortest = s.height;
                    }
                    boyCount++;
                }
                else
                {
                    if (s.height >= 272)
                    {
                        continue;
                    }
                    girlAvr += s.height;
                    if (s.height > girlHighest)
                    {
                        girlHighest = s.height;
                    }
                    if (s.height < girlShortest)
                    {
                        girlShortest = s.height;
                    }
                    girlCount++;
                }
            }

            // 取平均
            boyAvr /= boyCount;
            girlAvr /= girlCount;
            allAvr = (boyAvr + girlAvr) / 2;

            // 排版好難 Ctrl+C Ctrl+V 改改改 早知道寫成函式 很長不過都是重複的
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("男生平均" + boyAvr.ToString("#0.00") + "公分");

            Console.Write("\r\n");

            Console.WriteLine("     男生最高:" + boyHighest + " 公分");
            Console.Write("     ");

            // Foreach找出男生最高
            foreach (student s in studentsList)
            {
                if (s.height == boyHighest && s.gender == "男")
                {
                    Console.Write(s.name + " ");
                }
            }

            Console.WriteLine("\r\n");

            Console.WriteLine("     男生最矮:" + boyShortest + " 公分");
            Console.Write("     ");

            // Foreach找出男生最矮
            foreach (student s in studentsList)
            {
                if (s.height == boyShortest && s.gender == "男")
                {
                    Console.Write(s.name + " ");
                }
            }

            Console.WriteLine("\r\n\r\n");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("女生平均:" + girlAvr.ToString("#0.00") + "公分");

            Console.Write("\r\n");

            Console.WriteLine("     女生最高:" + girlHighest + " 公分");
            Console.Write("     ");
            foreach (student s in studentsList)
            {
                if (s.height == girlHighest && s.gender == "女")
                {
                    Console.Write(s.name + " ");
                }
            }

            Console.WriteLine("\r\n");

            Console.WriteLine("     女生最矮: " + girlShortest + " 公分");
            Console.Write("     ");
            foreach (student s in studentsList)
            {
                if (s.height == girlShortest && s.gender == "女")
                {
                    Console.Write(s.name + " ");
                }
            }

            Console.WriteLine("\r\n");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("全班平均:" + allAvr.ToString("#0.00") + "公分");

            Console.Write("\r\n");

            // Math函式比較男女最高最矮
            float allShortest = Math.Min(boyShortest, girlShortest);
            float allHighest = Math.Max(boyHighest, girlHighest);

            Console.WriteLine("     全班最高:" + allHighest + "公分");
            Console.Write("     ");
            foreach (student s in studentsList)
            {
                if (s.height == allHighest)
                {
                    Console.Write(s.name + " ");
                }
            }

            Console.Write("\r\n\r\n");

            Console.WriteLine("     全班最矮:" + allShortest + "公分");
            Console.Write("     ");
            foreach (student s in studentsList)
            {
                if (s.height == allShortest)
                {
                    Console.Write(s.name + " ");
                }
            }
        }

        // 尋找天蠍+O型
        static void Find()
        {
            // 複製一個List
            List<student> Data = new List<student>();
            Data = JsonConvert.DeserializeObject<List<student>>(data);

            // 用RemoveAll的方式剃除
            Data.RemoveAll(s => s.zodiacSign != "天蠍");
            Data.RemoveAll(s => s.bloodtype != "O");

            // Foreach輸出
            Console.WriteLine("天蠍+O型人口：\r\n");
            foreach (student s in Data)
            {
                Console.WriteLine(s.name + "\r\n");
            }
        }

        // 長條圖函式  不然要寫幾十次受不了
        static void DrawBar(double length, ConsoleColor cc)
        {
            Console.BackgroundColor = cc;
            for (int i = 0; i < length; i++)
            {
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }

    // 學生的class
    public class student
    {
        public string name { get; set; }
        public string studentNumber { get; set; }
        public string gender { get; set; }
        public int height { get; set; }
        public string bloodtype { get; set; }
        public string zodiacSign { get; set; }
    }
}
