﻿namespace NSE.WebApi.Core.Identidade
{
    public class IdentidadeSecrets
    {
        public string Secret { get; set; }
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; }
        public string Audiencia { get; set; }
    }
}
