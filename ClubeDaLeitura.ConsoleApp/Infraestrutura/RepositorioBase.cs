using System;
using ClubeDaLeitura.ConsoleApp.Dominio;

namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioBase
{
    public bool Editar(string idSelecionado, EntidadeBase novoEntidade)
    {
        EntidadeBase? entidadeSelecionada = SelecionarPorId(idSelecionado);

        if (entidadeSelecionada == null)
            return false;

        entidadeSelecionada.AtualizarRegistro(novoEntidade);

        return true;
    }

    public EntidadeBase? SelecionarPorId(string idSelecionado)
    {
        return null;
    }
}
