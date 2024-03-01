using Microsoft.AspNetCore.Mvc;

namespace TestingEFCoreBehavior.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestScopedController : ControllerBase
    {
        private readonly ILogger<TestScopedController> _logger;
        private readonly InitContext _initContext;

        public TestScopedController(ILogger<TestScopedController> logger, InitContext initContext)
        {
            _logger = logger;
            _initContext = initContext;
        }

        [HttpGet(Name = "GetScopedItemIds")]
        public IEnumerable<int> Get()
        {
            var items = _initContext.GetItems();
            return items.Select(x => x.Id).ToArray();
        }
    }
}
