using Microsoft.AspNetCore.Mvc;

namespace TestingEFCoreBehavior.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestPooledController : ControllerBase
    {
        private readonly ILogger<TestPooledController> _logger;
        private readonly TestContext _initContext;

        public TestPooledController(ILogger<TestPooledController> logger, TestContext initContext)
        {
            _logger = logger;
            _initContext = initContext;
        }

        [HttpGet(Name = "GetPooledItemIds")]
        public IEnumerable<int> Get()
        {
            var items = _initContext.Items.ToList();
            return items.Select(x => x.Id).ToArray();
        }
    }
}
