namespace ApiPrincipal.Routes
{
    public class RoutesPath
    {
        #region ControllerBase
        public const string ProducesJson = "application/json";
        public const string Base = "/ApiPrincipal";
        public const string BasePagination = "/{Pagina?}/{Total?}";
        public const string BaseOrder = "/{Columna?}/{Orden?}";
        #endregion

        #region AplicationValidateController
        public const string AplicationValidateController = "Aplicacion";
        public const string AplicationValidateController_LoginPKI = Base + "/" + AplicationValidateController + "/Login/";

        #endregion

        #region  QuicklypayController
        public const string QuicklypayController = "Quicklypay";
        public const string QuicklypayController_QuicklypayStarter = Base + "/" + QuicklypayController + "/QuicklypayStarter/";
        public const string QuicklypayController_QuicklypayBank = Base + "/" + QuicklypayController + "/QuicklypayBank/";
        public const string QuicklypayController_QuicklypayCreated = Base + "/" + QuicklypayController + "/QuicklypayCreated/";
        public const string QuicklypayController_QuicklypayGetDataTransaction = Base + "/" + QuicklypayController + "/QuicklypayGetDataTransaction/";

        #endregion
    }
}
