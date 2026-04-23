﻿using ClubeDaLeitura.ConsoleApp.Apresentacao;

namespace ClubeDaLeitura.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        TelaPrincipal telaPrincipal = new TelaPrincipal();
        telaPrincipal.Executar();
    }
}