namespace ClubeDaLeitura.ConsoleApp.Infraestrutura;

public class RepositorioCaixa
{
    private Caixa?[] caixas = new Caixa[100];

    public void Cadastrar(Caixa novaCaixa)
    {
        for (int i = 0; i < caixas.Length; i++)
        {
            if (caixas[i] == null)
            {
                caixas[i] = novaCaixa;
                break;
            }
        }
    }

    public Caixa?[] SelecionarTodas()
    {
        return caixas;
    }

    public bool Editar(string idSelecionado, Caixa novaCaixa)
    {
        Caixa? caixaSelecionada = null;

        for (int i = 0; i < caixas.Length; i++)
        {
            Caixa? c = caixas[i];

            if (c == null)
                continue;

            if (c.Id == idSelecionado)
            {
                caixaSelecionada = c;
                break;
            }
        }

        if (caixaSelecionada == null)
            return false;

        caixaSelecionada.AtualizarRegistro(novaCaixa);

        return true;
    }

    public bool Excluir(string idSelecionado)
    {
        for (int i = 0; i < caixas.Length; i++)
        {
            Caixa? c = caixas[i];

            if (c == null)
                continue;

            if (c.Id == idSelecionado)
            {
                caixas[i] = null;
                return true;
            }
        }

        return false;
    }
}