#define CONFUSION

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Common
{
    public enum SnType
    {
        /// <summary>
        /// 交易序号前缀
        /// </summary>
        TN = 1,
        /// <summary>
        /// 预约序号前缀
        /// </summary>
        BK = 2,
        /// <summary>
        /// 充值序号前缀
        /// </summary>
        RC = 3,
        /// <summary>
        /// 提现序号前缀
        /// </summary>
        WD = 4
    }

    public static class SnUtility
    {

#if CONFUSION
        public static readonly List<char> Characters = new List<char> { 'M', 'L', 'K', 'J', '1', 'E', 'W', '4', 'U', 'O', 'G', 'R', 'X', 'T', 'B', '3', 'C', 'F', 'N', 'S', '7', 'Y', 'Q', '0', '6', '2', 'H', 'P', '8', 'I', '5', 'V', 'D', 'Z', 'A', '9' };
#else
        public static readonly List<char> Characters = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
#endif
        public static readonly byte Base = (byte)Characters.Count;
        public const byte Padding = 6;

        /*
         * 用于10进制与其它进制x的互换，x取决于备选字符的个数
         * 算法参考百度百科：进制转换
         * ConvertTo 十进制转x进制
         * 原理：不断求商取余，余数和商是x进制对应字符的十进制表示，替换成x进制的字符即可
         * ConvertFrom x进制转十进制
         * 原理：从左到右，按位和权重累加
         * 为了使编码对客户无意义，字符顺序做了调整，不同位的起始字符也不同
         */

        public static string ConvertTo(long value)
        {
            var numbers = new List<byte>();
            while (true)
            {
                long quotient, remainder;
                quotient = Math.DivRem(value, Base, out remainder);
                numbers.Add((byte)remainder);
                if (quotient < Base)
                {
                    if (quotient > 0) numbers.Add((byte)quotient);
                    break;
                }
                value = quotient;
            }
            while (numbers.Count < Padding)
            {
                numbers.Add(0);
            }

            var sb = new StringBuilder();
            for (int i = numbers.Count - 1; i >= 0; i--)
            {
#if CONFUSION
                sb.Append(GetCharacter(numbers[i], i));
#else
                sb.Append(Characters[numbers[i]]);
#endif
            }
            return sb.ToString();
        }


        public static string ConvertTo(SnType type,long value)
        {
            var numbers = new List<byte>();
            while (true)
            {
                long quotient, remainder;
                quotient = Math.DivRem(value, Base, out remainder);
                numbers.Add((byte)remainder);
                if (quotient < Base)
                {
                    if (quotient > 0) numbers.Add((byte)quotient);
                    break;
                }
                value = quotient;
            }
            while (numbers.Count < Padding)
            {
                numbers.Add(0);
            }

            var sb = new StringBuilder();
            for (int i = numbers.Count - 1; i >= 0; i--)
            {
#if CONFUSION
                sb.Append(GetCharacter(numbers[i], i));
#else
                sb.Append(Characters[numbers[i]]);
#endif
            }
            return type.ToString()+sb.ToString();
        }



        public static long ConvertFrom(string value)
        {
            long result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                var t = Math.Pow(Base, value.Length - 1 - i);
#if CONFUSION
                var n = GetNumber(value[i], value.Length - 1 - i);
#else
                var n = Characters.IndexOf(value[i]);
#endif
                t *= n;
                result += (long)t;
            }
            return result;
        }

#if CONFUSION
        private static char GetCharacter(byte number, int index)
        {
            var i = (number + index) % Base;
            return Characters[i];
        }

        private static byte GetNumber(char character, int index)
        {
            var i = Characters.IndexOf(character);
            i -= index;
            if (i < 0) i += Base;
            return (byte)i;
        }
#endif

        public static long GetMaxValue(int length)
        {
            long result = 0;
            for (int i = 0; i < length; i++)
            {
                var t = Math.Pow(Base, length - 1 - i);
                t *= Base - 1;
                result += (long)t;
            }
            return result;
        }

        public static void RunTest()
        {
            //单个验证，方便调试
            while (true)
            {
                Console.WriteLine("Please input a number, else to exit");
                var input = Console.ReadLine();
                long value;
                if (!long.TryParse(input, out value)) break;
                //value = long.MaxValue;
                var text = ConvertTo(value);
                Console.WriteLine("Form {0} To {1}", value, text);
                var value_ = ConvertFrom(text);
                Console.WriteLine("Form {1} To {0}", value_, text);
                System.Diagnostics.Debug.Assert(value == value_);
            }

            //性能测试
            //Action<long> convert = x =>
            //{
            //    var text = ConvertTo(x);
            //    //Console.Write("Form {0} To {1}\t", x, text);
            //    var x_ = ConvertFrom(text);
            //    //Console.WriteLine("Form {1} To {0}", x_, text);
            //    System.Diagnostics.Debug.Assert(x == x_);
            //};

            //int count = 10000;
            //var stopwatch = new System.Diagnostics.Stopwatch();
            //stopwatch.Start();
            //for (int i = 0; i < count; i++)
            //{
            //    convert(i);
            //}
            //for (int i = int.MaxValue; i > int.MaxValue - count; i--)
            //{
            //    convert(i);
            //}
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.Elapsed);
        }
    }

}
