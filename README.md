# SpartaDungeon2

### 프로젝트 소개
- 스파르타 던전 (Text 게임) 만들기

### 개발 기간
- 2023/11/10 ~ 2023/11/14

### 멤버 구성
- 김수아

### 개발 환경
- "VisualStudio"

### 주요 기능
캐릭터와 아이템을 class로 만들었다
```
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
```

이부분은 인벤토리 장착관리 부분에서 원하는 아이템을 장착하고 뺄수있는 기능을 구현한것이다
장착이 되어있다는 표시는 [E] 이다.
```
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
```

이 코드로 문자열의 길이를 구해서
```
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
```

PrintitemStatDescription에서 일정한 여백으로 정렬을 하게했다
```
public static string PadRightForMixedText(string str, int totalLength)
        {
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);
        }
```

문자길이가 긴순, 공격력이 높은순, 방어력이 높은순으로 정렬하는것을 구현을 하기위해
세개의 메서드를 만들었다
```
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
```

아이템을 장착했을때 한번더 눌렀을때 장착이돼있다면 빼주고 장착이돼있지 않다면 껴주게했다
```
private static void ToggleEquipStatus(int idx)
            {
                _items[idx].IsEquipped = !_items[idx].IsEquipped;
            }
```

GetSumBonus~ 메서드를 만들어서 StatusMenu상태보기 부분에서 장착한 아이템의 능력치만큼 정보가 반영되고 
몇이 올라갔는지 표시해주게 했다
```
private static int GetSumBonusArk()
            {
                int sum = 0;
                for(int i =0; i < _items.Count; i++)
                {
                    if (_items[i].IsEquipped) sum += _items[i].Atk;
                }
                return sum;
            }
```

글자색을 바꾸는 것도 메서드로 만들어서 사용했다
```
private static void PrintTextWithHighlights(string s1, string s2, string s3 ="")
            {
                Console.Write(s1);
                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.Write(s2);
                Console.ResetColor();
                Console.WriteLine(s3);
            }
```

최솟값과 최댓값을 사용해서 그사잇값을 입력하지 않으면 계속 원하시는 행동을 입력하라는 메세지가 뜨게했고
보기가 있는 모든 부분에 최소 최댓값만 바꿔서 사용했다
```
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
```

아이템을 리스트로 만들었다
```
private static void GameDataSetting()
            {
                _player = new Character("Chad", "전사", 1, 10, 5, 100, 1500);
                _items = new List<Item>();
               
           
                _items.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 5, 0));
                _items.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 1, 2, 0, 0));
                _items.Add(new Item("왕 방패", "모든걸 막을 수 있는 방패입니다.", 2, 0, 8, 0));
                _items.Add(new Item("장미 칼", "매우 날카로워 스치기만 해도 데미지가 큰 칼입니다.", 3, 7, 0, 0));
            
               


            }
```
