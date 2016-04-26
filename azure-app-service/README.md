# Azure Mobile Services

### The Challenge

In this challenge, you will use Azure App Service and Xamarin.iOS or Xamarin.Android to build a scalable, native mobile app in C#.  This will involve logging into the Azure Portal and creating a new Azure Mobile App with SQL database.  You can then use one of the Xamarin starter projects to build your mobile client.

The walkthrough below should help you with the challenge, but you can also get in touch with Adrian Hall (Twitter: @fizzyinthehall or @AzureMobile) with questions - Adrian is at the conference and happy to help.

### Challenge Walkthrough

* Log in to your Microsoft Azure account at https://portal.azure.com.  (If you don't have an Azure account, you can obtain a [free 30-day trial](https://azure.microsoft.com/en-us/free/) at https://azure.microsoft.com/en-us/free/. If you are an MSDN subscriber, you should activate your Azure benefit and get free credits each month.)

* Download an appropriate IDE to your laptop.  You can use [Visual Studio 2015 Community](http://visualstudio.com) or [Xamarin Studio](https://www.xamarin.com/download) for this mini-hack.

* Click **+ New** -> **Web + Mobile** -> **Mobile App**.  Enter a name for your app (use your email address in the name to make it unique - for example: adrian-at-microsoft-azure-minihack; ensure the first letter is a letter).  Enter "xamarin-evolve2016" in the resource group.  Check the "Pin to dashboard" checkbox (so you can easily find the resources later).   Click on **Create**.

* Once your site has been created, it will open automatically.  Click on **Quick start**, then choose a QuickStart project - Xamarin.iOS or Xamarin.Android.

* Click on the grey box in section 1, then click on **Add** to add a Data Connection.  Click on **SQL Database _Configure Required Settings_**, then **Create a new database**.  Enter _evolve2016_ in the Name box.

* Click on **Server _Configure Required Settings_**, enter a unique name in the Server name (Tip: base the server name on your name to help it be unique, or use guidgenerator.com to create a GUID).  Enter a server username and password, then click on **OK**.  Click on **OK** on the New database blade as well.  Finally, click on **OK** on the Add data connection blade.

* Creating a database takes some time.  Once complete, the Data Connections blade will show the data connection you just created.  At this point, you can close the Data Connections blade.  (Tip: Click on the Bell Icon at the top of the page to view the progress of your deployments)

* Back on the Quickstart blade, move to Step 2 - select Node.js as the Backend language, check the box acknowledging that deploying overwrites the site contents, then click on **Create TodoItem table**

* While the site is being deployed, move to Step 3 - Create a New App and download the personalized project.  

You should be able to unpack the downloaded ZIP file and open the project in Xamarin Studio or Visual Studio 2015.  You can then compile the project.

* Run either the iOS or Android application and add some todo items

* If you head back to the [Azure Portal](https://portal.azure.com), click on your App Service, then select **Easy tables**, followed by **TodoItem**.  Note that you can see the raw data for your todo items in the SQL table.

Congratulations, you've completed the Azure App Service mini-hack!

## BONUS Challenge!  Convert your site to use ASP.NET

In the mini-hack above, you deployed a node.js site to get started with Azure without writing any code.  Azure Mobile Apps also provides an ASP.NET codebase and you can deploy this instead using a Windows PC and Visual Studio 2015.

* Log in to your Microsoft Azure account at https://portal.azure.com.

* Click on **All Resources** then select the app service site that you created earlier.

* Click on **All settings**, then **Quick start**, then choose a Xamarin Quick start project.  (It doesn't matter which one as you will not be downloading a client project this time).

* Under Step 2, select **C#** as the Backend language.  Click on **Download** to download a server project.

* Unzip the downloaded package and open the solution in Visual Studio 2015.  (Sorry - this step can only be done on Windows in Visual Studio 2015)

* If you have not already done so, install the Azure SDK using **Tools** -> **Extensions and Updates**

* Build the solution - this will restore the packages.

* Right-click on the project and select **Publish...**

* In the dialog, select **Microsoft Azure App Service** as the publish target.  (Don't have that as an option?  Ensure you are running Azure SDK v2.8.2 or later)

* Select your Azure Subscription.  (If you need to, enter your credentials)

* Select your App Service from the tree, then click on OK

* The information should be filled in for you and all you have to do is click on **Publish** to publish your site.

* Once published, you can now reload your iOS or Android mobile client - it will be using the ASP.NET backend.

Using the ASP.NET backend gives you access to all the power of ASP.NET, OWIN, Entity Framework and more.  However, you lose the ability to view the data on the server.  Use Visual Studio or SQL Server Management Studio for this purpose instead.
