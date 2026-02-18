namespace Api.AcceptanceTests.Features;

[TestClass]
public class BackgroundScraperTests : SystemTestBase
{
    [TestMethod]
    public void Given_BackgroundWorkerTrigger_Then_FetchesHTMLForActiveWatchesOlderThan24Hours()
    {
        // 1 - Web mock to return html. Ideally also has behavior
        // 2 - I pre-populated database and check only ones older than 24 hours or null are checked
        // So basically just check it fetches URLs available in the database
    }
}