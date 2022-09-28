using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AStarAlgorithm
{
    class ĐườngĐi
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int ChiPhí { get; set; }
        public int KhoảngCách { get; set; }
        public int ChiPhíChặng => ChiPhí + KhoảngCách;
        public ĐườngĐi TrướcĐó { get; set; }

        //Khoảng cách về cơ bản là khoảng cách ước tính, bỏ qua các bức tường tới mục tiêu đến
        //Vậy còn bao nhiêu đường đi qua trái, phải, trên, dưới để tới mục tiêu 
        public void ThiếtLậpKhoảngCách(int mụcTiêuX, int mụcTiêuY)
        {
            this.KhoảngCách = Math.Abs(mụcTiêuX - X) + Math.Abs(mụcTiêuY - Y);
        }
    }
    class AStarAlgorithm_TranslateVN
    {
        
        public void ChạyTrươngChình()
        {
            int tổngChiPhí = 0;
            //List<string> bảnĐồ = new List<string>
            //{
            //    "|A             |",
            //    "|--| |---------|",
            //    "|              |",
            //    "|---|--|-|---| |",
            //    "|              |",
            //    "|---| |---|-|--|",
            //    "|              |",
            //    "|--|-|------|  |",
            //    "|              |",
            //    "|   |-----|----|",
            //    "|         |    |",
            //    "|---|       | B|"
            //};
            List<string> bảnĐồ = new List<string>
            {
                "|A   |           |  |B     |  |        |",
                "|--| |-------| | |  |  |        |-| |  |",
                "|  |         | | |  |--|--|--|  |   |  |",
                "|    ||      | | |  |     |  |  | |-|  |",
                "|-----| |----| | |     |     |--|    | |",
                "|       |      | |  |--------| |---| |-|",
                "| |--|  | |----| |  |    |   |-|       |",
                "|    |--|      | |  |  |   |   |-| | | |",
                "|--| |-------| | |     | |---|-|   |-| |",
                "|  |         | | |  |----|   | | |     |",
                "|    |------ | | |  |    | | |   | | | |",
                "|-----|   || | | |     | | | | | |-| | |",
                "|       |    | | |  |--|   |   | | | | |",
                "|  | |-------| | |    |--------| | | | |",
                "|  |   |   |   | |  | |   | |  | |     |",
                "|  | |   |   | | |      |   |  | |--|  |",
                "|---------|-|--| |  |-----| |  |    |  |",
                "|                |        |      |  |  |",
                "|---|  |-|---| |-|  |---| |----| |-----|",
                "|                   |                  |"
            };
            var bắtĐầu = new ĐườngĐi();
            bắtĐầu.Y = bảnĐồ.FindIndex(x => x.Contains("A"));
            bắtĐầu.X = bảnĐồ[bắtĐầu.Y].IndexOf("A");


            var kếtThúc = new ĐườngĐi();
            kếtThúc.Y = bảnĐồ.FindIndex(x => x.Contains("B"));
            kếtThúc.X = bảnĐồ[kếtThúc.Y].IndexOf("B");

            bắtĐầu.ThiếtLậpKhoảngCách(kếtThúc.X, kếtThúc.Y);

            var cácVịTríHoạtĐộng = new List<ĐườngĐi>();
            cácVịTríHoạtĐộng.Add(bắtĐầu);
            var cácVịTríĐãThăm = new List<ĐườngĐi>();

            while (cácVịTríHoạtĐộng.Any())
            {
                var kiểmTraĐường = cácVịTríHoạtĐộng.OrderBy(x => x.ChiPhíChặng).First();

                if (kiểmTraĐường.X == kếtThúc.X && kiểmTraĐường.Y == kếtThúc.Y)
                {
                    //Ta đã chắn chắn tìm thấy điểm đến (chú ý cái thằng OrderBy ở trên)
                    //Đây là một sự lựa chọn ít tốn kém nhất
                    var đườngĐi = kiểmTraĐường;
                    Console.WriteLine("Thực hiện quay lui...");
                    Console.WriteLine("Tọa độ truy ngược về điểm bắt đầu ([X : Y]):");
                    //Console.Write("{0:-5} : {1:-5}", "X", "Y");
                    while (true)
                    {
                        if (đườngĐi.X == kếtThúc.X && đườngĐi.Y == kếtThúc.Y)
                        {
                            tổngChiPhí = đườngĐi.ChiPhíChặng;
                        }
                        if (đườngĐi.X == bắtĐầu.X && đườngĐi.Y == bắtĐầu.Y)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write($"[{đườngĐi.X} : {đườngĐi.Y}]");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write($"[{đườngĐi.X} : {đườngĐi.Y}]");
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.Write(" --> ");
                            Console.ResetColor();                        
                        }
                        if (bảnĐồ[đườngĐi.Y][đườngĐi.X] == ' ')
                        {
                            var dòngBảnĐồMới = bảnĐồ[đườngĐi.Y].ToCharArray();
                            dòngBảnĐồMới[đườngĐi.X] = '*';
                            bảnĐồ[đườngĐi.Y] = new string(dòngBảnĐồMới);
                        }
                        đườngĐi = đườngĐi.TrướcĐó;
                        if (đườngĐi == null)
                        {
                            Console.WriteLine("\n\nTổng chi phí từ A -> B: {0}", tổngChiPhí);
                            Console.WriteLine("Bản đồ sau khi chạy thuật toán A*:");
                            bảnĐồ.ForEach(x => {
                                var charArr = x.ToCharArray();
                                foreach (var ch in charArr)
                                {
                                    if (ch == '*')
                                    {
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.BackgroundColor = ConsoleColor.Green;
                                        Console.Write(ch);
                                        Console.ResetColor();
                                    }
                                    else
                                    {
                                        Console.Write(ch);
                                    }
                                }
                                Console.WriteLine();
                            });
                            Console.WriteLine("Đã Xong nhé! =))");
                            return;
                        }
                    }
                }

                cácVịTríĐãThăm.Add(kiểmTraĐường);
                cácVịTríHoạtĐộng.Remove(kiểmTraĐường);

                var cácĐườngCóThểĐi = LấyDanhSáchĐườngCóThểĐi(bảnĐồ, kiểmTraĐường, kếtThúc);

                foreach (var đườngCóThểĐi in cácĐườngCóThểĐi)
                {
                    //Chúng ta đã có các vị trí đã thăm cho nên không cần phải thăm lại chi nữa
                    if (cácVịTríĐãThăm.Any(x => x.X == đườngCóThểĐi.X && x.Y == đườngCóThểĐi.Y))
                        continue;

                    //Đường này đã tồn tại trong danh sách vị trí hoạt động, nhưng không sao. Đường đi mới này có giá trị tốt hơn
                    if (cácVịTríHoạtĐộng.Any(x => x.X == đườngCóThểĐi.X && x.Y == đườngCóThểĐi.Y))
                    {
                        var đườngTồnTại = cácVịTríHoạtĐộng.First(x => x.X == đườngCóThểĐi.X && x.Y == đườngCóThểĐi.Y);
                        if (đườngTồnTại.ChiPhíChặng > kiểmTraĐường.ChiPhíChặng)
                        {
                            cácVịTríHoạtĐộng.Remove(đườngTồnTại);
                            cácVịTríHoạtĐộng.Add(đườngCóThểĐi);
                        }
                    }
                    else
                    {
                        //Đường mới khám phá, thêm vào danh sách vị trí hoạt động 
                        cácVịTríHoạtĐộng.Add(đườngCóThểĐi);
                    }
                }
            }

            Console.WriteLine("Không tìm thấy đường nào hết nha!");
        }
        private static List<ĐườngĐi> LấyDanhSáchĐườngCóThểĐi(List<string> bảnĐồ, ĐườngĐi vịTríHoạtĐộng, ĐườngĐi điểmĐến)
        {
            var đườngCóThểĐi = new List<ĐườngĐi>()
            {
                new ĐườngĐi { X = vịTríHoạtĐộng.X, Y = vịTríHoạtĐộng.Y - 1, TrướcĐó = vịTríHoạtĐộng, ChiPhí = vịTríHoạtĐộng.ChiPhí + 1 },
                new ĐườngĐi { X = vịTríHoạtĐộng.X, Y = vịTríHoạtĐộng.Y + 1, TrướcĐó = vịTríHoạtĐộng, ChiPhí = vịTríHoạtĐộng.ChiPhí + 1},
                new ĐườngĐi { X = vịTríHoạtĐộng.X - 1, Y = vịTríHoạtĐộng.Y, TrướcĐó = vịTríHoạtĐộng, ChiPhí = vịTríHoạtĐộng.ChiPhí + 1 },
                new ĐườngĐi { X = vịTríHoạtĐộng.X + 1, Y = vịTríHoạtĐộng.Y, TrướcĐó = vịTríHoạtĐộng, ChiPhí = vịTríHoạtĐộng.ChiPhí + 1 },
            };

            đườngCóThểĐi.ForEach(đườngĐi => đườngĐi.ThiếtLậpKhoảngCách(điểmĐến.X, điểmĐến.Y));

            var maxX = bảnĐồ.First().Length - 1;
            var maxY = bảnĐồ.Count - 1;

            return đườngCóThểĐi
                    .Where(tile => tile.X >= 0 && tile.X <= maxX)
                    .Where(tile => tile.Y >= 0 && tile.Y <= maxY)
                    .Where(tile => bảnĐồ[tile.Y][tile.X] == ' ' || bảnĐồ[tile.Y][tile.X] == 'B')
                    .ToList();
        }
    }
}
