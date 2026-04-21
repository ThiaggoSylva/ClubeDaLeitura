using ClubeDaLeitura.ConsoleApp.Dominio.Base;

namespace ClubeDaLeitura.ConsoleApp.Dominio;

public class Amigo : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string NomeResponsavel { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    public Amigo(string nome, string nomeResponsavel, string telefone)
    {
        Nome = nome;
        NomeResponsavel = nomeResponsavel;
        Telefone = telefone;
    }

    public override string[] Validar()
    {
        string erros = string.Empty;

        if (string.IsNullOrWhiteSpace(Nome))
            erros += "O campo \"Nome\" deve ser preenchido;";

        else if (Nome.Length < 2 || Nome.Length > 100)
            erros += "O campo \"Nome\" deve conter entre 2 e 100 caracteres;";

        if (string.IsNullOrWhiteSpace(NomeResponsavel))
            erros += "O campo \"Nome do Responsável\" deve ser preenchido;";

        else if (NomeResponsavel.Length < 2 || NomeResponsavel.Length > 100)
            erros += "O campo \"Nome do Responsável\" deve conter entre 2 e 100 caracteres;";

        int contadorDigitos = 0;
        bool contemLetraOuSimbolo = false;

        string telefoneEncurtado = Telefone.Replace(" ", "").Replace("-", "");

        for (int i = 0; i < telefoneEncurtado.Length; i++)
        {
            char caractereAtual = telefoneEncurtado[i];

            if (char.IsDigit(caractereAtual))
            {
                contadorDigitos++;
            }
            else
            {
                contemLetraOuSimbolo = true;
                break;
            }
        }

        if (contadorDigitos < 10 || contadorDigitos > 11)
            erros += "O campo \"Telefone\" deve conter entre 10 e 11 dígitos;";

        if (contemLetraOuSimbolo)
            erros += "O campo \"Telefone\" deve conter apenas dígitos;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);
    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Amigo amigoAtualizado = (Amigo)entidadeAtualizada;

        Nome = amigoAtualizado.Nome;
        NomeResponsavel = amigoAtualizado.NomeResponsavel;
        Telefone = amigoAtualizado.Telefone;
    }
}