namespace MDFusionLabHaute.API.Helper
{
    public class PublicFile
    {
        public static ConnectionMapping<string>? _connection;
        public static ConnectionMapping<string> Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }
    }
}
