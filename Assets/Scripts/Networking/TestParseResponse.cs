public class TestParseResponse
{
    // this needs to match the key of the target tag in json
    // if for any reason this cant be the same as the key tag of the json you can use [JsonProperty(*Insert key here*)]
    public Inner parse;

    public override string ToString()
    {
        return parse.title;
    }
}

public class Inner
{
    public string title;
}
