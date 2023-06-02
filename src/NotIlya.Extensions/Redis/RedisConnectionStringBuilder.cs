namespace NotIlya.Extensions.Redis;

public class RedisConnectionStringBuilder
{
    private const string DefaultDatabaseKey = "defaultDatabase";
    private const string PasswordKey = "password";
    private const string AllowAdminKey = "allowAdmin";
    
    private readonly Dictionary<string, string> _connectionStringParameters = new();
    private string? _server;
    public string Server 
    { 
        get => _server ?? throw new InvalidOperationException("Server not provided"); 
        set => _server = value; 
    }

    public int DefaultDatabase
    {
        get => int.Parse(_connectionStringParameters[DefaultDatabaseKey]);
        set => _connectionStringParameters[DefaultDatabaseKey] = value.ToString();
    }

    public string Password
    {
        get => _connectionStringParameters[PasswordKey];
        set => _connectionStringParameters[PasswordKey] = value;
    }

    public bool AllowAdmin
    {
        get => bool.Parse(_connectionStringParameters[AllowAdminKey]);
        set => _connectionStringParameters[AllowAdminKey] = value.ToString().ToLower();
    }

    public string ConnectionString
    {
        get => Build();
        set => Parse(value);
    }

    public RedisConnectionStringBuilder()
    {
        
    }
    
    public RedisConnectionStringBuilder(string connectionString)
    {
        Parse(connectionString);
    }

    private void Parse(string connectionString)
    {
        string[] parameters = connectionString.Split(",");
        Server = parameters[0];
        parameters = parameters[1..parameters.Length];
        foreach (string parameter in parameters)
        {
            string[] splittedParameter = parameter.Split("=");
            _connectionStringParameters[splittedParameter[0]] = splittedParameter[1];
        }
    }

    private string Build()
    {
        List<string> parameters = _connectionStringParameters.Select(k => $"{k.Key}={k.Value}").ToList();
        parameters.Insert(0, Server);
        return string.Join(",", parameters);
    }

    public void AddCustomParameter(string key, string value)
    {
        if (key == "Server")
        {
            Server = value;
            return;
        }
        
        _connectionStringParameters[key] = value;
    }
    
    public bool Equals(RedisConnectionStringBuilder? other)
    {
        if (other is null)
        {
            return false;
        }

        return this == other;
    }

    public override bool Equals(object? obj)
    {
        if (obj is RedisConnectionStringBuilder other)
        {
            return Equals(other);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_connectionStringParameters, Server);
    }

    public static bool operator ==(RedisConnectionStringBuilder obj1, RedisConnectionStringBuilder obj2)
    {
        bool dictEquals = obj1._connectionStringParameters.OrderBy(k => k.Key)
            .SequenceEqual(obj2._connectionStringParameters.OrderBy(k => k.Key));
        bool serverEquals = obj1.Server == obj2.Server;
        return dictEquals && serverEquals;
    }

    public static bool operator !=(RedisConnectionStringBuilder obj1, RedisConnectionStringBuilder obj2)
    {
        return !(obj1 == obj2);
    }
}