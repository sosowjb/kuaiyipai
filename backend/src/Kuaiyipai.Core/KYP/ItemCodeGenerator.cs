using System;

namespace Kuaiyipai.KYP
{
    public static class ItemCodeGenerator
    {
        public static string GenerateCode()
        {
            return "ITEM" + DateTime.Now.ToString("YYYYMMDDHHmmss") + DateTime.Now.Ticks;
        }
    }
}