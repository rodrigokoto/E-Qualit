namespace Dominio.Validacao.AnaliseCriticas
{
    public class GestaoDeRiscoDeveTerCriticidadeMediaOuAltaValidation
    {

        public static bool PossuiGestaoDeRisco(int corRisco, bool? possuiGestaoRisco)
        {
            if (corRisco == 0)
            {
                return false;
            }

            if (corRisco == 2)
            {
                if ((bool)possuiGestaoRisco)
                {
                    return true;
                }
            }
            else if (corRisco == 3)
            {
                if ((bool)possuiGestaoRisco)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
