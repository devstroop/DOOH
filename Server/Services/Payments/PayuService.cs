namespace DOOH.Server.Services.Payments;

public class PayuService
{
    private ILogger<PayuService> _logger;
    private readonly HttpClient _httpClient;
    private bool _production;
    private string _key;
    private string _salt;

    public PayuService(HttpClient httpClient, IConfiguration configuration, ILogger<PayuService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        var env = configuration.GetValue<string>("Payment:Payu:Environment");
        _key = configuration.GetValue<string>("Payment:Payu:Key");
        _salt = configuration.GetValue<string>("Payment:Payu:Salt");
        _production = env.Equals("Production", StringComparison.CurrentCultureIgnoreCase);
        var baseAddr = _production ? "https://secure.payu.in" : "https://test.payu.in";
        _httpClient.BaseAddress = new Uri(baseAddr);
        // _httpClient.DefaultRequestHeaders.Add("key", key);
        // _httpClient.DefaultRequestHeaders.Add("salt", salt);
    }


    public async Task CreatePayment(decimal amount)
    {
        
        var temp = _httpClient.BaseAddress;
    }
}