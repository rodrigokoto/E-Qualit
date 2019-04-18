using Dominio.Entidade;
using Dominio.Enumerado;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context.BaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        private Cliente _cliente = null;
        private List<Funcionalidade> _funcionalidades = new List<Funcionalidade>();
        private List<Funcao> _funcoes = new List<Funcao>();
        private Usuario _usuarioAdmin = null;
        private Usuario _usuarioCoord = null;
        private Usuario _usuarioSupor = null;
        private Usuario _usuarioColab = null;

        private CargoProcesso _cargoProcesso = null;
        private UsuarioCargo _usuarioCargo = null;

        private Anexo _anexoClienteLogo = null;
        private ClienteLogo _clienteLogo = null;


        private Anexo _anexosSiteLogo = null;
        private SiteAnexo _siteAnexo = null;


        private Site _site = null;
        private List<UsuarioClienteSite> _usuarioClienteSite = new List<UsuarioClienteSite>();

        public void ConstrutorClienteLogo()
        {
            _anexoClienteLogo = new Anexo
            {
                IdAnexo = 1,
                Arquivo = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAHgAAAB4CAYAAAA5ZDbSAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMDY3IDc5LjE1Nzc0NywgMjAxNS8wMy8zMC0yMzo0MDo0MiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjcyOEU2QzExNDdEMzExRTc5NzVGRTg1MDg3QUIzQkIzIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjcyOEU2QzEyNDdEMzExRTc5NzVGRTg1MDg3QUIzQkIzIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6NzI4RTZDMEY0N0QzMTFFNzk3NUZFODUwODdBQjNCQjMiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6NzI4RTZDMTA0N0QzMTFFNzk3NUZFODUwODdBQjNCQjMiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7bOCyMAAAtaklEQVR42ux9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"),
                Extensao = "png",
                Nome = "cliente-padrao.png",
                DtAlteracao = DateTime.Now,
                DtCriacao = DateTime.Now,

            };

            _clienteLogo = new ClienteLogo
            {
                IdClienteLogo = 1,
                IdAnexo = 1,
                IdCliente = 1
            };
        }

        public void ConstrutorSiteLogo()
        {
            _anexosSiteLogo = new Anexo
            {
                IdAnexo = 2,
                Arquivo = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAHgAAAB4CAYAAAA5ZDbSAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyZpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuNi1jMDY3IDc5LjE1Nzc0NywgMjAxNS8wMy8zMC0yMzo0MDo0MiAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTUgKFdpbmRvd3MpIiB4bXBNTTpJbnN0YW5jZUlEPSJ4bXAuaWlkOjcyOEU2QzExNDdEMzExRTc5NzVGRTg1MDg3QUIzQkIzIiB4bXBNTTpEb2N1bWVudElEPSJ4bXAuZGlkOjcyOEU2QzEyNDdEMzExRTc5NzVGRTg1MDg3QUIzQkIzIj4gPHhtcE1NOkRlcml2ZWRGcm9tIHN0UmVmOmluc3RhbmNlSUQ9InhtcC5paWQ6NzI4RTZDMEY0N0QzMTFFNzk3NUZFODUwODdBQjNCQjMiIHN0UmVmOmRvY3VtZW50SUQ9InhtcC5kaWQ6NzI4RTZDMTA0N0QzMTFFNzk3NUZFODUwODdBQjNCQjMiLz4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz7bOCyMAAAtaklEQVR42ux9B5xU1fX/eW1mp26vlIWlChqqIIKCIFYEC0asUZNfNOav+Wtiior+UtSfiRr9x5iY2H6SbqKJiS1WVIxi7yJFlLJ9d3p57f7Pue/O7uyyyxZmlgW5fA4zO/Nm5r37vaefe540ZdENsB+OAqSxSJORapDGI81DGoVkZx2nILUjvYb0FtL7SG8jte4vE6HuR6B6kaYiLUGahTQFqRhJQ/IheXr5XDXSRKTlSHGkrUh/FNR+AOC9f/4E5DFIxwqOLUdyD+J7igWNRJqJdCHSXUi/R0ocAHhoRxXSQqRVSLMFFyo5lgYkBe5AWox0FdKnBwDO71AEl54iOJV07Ig8/6ZLLKJKpMuR3jkAcH7GAiEyj0Iasxd+n373TqSvI31wAODcjcOQviJ0bN1ePpfDkX6M9E2k+gMA79moERx7HtKEYXRepwiX6sYDAA9ukL+6Qrg6xw/TObsI6VmkVw8APLBxHNK3xONwHrViER4AeAAuCRkv3xkCqzhX41BhWTcO9xOV9/Lvz0H6DdLP9yFwaVDY84R94UT3FsClwq/8E9LZ+2D8gEKfi/cFN3NvnCAFKb4v3J99eUwW9P4BDu6qu+7fD8ClMQbpkAMiunNQsOIepLmwf4wycLJQX3gRHUQ6B2k1OEmC/WkQF0tI7IsKMOVgb0L6L8httme4jApwUoxtX0QRTZx7M9LF+ym4GWu65IsooinNdg3SJeJvC8lAMgVZWQuMwNfEZ2Qh8vaVQRKq6IsIcI0QW1TwtRkpBk75SxRJB6cuiglA6RwCQtyVCc4nP3kSOJUV1cN4/qj2y/tFBLgF6ZcC0IEOSXBGsQB3Gjjx6ROG4WQqwz3Yka+Ti+3BZ4mzE4J2IL2O9ATSX5DOBCdlN1wGg2E+ZNg3BgH9INKl4KTrPh4m52UKlXMA4BwC/RvhVz85DM4nuYfSaj8EmEkg2XJXYgM2nN8Ap+Ljsb08f6RGQl9EHdyz9WThepIZmAUpBFUWeNug6C5O9Dzb0iLcbdXs7et2gpNDJst7zl6av9hwDnIMKcAErmKq0DpuK0SrGkExXI6DrOkQaKgCf1MpWC6j64eQu73thcBk23lvV07/COl/kNaIoMNQj9AgPYV9H2ASu4qudYhlYsnWcZ9CdEQDKGkEV3a4VTE1iFc2QbS6AQ+RssxThuJbgWB9BXjai8AdCiLnp3sS56SL70a6bC8ESZqHuyWtymbuo4ikVwmM9omf4+U7OpZEbbyszQE3A3oHdyugWD2fR/uYbRCpboTK9yeDO+rflcsdPXiH8JOHsgIzhfT5cLdK1aYpn+SFe23VglQwwrmSdefoAQwt7kVQdWg8+GOofmcqaEkPivVdQKZtJeuHGGCy6DcNe4DTwXypEAm/3J2L1cINMDLMUoURcMeQi8nwkrpIRkv4xvQ4VIkNKrjbPOwBls19YPcKgkmLJYTimsS5v7EcLFQB3Yyut4S4DgzRWdHuhs+GPcDDBUJwAvdaln7bNUKE+rx1wqfAFBut7jKwFSv7Xdoz1DqEANPvpQ8A3PughAJljaiQ/CBwyl/KBNhbkJ4GZ7d9stN4Q32uWRAvb0HrurI7wBHBVWOG4NxpIb07rIUezmIyZe4VgMlfpU1ltKP+CHA2XHsF4JnIGvlQFHOmXfY3Q1aBuYr6OO2PQ3hkPQS3V3MDLEsPD1Xrhc3CBx9WQ5YlSKdNaA8ngTEGE8aWDSnAfgHqVwTHVmWJ5F3OFZxC+P8Lzj7gb3foO2GhE7DdfGICuH0IAR52Btamra1QVR6AFcceDKFIAv778qVDAnAh0rlIpyMdDAMrcaHzO02AR5mkpswbZFnL5DuTNe0AbcPQtVp4bzjpX+LcxuYYnHPqDDj2yEmw8qRZfMri8WTeAaY9tdcjzd8Nt/ZnfBnpJaRfcNQNDWIVzeCO+MEV82XHq4fCRaIf2zYcgNV1E9K6hdyahCsvXgSrL3c2ZLa3h0CWnPhDPgFehvQTcCoycjE6iswp+pYORkD3xcEdDuJVmBlwh6LiY+vedo/IgGpojsK8WWPgpu+fAKFoEg6dNhL0dAqSyTRyNGo4xvJmRZOxdLEQqWNz+L3UPYfKeNoZimXy3ztEdCf3DkUB3Bt7y4JWFRnC0RTUN4Vh0bzxcNeNp0JNdRkXKslECnTDQvClLsHxXANMabvVgntzPcgoo0Zm7ZnYNneTOg0tCpvV5HmOae7WIYWHlmMlME0LPtzQCHNnjUYj6kvw7f+aj+AWQTgShQykdFz31Ifa+coeJ2KoKpJ2DFLDkneEXyoLt6hMuEN7MgLCYANLM8HbUgqeUGG2/vULLs/nINfozaEEl9ydFLo+ChpS13//ODh24USYNa0O0skEhEMJkGSpLytVcsCVTPzf4pgwNijGJg6iCos1wggxuwFMZbDU2+o4GHijsswK5FdDsWhPqAjc0QDXw4KLq4ZAB78ITkg07zrWshjEEjoUuFVuMN28+iRYfuw0Pq3hcBhfk7n13FeuUg23nogOhhfcvnfB7dmE4Gqgaq3AbBc+d8EA0p3bBLi9DQL/d0jng5O7HShHW2LRQC8imrZyFuRx3m2hf/PuihmmDcmUASWFHvjamXPgtGUzoabcA4lEHIG3hRHVT71tWwHOGMnYNKQZIClx8PnfRJCbQXM3oOxHu4UpkKO8NiXIfwZO3fRPBWf3d1B5TG+pLzrBiYOUDP0dlI58eSj07Y6GCHxt1aFwy7XLwERANVXh3GzbjHP3gHxkEs0gGRTo5WKa2R6ItZ0A0fCRkIjO4rhKkpHr66D+j78d4GdaxQLpTT3UQH6LCP8BeWyCRhZyPKnDhxsb4KKz58JtPzoFmcsGAy3jOIJLulgahJkk9yiJ1FawjAqItx0DsfACzr0SLYTcVadQAJlaESUH8Jl2wflAbhKTWXeAx+QR3Aak5/PJuZ/taIfDZ9XCw3d/FW5E39Y0KYhhZtzZwS+cXnGXEAPUxUZyIpeNvsJXOaeTIUZ6OgdghwVgo/p5PIlnk3Svt60IgturshMNtMUln53wnoY8JRdoFutRJB91+Di466YvQ8DvhXQqCfG4zo2oPR19izTUyXp6NISavgzJKOpoOSlEtpSLa0sN4Pj6DPdSdSYVADC5o8x2ugiC5Gu8nmvfV1VliMbSsHFLMyw5Yjw8cNuZEPBpEEG/lgIWuQC3n4EO5uhmBDQVm851tT/4H7AtdKf2TOWxAYiBdsjq9OqIaDtjQVPyYkkeDSwKTT6Tqy8j4HTdgg8/qodFCybAWafMhu9cvAACATdEo2knWJHLhTQgPJQYpNDwIlT8wVeBWXvkdmb2BfdnvID0Sgbcbon+USK4ki8D6ze5NK52NkagsswPt//kFFh65AQ4aOJIFMkJ5NxUzsEdRKgST0BJQDp6KK+W9AVeQX3sFoGRAetkt4g+9WdQJqmZuJZEc9G2EdkRLGrjPzZP4FLl5BN7anAQ15JfS3r1pqtOhAVzxsHk8VXcrY9EItyv5UGLPFRYDyJkhaJRjnNOpsv2F65D/8wzGMMrmAk99jE+7fA/cX254j5wRX086Q9OcmFJHnUvBWYGvZMxE5H6fGcIFEWCn193MlxwxmF8nqKxGLdieNAij2NwyQbJcsR1bAY/WW/gbRFo6vfJyiLy5OrHsX8FavxJ0SvZgqLParJdJAp/npinuflIAJwc1MSiX7ujMcwBXjJ/PJx87MFwPoLrRKMG59MOHcDZOhlBdns34goNIRf3O1JI5TiL+nHcdiEiLYZ6199U7uxp6kwRUs/IyjzMC/lf1D9zwF3sMuL4w60tcN7ps+HIwybAV1cdxiVfNBrlelYawg02e5guxDOV05BAkP3Bl4WI7tfZUyXl/H4c96DQv3w3g7+hnOtg053O+L7L8zQvG5AeGag4puqKtvYEHDZzNNxy7cmweP5Y9Gv9qHvjIsw49P1lcpIPNtIjEVuVu1P9HPP74bcS9/6NuIkCGoXbalD/esHsDG7M7OciGcx4CPrZKjjDsQTs2NEl8NubVsLUSVVQXlYEppHaK1ybc4AlOTUQA4v05oX9OO733DVC3UvcS9UbiqGhcZXK6PAj+qnDBzrI336gXwYUcuWmLa0c2PmH1sHVlx0Fh80aD8zWs4Ddu12hclPRIVEKKwk261esgcRqXz0eqeZpDYJr2S6DbzhzRwI80Z+1SI7N05zQfuMtffoSCG44koSzT5kBK46ZAiuOmwGGnoJYLMrV1N4GdncAU/5wNDiNyzb2yZoILrN8kIhPA4/vPeEu9TqobPa8fpzXbRRcYOjr0o7E8o8ngCviz94fTF1rp+VhPh4FJ2vUu2nJnCqLxpYYrL5sCVx+keOlZfzZ4dbHLRtgMlpojy1VXdA2TLJkqHqBsj5P9+4uoF9s+SGVnAgeP7pLuwd4hQB5d+PPSPdmolakd93hAJieVAZcCmqcn4eZJH/3R7tziyKxFM/NUhrv6ksXc3B15FraTZBvf3ZPAaZ6Kron0Fe7vU9gnyHE1p0gKip2taRRjGqNffnBM8R37W78G+kHNJcELrVwIOPKcuvZlRuniNBkLkebuP71PfmztBUkiuCOqy2Do+aPgysuOgrKigp4mWpaN4aNON4dwN/sAdxszqZKSSrJeXhXmaWApETQTUJ7yNZ2F9ignQ2H9GE1X80jV2RYIaje1hJQU1186yn9WCSDGeSK/aEncLfXh+GgieUw65BpcNWlC6GyvBAs0+QJA8u2hzW4GYBJHK/s4zgqrTlN+IZWT1zMqKxHsnr7/NI+dG9KSIk3OLioaynnW/7xeCdz1JlcoHLcXHfUoUYq9/RkRH24pREm1pXDfbeeDRPGVqAaSqB1nBj2oHYHeCr0rzaKFgKVpTb0zKC9DjLYvgu931WlVRhVv+QBDeRcarpS8eEkB1xKKjjimbg3HzfwuD0T1CDgqEQmbZjg87jgnptPhyPn1sGY0RUdRtS+BG4GYKufTqy963FkUrrAF1iLvnC6JwuaskW0N2lxL9/5uTBs7smoc/oBd1shSIYKli+RrXtJgnwpx9dP3XluyCQFGlsi/CRuWX0SLDliMoweUcynh3za4WpE9Qfgj4WY6ium+/ku0R2mgqK18ArMXsaFvXGdLElbbMYuzw4Jtod3AsTKYGLLCEj7UWrbHeBOhBw3IUVOfBbdnR8qiqzvaAjzwMWsQ0bC6Sd+CS5YNY97iRRidIrdJNhXBwFM+1x/J6zI3kaTOKYrQ5ul4Cl8CQFuR8OjsDuDjwNn26iUNamZpx8alvUd/OtxVdOgNRoGiMfg4uWngNTsg3V/aQOPV4VYXM987r+EFb7HQ3bChttQxd6SSpvbt2xtgTNWTIcj5o6Hb54/n19XLBaD/WWoQvTeKvzLs6Br8bgtwP0h0j+7xumCoHo2gebeCTav7OgCbqFYMLM7jmfU2gzilmU9io/XxNLJjczCr4+H4bQjjodTZx0OZ805Ej77pA3m/PZWGOUt5B9DQ3UZPp6TI67lReWxRPrutvbEYwvnjYMffud4OHnpZPAH/PsVsN3dJKrWp92Aj4Nz188pwuFfLyI7rzi1WQo3qKgtoaptA3/xYzhpNoLg5RugOIdKvFLjRuSTMwnSlJ4GwzLBSKdfAdO4Bb/n0YJgcbKqsBjuOesSCCIHTxkxBooLnY2B20LNUFLo5ZyGenE0fucVsId3a+E77nCBRaJJih/fFo0kbxozqhjuveV0qK4s5UmBzkjU/jUkOPuhnlyiEhHUQO5lMW4lKx6QzRgU2AkYGfocjGA9tI7eCbLh4dxLJTwESto2r0HEf0zrIWboTDL0T/2q+psRxWUPKrK0hVjyymVnwbIZc6HUG+RfbaV1SOhOwZmmqpDUGfzg+n/AXx5994byUt8PlD2YePpOGyXFzqaoXuBS7vzmBfMv/8Z5R4KnQEFL2alsHC46tmvJTh92L8t+YF3tXvFlvW0Ab8F3W0BCi1jWHBfITkFRugGKUjuhWG9EzvKAyaQuZTpijs6PpRJXQSqNqJkt4C647ZrlZ69ZPGX654smTu2U+/i+iScQT8Q69zZKkmzbtqa5FMldoCl1Y8qOR1flXAoD+rxuzoEDBxdXqWVBWygJFeX+f926etmPli2dpjkGpYnWcZIJcCUYXNEey6JsL8OGzhuQZJM99CJacWctBXF+sgsK0o1QiqCasgc0OwnlyW343A0p2QsKcQXhrtig8l6XqNssazw+rD60doKntqT85erCsl8fPmbCf1YeOr9MVdW5qG/LddMMMGZ7TdN0C12fuakFJ8TYyznOttS29jj63axCJrUwSHD5am1NQO3IIrjuimNHHHfU1DsMIz1aN0w/cjWBm6u9s91LgDOAW9B5x5mkoDg4+6xi4m9KP2VuWBIXcQGiiKDUHgE8smV9xyVKyLF8RcsKFBgRCJgR9FQUTmnF5+hZoLJVBt6IF/wtQYiWREAzNDmWTnl97oL/ufyoZcmTps/Z5JK1nS63ayxaNTNT8UStyew6y7Tr8AtGyrJU2F2koLsCXo8Lf1+FR595Bx599iNAS5dvnxwovFxVIOeHIymYPL7C/s7FR8DyYw6ey2w2l2qPGUofWYFBSYU8jLSwgVLieVxQSgDckgU0LYhmETb+HJyCxN0W5KvBZH1HMEpWCvi+U0oBWiiiE2qAdgvvIud5CwU0tMq3VfB3oyVRYAlrE5pf71YGCsHv4a2bi4xkenNCT9GJBNCAKUZgyZIqxHmlA+h5ACfdp2lSsdfnLccfLtqwqX787fe8VPb5jjBUVwRAkgfGwLRAaU9PY0sUpk+tsa+9YiksWTBZRpXBs0F8/VKEbPg0AXYL6m/VR1R4NkQ7BciZvl1vdwccJay7g4NlmThIzhS1eZFbF4gEQYXQUSRKqIXQOhTPbXRC5Tsq6DN20hNNgJehLOpIOIWQQ0K7M2CoLUFJSRDtALnwjw+vL9zREK58493tP/9gQ2NZWbEPDS6ZV00MZKAbBs2tMZgysYJd9+1jYPH8gySqsIhEUx2CeLg2eO7nogsIGpf1WlyA/AE4e5jJ89kEPRtZvIr9FAmUlTgjlFgf2w2ievx7PfLAP5GT1+CjXrK9GNLBFKRSOtjJ/l0I1TKVlBRBU3O7+cCDb7au+dvrrZ9ub5/j97tGB/xucGkqz9b0WyzL3GeGtjDaC6UBuPLihRKBSxu5YvEUqKoykEnc14ZPhHGJ6NZDlDyinPqfpYlHX9phlciKVidL6qU4WxchV3p6NSdE3zHk0DttS7+NWdZGNS1BKBqH2nGl8Pe7fgDlJUGw0wbE0skO0Un/iCvdbo0H9K+79Rl44ZWN8J9H34XqL406OBgoeExVpFF0nD0AJOi7LdsC3bBhdHXRW19ZOSt85inTR6IurolEkl5ZUWAfuMVRHnxg+GU2wKMVreAOWXGd1IGkJHUzMFmWo+W8juLvcctIf8u2zI1kKO1oaIM50ybAvTdeAiMrinEZ0AbmFBQW+sA2LAjFTbj7Dy/Dw0+8Bx9+0sg5q7oy6GU2bQhnZw0q/IjSgDZP2xZbc+6pM26+5rKjI26Pa3JzS3QcWvyVqHIqhZopx3MvFnH30twY0MN6pFXb0MnqANXl/a6said1ylAnvIjANSOKzQh2CXJCFTc/LbtjAUiK63jJsiJgGt8w0np7ZUkA3vtoC8w77XuwcM4UuPenlyG4hbB1WzOsW78Frv/Fc2hYm7xCYmR1UWab5Bk4z2fsifjEM7kXfd4rkPPD/NxseytjXbxbstxHMqcTAIVlaT/xKDy2RgBO0bLi/QzguIoQkmuyWHa7z5FQkTFSZMiJKHaBmfoTtmWswde24kEjmOpeqbjklRKyKrNFlhE/o2iu05GLn0Cz6X/R7GVBvxdBtOCvT74KBZ5fwanHLIZrf/oEUNamMFDAKyXKSnx8Hdk2owjItTD4NoSUkbgPT+l7kZgejicMnqzvwSoNC/qgm0FQxpwqTaoVm4CXdDDaFdQsdXS2rBvufO5oTqmDPymMj//dp6IeLVQ197dBUQt5agw51Ea2NVPR+yQmXSmpaph8Ff66mf6XlY6/o3kCF0uqNqojaKOqKNndlzJLf9E29c00s4gh1I0shUeeWgd/fvgTGFU1GipK/R26VTzQd1BTljGDvC7qOPdrcPp9mDwUPnAkWgStE/pcxYmajidI0mwaSrIp+HwcG+Z3iZMzCpSkrs3eRftjDV7NL1VEbqmsFcyTmROQJxfJSEYesvT0d11ub5he02PtIGsu8nmTZiJ2A2N2qTtQegWIzmr0oLgKphmJ6CQCmLtaxFqmAYECF5QFi0FFTreZ2T3m/TOR3BjM+Ds47YY/6+omiTopUX3Rk5smZU2GY/yBs/CoD40imTg5r+M1vo4TpSbTep3E2F14yKLhaKZl3V+eInObFVl+RJblO4R/DCqK5cOQPFwM0TUaadM2Ug/bZiqUNpPI2G5gRhonzgDJ5S6XNNcipOldPUqaSFnBv8pZh11GIcckWs0VCG4RTuYunXqOh8EV0NEquR/pOuHo862Z1A0u0hABTVMo1gZoD/A4NEWtsn0jvoZlxwdvbgvxnhgBrxea20P4HTqUBAP8mFCU4gnMRFftE0VWPkLQFw1TO5wk12eoWZ9yacoDRYHAKwhwx6mqiqqOV1TFJVHMF8WwZSabbCO5iSH3ceeSaS7N662wTOtozeM/Sy3wHy4pmlfKTBrnFhXFt456W4+Q0cZ1OaO2DxaoWqDj1jpZg/y17w3iYqii5EZwSm3ixH1NLTEIR9MwrrYEli0/BJYurENfOAKRWDoLVCaMf+fRzorNRdC1i8YSyMHOq22RqGNc8s/ZZcxmX2fMOm2YgWoLe+JDPOe/Idc+jgvwM1yNyXgyBUW4SDu6zXKu49E7JpoFSrJpmhW2YVRqBb65CPoy2eNb6HJ5q9FPDgh51lHhjwsobiYjzXo8/JRlpF6RRHKfFoxHqwNNKcHD09nuCFmtN4FT7DcQrl0Lzm3jXwERu6aWuqcePwUoMDLrkBpYteIQ2Px5G4QjaZD6oTHlrM6sdN5y582pR+LzY1Ben4svzsCDtL0ieztHJllBoNKNrp5HEJ5BrqUtNs147nZv6khNRMIsHk/zi+Vr3LbKDVO6U5IKkratVSMjF0pWGt9JcwsbVzUX5ZJspVCrPgtm+reqy/2Orad2mEZKN/Uk33ylacXg8lWgmE5lny0ZVVQ9edwALnUrOF3xaCtpC1U9ck5risG5K6fB9d9bCh63Ci3tcfSrm3A9SqKH44AEqleEZGlD29E4UwfJTj24thddZVuEhl9jzu6SN3DyP+MhYF6oL7HOmMRukg16IhoyWFKY2ZQtQjkty7W0npmREOLNdgwS5EVFst+RwHgcZfsLLl/xm5apNyHn8hiwheLZMlOouZIg2U5miIhx44rPEt1o46T+RKb4YmPwJ3A2Yq8nMR/FhVhW4uXi95xTEdzvLoX2UAK2iZ5S5H7Z/Qe2iFvJEr9ByEL8/kPwZ8shv+0Qd8OkfCSZiPXjZbzPEwiMbWFOYoFlSp8GlC5EFGknAcpQ2e1oKdsRwbzhmVNwjk5vSJGstTJY/0IJjkpc+ZSCR7aFhkw6Tvq3EBQPlb8+gQbJ4xauEXKrHOO94zJoN+CFfQHLrXbDatAU+Qb8+INNzbEGSjgkUxbMmT4Cbll9HPh9bnC7FWhFcBPJfjcMc4kAPXHqXGEHjMLfJE4NDjGQ3OAHp5r1Q3D6cFE2aBteP+3w2AE5usEIWkf6i8AUtGZZeSe/cydiB4L6miKxF0F2vSkr8iYZf5wB+cMGWqHIqdFGkFSXB2Tvjw1buxS54AQNjPNReqzr9jtkdd8AjtjrXSYxm6ziR9pDyT/G4voLRYVuOP3Eg8FdoEIolIKLz5sDdbXFaBileZaJxLXcs+OrikDFSEFTcJq/hOc3ChdNhZL/xuGd88h4ZTdtFiAKMyeXS6XKG/CtJnyfuHMH4wUAubfTkYOt5yQrudECrVzq8KpIQOutyN3XW8x6nXFR2/keM+JOBEtRKy3m+rVlySfbQP2mYTxI2hrJtlfaVvxNkgLI8ZOQKWnj2sxdObZD4oRM074nEk//qaSo4N2F82r10TVFMH1qNaw45iDwFGj8wIbWGLo2CV5Rwrp+nkKNk5jDlVRDPR6vpUZwZiGee0kmApLn8isygrYLH3QzAvgZnuhW5ErkSBbtMJTYkN3+hwBmzcxO/Rl/dI5N/k5HuoiNR+f/XJlZUcb0DY7VLPMYtSbLXrQwF5u29k2TKcdlKlMYmq6mFfPJtq6UVPDiceRY+2c4wfMyPiiPtDDgWzDD0VQD/v0vy2IPlxf7nnCrKgoHG85bOR2OPXJiR2td2044xjuemyJLlDCgWPJ4PE8qE6oTlvkIPGasDJIHpDxYRl2+jhHX1QtR2oDng8/ZZ7bzNzIGd+d2Qk+35xvioZp6injyTxKTzrJBnZsREyj8vMjHl6FDMgu58Hm0xFFXAzm51RbFa231CJvJYxgYHcEyXAvIXfpVhcUHvxYITkbDK/UN5OSTSKyT9U0ilfoeN9SH68dPqPzbhWfMemz2tJFPzT90gsmsBDS3JnmfxspyP1rFCep9UYYieCziXIvrgvLSI7klLvF9ThTerMi+qXRX7twjgDN3USPKVE40gShIZA6AVDrTxBzfPAzDdGQS/qgLUj8Gpt2PYJRJwvWkaDVy9XxcAPNxwpIIcprZVHYjO8YYP4IJQZAAI7X9jpq609YUlc0AU29dzqzUJem0DbEkvyNICjn31cNmjHr68K/Me7KmMvDa/Fm1MG5sKbpUmt805DJNVUtQt1bhQqhLp62JCG5Gh45CjVA12HAwSQ0kcsZTKBV0mfpN4HN6zbbsKAdOgjZ8L4yLpFEA2A6dLYxR0nCw97mksspEfBihfFS2zStspt2COreciQI71hmF8gCJv45CwWxDIpUEO/m7soq5V/mCY/V4vGVVS0vz/9N1qzQYLHimosz3n7Ejil+dML78o2VLJtcvnDelEuzE8k1bWyc1t0TH4OxXGqZdwnO2EveVgxTI6CenUcEalaxEsygtuIo2dodUVW1RFIX+TpqmqRuGwSsc8bVUgdsTIVAt22pLpVKGx+MFv9/fpZqE6tRCoRCk0+mcdYEdOoBtMyuebK+R7XTcto3LkJMXdnaTZb0oJMuQJPYss+J/su34/Sia8Tt8C1Q1tHru7KmpEVUFaybUFv3nkMk19ZPHlQWrK/xfT+vpQ5saGyfjgdW0RZNEsohlU/SJZpWKp+rxJ8mqzICXEkDGxCOB2MacasOwcDfacaEQl7Wj7RBDLo243W6buNfr9YLL5eI6PZlM8i0q9HpBQQG6XAH+uo4+fFFREaxduxb+9c9/4vPOws94PAFnrFoFkw+aAol4DFWNwRt27wub0tSuuoqeWw9JYL6FNvKFCDAaRxKlykpFVIcUbkiiwnhgO/HxBYlZ96Ol3eQP1IDmGSG3hJIltWWRX8wc51u3auXcxEGTxs3Aj01rbWkdH0+kyouKinFm9FcikXCbZVntyKlR2rMkOVyXKR1NoOwgYCMCvEyNcAz6KB4n4FyaBiWlpfDrX90JDz38ENSOrgWPx4PGms17RF5wwQUwa+Z0WLduHfzxD7/nBQmUhVI1FRobGmDbtm18QXRmqCxobGwAVdWgtrYWLvvWtzjAtPtQGubbXaTC2plddZVl8wAHca/E7CrUtfOQm+tEjBZFm70DxflGFFwb0JKOUVjSlgIw4ZCL6Hbikh7fxsrc74MLpeOCBYvlQ6ZN8zY3t5hr176Qqq6ugXPPPQeaGltg9pxDIRjww86d9T1yQn9qltmuH0IQVCgrL4f7778Pbr/t53y/kdfr41zMo21IwWAQvAg4imSIIkh0TyJeJICf1xDYAnyPZYto/I4kaiFDN7gEOHrpUrjyyu/yz0do77DkZGPtbieWSW4415JVfNztfch6t6MWjXVek93tGjuOFTkBUmcUD6DzLMDz70g2UHy6d4Ap2pnRwXKXEKkT0gR+UyaKJI2uqYSa2sXglT6DYuVjT8woPixt2NNMwyy2mYVfy+zCYOFOFHXPNdTv2GxZJpx9zrlw+LzDYenSJbxYjt/dCye/taWFg0Di0ufz8eddsvP4vq7rHKQg5zyri/yhGq/777sf7rnnbigpKSHODNjMno3Aj8YL24AT+C4aVgnSpfibbpyUI5kT2VqPn3+OUV5Tkqh0Z77IWdeLBEc4E21ramqCxYsXww+uuhrXuAINfJHC/gdwUjehtqoYZk6qABtdJMNWyoC5ViPHfxWP8JEYo2pHWuEkHvH5Ztu0vmWaxqPbt2+HQgRp1Vlnc67T0YApLi5G4M/D173w8MP/gDfeeIMDKaJc/HtoYYzABfX82pfg308+AYVFRV1CnZZpwVNP/RtQ/+KF8yogijE/gNczXWK8c/tXs4oEaM8x3c+piocJAY6SnPpiAvwRcAr1yIqmjrbrM6tIkRVoam6CWbNnw8xZs2HFilNRL6chjtxN18xsNmwAHlSnu8zt1sbUlMDMg6rx4mzQLR+tl8sl56ZXfAG4NBX1pkb6tRRPxYWP4/ACbsdJaJo6deprZJX+9cE/c31HCXjSkxs2bODGzwcfvA87dmxHoAo6fpMA3PLpFihGA2jTpi2wcSMd69nF/CP9S+CKuaJ4l1vEPtzdHGRd+LpVPO0GWU59Z7NyH3Tzz2jRlpWVwevr18P7770H77z9NkTCYTgDF+tBU6YA7afS9TSkacck7GOtDDPg1tUUw/SJVRxc2lSNK4gmIqt/FXsVD6X9wJuY07T7/+BjLYI0DgGljjuvEeeSbsyAR+L27bffopowFM9+GI3Gkd2t+H3DRx/xqkzSobW1Y3Z5vwcdbkHnDbXC3Yw0CvTTHclngbMjYFtW/jkh0ojt0MPdzuj7yysqOMe+9ebrnJt+cfvPYdTIUXD6GWdCVXU11IwYAc0ozsnq3meakSY5uCUwbUIV6KadfcOmcui8vSsloqlS8t/i73fEJN+Mx/uRpgrOiGdPGImYjEjmYrkH8AL4PnEnTVrW+wr02CRmF79O6sFOe0NQb2EwuzfLnc6ZziUYLBQdegzYsmUzXPX9K+HIRYtg8dFLYfKkKfy6iKOHNcB0AYmUAXUjUCwj55L/2s3SJWQy7R/ehKy7pIgJelG8Nh86t4wSwAtF6o6CEk8KnZdJ7xHnHyRcpeclatxiOz00yAjDCaVFQs1NFwlRS1UfL2UvnD5GsfgsiejNTqUEF9us23VlOvxsgh664WXAJonEMx+VlfDW66/D008+idy8ClZ+eRUH2bbt4QkwXS31RB5bXQSzJ1fxmmebQU/uDcvSba4euCKRdVxK+NaXIJ0sAN6SBTCBR1WT1IJJx/m7iAAmH5VcFZrMYDAQ1HWDdkNk7ldMBQV0h5arhfjta1Bj8ltFavFFwcmt3Y5BgNk1hB84dWTr+yzFQCDJ1apAoJ9/7llYtvxk8Pl9kMLzHmpZ3aeXTkyaRm6tqymCOVNr+KYGy5Ycy03uQjZV84nnLtkZ2e8r4nUQx1lIEpJPvF4ojskcT+/RtlMVyatpKtVwi0YpDEKhNtLZMXz9PkmSb0H6AI8rQToZ6Wr8u7Lb7/dENILi0d/tPXEtCkXZVKGLtw6Ie1SVJ1n+9757II0+s7DqhxcHow0FVYUuGFWqwM6Gxs6649wsLrubAWR0kwYZUWtS3Jh8YGrzgC7QiYj1Ivy7HM/FQOAjSJ/i8/HAS27Ycvz7NwhMYx8JAkOI/xIRKcuWoZJTRG6HvF7fDT6fdy1+3ycD3TROXsCLa5/nKuXr37iE+/F7HWBeK2k7nnuR24Iilw476+P52hHfn9UiZdotuFzaOcgVP0ZOHkE3Ss46oJXxhpn8LzceHujH6Up9eQwUHg0G048gwBuIA8lqHtAqRimgadouAZuhSzZkXalNjUARWJfMoMxn8uc+zQTTYk4B+V4MqeJkU6rPhXQqPh8hDJbPheE2CV+b2AUaJnULKwzyhyVJC4VCfoq+jRkzTojdgX0lLQzy8cklHHKA5Swzt9idBleBxSNVBaoTMbGoioNRWG+vp0J1IUorBWeQz/ptYRxRRcdx4u/SzjW7Z4syUwRIIxqNwebNG7MDKP0e1H747rvvgqLiYjjjzLOhob7ekQRDwDNqubvTh1eo94bkhCcNW9pdWL8/hnd3mZTtp0q9iEfWx4/KWa+Txf038TfpWioIp5LceZCjqcuAmzG8KLkwGDVFn6UbYl23+mpQUAKcseosHk+n0dbaiu8lQM6TAaYqWZzpcOyg5ya7RjYhXKC5Ikr0nPj6dBbAPbUwMrs9t3tYJHbW8+zvcWUZaXnhjT2xggOBIPrtOvzkh9fxsGZxSSmE2ttg6THHwYQJk2DLp5vzYuOoOfzKZNbkUxamTvil1Omdmn0/AZ0brC3YtSDNkxUJA8caBm83jlayDMMiQW09SIhcWPjSbs51UNKA8skUC/nJj/4bSKeTy7fi5FNhwYIj4XTk6oICN7SF2iERj4sU5F4IVe5mtEJn0675wv2ZIv4msUmlOJmW/s1i0iTobFFMEaOvC8CoOpE61U7NAlcVn8uUnNK9D6nR+L1CPAegf7sSdqcGpCz3Sck6L6rnfi9XE1VeXt4hEdY+9yw898zT8PLLL0E4HIKzz/0KnLBsOdfTueDoXAJME/8UODvlibNWZb13REdQhVfus987NhDXD9QA9RzB3bSddKTQryugc8eBIt6nRfGYiG55RGSJWvy/LN6fknVdWtbzTAjV2+2a1azfyM4aUZE61TZTOpGySlcIP/3VXIpPcp0oYUGPzz/3DK/52rTxE55Nm33oXK6b9xTkXNabkIikrZ2/QuuzWerawEUWOvVFtDgulhTlQWdrCx+0KG4hP1bskJuPdC5SEMkk/1fsnpNEYfUD4vgWcfxRSFcjXSI+A+JzmR139PmEeJ7M7MLLosx7ukQbuvgpswYewpSkTU66TzoWH5aI388pUeUI5XJLUSePGDESTMOEH157DT+3qsqqPfafFcpr5nAkcDqeQADX4Qk24ON4SVEDkrNXqRWtyQcUTXtQcXl0GZWR5aTRaGG8jFbk25IsR2RZqcfHT/Cqn8LHvyM9j0TPnyNQcUKSvE5bUV7B10JIO/EzG/HxY/zMm/j4ijieqA3JRIohfYr0T6T3kNJUS4VkI7WI1x7Dc3yXL0SesZfexb/fw/Ny8b1YsnQ30DU5n8sL8QS9pnGrOxKJomsWgbq68WCYRq9hZMdK71ywKor9AnemuR2+NmnSpJyHpfjJIqku91myrN6CE5bp95zGc7qJObvznd2I6ZTo0UV3bZEUyWlKTdUO1i6uFq0Gci/48aqzw4a8OyeCn6l1sHsxuKQ+jTASEtQVwDIh05dEVmRN4tunbX0oYj2SMMg++uADOGnFCrj7ngdgZ0N9F1TzXtHR75OVlT8gp5LY/BplbEjMotMfs3BF8ubbaFWa6VS2T2NBj7ft6bZke3abBmJcDWQYu/n9nI/ML5BuLkGxvaci+v8LMABvfodLiZbswQAAAABJRU5ErkJggg=="),
                Extensao = "png",
                Nome = "cliente-padrao.png",
                DtAlteracao = DateTime.Now,
                DtCriacao = DateTime.Now,
            };

            _siteAnexo = new SiteAnexo
            {
                IdSiteAnexo = 1,
                IdAnexo = 2,
                IdSite = 1
            };
        }

        public void ConstroiLogos()
        {
            ConstrutorClienteLogo();
            ConstrutorSiteLogo();
        }

        public void ConstrutorCliente()
        {
            _cliente = new Cliente
            {
                IdCliente = 1,
                NmFantasia = "Primeiro cliente",
                NmUrlAcesso = "teste2",
                NuDiasTrocaSenha = 2,
                NuTentativaBloqueioLogin = 1,
                NuArmazenaSenha = 1,
                FlTemCaptcha = true,
                FlAtivo = true,
                DtValidadeContrato = DateTime.Now,
                FlExigeSenhaForte = true,
                
            };


        }

        public void ConstrutorUsuario()
        {
            _usuarioAdmin = new Usuario
            {
                IdUsuario = 1,
                NmCompleto = "Administrador",
                CdIdentificacao = "administrador@g2it.com.br",
                NuCPF = "13229007875",
                CdSenha = "BFD322B079C47C4FCF7F062B19BADE40D86334B0", //máster-ISOTEC
                DtExpiracao = DateTime.Now.AddYears(1),
                FlCompartilhado = false,
                FlRecebeEmail = false,
                FlBloqueado = false,
                FlAtivo = true,
                FlSexo = "M",
                NuFalhaLNoLogin = 0,
                DtAlteracaoSenha = DateTime.Now.AddYears(1),
                DtInclusao = DateTime.Now.AddYears(0),
                IdPerfil = 1
            };

            _usuarioCoord = new Usuario
            {
                IdUsuario = 2,
                NmCompleto = "Coordenador",
                CdIdentificacao = "coordenador@g2it.com.br",
                NuCPF = "77581237591",
                CdSenha = "69D9BC5D2561604B75B00F62294F3399101B6253",
                DtExpiracao = DateTime.Now.AddYears(2),
                FlCompartilhado = false,
                FlRecebeEmail = false,
                FlBloqueado = false,
                FlAtivo = true,
                FlSexo = "M",
                NuFalhaLNoLogin = 0,
                DtAlteracaoSenha = DateTime.Now.AddYears(1),
                DtInclusao = DateTime.Now.AddYears(0),
                IdPerfil = 3
            };
            _usuarioSupor = new Usuario
            {
                IdUsuario = 3,
                NmCompleto = "Suporte",
                CdIdentificacao = "suporte@g2it.com.br",
                NuCPF = "77581237591",
                CdSenha = "69D9BC5D2561604B75B00F62294F3399101B6253",
                DtExpiracao = DateTime.Now.AddYears(2),
                FlCompartilhado = false,
                FlRecebeEmail = false,
                FlBloqueado = false,
                FlAtivo = true,
                FlSexo = "M",
                NuFalhaLNoLogin = 0,
                DtAlteracaoSenha = DateTime.Now.AddYears(1),
                DtInclusao = DateTime.Now.AddYears(0),
                IdPerfil = 2
            };
            _usuarioColab = new Usuario
            {
                IdUsuario = 4,
                NmCompleto = "Colaborador",
                CdIdentificacao = "colaborador@g2it.com.br",
                NuCPF = "77581237591",
                CdSenha = "69D9BC5D2561604B75B00F62294F3399101B6253",
                DtExpiracao = DateTime.Now.AddYears(2),
                FlCompartilhado = false,
                FlRecebeEmail = false,
                FlBloqueado = false,
                FlAtivo = true,
                FlSexo = "M",
                NuFalhaLNoLogin = 0,
                DtAlteracaoSenha = DateTime.Now.AddYears(1),
                DtInclusao = DateTime.Now.AddYears(0),
                IdPerfil = 4
            };

            _cargoProcesso = new CargoProcesso
            {
                IdCargoProcesso = 1,
                IdCargo = 1,
                IdProcesso = 1,
                IdFuncao = 11
            };

            _usuarioCargo = new UsuarioCargo
            {
                IdUsuarioProcesso = 1,
                IdCargo = 1,
                IdUsuario = 4
            };
        }

        public void ConstrutorFuncionalidades()
        {
            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.Administrativo,
                Nome = "Administrativo",
                Tag = "FuncionalidadeNomeAdministrativo",
                Url = "Administrativo",
                NuOrdem = 1,
                CdFormulario = "1"
            });


            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.ControlDoc,
                Nome = "Control-Doc",
                Tag = "FuncionalidadeNomeControlDoc",
                Url = "ControlDoc/DocumentosElaboracao",
                NuOrdem = 2,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade,
                Nome = "Não Conformidade",
                Tag = "FuncionalidadeNomeNaoConformidade",
                Url = "NaoConformidade",
                NuOrdem = 3,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva,
                Nome = "Ação Corretiva",
                Tag = "FuncionalidadeNomeAcaoCorretiva",
                Url = "AcaoCorretiva",
                NuOrdem = 4,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.Indicadores,
                Nome = "Indicadores",
                Tag = "FuncionalidadeNomeIndicador",
                Url = "Indicador",
                NuOrdem = 5,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.Auditoria,
                Nome = "Auditoria",
                Tag = "FuncionalidadeNomeAuditoria",
                Url = "Auditoria",
                NuOrdem = 6,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica,
                Nome = "Análise Crítica",
                Tag = "FuncionalidadeNomeAnaliseCritica",
                Url = "AnaliseCritica",
                NuOrdem = 7,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.Licencas,
                Nome = "Licenças",
                Tag = "FuncionalidadeNomeAnaliseLicencas",
                Url = "Licencas",
                NuOrdem = 8,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.Instrumentos,
                Nome = "Instrumentos",
                Tag = "FuncionalidadeNomeInstrumento",
                Url = "Instrumento",
                NuOrdem = 9,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.Fornecedores,
                Nome = "Fornecedores",
                Tag = "FuncionalidadeNomeFornecedor",
                Url = "Fornecedor",
                NuOrdem = 10,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos,
                Nome = "Gestão de Riscos",
                Tag = "FuncionalidadeNomeGestaoDeRisco",
                Url = "GestaoDeRisco",                
                NuOrdem = 11,
                CdFormulario = "1"
            });


            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.RecursosHumanos,
                Nome = "Recursos Humanos",
                Tag = "FuncionalidadeNomeRH",
                Url = "RH",
                NuOrdem = 14,
                CdFormulario = "1"
            });

            _funcionalidades.Add(new Funcionalidade
            {
                IdFuncionalidade = (int)Funcionalidades.Docs,
                Nome = "Workflow",
                Tag = "FuncionalidadeNomeWorkflow",
                Url = "ControlDoc/Workflow",
                NuOrdem = 14,
                CdFormulario = "1"
            });

        }

        public void ConstrutorSite()
        {
            _site = new Site
            {
                IdSite = 1,
                IdCliente = 1,
                NmFantasia = "Primeira Unidade",
                NmRazaoSocial = "Razao Social",
                NuCNPJ = "08263979000100",
                DsObservacoes = "Observacao",
                DsFrase = "Frase de lembrete",
                FlAtivo = true,
                SiteFuncionalidades = new List<SiteFuncionalidade>
                {
                    new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 1,
                        IdFuncionalidade = (int)Funcionalidades.Administrativo,
                        IdSite =  1
                    },
                     new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 2,
                        IdFuncionalidade = (int)Funcionalidades.ControlDoc,
                        IdSite = 1
                    },
                      new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 3,
                        IdFuncionalidade = (int)Funcionalidades.NaoConformidade,
                        IdSite = 1
                    },
                     new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 4,
                        IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva,
                        IdSite = 1
                    },
                       new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 5,
                        IdFuncionalidade = (int)Funcionalidades.Indicadores,
                        IdSite = 1
                    },
                     new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 6,
                        IdFuncionalidade = (int)Funcionalidades.Auditoria,
                        IdSite = 1
                    },
                      new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 7,
                        IdFuncionalidade = (int)Funcionalidades.AnaliseCritica,
                        IdSite = 1
                    },
                     new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 8,
                        IdFuncionalidade = (int)Funcionalidades.Licencas,
                        IdSite = 1
                    },

                    new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 9,
                        IdFuncionalidade = (int)Funcionalidades.Instrumentos,
                        IdSite = 1
                    },

                    new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 10,
                        IdFuncionalidade = (int)Funcionalidades.Fornecedores,
                        IdSite = 1
                    },
                     new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 11,
                        IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos,
                        IdSite = 1
                    },

                    new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 12,
                        IdFuncionalidade = (int)Funcionalidades.RecursosHumanos,
                        IdSite = 1
                    },

                    new SiteFuncionalidade
                    {
                        IdSiteFuncionalidade = 13,
                        IdFuncionalidade = (int)Funcionalidades.Docs,
                        IdSite = 1
                    }

                },
                Processos = new List<Processo>
                {
                    new Processo
                    {
                        IdProcesso = 1,
                        IdSite = 1,
                        Nome = "Qualidade",
                        FlAtivo = true,
                        FlQualidade = true,
                        DataCadastro = DateTime.Now
                    },
                     new Processo
                    {
                        IdProcesso = 2,
                        IdSite = 1,
                        Nome = "Financeiro",
                        FlAtivo = true,
                        FlQualidade = true,
                        DataCadastro = DateTime.Now
                    }
                },
                Cargos = new List<Cargo>
                 {
                       new Cargo
                       {
                           IdCargo = 1,
                           NmNome = "Gerente de Qualidade",
                           IdSite = 1,
                           Ativo = true,
                       },
                       new Cargo
                       {
                           IdCargo = 2,
                           NmNome = "Gerente de Finanças",
                           IdSite = 1,
                           Ativo = true,
                       },
                 },

            };
        }

        public void ConstrutorFuncoes()
        {

            _funcoes.Add(new Funcao
            {
                IdFuncao = 1,
                NmNome = "Cadastro de Departamento",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.Administrativo
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 2,
                NmNome = "Cadastro de usuários",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.Administrativo
            });


            _funcoes.Add(new Funcao
            {
                IdFuncao = 3,
                NmNome = "Elaborar",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.RecursosHumanos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 4,
                NmNome = "Verificar",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.RecursosHumanos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 5,
                NmNome = "Aprovar",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.RecursosHumanos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 102,
                NmNome = "Elaborar",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 103,
                NmNome = "Verificar",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 104,
                NmNome = "Aprovar",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 6,
                NmNome = "Revisar",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 7,
                NmNome = "Visualizar",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 8,
                NmNome = "Imprimir",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 9,
                NmNome = "Cópia Controlada",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });
            _funcoes.Add(new Funcao
            {
                IdFuncao = 105,
                NmNome = "Cópia Não Controlada",
                NuOrdem = 8,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 10,
                NmNome = "Obsoletos",
                NuOrdem = 9,
                IdFuncionalidade = (int)Funcionalidades.ControlDoc
            });


            _funcoes.Add(new Funcao
            {
                IdFuncao = 11,
                NmNome = "Cadastrar",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 12,
                NmNome = "Criar Tipo de NC",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 13,
                NmNome = "Definir ação",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 14,
                NmNome = "Implementar ação",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 15,
                NmNome = "Reverificação",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 16,
                NmNome = "Verificação da eficácia",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 18,
                NmNome = "Destravar",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 19,
                NmNome = "Editar",
                NuOrdem = 8,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 20,
                NmNome = "Análise da Causa",
                NuOrdem = 9,
                IdFuncionalidade = (int)Funcionalidades.NaoConformidade
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 21,
                NmNome = "Definir as consequências",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 22,
                NmNome = "Definir ação",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 23,
                NmNome = "Implementar ação",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva
            });


            _funcoes.Add(new Funcao
            {
                IdFuncao = 24,
                NmNome = "Verificação da eficácia",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 25,
                NmNome = "Destravar",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 26,
                NmNome = "Editar",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.AcaoCorretiva
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 27,
                NmNome = "Realizar medição",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 28,
                NmNome = "Realizar análise",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 29,
                NmNome = "Análise da Causa",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 30,
                NmNome = "Definir ação",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });



            _funcoes.Add(new Funcao
            {
                IdFuncao = 31,
                NmNome = "Verificação da eficácia",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 32,
                NmNome = "Destravar",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 33,
                NmNome = "Alterar meta",
                NuOrdem = 8,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 34,
                NmNome = "Editar",
                NuOrdem = 9,
                IdFuncionalidade = (int)Funcionalidades.Indicadores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 35,
                NmNome = "Cadastro de Norma",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 36,
                NmNome = "Cadastro de Processo - Destravar",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 37,
                NmNome = "Cadastro de Processo - Editar",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 38,
                NmNome = "Cadastro de Processo - Excluir",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 39,
                NmNome = "Elaborar PAI - Destravar",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 40,
                NmNome = "Elaborar PAI - Editar",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 41,
                NmNome = "Elaborar PAI - Excluir",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 42,
                NmNome = "Elaborar PLAI - Destravar",
                NuOrdem = 8,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 43,
                NmNome = "Elaborar PLAI - Editar",
                NuOrdem = 9,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 44,
                NmNome = "Elaborar PLAI - Excluir",
                NuOrdem = 10,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 45,
                NmNome = "Anexar PLAI",
                NuOrdem = 11,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 46,
                NmNome = "Inserir Temas para análise",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 47,
                NmNome = "Registro da ata",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 48,
                NmNome = "Definir ação",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 49,
                NmNome = "Implementar ação",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 50,
                NmNome = "Verificação da eficácia",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 51,
                NmNome = "Destravar",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 52,
                NmNome = "Editar",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.AnaliseCritica
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 53,
                NmNome = "Cadastro de Licença",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.Licencas
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 54,
                NmNome = "Destravar",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.Licencas
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 55,
                NmNome = "Editar",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.Licencas
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 56,
                NmNome = "Excluir",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.Licencas
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 57,
                NmNome = "Cadastro de instrumento",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.Instrumentos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 58,
                NmNome = "Editar instrumento",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.Instrumentos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 59,
                NmNome = "Excluir instrumento",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.Instrumentos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 60,
                NmNome = "Cadastro de calibracao",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.Instrumentos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 61,
                NmNome = "Editar calibracao",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.Instrumentos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 62,
                NmNome = "Excluir calibracao",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.Instrumentos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 64,
                NmNome = "Cadastro de produtos - Destravar",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 65,
                NmNome = "Cadastro de produtos - Editar",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 66,
                NmNome = "Cadastro de produtos - Excluir",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 67,
                NmNome = "Critérios de criticidade - Destravar",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 68,
                NmNome = "Critérios de criticidade - Editar",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 69,
                NmNome = "Critérios de criticidade - Excluir",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 70,
                NmNome = "Avaliar Criticidade - Destravar",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 71,
                NmNome = "Avaliar Criticidade - Editar",
                NuOrdem = 8,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 72,
                NmNome = "Avaliar Criticidade - Excluir",
                NuOrdem = 9,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 73,
                NmNome = "Critérios de Qualificação - Editar",
                NuOrdem = 11,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 74,
                NmNome = "Critérios de Qualificação - Excluir",
                NuOrdem = 12,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 75,
                NmNome = "Critérios de Avaliação - Destravar",
                NuOrdem = 13,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 76,
                NmNome = "Critérios de Avaliação - Editar",
                NuOrdem = 14,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 77,
                NmNome = "Critérios de Avaliação - Excluir",
                NuOrdem = 15,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 78,
                NmNome = "Cadastro de Fornecedores - Destravar",
                NuOrdem = 16,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 79,
                NmNome = "Cadastro de Fornecedores - Editar",
                NuOrdem = 17,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 80,
                NmNome = "Cadastro de Fornecedores - Excluir",
                NuOrdem = 18,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 81,
                NmNome = "Realizar Qualificação - Destravar",
                NuOrdem = 19,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 82,
                NmNome = "Realizar Qualificação - Editar",
                NuOrdem = 20,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 83,
                NmNome = "Realizar Avaliação - Destravar",
                NuOrdem = 21,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 84,
                NmNome = "Realizar Avaliação - Editar",
                NuOrdem = 22,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 85,
                NmNome = "Realizar Reavaliação - Destravar",
                NuOrdem = 23,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 86,
                NmNome = "Realizar Reavaliação - Editar",
                NuOrdem = 24,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 87,
                NmNome = "Excluir Fornecedor",
                NuOrdem = 25,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 88,
                NmNome = "Critérios Qualificação - Destravar",
                NuOrdem = 10,
                IdFuncionalidade = (int)Funcionalidades.Fornecedores
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 89,
                NmNome = "Registro",
                NuOrdem = 1,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 90,
                NmNome = "Definir ação",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 91,
                NmNome = "Implementar ação",
                NuOrdem = 3,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 92,
                NmNome = "Reverificação",
                NuOrdem = 4,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 93,
                NmNome = "Verificação da eficácia",
                NuOrdem = 5,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 94,
                NmNome = "Anular AM - Destravar",
                NuOrdem = 6,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 95,
                NmNome = "Anular AM - Editar",
                NuOrdem = 7,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });

            _funcoes.Add(new Funcao
            {
                IdFuncao = 96,
                NmNome = "Cria nova GR",
                NuOrdem = 8,
                IdFuncionalidade = (int)Funcionalidades.GestaoDeRiscos
            });


            _funcoes.Add(new Funcao
            {
                IdFuncao = 99,
                NmNome = "Edição de Norma",
                NuOrdem = 2,
                IdFuncionalidade = (int)Funcionalidades.Auditoria
            });


        }

        public void ConstrutorUsuarioClienteSite()
        {
            _usuarioClienteSite = new List<UsuarioClienteSite> {
                new UsuarioClienteSite {
                    IdUsuarioClienteSite = 1,
                    IdUsuario = 2,
                    IdSite = 1,
                    IdCliente = 1
                },
            new UsuarioClienteSite
            {
                IdUsuarioClienteSite = 2,
                IdUsuario = 4,
                IdSite = 1,
                IdCliente = 1
            }
            };
        }

        public void ConstrutorDadosEssenciais()
        {
            ConstrutorCliente();
            ConstrutorUsuario();
            ConstrutorFuncionalidades();
            ConstrutorSite();
            ConstrutorFuncoes();
            ConstrutorUsuarioClienteSite();
            ConstrutorClienteLogo();
            ConstrutorSiteLogo();
            //ConstrutorSubModulos();
            //ConstrutorListaValor();

        }

        protected override void Seed(Context.BaseContext context)
        {
            ConstrutorDadosEssenciais();

            context.Usuario.AddOrUpdate(x => x.IdUsuario, _usuarioAdmin);
            context.Usuario.AddOrUpdate(x => x.IdUsuario, _usuarioCoord);
            context.Usuario.AddOrUpdate(x => x.IdUsuario, _usuarioSupor);
            context.Usuario.AddOrUpdate(x => x.IdUsuario, _usuarioColab);

            context.Cliente.AddOrUpdate(x => x.IdCliente, _cliente);

            context.Anexo.AddOrUpdate(x => x.IdAnexo, _anexoClienteLogo);
            context.ClienteLogo.AddOrUpdate(x => x.IdClienteLogo, _clienteLogo);

            _funcionalidades.ForEach(funcionalidade => context.Funcionalidade.AddOrUpdate(funcionalidade));

            context.Site.AddOrUpdate(_site);
            context.Anexo.AddOrUpdate(x => x.IdAnexo, _anexosSiteLogo);

            context.SiteAnexo.AddOrUpdate(x => x.IdSiteAnexo, _siteAnexo);

            context.CargoProcesso.AddOrUpdate(x => x.IdCargoProcesso, _cargoProcesso);

            context.UsuarioCargo.AddOrUpdate(x => x.IdUsuarioProcesso, _usuarioCargo);


            _site.SiteFuncionalidades.ToList()
                .ForEach(siteModulo =>
                context.SiteModulo.AddOrUpdate(x => x.IdSiteFuncionalidade,
                siteModulo
                ));

            _site.Processos.ToList()
                .ForEach(processo =>
                context.Processo.AddOrUpdate(processo)
                );

            _site.Cargos.ToList()
                .ForEach(cargo =>
                context.Cargo.AddOrUpdate(x=>x.IdCargo, cargo)
                );

            _funcoes.ForEach(funcao => context.Funcao.AddOrUpdate(x => x.IdFuncao, funcao));

            _usuarioClienteSite.ForEach(usuarioClienteSite => 
            context.UsuarioClienteSite.AddOrUpdate(x=>x.IdUsuarioClienteSite, usuarioClienteSite)
            );

        }
    }
}
