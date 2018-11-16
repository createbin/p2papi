using ZFCTAPI.Data.PRO;

namespace ZFCTAPI.Services.Messages
{
    public partial interface IWorkflowMessageService
    {
        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        bool SendCustomerEmailValidationMessage(P2PInfo p2pInfo, string validateCode, int languageId);
    }
}