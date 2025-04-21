using RSSCargo.BLL.Services;
using RSSCargo.DAL.Models;
using RSSCargo.BLL.Services.Contracts;
using RSSCargo.BLL.Services.Rss;
using Moq;
using System.Xml;

namespace RSSCargo.Tests;

public class RssFeedServiceTests
{
    private readonly RssFeedService _rssFeedService;
    private readonly Mock<IUserFeedService> _userFeedServiceMock;

    public RssFeedServiceTests()
    {
        _userFeedServiceMock = new Mock<IUserFeedService>();
        _rssFeedService = new RssFeedService(_userFeedServiceMock.Object);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void GetUserFeeds_ReturnsAllRssFeedsOfUser(int userId)
    {
        var rssFeedsOfUser = GetFeedsOfUser().Where(feed => feed.UserId == userId);

        var feedsOfUser = rssFeedsOfUser as UserFeed[] ?? rssFeedsOfUser.ToArray();
        _userFeedServiceMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(feedsOfUser);
        var result = _rssFeedService.GetUserFeeds(userId).Select(x => new { x.Link, x.Description, x.Title, x.Authors, x.Id, x.LastUpdatedTime }).ToList();
        var expectedResult = feedsOfUser.Select(userFeed => new RssFeed(userFeed.Id, userFeed.RssFeed)).Select(x => new { x.Link, x.Description, x.Title, x.Authors, x.Id, x.LastUpdatedTime }).ToList();

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    public void GetUserFeed_ReturnsRssFeedOfUserById(int userId, int feedId)
    {
        var rssFeedsOfUser = GetFeedsOfUser().Where(feed => feed.UserId == userId);

        var feedsOfUser = rssFeedsOfUser as UserFeed[] ?? rssFeedsOfUser.ToArray();
        _userFeedServiceMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(feedsOfUser);
        var rssFeedResult = _rssFeedService.GetUserFeed(userId, feedId);
        var result = new { rssFeedResult.Link, rssFeedResult.Description, rssFeedResult.Authors, rssFeedResult.Title, rssFeedResult.Id, rssFeedResult.LastUpdatedTime };
        var feed = feedsOfUser.First(userFeed => userFeed.Id == feedId);
        var expectedFeedResult = new RssFeed(feedId, feed.RssFeed);
        var expectedResult = new { expectedFeedResult.Link, expectedFeedResult.Description, expectedFeedResult.Authors, expectedFeedResult.Title, expectedFeedResult.Id, expectedFeedResult.LastUpdatedTime };

        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(5, 1)]
    public void GetUserFeed_IfNoRssFeedOrUserById_ExceptionThrown(int userId, int feedId)
    {
        var rssFeedsOfUser = GetFeedsOfUser().Where(feed => feed.UserId == userId);

        _userFeedServiceMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(rssFeedsOfUser);

        Assert.Throws<InvalidOperationException>(() => _rssFeedService.GetUserFeed(userId, feedId));
    }

    [Theory]
    [InlineData("http://rss.cnn.com/rss/edition_world.rss")]
    [InlineData("https://feeds.bbci.co.uk/news/rss.xml")]
    public void ValidateFeed_WithValidUrl_ReturnsTrue(string validUrl)
    {
        // Act
        var result = _rssFeedService.ValidateFeed(validUrl);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("http://invalid.url/not-existing-feed.xml")]
    [InlineData("not-a-url")]
    [InlineData("ftp://invalid.protocol/feed.xml")]
    public void ValidateFeed_WithInvalidUrl_ReturnsFalse(string invalidUrl)
    {
        // Act
        var result = _rssFeedService.ValidateFeed(invalidUrl);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetUserFeeds_WithEmptyUserFeeds_ReturnsEmptyCollection()
    {
        // Arrange
        const int userId = 999;
        var emptyFeedCollection = new List<UserFeed>();
        _userFeedServiceMock.Setup(repo => repo.GetUserFeeds(userId)).Returns(emptyFeedCollection);

        // Act
        var result = _rssFeedService.GetUserFeeds(userId);

        // Assert
        Assert.Empty(result);
    }

    private static IEnumerable<UserFeed> GetFeedsOfUser()
    {
        var userFeedFirst = new UserFeed
        {
            Id = 1,
            UserId = 1,
            RssFeed = "http://rss.cnn.com/rss/edition_world.rss"
        };
        var userFeedSecond = new UserFeed
        {
            Id = 2,
            UserId = 1,
            RssFeed = "http://rss.cnn.com/rss/edition_business.rss"
        };
        var userFeedThird = new UserFeed
        {
            Id = 3,
            UserId = 2,
            RssFeed = "http://rss.cnn.com/rss/edition_world.rss"
        };

        return new List<UserFeed> { userFeedFirst, userFeedSecond, userFeedThird };
    }
}

