﻿namespace Bshare.Functions
{
    public static class ShortLinkGenerator
    {
        // public int Length { get; set; }
        // public ShortLinkGenerator(int length)
        // {
        //     Length = length;
        // }

        public static string LinkGenerate(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz12345679";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}