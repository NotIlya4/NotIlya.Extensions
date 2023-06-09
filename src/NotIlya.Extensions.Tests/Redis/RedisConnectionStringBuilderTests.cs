﻿using NotIlya.Extensions.Redis;

namespace NotIlya.Extensions.Tests.Redis;

public class RedisConnectionStringBuilderTests
{
    private readonly string _conn = "localhost:123,password=asdasd,allowAdmin=false";

    [Fact]
    public void Ctor_Conn_ParseIt()
    {
        var builder = new RedisConnectionStringBuilder(_conn);
        
        Assert.Equal("localhost:123", builder.Server);
        Assert.Equal("asdasd", builder.Password);
        Assert.Equal(false, builder.AllowAdmin);
    }

    [Fact]
    public void ConnectionString_Conn_ResultAfterParse()
    {
        var builder = new RedisConnectionStringBuilder(_conn);
        builder.AllowAdmin = true;
        
        Assert.Equal("localhost:123,password=asdasd,allowAdmin=true", builder.ConnectionString);
    }
}