namespace ApplicationService.Servico
{
    public class GestaoDeRiscoServico
    {

        public static bool PossuiGestaoDeRisco(string corRisco, bool? possuiGestaoRisco)
        {
            if (corRisco == null)
            {
                return false;
            }

            if (corRisco.Equals("Vermelho"))
            {
                if ((bool)possuiGestaoRisco)
                {
                    return true;
                }
            }
            else if (corRisco.Equals("Amarelo"))
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
