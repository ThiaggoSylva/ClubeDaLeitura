using System;
using System.Globalization;

namespace ClubeDaLeitura.ConsoleApp.Util;

public static class ConsoleColorHelper
{
    public static ConsoleColor ObterCor(string cor)
    {
        if (string.IsNullOrWhiteSpace(cor))
            return ConsoleColor.White;

        cor = cor.Trim();

        // 🎯 HEX (#RRGGBB)
        if (cor.StartsWith("#"))
            return ConverterHexParaConsoleColor(cor);

        // 🎨 NOME
        return cor.ToLower() switch
        {
            "vermelho" => ConsoleColor.Red,
            "azul" => ConsoleColor.Blue,
            "verde" => ConsoleColor.Green,
            "amarelo" => ConsoleColor.Yellow,
            "roxo" => ConsoleColor.Magenta,
            "laranja" => ConsoleColor.DarkYellow,
            "preto" => ConsoleColor.Black,
            "branco" => ConsoleColor.White,
            "cinza" => ConsoleColor.Gray,
            "rosa" => ConsoleColor.Magenta,
            _ => ConsoleColor.White
        };
    }

    private static ConsoleColor ConverterHexParaConsoleColor(string hex)
    {
        try
        {
            hex = hex.Replace("#", "");

            int r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            int g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            int b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

            // 🎯 aproximação simples
            if (r > 200 && g < 100 && b < 100) return ConsoleColor.Red;
            if (r < 100 && g > 200 && b < 100) return ConsoleColor.Green;
            if (r < 100 && g < 100 && b > 200) return ConsoleColor.Blue;
            if (r > 200 && g > 200 && b < 100) return ConsoleColor.Yellow;
            if (r > 200 && b > 200) return ConsoleColor.Magenta;
            if (g > 200 && b > 200) return ConsoleColor.Cyan;

            return ConsoleColor.Gray;
        }
        catch
        {
            return ConsoleColor.White;
        }
    }
}
