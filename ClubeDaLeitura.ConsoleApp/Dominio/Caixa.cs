using System;
using System.Security.Cryptography;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Caixa
{

    public string Id { get; set; } = string.Empty;
    public string Etiqueta { get; set; } = string.Empty;
    public string Cor { get; set; } = string.Empty;

    public int DiasDeEmprestimos { get; set; } = 7;

    public Caixa(string etiqueta, string cor, int diasDeEmprestimos)
    {
        Id = Convert
                .ToHexString(RandomNumberGenerator.GetBytes(20))
                .ToLower()
                .Substring(0, 7);
        Etiqueta = etiqueta;
        Cor = cor;   
        DiasDeEmprestimos = diasDeEmprestimos;
    }
}
