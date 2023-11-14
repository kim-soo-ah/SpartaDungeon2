using System.Dynamic;

namespace spartaDungeon2
{
    
    
        public class Character
        {
            public string Name { get; }
            public string Job { get; }
            public int Level { get; }
            public int Atk { get; }
            public int Def { get; }
            public int Hp { get; }
            public int Gold { get; }

            public Character(string name, string job, int level, int atk, int def, int hp, int gold)
            {
                Name = name;
                Job = job;
                Level = level;
                Atk = atk;
                Def = def;
                Hp = hp;
                Gold = gold;

            }
        }

        public class Item
        {
            public string Name;
            public string Description;
            public int Type;
            public int Atk;
            public int Def;
            public int Hp;
            public bool IsEquipped { get; set; }
            
            public Item(string name, string description, int type, int atk, int def, int hp, bool isequipped = false)
            {
                Name = name;
                Description = description;
                Type = type;
                Atk = atk;
                Def = def;
                Hp = hp;
                IsEquipped = isequipped;
            }

        public void PrintitemStatDescription(bool withNumber = false, int idx = 0)
        {
            
            Console.Write("- ");
            if(withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }
            if (IsEquipped)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
                Console.Write(PadRightForMixedText(Name, 9));
            }
            else Console.Write(PadRightForMixedText(Name, 12));
            Console.Write("  |  ");

            if (Atk != 0) Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk}");
            if (Def != 0) Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def}");
            if (Hp != 0) Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");

            Console.Write("  |  ");
            Console.WriteLine(Description);

        }

        public static int GetPrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2; // 한글과 같은 넓은 문자에 대해 길이를 2로 취급
                }
                else
                {
                    length += 1; // 나머지 문자에 대해 길이를 1로 취급
                }
            }

            return length;
        }

        public static string PadRightForMixedText(string str, int totalLength)
        {
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);
        }

    }

    internal class Program
        {
            static Character _player;
            static List<Item> _items;
            static void Main(string[] args)
            {
                GameDataSetting();
                PrintStartLogo();
                StartMenu();
            }

            private static void StartMenu()
            {
                Console.Clear();
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine("■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine();

                switch(CheckValidInput(1, 2))
                {
                    case 1:
                        StatusMenu();
                        break;
                    case 2:
                        InventoryMenu();
                        break;


                }
               
            }

            private static void InventoryMenu()
            {
                Console.Clear();

                ShowHighlightedText("■ 인벤토리 ■");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();

                Console.WriteLine("[아이템 목록]");

                for(int i = 0; i < _items.Count; i++)
                {
                  _items[i].PrintitemStatDescription();
                
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine("1. 장착관리");
                Console.WriteLine("2. 아이템 정렬");
                Console.WriteLine();

                switch (CheckValidInput(0, 2))
                {
                    case 0:
                        StartMenu();
                        break;
                    case 1:
                        EquipMenu();
                        break;
                    case 2:
                        SortedItems();
                        break;


                }
            }

        private static void SortedItems()
        {
            Console.Clear();
            ShowHighlightedText("■ 인벤토리 - 아이템 정렬 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].PrintitemStatDescription();

            }
            Console.WriteLine();
            Console.WriteLine("1. 이름");
            Console.WriteLine("2. 공격력");
            Console.WriteLine("3. 방어력");
            Console.WriteLine("0. 나가기");

            switch (CheckValidInput(0, 4))
            {
                case 0:
                    InventoryMenu();
                    break;
                case 1:
                    ByName();
                    break;
                case 2:
                    ByAtk();
                    break;
                case 3:
                    ByDef();
                    break;
               


            }

        }

        private static void ByDef()
        {
            Console.Clear();

            ShowHighlightedText("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록] - [방어력]");
            _items = _items.OrderByDescending(item => item.Def).ToList();
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].PrintitemStatDescription();
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    SortedItems();
                    break;


            }
        }

        private static void ByAtk()
        {
            Console.Clear();

            ShowHighlightedText("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록] - [공격력]");
            _items = _items.OrderByDescending(item => item.Atk).ToList();
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].PrintitemStatDescription();
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    SortedItems();
                    break;


            }
        }

        private static void ByName()
        {
            Console.Clear();

            ShowHighlightedText("■ 인벤토리 ■");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            Console.WriteLine("[아이템 목록] - [이름]");
            _items = _items.OrderByDescending(item => item.Name.Length).ToList();
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].PrintitemStatDescription();
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    SortedItems();
                    break;


            }
        }

        private static void EquipMenu()
            {
                Console.Clear();
                ShowHighlightedText("■ 인벤토리 - 장착 관리 ■");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();

                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < _items.Count; i++)
                {
                    _items[i].PrintitemStatDescription(true, i+1);
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");

                int keyInput = CheckValidInput(0, _items.Count);

                switch (keyInput)
                {
                    case 0:
                        InventoryMenu();
                        break;
                    default:
                        ToggleEquipStatus(keyInput - 1);
                        EquipMenu();
                        break;


                }



            }

            private static void ToggleEquipStatus(int idx)
            {
                _items[idx].IsEquipped = !_items[idx].IsEquipped;
            }

            private static void StatusMenu()
            {
                Console.Clear();

                ShowHighlightedText("■ 상태 보기 ■");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine();

                PrintTextWithHighlights("LV. ", _player.Level.ToString("00")) ;
                Console.WriteLine();
                Console.WriteLine("{0} ( {1} )", _player.Name, _player.Job);

                int bonusAtk = GetSumBonusArk();
                PrintTextWithHighlights("공격력 : ", ( _player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? string.Format(" (+{0})", bonusAtk) : "");
                int bonusDef = GetSumBonusDef();
                PrintTextWithHighlights("방어력 : ", (_player.Def + bonusDef).ToString(), bonusDef > 0 ? string.Format(" (+{0})", bonusDef) : "");
                int bonusHp = GetSumBonusHp();
                PrintTextWithHighlights("체력 : ", (_player.Hp + bonusHp).ToString(), bonusHp > 0 ? string.Format(" (+{0})", bonusHp) : "");
                PrintTextWithHighlights("Gold : ", _player.Gold.ToString(), " G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();


                switch (CheckValidInput(0, 0))
                {
                    case 0:
                        StartMenu();
                        break;
                }
            }

            private static int GetSumBonusArk()
            {
                int sum = 0;
                for(int i =0; i < _items.Count; i++)
                {
                    if (_items[i].IsEquipped) sum += _items[i].Atk;
                }
                return sum;
            }
            private static int GetSumBonusDef()
            {
                int sum = 0;
                for(int i =0; i < _items.Count; i++)
                {
                    if (_items[i].IsEquipped) sum += _items[i].Def;
                }
                return sum;
            }
            private static int GetSumBonusHp()
            {
                int sum = 0;
                for(int i =0; i < _items.Count; i++)
                {
                    if (_items[i].IsEquipped) sum += _items[i].Hp;
                }
                return sum;
            }
            private static void ShowHighlightedText(string text)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine(text);
                Console.ResetColor();
            }

            private static void PrintTextWithHighlights(string s1, string s2, string s3 ="")
            {
                Console.Write(s1);
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.Write(s2);
                Console.ResetColor();
                Console.WriteLine(s3);
            }

            private static int CheckValidInput(int min, int max)
            {
                int keyInput;
                bool result;
                
                do
                {
                    Console.WriteLine("원하시는 행동을 입력해주세요.");
                    result = int.TryParse(Console.ReadLine(), out keyInput);
                }while (result == false || CheckIfValid(keyInput, min, max) == false);


                return keyInput;
            }

            private static bool CheckIfValid(int keyInput, int min, int max)
            {
                if(min <= keyInput && max >= keyInput) return true;
                return false;
            }

            private static void PrintStartLogo()
            {
                Console.WriteLine("=============================================================================");
                Console.WriteLine("        ___________________   _____  __________ ___________ _____    ");
                Console.WriteLine("       /   _____/\\______   \\ /  _  \\ \\______   \\\\__    ___//  _  \\   ");
                Console.WriteLine("       \\_____  \\  |     ___//  /_\\  \\ |       _/  |    |  /  /_\\  \\  ");
                Console.WriteLine("       /        \\ |    |   /    |    \\|    |   \\  |    | /    |    \\ ");
                Console.WriteLine("      /_______  / |____|   \\____|__  /|____|_  /  |____| \\____|__  / ");
                Console.WriteLine("              \\/                   \\/        \\/                  \\/  ");
                Console.WriteLine(" ________    ____ ___ _______     ________ ___________________    _______");
                Console.WriteLine(" \\______ \\  |    |   \\\\      \\   /  _____/ \\_   _____/\\_____  \\   \\      \\");
                Console.WriteLine("  |    |  \\ |    |   //   |   \\ /   \\  ___  |    __)_  /   |   \\  /   |   \\\r\n");
                Console.WriteLine("  |    |   \\|    |  //    |    \\\\    \\_\\  \\ |        \\/    |    \\/    |    \\\r\n");
                Console.WriteLine(" /_______  /|______/ \\____|__  / \\______  //_______  /\\_______  /\\____|__  /\r\n");
                Console.WriteLine("         \\/                  \\/         \\/         \\/         \\/         \\/");
                Console.WriteLine("=============================================================================");
                Console.WriteLine("                           PRESS ANYKEY TO START                             ");
                Console.WriteLine("=============================================================================");
                Console.ReadKey();
            }

            private static void GameDataSetting()
            {
                _player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);
                _items = new List<Item>();
               
           
                _items.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 5, 0));
                _items.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 1, 2, 0, 0));
                _items.Add(new Item("왕 방패", "모든걸 막을 수 있는 방패입니다.", 2, 0, 8, 0));
                _items.Add(new Item("장미 칼", "매우 날카로워 스치기만 해도 데미지가 큰 칼입니다.", 3, 7, 0, 0));
            
               


            }

            
    }
            
 }
    