Tech:

.NetCore 2.2 Console Application

Architecture:

Three main layers - Core, Presentation and Persistence

Core - It contains DOMAIN which doesn't depend on anything, and APPLICATION which depends on Domain. Together, they represent the core business which remains unaffected by changes to Presentation and Persistence. This is achieved by creating interfaces in Application layer and forcing Presentation and Persistence depend on CORE.
Presentation - This couble be any application eg. Console Application in this case. Changing Presentation wouldn't impact Core.
Persistence - This contains datasource specific classes and logic. Again, changing this wouldn't impact Core.

Running it Locally:

Set Users.Presentation as start up project.
Set File Path in appsettings.json or edit example_data.json under Users.Persistence.
