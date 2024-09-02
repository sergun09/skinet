namespace Core.Specifications;

public class ProductSpecParams : PagingParams
{
   
    private List<string> _brands = new();

	public List<string> Brands
	{
		get { return _brands; }
		set 
		{ 
			_brands = value.SelectMany(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries)).ToList(); 
		}
	}

    private List<string> _types = new();

    public List<string> Types
    {
        get { return _types; }
        set
        {
            _types = value.SelectMany(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { get; set; }

    private string? _search;

    public string Search
    {
        get { return _search ?? ""; }
        set { _search = value.ToLower();}
    }

}
