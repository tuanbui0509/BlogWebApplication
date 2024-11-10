workspace "Blog Web Application" "This is a description of the blog web application system."{

    model {
        user = person "User" "A visitor who reads and interacts with the blog."
        admin = person "Admin" "An admin user who manages the blog."
        system = softwareSystem "Blog Web System" {
            adminPanel = container "Admin Panel" "Allows admins to manage the blog" {
                tags = "Admin Panel"
                technology "HTML, CSS, JavaScript, Frontend Framework ReactJS"
            }
            webServer = container "Web Server" "Serves web pages to users" {
                tags = "Web Server"
                technology "HTML, CSS, JavaScript, Frontend Framework NextJS"
            }
            applicationServer = container "Application Server" "Hosts the blog application logic." {
                tags = "Application Server"
                technology "Backend Framework (e.g., Django, ASP.NET, Spring Boot)"
                authenticationController = component "Authentication Controller" "Handles user authentication"
                postController = component "Post Controller" "Allow users handles blog post-related actions"
                userController = component "User Controller" "Handles user-related actions"
                commentComponent = component "Comment Component" "Handles comment-related actions"
                paymentController = component "Make payment Controller" "Make payment for premium blog and donate."
                emailComponent = component "E-mail Component" "Sends e-mails to users."
                databaseComponent = component "Database Component" "Manages connections to the database"
            }
            database = container "Database" "Stores blog posts, user information, comments, etc" {
                tags = "Database"
                technology "Relational or NoSQL database"
            }
        }

        email = softwaresystem "E-mail System" "Verify e-mail, 2FA, OTP, Password, Notification." "Existing System"
        payment = softwaresystem "Payment System" "Make payment for premium blog or donate." "Existing System"
        
        # relationships to/from models
        system -> email "Send e-mail using"
        email -> user "Send e-mail to"
        system -> payment "Donate, make payment"

        # relationships to/from actors
        admin -> adminPanel "Manages articles, users, ..."
        user -> applicationServer "Visits blog domail" "HTTPS"
        user -> webServer "View, read, write, like, comment blog, view account, make payment premium blog, ect" "HTTPS"
       
        # relationships to/from containers
        adminPanel -> webServer "Manages blog content"
        webServer -> applicationServer "For dynamic content rendering" "HTTP" 
        applicationServer -> database "Stores and retrieves data" "JDBC, ORM"
        applicationServer -> email "Send e-mail using"
        applicationServer -> payment "Makes API call to" "HTTPS"
        
        # relationships to/from components
        webServer -> postController "Makes API calls to" "JSON/HTTPS"
        webServer -> userController "Makes API calls to" "JSON/HTTPS"
        webServer -> authenticationController "Makes API calls to" "JSON/HTTPS"
        webServer -> paymentController "Makes API calls to" "JSON/HTTPS"
        authenticationController -> databaseComponent "Uses"
        userController -> databaseComponent "Uses"
        userController -> emailComponent "Uses"
        userController -> commentComponent "Uses"
        postController -> commentComponent "Uses"
        databaseComponent -> database "Reads from and writes to" "SQL/TCP"
        paymentController -> payment "Makes API calls to" "XML/HTTPS"
        emailComponent -> email "Sends e-mail using"
    }   

    views {
        systemContext system {
            include *
            autolayout lr
        }

        container system {
            include *
            autolayout lr
        }

        component applicationServer {
            include *
            autoLayout lr
        }

        theme default
        styles {
            element "Person" {
                color #ffffff
                fontSize 22
                shape Person
            }
            element "Customer" {
                background #08427b
            }
            element "Bank Staff" {
                background #999999
            }
            element "Software System" {
                background #1168bd
                color #ffffff
            }
            element "Existing System" {
                background #999999
                color #ffffff
            }
            element "Container" {
                background #438dd5
                color #ffffff
            }
            element "Web Browser" {
                shape WebBrowser
            }
            element "Database" {
                shape Cylinder
            }
            element "Component" {
                background #85bbf0
                color #000000
            }
            element "Failover" {
                opacity 25
            }
        }
    }
}