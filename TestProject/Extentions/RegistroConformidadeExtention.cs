using Dominio.Entidade;
using Dominio.Enumerado;
using System;

namespace TestProject.Extentions
{
    public static class RegistroConformidadeExtention
    {
        public static RegistroConformidade CriarNC(this RegistroConformidade nc) =>
                nc = new RegistroConformidade
                {
                    DescricaoRegistro = "O que Falhou",
                    IdEmissor = 1,
                    DtEmissao = DateTime.Now,
                    TipoRegistro = "nc", //enum
                    StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,//enum
                    IdSite = 1,
                    IdProcesso = 1,
                    EvidenciaImg = "imgEtapa1",
                    Tags = "Etapa1 Nova Nao Conformidade",

                    ENaoConformidadeAuditoria = false,


                    IdTipoNaoConformidade = 1,
                    IdResponsavelInicarAcaoImediata = 1,
                    IdUsuarioAlterou = 1,
                    IdUsuarioIncluiu = 1
                };



        public static RegistroConformidade GetByIdNC(this RegistroConformidade nc) =>
                nc = new RegistroConformidade
                {
                    IdRegistroConformidade = 1,
                    NuRegistro = 1,
                    DescricaoRegistro = "O que Falhou",
                    IdEmissor = 1,
                    DtEmissao = DateTime.Now,
                TipoRegistro = "nc", //enum
                StatusEtapa = (byte)EtapasRegistroConformidade.AcaoImediata,//enum
                IdSite = 1,
                IdProcesso = 1,
                EvidenciaImg = "imgEtapa1",
                Tags = "Etapa1 Nova Nao Conformidade",

                ENaoConformidadeAuditoria = false,


                IdTipoNaoConformidade = 1,
                IdResponsavelInicarAcaoImediata = 1,
                IdUsuarioAlterou = 1,
                IdUsuarioIncluiu = 1
            };
            

           
        
    }
}
