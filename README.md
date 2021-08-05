# Codecool shop (sprint 2)

## Story

It's time to enhance the Online Shop, an online eCommerce web-application with Java.
Where users can not only browse products, add them into a Shopping Cart,
checkout items and make payments. But also can log in and see the abandoned shopping cart or order history.

> Did you know that the very first product on eBay was a broken laser pointer?
> If you don't believe, check their history: [eBay history](https://www.ebayinc.com/company/our-history/)

## What are you going to learn?

- how to log properly and why does it important
- how to use setting files to store project settings
- writing tests and mocking out dependencies to ensure the correct functionality and gain confidence at future  modification
- using database to make the data persistent by using an ORM
- how to use the `DAO` design pattern in `Csharp`,
- refreshing SQL knowledge


## Tasks

1. Create a new sprint tab on the existing backlog. Last week you had a long list of stories, a few new stories this week.
    - The new items are added to the backlog
    - The team has created a sprint 2 plan based upon the unified backlog

2. As you will work in a new repository but you need the code from the previous sprint, add the `codecool-shop-2` repository as a new remote to the previous sprint's repository, then pull (merge) and push your changes into it.
    - There is a merge commit in the project's repository that contains code from the previous sprint

3. As a Developer, I want to cover the `ProductService` and any other Service objects with unit tests. So that I can safely change the implementation later.
    - All methods of the Services should be tested, independently from their DAO dependencies, using mocking.
    - Both "happy-paths" and "sad-paths" are tested.
    - At least 11 unit tests pass. 

4. As a Developer, I want to have proper log messages when any failure happens to make it possible to find any malfunction in the system and track back user complaints.
    - Serilog can be utilized for providing diagnostic logging.          
    - All exceptions are logged with proper description message.

5. As an Operator of the Shop, I want the product data to be persistent. So that I can restart the application without loosing product data.
    - There is an empty MSSQL database called `codecoolshop`
    - Entity Framework Core code-first approach is used to provide the database schema. The database is then seeded with products. 
    - When the page is loaded and DB is used, suppliers, product categories and products are loaded from the database via `ProductDao`, `ProductCategoryDao`and `SupplierDao`.

6. As a Developer, I want to read the DAO settings and DB connection parameters (url, database name, usr, pwd) from a config file so I am able to change the settings of the application on every environment without compiling again
    - In my appsettings.json file, I want to add my local database parameters along with the following structure (exact values may vary) ```
 "Mode": "sql",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=codecoolshop;Trusted_Connection=True;MultipleActiveResultSets=true"}```
    - Application reads the database type and either operates on Entity Framework in-memory or SQL variations.
    - Application reads the DB settings and database type from the appsettings file.

7. As an Operator of the Shop, I want to keep Order data safe and persistent, so that I won't loose money because of technical issues.
    - Given the User started a checkout process. Then ensure it saves all Order data into database (in each and every step, except cart).

8. As a User, I want to sign up (make a personal account) so that I can store Orders to my personal account.
    - there is a `Sign up` option on the website
    - it has a form with all the required fields:
- `name`
- `email`
- `password`
    - when the user submits the form with correct/valid information then ensure the system saves it's data as a new `User`
    - the system sends a welcome email after successful registration
    - When the User submits the form with incorrect/invalid information then ensure the program shows the same form with the incorrect data, and some description about the errors.

9. As a User, I want to able to login, so I can authenticate myself and access my personal data. I want to be able to logout so I can close my session.
    - There is a `Login` menu on the website
    - When the user chooses the "Login" menu
then ensure to provide a login form with the following fields:
- email address
- password
    - When the user submits the form with valid information then authenticate and give a new logged-in session to the User
    - When the user submits the form with invalid information then provide an error message
    - Ensure to provide a Logout option for loggend in users. When the user chooses the "Logout" option then reset the session and redirect back to the login form.

10. As a loggedin User, I want to see my Order history, so that I can see my previous Orders and follow their status.
    - There is an `Order history` menu item
    - provide a list with all the Orders of that user, with the following details:
- order date
- order status (checked / paid / confirmed / shipped)
- total price
- product list (with product name, price)

11. As a logged-in User, I want to save the current items of my Shopping cart so that I can order my selected Products later.
    - there is a `Save my cart` button (on the Shopping cart review page)
    - by clicking on this button the system saves the cart items into the database - for that loggedin User
    - Given that there is a User with a previously saved shopping cart. When the user finished the login process then ensure to refill the user's shopping cart with the saved items.

12. As a loggedin User, I want to save my billing and shipping info (to my personal account) so that I don't need to type these data all the time - during checkout.
    - there is a `Billing info` menu item
    - after clicking on the menu a provide a form where the user can fill in
the personal billing and shipping info (what is needed for the checkout)
    - Given there's a Shopping Cart review page. When I click on the "Checkout" button then ensure the system shows the pre-filled billing and shipping info on the checkout form.

## General requirements

- Advanced OOP concepts are used in the project: inheritance, there is at
least on abstract class, there is at least one interface implemented
- The page doesn't show a server error anytime during the review
- All code is pushed to GitHub repository by atomic commits. The implemented feature related commits managed on separated feature branches and merged by a pull request to the `master` branch.

## Hints

- It's not required to integrate real payment services - you can use fake payment implementations.
- Use the DAO implementations via interfaces so that it will be easy to change the implementation behind the interface.

## Background materials

- <i class="far fa-exclamation"></i> [Serilog in ASP.NET Core 3.1](https://codewithmukesh.com/blog/serilog-in-aspnet-core-3-1/)
- <i class="far fa-exclamation"></i> [ASP.NET Core session basics](https://tutexchange.com/using-session-in-asp-net-core-3-0/)
- <i class="far fa-exclamation"></i> [Querying in Entity Framework](https://www.entityframeworktutorial.net/querying-entity-graph-in-entity-framework.aspx)
- <i class="far fa-exclamation"></i> [Learning Entity Framework Core](https://www.entityframeworktutorial.net/efcore/entity-framework-core.aspx)
- <i class="far fa-exclamation"></i> [ASP.NET Core Identity](https://www.tutorialspoint.com/asp.net_core/asp.net_core_identity_configuration.htm)

