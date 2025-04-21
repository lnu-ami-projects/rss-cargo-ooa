# RSS Cargo Application - Manual Testing Results

### User Authentication

#### TC001
**ID**: TC001  
**Description**: Verify that a user can register with valid data  
**Precondition**: User is not registered, application is running  
**Data**: Name = "TestUser", Email = "test@example.com", Password = "Test@1234"  
**Steps**:
1. Navigate to the application login page
2. Click on "Register" link
3. Enter valid name, email, and password
4. Click "Register" button

**Expected**: User is registered successfully and redirected to the Login Page  
**Actual**: User registered successfully and redirected to Login Page  
**Status**: ✅ PASS

#### TC002
**ID**: TC002  
**Description**: Verify that system prevents registration with an email that is already registered  
**Precondition**: User with email "test@example.com" is already registered  
**Data**: Name = "AnotherUser", Email = "test@example.com", Password = "Another@1234"  
**Steps**:
1. Navigate to the application login page
2. Click on "Register" link
3. Enter name, already registered email, and password
4. Click "Register" button

**Expected**: Error message "Email already in use" is displayed  
**Actual**: Error message "Email already in use" displayed  
**Status**: ✅ PASS

#### TC003
**ID**: TC003  
**Description**: Verify that a user cannot register with invalid email format  
**Precondition**: User is not registered, application is running  
**Data**: Name = "TestUser", Email = "invalidemail", Password = "Test@1234"  
**Steps**:
1. Navigate to the application login page
2. Click on "Register" link
3. Enter name, invalid email format, and password
4. Click "Register" button

**Expected**: Error message about invalid email format is displayed  
**Actual**: Error message "Please enter a valid email address" displayed  
**Status**: ✅ PASS

#### TC004
**ID**: TC004  
**Description**: Verify that a user cannot register with a short password  
**Precondition**: User is not registered, application is running  
**Data**: Name = "TestUser", Email = "test@example.com", Password = "123"  
**Steps**:
1. Navigate to the application login page
2. Click on "Register" link
3. Enter name, email, and short password
4. Click "Register" button

**Expected**: Error message about password length requirements is displayed  
**Actual**: Error message "Password must be at least 6 characters long" displayed  
**Status**: ✅ PASS

#### TC005
**ID**: TC005  
**Description**: Verify user login with valid credentials  
**Precondition**: User is registered with email "test@example.com" and password "Test@1234"  
**Data**: Email = "test@example.com", Password = "Test@1234"  
**Steps**:
1. Navigate to the application login page
2. Enter valid email and password
3. Click "Login" button

**Expected**: User is successfully logged in and redirected to the Main Page  
**Actual**: User logged in successfully and redirected to Main Page  
**Status**: ✅ PASS

#### TC006
**ID**: TC006  
**Description**: Verify user login with invalid email  
**Precondition**: Application is running  
**Data**: Email = "nonexistent@example.com", Password = "Test@1234"  
**Steps**:
1. Navigate to the application login page
2. Enter non-existent email and password
3. Click "Login" button

**Expected**: Error message "Invalid email or password" is displayed  
**Actual**: Error message "Invalid email or password" displayed  
**Status**: ✅ PASS

#### TC007
**ID**: TC007  
**Description**: Verify user login with incorrect password  
**Precondition**: User is registered with email "test@example.com" and password "Test@1234"  
**Data**: Email = "test@example.com", Password = "WrongPassword"  
**Steps**:
1. Navigate to the application login page
2. Enter correct email and incorrect password
3. Click "Login" button

**Expected**: Error message "Invalid email or password" is displayed  
**Actual**: Error message "Invalid email or password" displayed  
**Status**: ✅ PASS

#### TC008
**ID**: TC008  
**Description**: Verify user logout functionality  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Click on user profile in the top panel
2. Select "Logout" option

**Expected**: User is logged out and redirected to the Login Page  
**Actual**: User logged out and redirected to Login Page  
**Status**: ✅ PASS

#### TC009
**ID**: TC009  
**Description**: Verify password reset request functionality  
**Precondition**: User is registered with email "test@example.com"  
**Data**: Email = "test@example.com"  
**Steps**:
1. Navigate to the login page
2. Click on "Forgot Password" link
3. Enter registered email
4. Click "Submit" button

**Expected**: Password reset email is sent and confirmation message displayed  
**Actual**: Password reset email sent and confirmation message displayed  
**Status**: ✅ PASS

#### TC010
**ID**: TC010  
**Description**: Verify password reset link functionality  
**Precondition**: User has requested password reset and received email  
**Data**: NewPassword = "NewPass@123"  
**Steps**:
1. Open password reset email
2. Click on password reset link
3. Enter new password
4. Click "Reset Password" button

**Expected**: Password is reset successfully and confirmation message displayed  
**Actual**: Password reset successfully and confirmation message displayed  
**Status**: ✅ PASS

#### TC011
**ID**: TC011  
**Description**: Verify that session persists after browser refresh  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Refresh the browser while on any page

**Expected**: User remains logged in and on the same page  
**Actual**: User remained logged in and on the same page  
**Status**: ✅ PASS

#### TC012
**ID**: TC012  
**Description**: Verify login with "Remember Me" option checked  
**Precondition**: User is registered with email "test@example.com" and password "Test@1234"  
**Data**: Email = "test@example.com", Password = "Test@1234", RememberMe = checked  
**Steps**:
1. Navigate to the login page
2. Enter valid email and password
3. Check "Remember Me" checkbox
4. Click "Login" button
5. Close browser and reopen
6. Navigate to application URL

**Expected**: User is still logged in after browser restart  
**Actual**: User remained logged in after browser restart  
**Status**: ✅ PASS

#### TC013
**ID**: TC013  
**Description**: Verify registration with password complexity requirements  
**Precondition**: User is not registered, application is running  
**Data**: Name = "ComplexUser", Email = "complex@example.com", Password = "Simple1"  
**Steps**:
1. Navigate to the registration page
2. Enter name, email, and password without special characters
3. Click "Register" button

**Expected**: Error message about password complexity requirements is displayed  
**Actual**: Error message "Password must contain at least one special character" displayed  
**Status**: ✅ PASS

#### TC014
**ID**: TC014  
**Description**: Verify user registration with very long name  
**Precondition**: User is not registered, application is running  
**Data**: Name = 80 character long string, Email = "longname@example.com", Password = "Test@1234"  
**Steps**:
1. Navigate to the registration page
2. Enter very long name, valid email and password
3. Click "Register" button

**Expected**: User is registered successfully  
**Actual**: User registered, but name was not properly truncated in UI 
**Status**: ❌ FAIL

#### TC015
**ID**: TC015  
**Description**: Verify user account lockout after multiple failed login attempts  
**Precondition**: User is registered with email "test@example.com" and password "Test@1234"  
**Data**: Email = "test@example.com", Password = "WrongPassword"  
**Steps**:
1. Navigate to the login page
2. Enter correct email but incorrect password
3. Click "Login" button
4. Repeat steps 2-3 five more times

**Expected**: Account is temporarily locked and message displayed  
**Actual**: Account was temporarily locked and appropriate message displayed  
**Status**: ✅ PASS

### RSS Feed Management

#### TC016
**ID**: TC016  
**Description**: Verify adding a valid RSS feed URL  
**Precondition**: User is logged in  
**Data**: RSS URL = "https://news.google.com/rss"  
**Steps**:
1. Enter valid RSS URL in the add feed field
2. Click "Add" button

**Expected**: RSS feed is added to the user's subscriptions  
**Actual**: RSS feed was added to the user's subscriptions  
**Status**: ✅ PASS

#### TC017
**ID**: TC017  
**Description**: Verify adding an invalid RSS feed URL  
**Precondition**: User is logged in  
**Data**: RSS URL = "https://invalid-url.com/not-rss"  
**Steps**:
1. Enter invalid RSS URL in the add feed field
2. Click "Add" button

**Expected**: Error message "Invalid RSS feed URL" is displayed  
**Actual**: Error message "Invalid RSS feed URL" displayed  
**Status**: ✅ PASS

#### TC018
**ID**: TC018  
**Description**: Verify viewing all subscribed RSS feeds  
**Precondition**: User is logged in and has subscribed to at least two RSS feeds  
**Data**: N/A  
**Steps**:
1. Click on "All" under "Feeds" section in the side panel

**Expected**: All articles from subscribed RSS feeds are displayed on the Main Page  
**Actual**: All articles from subscribed feeds displayed on Main Page  
**Status**: ✅ PASS

#### TC019
**ID**: TC019  
**Description**: Verify filtering articles from specific RSS feed  
**Precondition**: User is logged in and has subscribed to multiple RSS feeds  
**Data**: N/A  
**Steps**:
1. Click on a specific RSS feed name in the feeds list

**Expected**: Only articles from the selected RSS feed are displayed  
**Actual**: Only articles from selected RSS feed displayed  
**Status**: ✅ PASS

#### TC020
**ID**: TC020  
**Description**: Verify removing an RSS feed from subscriptions  
**Precondition**: User is logged in and has subscribed to at least one RSS feed  
**Data**: N/A  
**Steps**:
1. Navigate to subscribed feeds list
2. Find the RSS feed to remove
3. Click "Unsubscribe" or remove button

**Expected**: RSS feed is removed from the user's subscriptions  
**Actual**: RSS feed removed from user's subscriptions  
**Status**: ✅ PASS

#### TC021
**ID**: TC021  
**Description**: Verify adding duplicate RSS feed URL  
**Precondition**: User is logged in and has already subscribed to "https://news.google.com/rss"  
**Data**: RSS URL = "https://news.google.com/rss"  
**Steps**:
1. Enter already subscribed RSS URL
2. Click "Add" button

**Expected**: Error message "You are already subscribed to this feed" is displayed  
**Actual**: Error message "You are already subscribed to this feed" displayed  
**Status**: ✅ PASS

#### TC022
**ID**: TC022  
**Description**: Verify RSS feed validation for proper XML format  
**Precondition**: User is logged in  
**Data**: URL = "https://example.com" (valid website but not RSS)  
**Steps**:
1. Enter URL of regular webpage (not RSS)
2. Click "Add" button

**Expected**: Error message "URL does not point to a valid RSS feed" is displayed  
**Actual**: Error message "URL does not point to a valid RSS feed" displayed  
**Status**: ✅ PASS

#### TC023
**ID**: TC023  
**Description**: Verify refresh functionality for RSS feeds  
**Precondition**: User is logged in and has subscribed to at least one RSS feed  
**Data**: N/A  
**Steps**:
1. Navigate to subscribed feeds
2. Click refresh button

**Expected**: Feeds are refreshed with latest content  
**Actual**: Feeds refreshed with latest content  
**Status**: ✅ PASS

#### TC024
**ID**: TC024  
**Description**: Verify adding RSS feed with special characters in URL  
**Precondition**: User is logged in  
**Data**: RSS URL = "https://example.com/rss?id=123&type=news"  
**Steps**:
1. Enter RSS URL with special characters
2. Click "Add" button

**Expected**: RSS feed is added successfully  
**Actual**: Feed was not added; application did not handle special characters correctly
**Status**: ❌ FAIL

#### TC025
**ID**: TC025  
**Description**: Verify sorting of RSS feeds by name  
**Precondition**: User is logged in and has multiple RSS feeds  
**Data**: N/A  
**Steps**:
1. Navigate to subscribed feeds
2. Click on sort option for name

**Expected**: Feeds are displayed in alphabetical order  
**Actual**: Feeds displayed in alphabetical order  
**Status**: ✅ PASS

#### TC026
**ID**: TC026  
**Description**: Verify sorting of RSS feeds by most recent update  
**Precondition**: User is logged in and has multiple RSS feeds  
**Data**: N/A  
**Steps**:
1. Navigate to subscribed feeds
2. Click on sort option for most recent update

**Expected**: Feeds are displayed with most recently updated first  
**Actual**: Feeds displayed with most recently updated first  
**Status**: ✅ PASS

#### TC027
**ID**: TC027  
**Description**: Verify adding feed with very long title  
**Precondition**: User is logged in  
**Data**: RSS feed with very long title (>100 characters)  
**Steps**:
1. Add RSS feed with very long title
2. View feeds list

**Expected**: Feed title is properly truncated in the UI  
**Actual**: Feed title overflowed in UI; not truncated properly
**Status**: ❌ FAIL

#### TC028
**ID**: TC028  
**Description**: Verify RSS feed count in sidebar  
**Precondition**: User is logged in and has subscribed to multiple RSS feeds  
**Data**: N/A  
**Steps**:
1. Add a new RSS feed
2. Check the feed count in sidebar

**Expected**: Feed count is incremented by 1  
**Actual**: Feed count incremented by 1  
**Status**: ✅ PASS

#### TC029
**ID**: TC029  
**Description**: Verify handling of RSS feed with no items  
**Precondition**: User is logged in  
**Data**: RSS URL = URL to empty RSS feed  
**Steps**:
1. Add RSS feed with no items
2. View the feed

**Expected**: "No items in this feed" message is displayed  
**Actual**: "No items in this feed" message displayed  
**Status**: ✅ PASS

#### TC030
**ID**: TC030  
**Description**: Verify feed article count  
**Precondition**: User is logged in and subscribed to feeds  
**Data**: N/A  
**Steps**:
1. View a specific RSS feed
2. Count the number of articles

**Expected**: Number of articles matches feed metadata (up to maximum limit)  
**Actual**: Number of articles matched feed metadata  
**Status**: ✅ PASS

#### TC031
**ID**: TC031  
**Description**: Verify RSS feed article date sorting  
**Precondition**: User is logged in and subscribed to feeds  
**Data**: N/A  
**Steps**:
1. View a specific RSS feed
2. Check the article dates

**Expected**: Articles are sorted by date in descending order (newest first)  
**Actual**: Articles sorted by date with newest first  
**Status**: ✅ PASS

#### TC032
**ID**: TC032  
**Description**: Verify editing RSS feed display name  
**Precondition**: User is logged in and has at least one RSS feed  
**Data**: New name = "Custom Feed Name"  
**Steps**:
1. Navigate to feed settings
2. Edit feed display name
3. Save changes

**Expected**: Feed is displayed with new custom name  
**Actual**: Feed displayed with new custom name  
**Status**: ✅ PASS

#### TC033
**ID**: TC033  
**Description**: Verify marking feed article as read  
**Precondition**: User is logged in and has unread articles  
**Data**: N/A  
**Steps**:
1. View feed articles
2. Click on "Mark as read" for an article

**Expected**: Article is marked as read and visually indicated  
**Actual**: Article marked as read with visual indication  
**Status**: ✅ PASS

#### TC034
**ID**: TC034  
**Description**: Verify marking all feed articles as read  
**Precondition**: User is logged in and has unread articles  
**Data**: N/A  
**Steps**:
1. View feed articles
2. Click "Mark all as read" button

**Expected**: All articles are marked as read  
**Actual**: All articles marked as read  
**Status**: ✅ PASS

#### TC035
**ID**: TC035  
**Description**: Verify filtering RSS feeds by unread articles  
**Precondition**: User is logged in and has mix of read and unread articles  
**Data**: N/A  
**Steps**:
1. View feed list
2. Click "Unread" filter option

**Expected**: Only unread articles are displayed  
**Actual**: Some read articles still showed up under "Unread" filter  
**Status**: ❌ FAIL

#### TC036
**ID**: TC036  
**Description**: Verify feed auto-refresh functionality  
**Precondition**: User is logged in and auto-refresh is enabled  
**Data**: N/A  
**Steps**:
1. View feeds
2. Wait for auto-refresh interval

**Expected**: Feeds automatically refresh with new content  
**Actual**: Feeds automatically refreshed with new content  
**Status**: ✅ PASS

#### TC037
**ID**: TC037  
**Description**: Verify feed health/status indicator  
**Precondition**: User is logged in and has feeds with different statuses  
**Data**: N/A  
**Steps**:
1. View feeds list
2. Check status indicators

**Expected**: Feed status indicators show correct states (healthy, error)  
**Actual**: Feed status indicators showed correct states  
**Status**: ✅ PASS

#### TC038
**ID**: TC038  
**Description**: Verify batch operations - select multiple feeds  
**Precondition**: User is logged in and has multiple feeds  
**Data**: N/A  
**Steps**:
1. View feeds list
2. Select multiple feeds using checkboxes
3. Check that the count of selected feeds is correct

**Expected**: Multiple feeds selection works correctly  
**Actual**: Multiple feeds selection worked correctly with accurate count  
**Status**: ✅ PASS

#### TC039
**ID**: TC039  
**Description**: Verify batch delete operation  
**Precondition**: User is logged in and has multiple feeds selected  
**Data**: N/A  
**Steps**:
1. Select multiple feeds using checkboxes
2. Click "Delete selected" button
3. Confirm deletion

**Expected**: All selected feeds are deleted  
**Actual**: All selected feeds deleted  
**Status**: ✅ PASS

#### TC040
**ID**: TC040  
**Description**: Verify feed URL validation for malformed URLs  
**Precondition**: User is logged in  
**Data**: RSS URL = "htp:/malformed-url"  
**Steps**:
1. Enter malformed URL
2. Click "Add" button

**Expected**: Error message about invalid URL format is displayed  
**Actual**: Error message about invalid URL format displayed  
**Status**: ✅ PASS

### Cargo Container Management

#### TC041
**ID**: TC041  
**Description**: Verify creating new cargo container  
**Precondition**: User is logged in  
**Data**: Container name = "Tech News"  
**Steps**:
1. Navigate to cargo containers
2. Click "Create New Container"
3. Enter container name
4. Save container

**Expected**: New cargo container is created  
**Actual**: New cargo container created  
**Status**: ✅ PASS

#### TC042
**ID**: TC042  
**Description**: Verify adding RSS feed to cargo container  
**Precondition**: User is logged in, has RSS feeds and cargo container  
**Data**: N/A  
**Steps**:
1. Navigate to cargo container details
2. Click "Add Feed"
3. Select RSS feed from list
4. Save changes

**Expected**: RSS feed is added to cargo container  
**Actual**: RSS feed added to cargo container  
**Status**: ✅ PASS

#### TC043
**ID**: TC043  
**Description**: Verify removing RSS feed from cargo container  
**Precondition**: User is logged in, cargo container has feeds  
**Data**: N/A  
**Steps**:
1. Navigate to cargo container details
2. Find feed to remove
3. Click "Remove" button
4. Confirm removal

**Expected**: RSS feed is removed from cargo container  
**Actual**: RSS feed removed from cargo container  
**Status**: ✅ PASS

#### TC044
**ID**: TC044  
**Description**: Verify viewing available cargo containers  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Click "Follow New" under "Cargos" section

**Expected**: Available cargo containers are displayed  
**Actual**: Available cargo containers displayed  
**Status**: ✅ PASS

#### TC045
**ID**: TC045  
**Description**: Verify subscribing to a cargo container  
**Precondition**: User is logged in and there are available containers  
**Data**: N/A  
**Steps**:
1. Click "Follow New" under "Cargos" section
2. Select a cargo container
3. Click "Subscribe" button

**Expected**: Container is added to user's subscribed containers  
**Actual**: Container added to user's subscribed containers  
**Status**: ✅ PASS

#### TC046
**ID**: TC046  
**Description**: Verify viewing subscribed cargo containers  
**Precondition**: User is logged in and subscribed to containers  
**Data**: N/A  
**Steps**:
1. Click "Followed" under "Cargos" section

**Expected**: User's subscribed containers are displayed  
**Actual**: User's subscribed containers displayed  
**Status**: ✅ PASS

#### TC047
**ID**: TC047  
**Description**: Verify viewing RSS feeds within a cargo container  
**Precondition**: User is logged in, has subscribed to container with feeds  
**Data**: N/A  
**Steps**:
1. Select a specific cargo container

**Expected**: List of RSS feeds within container is displayed  
**Actual**: List of RSS feeds within container displayed  
**Status**: ✅ PASS

#### TC048
**ID**: TC048  
**Description**: Verify filtering articles by cargo container  
**Precondition**: User is logged in and subscribed to container with feeds  
**Data**: N/A  
**Steps**:
1. Select a specific cargo container

**Expected**: Only articles from feeds in that container displayed  
**Actual**: Only articles from feeds in selected container displayed  
**Status**: ✅ PASS

#### TC049
**ID**: TC049  
**Description**: Verify unsubscribing from a cargo container  
**Precondition**: User is logged in and subscribed to a container  
**Data**: N/A  
**Steps**:
1. Navigate to subscribed containers
2. Find container to unsubscribe from
3. Click "Unsubscribe" button

**Expected**: Container is removed from subscribed containers  
**Actual**: Container removed from subscribed containers  
**Status**: ✅ PASS

#### TC050
**ID**: TC050  
**Description**: Verify deleting a cargo container (admin function)  
**Precondition**: User is logged in as admin, container exists  
**Data**: N/A  
**Steps**:
1. Navigate to admin container management
2. Find container to delete
3. Click "Delete" button
4. Confirm deletion

**Expected**: Cargo container is deleted from system  
**Actual**: Cargo container deleted from system  
**Status**: ✅ PASS

#### TC051
**ID**: TC051  
**Description**: Verify editing cargo container name  
**Precondition**: User is logged in as admin, container exists  
**Data**: New name = "Updated Container Name"  
**Steps**:
1. Navigate to container management
2. Select container to edit
3. Change name
4. Save changes

**Expected**: Container name is updated  
**Actual**: Container name updated  
**Status**: ✅ PASS

#### TC052
**ID**: TC052  
**Description**: Verify cargo container description  
**Precondition**: User is logged in as admin, container exists  
**Data**: Description = "Container description text"  
**Steps**:
1. Navigate to container management
2. Add/edit description
3. Save changes
4. View container details

**Expected**: Container description is displayed  
**Actual**: Container description displayed  
**Status**: ✅ PASS

#### TC053
**ID**: TC053  
**Description**: Verify cargo container creation with duplicate name  
**Precondition**: User is logged in, container "Tech News" exists  
**Data**: Container name = "Tech News"  
**Steps**:
1. Attempt to create container with existing name
2. Submit form

**Expected**: Error message about duplicate name is displayed  
**Actual**: Error message about duplicate name displayed  
**Status**: ✅ PASS

#### TC054
**ID**: TC054  
**Description**: Verify cargo container feed count display  
**Precondition**: User is logged in, container has multiple feeds  
**Data**: N/A  
**Steps**:
1. Navigate to containers list
2. Check feed count display

**Expected**: Correct number of feeds is displayed  
**Actual**: Feed count was not updated after removing a feed 
**Status**: ❌ FAIL

#### TC055
**ID**: TC055  
**Description**: Verify cargo container subscriber count (admin view)  
**Precondition**: User is logged in as admin, container has subscribers  
**Data**: N/A  
**Steps**:
1. Navigate to admin container management
2. Check subscriber count

**Expected**: Correct number of subscribers is displayed  
**Actual**: Correct number of subscribers displayed  
**Status**: ✅ PASS

#### TC056
**ID**: TC056  
**Description**: Verify adding same feed to multiple containers  
**Precondition**: User is logged in, multiple containers exist  
**Data**: RSS feed URL = "https://news.google.com/rss"  
**Steps**:
1. Add feed to first container
2. Add same feed to second container

**Expected**: Feed appears in both containers  
**Actual**: Feed appeared in both containers  
**Status**: ✅ PASS

#### TC057
**ID**: TC057  
**Description**: Verify cargo container creation with no name  
**Precondition**: User is logged in  
**Data**: Container name = "" (empty)  
**Steps**:
1. Attempt to create container with empty name
2. Submit form

**Expected**: Validation error message is displayed  
**Actual**: Validation error message displayed  
**Status**: ✅ PASS

#### TC058
**ID**: TC058  
**Description**: Verify cargo container sorting by name  
**Precondition**: User is logged in and has multiple containers  
**Data**: N/A  
**Steps**:
1. Navigate to containers list
2. Click sort by name option

**Expected**: Containers are sorted alphabetically  
**Actual**: Sorting did not update after container rename
**Status**: ❌ FAIL

#### TC059
**ID**: TC059  
**Description**: Verify cargo container sorting by feed count  
**Precondition**: User is logged in and has multiple containers  
**Data**: N/A  
**Steps**:
1. Navigate to containers list
2. Click sort by feed count option

**Expected**: Containers are sorted by number of feeds  
**Actual**: Containers sorted by number of feeds  
**Status**: ✅ PASS

#### TC060
**ID**: TC060  
**Description**: Verify cargo container with empty feeds list  
**Precondition**: User is logged in, container has no feeds  
**Data**: N/A  
**Steps**:
1. Select empty container

**Expected**: "No feeds in this container" message displayed  
**Actual**: "No feeds in this container" message displayed  
**Status**: ✅ PASS

### Article View & Interaction

#### TC061
**ID**: TC061  
**Description**: Verify viewing article details from RSS feed  
**Precondition**: User is logged in and has articles available  
**Data**: N/A  
**Steps**:
1. Navigate to feed articles
2. Click on article headline

**Expected**: User is redirected to original article source  
**Actual**: User redirected to original article source  
**Status**: ✅ PASS

#### TC062
**ID**: TC062  
**Description**: Verify article sorting by publication date  
**Precondition**: User is logged in with multiple articles  
**Data**: N/A  
**Steps**:
1. View feed articles
2. Check article order

**Expected**: Articles are sorted by date (newest first)  
**Actual**: Articles sorted by date with newest first  
**Status**: ✅ PASS

#### TC063
**ID**: TC063  
**Description**: Verify article preview content  
**Precondition**: User is logged in and viewing articles  
**Data**: N/A  
**Steps**:
1. View article list
2. Check article previews

**Expected**: Article descriptions/previews are displayed correctly  
**Actual**: Article descriptions/previews displayed correctly  
**Status**: ✅ PASS

#### TC064
**ID**: TC064  
**Description**: Verify article image thumbnails  
**Precondition**: User is logged in, viewing articles with images  
**Data**: N/A  
**Steps**:
1. View articles with thumbnail images

**Expected**: Article thumbnail images are displayed correctly  
**Actual**: Article thumbnail images displayed correctly  
**Status**: ✅ PASS

#### TC065
**ID**: TC065  
**Description**: Verify reading article in application frame  
**Precondition**: User is logged in and viewing articles  
**Data**: N/A  
**Steps**:
1. View article list
2. Click "Read in app" button for an article

**Expected**: Article opens in application frame without leaving app  
**Actual**: Article opened in application frame  
**Status**: ✅ PASS

#### TC066
**ID**: TC066  
**Description**: Verify article sharing functionality  
**Precondition**: User is logged in and viewing articles  
**Data**: N/A  
**Steps**:
1. Find an article
2. Click share button
3. Select sharing method

**Expected**: Article link is shared via selected method  
**Actual**: Article link shared via selected method  
**Status**: ✅ PASS

#### TC067
**ID**: TC067  
**Description**: Verify article favoriting functionality  
**Precondition**: User is logged in and viewing articles  
**Data**: N/A  
**Steps**:
1. Find an article
2. Click favorite/star button

**Expected**: Article is added to favorites  
**Actual**: Article added to favorites  
**Status**: ✅ PASS

#### TC068
**ID**: TC068  
**Description**: Verify viewing only favorite articles  
**Precondition**: User is logged in and has favorited articles  
**Data**: N/A  
**Steps**:
1. Click "Favorites" filter

**Expected**: Only favorited articles are displayed  
**Actual**: Only favorited articles displayed  
**Status**: ✅ PASS

#### TC069
**ID**: TC069  
**Description**: Verify article date format  
**Precondition**: User is logged in and viewing articles  
**Data**: N/A  
**Steps**:
1. View article list
2. Check date format

**Expected**: Article dates are displayed in correct format  
**Actual**: Some article dates displayed in inconsistent formats (e.g., different locales)  
**Status**: ❌ FAIL

#### TC070
**ID**: TC070  
**Description**: Verify handling of articles with no publication date  
**Precondition**: User is logged in, feed contains article without date  
**Data**: N/A  
**Steps**:
1. View article without publication date

**Expected**: Article displays with default or placeholder date  
**Actual**: Article displayed with placeholder date  
**Status**: ✅ PASS

### Search Functionality

#### TC071
**ID**: TC071  
**Description**: Verify searching for RSS feeds by name  
**Precondition**: User is logged in  
**Data**: Search term = "News"  
**Steps**:
1. Navigate to search area
2. Enter "News" as search term
3. Click search button

**Expected**: RSS feeds with "News" in name are displayed  
**Actual**: RSS feeds with "News" in name displayed  
**Status**: ✅ PASS

#### TC072
**ID**: TC072  
**Description**: Verify searching for containers by name  
**Precondition**: User is logged in  
**Data**: Search term = "Technology"  
**Steps**:
1. Navigate to search area
2. Enter "Technology" as search term
3. Click search button

**Expected**: Containers with "Technology" in name are displayed  
**Actual**: Containers with "Technology" in name displayed  
**Status**: ✅ PASS

#### TC073
**ID**: TC073  
**Description**: Verify search with no matching results  
**Precondition**: User is logged in  
**Data**: Search term = "XYZ123NonExistentTerm"  
**Steps**:
1. Navigate to search area
2. Enter non-existent term
3. Click search button

**Expected**: "No results found" message is displayed  
**Actual**: "No results found" message displayed  
**Status**: ✅ PASS

#### TC074
**ID**: TC074  
**Description**: Verify searching for article content  
**Precondition**: User is logged in with articles available  
**Data**: Search term = specific word known to be in an article  
**Steps**:
1. Navigate to article search
2. Enter search term
3. Submit search

**Expected**: Articles containing search term are displayed  
**Actual**: Articles containing search term displayed  
**Status**: ✅ PASS

#### TC075
**ID**: TC075  
**Description**: Verify search filter options  
**Precondition**: User is logged in  
**Data**: Search term = "News", Filter = "Feeds only"  
**Steps**:
1. Enter search term
2. Select "Feeds only" filter
3. Submit search

**Expected**: Only feed results are displayed (no containers)  
**Actual**: Only feed results displayed  
**Status**: ✅ PASS

### User Interface

#### TC076
**ID**: TC076  
**Description**: Verify responsive design on desktop  
**Precondition**: Application is running on desktop browser  
**Data**: N/A  
**Steps**:
1. Open application in desktop browser
2. Observe layout

**Expected**: UI elements are properly displayed for desktop  
**Actual**: UI elements properly displayed for desktop  
**Status**: ✅ PASS

#### TC077
**ID**: TC077  
**Description**: Verify responsive design on tablet  
**Precondition**: Application is running on tablet device/emulator  
**Data**: N/A  
**Steps**:
1. Open application on tablet
2. Observe layout

**Expected**: UI adapts to tablet screen size  
**Actual**: UI adapted to tablet screen size  
**Status**: ✅ PASS

#### TC078
**ID**: TC078  
**Description**: Verify responsive design on mobile  
**Precondition**: Application is running on mobile device/emulator  
**Data**: N/A  
**Steps**:
1. Open application on mobile device
2. Observe layout

**Expected**: UI adapts to mobile screen size  
**Actual**: UI adapted to mobile screen size  
**Status**: ✅ PASS

#### TC079
**ID**: TC079  
**Description**: Verify navigation through side panel  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Click various options in side panel
2. Observe navigation changes

**Expected**: Application navigates to corresponding pages  
**Actual**: Application navigated to corresponding pages  
**Status**: ✅ PASS

#### TC080
**ID**: TC080  
**Description**: Verify user profile access  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Click on user profile in top panel

**Expected**: User profile options are displayed  
**Actual**: User profile options displayed  
**Status**: ✅ PASS

#### TC081
**ID**: TC081  
**Description**: Verify theme switching (light/dark)  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Access theme settings
2. Switch between light and dark themes

**Expected**: Application theme changes accordingly  
**Actual**: Application theme changed accordingly  
**Status**: ✅ PASS

#### TC082
**ID**: TC082  
**Description**: Verify keyboard navigation  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Use tab key to navigate interface
2. Use enter to select options

**Expected**: Keyboard navigation works properly  
**Actual**: Keyboard navigation worked properly  
**Status**: ✅ PASS

#### TC083
**ID**: TC083  
**Description**: Verify notification display  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Perform action that triggers notification
2. Observe notification display

**Expected**: Notification is displayed and automatically dismisses  
**Actual**: Notification displayed and automatically dismissed  
**Status**: ✅ PASS

#### TC084
**ID**: TC084  
**Description**: Verify collapsible side panel  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Click collapse/expand button on side panel

**Expected**: Side panel collapses/expands accordingly  
**Actual**: Side panel did not collapse on mobile view
**Status**: ❌ FAIL

#### TC085
**ID**: TC085  
**Description**: Verify feed loading indicators  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Navigate to feeds that require loading
2. Observe loading indicators

**Expected**: Loading indicators display while content loads  
**Actual**: Loading indicators displayed while content loaded  
**Status**: ✅ PASS

### Data Validation & Error Handling

#### TC086
**ID**: TC086  
**Description**: Verify handling of special characters in RSS content  
**Precondition**: User is logged in, subscribed to feed with special characters  
**Data**: N/A  
**Steps**:
1. View feed with special characters

**Expected**: Special characters are displayed correctly  
**Actual**: Special characters displayed correctly  
**Status**: ✅ PASS

#### TC087
**ID**: TC087  
**Description**: Verify handling of multilingual content  
**Precondition**: User is logged in, subscribed to feed with multilingual content  
**Data**: N/A  
**Steps**:
1. View feed with multilingual content

**Expected**: Multilingual characters display correctly  
**Actual**: Multilingual characters displayed correctly  
**Status**: ✅ PASS

#### TC088
**ID**: TC088  
**Description**: Verify character limit display for titles  
**Precondition**: User is logged in, has feed with very long titles  
**Data**: N/A  
**Steps**:
1. View feed with long titles

**Expected**: Long titles are properly truncated with ellipsis  
**Actual**: Long titles were cut off mid-word without ellipsis 
**Status**: ❌ FAIL

#### TC089
**ID**: TC089  
**Description**: Verify user-friendly error messages  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Intentionally cause an error
2. Observe error message

**Expected**: User-friendly error message is displayed  
**Actual**: User-friendly error message displayed  
**Status**: ✅ PASS

#### TC090
**ID**: TC090  
**Description**: Verify form validation error display  
**Precondition**: User is on registration page  
**Data**: Name = "", Email = "", Password = ""  
**Steps**:
1. Submit empty form

**Expected**: Validation errors are displayed for each field  
**Actual**: Validation errors displayed for each field  
**Status**: ✅ PASS

#### TC091
**ID**: TC091  
**Description**: Verify handling of unavailable RSS source  
**Precondition**: User subscribed to feed that becomes unavailable  
**Data**: N/A  
**Steps**:
1. View feed that is unavailable

**Expected**: Proper error message about unavailable source  
**Actual**: Proper error message about unavailable source displayed  
**Status**: ✅ PASS

#### TC092
**ID**: TC092  
**Description**: Verify handling of malformed RSS content  
**Precondition**: User subscribed to feed with malformed content  
**Data**: N/A  
**Steps**:
1. View feed with malformed content

**Expected**: Application handles malformed content gracefully  
**Actual**: Application handled malformed content gracefully  
**Status**: ✅ PASS

#### TC093
**ID**: TC093  
**Description**: Verify handling of very large RSS feed  
**Precondition**: User subscribed to feed with many items  
**Data**: N/A  
**Steps**:
1. View feed with many items (100+)

**Expected**: Feed loads with pagination or lazy loading  
**Actual**: Feed loaded with pagination  
**Status**: ✅ PASS

#### TC094
**ID**: TC094  
**Description**: Verify sanitization of HTML in RSS content  
**Precondition**: User subscribed to feed with HTML content  
**Data**: N/A  
**Steps**:
1. View feed with HTML in content

**Expected**: HTML is either rendered safely or sanitized  
**Actual**: HTML rendered safely with proper sanitization  
**Status**: ✅ PASS

#### TC095
**ID**: TC095  
**Description**: Verify error handling with network interruption  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Disable network connection
2. Attempt to refresh feeds

**Expected**: Proper offline/error message is displayed  
**Actual**: Proper offline message displayed  
**Status**: ✅ PASS

### Security

#### TC096
**ID**: TC096  
**Description**: Verify protection against XSS attacks  
**Precondition**: User is logged in  
**Data**: Input with script tag = "<script>alert('XSS')</script>"  
**Steps**:
1. Enter input with script tag in search field
2. Submit form

**Expected**: Script is not executed, input is sanitized  
**Actual**: Script not executed, input sanitized  
**Status**: ✅ PASS

#### TC097
**ID**: TC097  
**Description**: Verify protection against SQL injection  
**Precondition**: Application is running  
**Data**: Login = "admin' OR '1'='1", Password = "any"  
**Steps**:
1. Enter SQL injection string in login form
2. Submit form

**Expected**: Login fails, no unauthorized access  
**Actual**: Login failed, no unauthorized access  
**Status**: ✅ PASS

#### TC098
**ID**: TC098  
**Description**: Verify user data isolation  
**Precondition**: Two users with different subscriptions exist  
**Data**: N/A  
**Steps**:
1. Login as first user and note feeds
2. Logout
3. Login as second user

**Expected**: Each user only sees their own subscriptions  
**Actual**: Each user only saw their own subscriptions  
**Status**: ✅ PASS

#### TC099
**ID**: TC099  
**Description**: Verify unauthorized access prevention  
**Precondition**: User is not logged in  
**Data**: N/A  
**Steps**:
1. Try to directly access protected URL

**Expected**: User is redirected to login page  
**Actual**: User redirected to login page  
**Status**: ✅ PASS

#### TC100
**ID**: TC100  
**Description**: Verify session timeout handling  
**Precondition**: User is logged in  
**Data**: N/A  
**Steps**:
1. Leave session idle until timeout
2. Try to perform an action

**Expected**: User is redirected to login page after timeout  
**Actual**: User redirected to login page after timeout  
**Status**: ✅ PASS