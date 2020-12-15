// Copyright (c) Frederic Forjan 
// Licensed under MIT License

using System;
using System.Collections.Generic;
using System.Globalization;

namespace TeslaVideoCenter.Common.Services
{
    public class ChineseLetterToLetter
    {
        public static readonly IReadOnlyDictionary<char, string> ConversionTable = new Dictionary<char, string> {
            {'A', "诶"   },
            {'B', "比"},
            {'C', "西"},
            {'D', "迪"},
            {'E', "伊"},
            {'F', "艾弗"},
            {'G', "吉"},
            {'H', "艾尺"},
            {'I', "艾"},
            {'J', "杰"},
            {'K', "开"},
            {'L', "艾勒"},
            {'M', "艾马"},
            {'N', "艾娜"},
            {'O', "哦"},
            {'P',"屁"},
            {'Q', "吉吾"},
            {'R', "艾儿"},
            {'S', "艾丝"},
            {'T', "提"},
            {'U', "伊吾"},
            {'V', "维"},
            {'W', "豆贝尔维"},
            {'X', "艾克斯"},
            {'Y', "吾艾"},
            {'Z', "贼德"}
        };


        public static string Convert(string input)
        {
            string result = string.Empty;
            foreach (var c in input)
            {
                if (ChineseLetterToLetter.ConversionTable.TryGetValue(Char.ToUpper(c, CultureInfo.CurrentCulture), out var chineseLetter))
                {
                    result += chineseLetter;
                }
                else
                {
                    result += c;
                }
            }

            return result;
        }
    }
}
