# AmazonWebScraper

Intended as a short project to upskill in the following areas whilst on bench:
- TDD, in addition to intergration & acceptance tests
- Docker
- Terraform
- CI/CD

You will be able to submit an amazon URL and target price and get notifed via email when the product has reached the price.

Note the project will be utilizing Made Tech's sandbox AWS environment, so the longterm api won't be valid as a real-life usecase.

With that in mind, here's three tickets to split out the work into a real-life ask:

## aws-1: API endpoint for creating price watches

User story: 
As a user, I want to submit a product URL, target price and email so that I can be notified of price drops.

AC:
- Given a valid Amazon URL, a positive decimal target price & email when I send a `POST` request to `/api/watches`, the system returned a `201 Created` status
- Given a successful submission, then a record is created in the MySQL table with a unique `WatchId`, `Url`,  `TargetPrice`, `Email` and `Status` of `Active`
- Given an invalid URL format, a negative price or invalid email address format, when I submit the request, then the system returns a `400 Bad request`
- Given the system is offline, then the API should return a `503 Service Unavailable`

## aws-2: Amazon price extraction logic

User store: 
As the system, I need to parse the raw HTML of an Amazon page to find the current price

AC:
- Given a raw HTML string containing a standard Amazon page, when the parser is called, then it returns the price as a decimal
- Given a HTML string where the price contains a currency symbol, then the parser strips the symbol and return only the numeric value
- Given  a HTML string where the product is out of stock, or the price tag is missing, then the parser throws a specific `PriceNotFoundException`

## aws-3: Background scraper orchestration

User story: 
As the system, I want to periodically check all active watches so that the price data is kept up to date

AC:
- When the background worker triggers, it fetches the HTML for every watch with "Active" `Status` and `LastCheckedAt` is either older than 24 hours or null
- Given a successfully parsed price, then the system updates the corresponding MySQL record with `LastCheckedAt` and `CurrentPrice`
