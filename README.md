# PubsOfLondon

## The Plan
- Take this [page from wikipedia](https://en.wikipedia.org/wiki/List_of_pubs_in_London)
- Turn it into a website

# Rough idea
- Map locations
- Best choice of multiple locations
- Trip Advisor? 

# Initial Roadmap
1. Pull Wiki info as JSON 
1. JSON into NoSQL Database
1. Azure search index - indexer updates on NoSQL updates
1. Azure API that returns the information stored
1. Site that displays the information from the API (Headless)

# Enhancements
- GraphQL
- Angular / React front end
- Domain name

# Wikipedia Notes
Api Information: https://www.mediawiki.org/wiki/API:Main_page#Endpoint
Pub page template: https://en.wikipedia.org/wiki/Template:Pubs_in_London

Generated the xml in Sample data using the url: https://en.wikipedia.org/wiki/Special:Export and the title List_of_pubs_in_London
