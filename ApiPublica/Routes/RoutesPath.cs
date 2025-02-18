namespace ApiPublica.Routes
{
    public class RoutesPath
    {
        #region ControllerBase
        public const string ProducesJson = "application/json";
        public const string Base = "/Api";
        public const string BasePagination = "/{Pagina?}/{Total?}";
        public const string BaseOrder = "/{Columna?}/{Orden?}";
        #endregion

        #region AplicationValidateController
        public const string AplicationValidateController = "Aplicacion";
        public const string AplicationValidateController_LoginPKI = Base + "/" + AplicationValidateController + "/Login/";
        #endregion
        #region TransactionController
        public const string TransactionController = "Transaccion";
        public const string TransactionController_Create = Base + "/" + TransactionController + "/Create/";
        public const string TransactionController_ListTransaction = Base + "/" + TransactionController + "/ListTransaction/";
        public const string TransactionController_HistoriTransaction = Base + "/" + TransactionController + "/HistoriTransaction/";


        #endregion
    }
}
