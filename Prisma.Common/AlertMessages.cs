
namespace MDFusionLabHaute.Common
{
    public class AlertMessages
    {
        public static Dictionary<int, string> Message { get { return _messages; } }

        private static Dictionary<int, string> _messages = new Dictionary<int, string>()
        {
            {100002,"Mandatory values are missing. Please check" },
            {100015,"Old data has been update. Please reload screen to reflect new values" },
            {100020,"already exists"},

            {100021,"User Name already exists"},
            {100022,"Email Id already exists"},
            {100023,"Consultant ID already exists"},

            {200024,"Unique ID already exists"},
            {200026,"Status Is Empty For The WorkFlow"},
            {200036,"Prime Broker and Currency already exists" },
            {200037,"More updated information is available. Please refresh your screen" },
            {200038,"Successfully added"},
            {200039,"Updated successfully"},
            {200040,"Deleted successfully"},
            {200097,"Selected Custodian Account,Currency SSI and Portfolio has been created already"},
            {200035,"Dispense successfully"},

            {300001,"Record Not Saved"},
            {300002,"Record Not Updated"},
            {300010,"MSInstructionId already exists"},
            {300011,"NT Account already exists"},
            {300012,"JPM Template already exists"},
            {300013,"Configuration exists for selected Prime Broker"},

            {1000148,"Successfully Approved"},
            {1000149,"Partially Approved"},
            {1000150,"Not Approved"},
            {1000151,"SSI Record is pending for approval, please edit in nonApproved list"},

            {4000152,"This QR code belongs to another user"},
            {4000153, "Code already exists in canister board" }
        };


    }

    public class Constants  
    {
        #region FOR RESPONSE OBJECT STATUS

        public const string RESPONSE_SUCCESS = "SUCCESS";
        public const string RESPONSE_ERROR = "ERROR";
        public const string RESPONSE_EXISTS = "EXISTS";
        public const string RESPONCE_EMAIL_EXISTS = "EMAILEXISTS";
        public const string RESPONSE_INVALID = "INVALID";
        public const string RESPONSE_ACTIVE = "ACTIVE";
        public const string ACCOUNT_NOT_EXIST = "Account doest not exist.";

        #endregion
    }
}
