namespace WebApp.Models;

// Partial class
public partial class WebApp {
    // Parameter for SQL Server binary types
    class SqlBinaryParameter : SqlMapper.ICustomQueryParameter
    {
        private readonly byte[]? _value;

        private bool _nullable = true;

        public byte[]? Value => _value;

        public SqlBinaryParameter(object? value, bool nullable = true)
        {
            if (value is byte[] b)
                _value = b;
            _nullable = nullable;
        }

        public void AddParameter(IDbCommand command, string name)
        {
            command.Parameters.Add(new SqlParameter
            {
                ParameterName = name,
                SqlDbType = SqlDbType.VarBinary,
                Value = _value != null ? _value : (_nullable ? DBNull.Value : new byte[0])
            });
        }
    }
} // End Partial class
