namespace ClubeDaLeitura.ConsoleApp.Dominio.Base;
public class Amigo : EntidadeBase
{
    public string Nome { get; set; }
    public string NomeResponsavel { get; set; }

    public string Telefone { get; set; }
}

public Amigo(string nome, string nomeResponsavel, string telefone)
    {
        Nome = nome;
        NomeResponsavel = nomeResponsavel;
        Telefone = telefone;
    }

    public override string[] Validar()
    {
        string erros = string.Empty;

        if (string.IsNullOrEmpty(Nome))
            erros += "O campo \"Nome\" é obrigatório;";

        else if (Nome.Length < 2 || Nome.Length > 100)
            erros += "O campo \"Nome \" deve conter entre 2 e 100 caracteres;";

        if (string.IsNullOrEmpty(NomeResponsavel))
            erros += "O campo \"Nome do Responsável\" é obrigatório;";

        else if (NomeResponsavel.Length < 2 || NomeResponsavel.Length > 100)
            erros += "O campo \"Nome do Responsável\" deve conter entre 2 e 100 caracteres;";


        int contadorDigitos = 0;

        bool contemLetraOuSimbolo = false;

        string telefoneEncurtado = Telefone.Replace(" ", "").Replace("-", "");
        for (int i = 0; i < telefoneEncurtado.Length; i++)
        {
            char caractereAtual = telefoneEncurtado[i];
            if (char.IsDigit(caractereAtual))
                contadorDigitos++;

            else
            {
                contemLetraOuSimbolo = true;
                break;
            }
        }

        if (contadorDigitos < 10 || contadorDigitos > 11)
            erros += "O campo \"Telefone\" deve conter entre 10 e 11 dígitos;";

        if (contemLetraOuSimbolo)
            erros += "O campo \"Telefone\" deve conter apenas digitos;";

        return erros.Split(';', StringSplitOptions.RemoveEmptyEntries);

    }

    public override void AtualizarRegistro(EntidadeBase entidadeAtualizada)
    {
        Amigo amigoAtualizado = (Amigo)entidadeAtualizada;

        Nome = Nome;
        NomeResponsavel = NomeResponsavel;
        Telefone = Telefone;
    }