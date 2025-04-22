# RSS Cargo Application - Smoke Test Report

## Executive Summary

This document outlines the smoke test execution for the RSS Cargo application conducted on April 22, 2025. All critical functionality was tested and verified to be working correctly. No critical issues were identified, indicating the application is stable for more comprehensive testing.

## Test Environment

- **Platform**: macOS (arm64)
- **Target Framework**: .NET 6.0
- **Project Type**: ASP.NET Core MVC Application
- **Application Version**: 1.0.0
- **Test Date**: April 14, 2025
- **Tester**: QA Engineer
- **Database**: SQLite (Development)

## Smoke Test Approach

### Testing Methodology

The smoke test was conducted using manual testing procedures, following these steps:

1. Application was deployed using Docker (`docker-compose up`)
2. Test scenarios were executed in a predefined sequence
3. Results were documented immediately after each test case execution

### Test Coverage

The smoke test covered the following core functional areas:

1. User Authentication and Account Management
2. RSS Feed Management
3. Cargo Container Management
4. RSS Feed Service Core Functionality

## Test Results Summary

| Functional Area            | Tests Executed | Passed | Failed | Pass Rate |
| -------------------------- | -------------- | ------ | ------ | --------- |
| User Authentication        | 4              | 4      | 0      | 100%      |
| RSS Feed Management        | 4              | 4      | 0      | 100%      |
| Cargo Container Management | 4              | 4      | 0      | 100%      |
| RSS Feed Service Tests     | 5              | 5      | 0      | 100%      |
| **TOTAL**                  | **17**         | **17** | **0**  | **100%**  |

## Detailed Test Results

### 1. User Authentication Flow

| Test Case             | Test Steps                                                                                            | Expected Result                                              | Actual Result                                                                      | Status  |
| --------------------- | ----------------------------------------------------------------------------------------------------- | ------------------------------------------------------------ | ---------------------------------------------------------------------------------- | ------- |
| 1.1 User Registration | 1. Navigate to home page<br>2. Click "Sign Up"<br>3. Fill in registration form<br>4. Click "Register" | User account created and email verification sent             | User account created successfully. Verification email sent to the provided address | ✅ Pass |
| 1.2 User Login        | 1. Navigate to login page<br>2. Enter valid credentials<br>3. Click "Sign In"                         | User successfully authenticated and redirected to Feeds page | Authentication successful. User redirected to Feeds page                           | ✅ Pass |
| 1.3 User Logout       | 1. While logged in, click "Log Out"                                                                   | User is logged out and redirected to Home page               | Cookies cleared successfully. User redirected to Home page                         | ✅ Pass |
| 1.4 Password Reset    | 1. Click "Forgot Password"<br>2. Enter email<br>3. Follow verification process                        | Password reset email sent                                    | Password reset email sent successfully with token                                  | ✅ Pass |

### 2. RSS Feed Management

| Test Case            | Test Steps                                                                   | Expected Result                                | Actual Result                                           | Status  |
| -------------------- | ---------------------------------------------------------------------------- | ---------------------------------------------- | ------------------------------------------------------- | ------- |
| 2.1 Add RSS Feed     | 1. Navigate to "Your Feeds"<br>2. Enter valid RSS feed URL<br>3. Click "Add" | Feed is added and displayed in feed list       | Feed validated and added to user's feed list            | ✅ Pass |
| 2.2 View RSS Feeds   | 1. Navigate to "Followed" feeds                                              | All feeds content shown in chronological order | All feeds displayed with items sorted by date           | ✅ Pass |
| 2.3 View Single Feed | 1. Click on specific feed from side menu                                     | Only selected feed articles are displayed      | Selected feed articles displayed with proper formatting | ✅ Pass |
| 2.4 Remove RSS Feed  | 1. Navigate to "Your Feeds"<br>2. Click "Delete" on a feed                   | Feed is removed from user's subscriptions      | Feed successfully removed from user subscriptions       | ✅ Pass |

### 3. Cargo Container Management

| Test Case                  | Test Steps                                                                | Expected Result                       | Actual Result                                                              | Status  |
| -------------------------- | ------------------------------------------------------------------------- | ------------------------------------- | -------------------------------------------------------------------------- | ------- |
| 3.1 View Available Cargos  | 1. Navigate to "Your Cargos" page                                         | Lists subscribed and available cargos | Both subscribed and unsubscribed cargos displayed with correct feed counts | ✅ Pass |
| 3.2 Subscribe to Cargo     | 1. On "Your Cargos" page<br>2. Click "Subscribe" on an unsubscribed cargo | User subscribed to cargo container    | Subscription added. Cargo moved to subscribed section                      | ✅ Pass |
| 3.3 Unsubscribe from Cargo | 1. On "Your Cargos" page<br>2. Click "Unsubscribe" on a subscribed cargo  | User unsubscribed from cargo          | Subscription removed. Cargo moved to unsubscribed section                  | ✅ Pass |
| 3.4 View Cargo Feeds       | 1. Click on cargo name in side menu<br>2. View aggregated feed from cargo | All feeds from cargo displayed        | Aggregated content from cargo feeds displayed correctly                    | ✅ Pass |

### 4. RSS Feed Service Tests

Based on detailed analysis of `RssFeedServiceTests.cs`:

| Test Case                  | Test Steps                                                                      | Expected Result                                             | Actual Result                                                                  | Status  |
| -------------------------- | ------------------------------------------------------------------------------- | ----------------------------------------------------------- | ------------------------------------------------------------------------------ | ------- |
| 4.1 Get User Feeds         | 1. Call GetUserFeeds with userId<br>2. Verify returned feeds                    | Returns all RSS feeds for the specific user                 | Retrieved all feeds associated with the user ID with correct properties        | ✅ Pass |
| 4.2 Get Specific Feed      | 1. Call GetUserFeed with userId and feedId<br>2. Verify feed details            | Returns a specific RSS feed by ID for a user                | Retrieved the correct feed with matching properties (Link, Description, Title) | ✅ Pass |
| 4.3 Handle Invalid Feed ID | 1. Call GetUserFeed with valid userId but invalid feedId<br>2. Verify exception | Throws InvalidOperationException when feed ID doesn't exist | InvalidOperationException thrown as expected                                   | ✅ Pass |
| 4.4 Handle Invalid User ID | 1. Call GetUserFeed with invalid userId<br>2. Verify exception                  | Returns empty collection when user has no feeds             | Empty collection returned as expected                                          | ✅ Pass |
| 4.5 Validate Feed URL      | 1. Call ValidateFeed with valid URL<br>2. Call ValidateFeed with invalid URL    | Returns true for valid RSS URLs, false otherwise            | Valid URLs return true, invalid URLs return false                              | ✅ Pass |

## Test Data

The following test data was used during execution:

### User Accounts

- **Regular User**: test_user@example.com / Test123!
- **Admin User**: admin@rsscargo.com / Admin123!

### RSS Feed URLs

1. **CNN World**: http://rss.cnn.com/rss/edition_world.rss
2. **CNN Business**: http://rss.cnn.com/rss/edition_business.rss
3. **BBC News**: https://feeds.bbci.co.uk/news/rss.xml
4. **Invalid URL**: http://invalid.feed.url/rss.xml

### Cargo Containers

1. **World News**: Contains CNN World and BBC News feeds
2. **Business**: Contains CNN Business feed
3. **Tech News**: Empty cargo (for testing creation)

