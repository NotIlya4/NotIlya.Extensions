namespace NotIlya.Extensions.Tests;

public class StringExtensionsTests
{
    private readonly List<string> _strings = new List<string>() {"Biba", "Boba", "Aaa", "asd"};
    
    [Fact]
    public void JoinStrings_StringsList_ConcatedStringsWithSeparator()
    {
        string expect = "Biba, Boba, Aaa, asd";

        string result = _strings.JoinStrings();
        
        Assert.Equal(expect, result);
    }
    
    [Fact]
    public void JoinStrings_StringsListWithSpecifiedSeparator_ConcatedStringsWithNewSeparator()
    {
        string expect = "Biba||Boba||Aaa||asd";

        string result = _strings.JoinStrings("||");
        
        Assert.Equal(expect, result);
    }
}