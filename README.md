1. Author: Igor Dubovets
2. Project: WebCrawler, 
- Input: URL of website passed as command line argument
- Output: Site.xml file is created that contains site map with page URL and all links found on the page.
3. Project built in MS Visual Studio 2017 Community Edition 
4. MSTestv2 used for automated unit tests
5. Assumption 1: Naive approach, no compliance with web bot guidlines and standarts (like robots.txt etc)
7. Assumption 2: Not implemented user login and crawling of websites that require authentication
8. Assumption 3: Web site that we crawl is small enough to generate XML site map of reasonable size
9. Assumption 4: For simplicity not used multithreading (several crawlers, queue of links to crawl) 