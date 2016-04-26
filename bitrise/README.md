![Cover](/Evolve/minihack_cover.jpg)

Welcome, space cadet to the Bitrise Mini Hack. This simple hack will introduce you to the Bitrise platform which allows you to automate your mobile app development tasks from building through testing to deployment.

By completing this hack you will have a cross-platform mobile app connected to Bitrise for automated testing and deployment, all in a few minutes. 🚀

Sounds awesome, right? We think so too!

We think you'll be able to complete this mini hack in 20 minutes. If you get stuck or have any questions, no problem. Head over to the Bitrise booth and we'll be happy to help you out. If you don't feel like taking the walk, you can also head over to our [DevCenter](http://devcenter.bitrise.io) for handy tutorials.

Okay, enough talk, let’s automate! 🤖

##So the challenge is to...

Add a new Xamarin app to Bitrise and send it to Xamarin Test Cloud for testing. Easy as π.

##Getting started

1. Create a new account on Bitrise. It's best to use the GitHub option, because that way all your repos will be listed when adding a new project later. You'll get a 14 day trial for our Pro plan, but we have a forever free plan, too.
2. You'll be presented with a quickstart guide. Just scroll down to the bottom and click on *Try Sample App*.
3. You'll be redirected to the Add new App page, where you can select which Sample App you want to try out. Of course, you should click on the Xamarin one this time. 😉
4. Sample Apps are great, because you don't have to set anything when adding the project, it'll do everything automatically for you: adding an SSH key, scanning the master branch for project types, etc.
7. You can skip the webhook registration, since you probably won't push code to this repository. But it's up to you, we won't judge. ⚖
8. And you're done! Feel free to start a new build so you'd see that everything is working fine and your basic configuration will produce a green build.

##Setting up XTC 🌩

5. Your build should take a few minutes. After it's done, go to your new app's *Workflow* tab and click on the *Manage workflows* button on the right. Bitrise prepares a basic workflow for every project type when you add it. You can customise this basic workflow to fit your needs. You can select from numerous integrations for testing, deployment or notification to add to your workflow. We have more than 70 and they are all open source. Let's stick to testing for now, everything else is already added.
6. Add the Xamarin Test Cloud step at the end of your workflow, but *before* the Bitrise deploy step.
7. Now you should head over to [XTC's website](https://xamarin.com/test-cloud/) and login. Create a new test run for an iOS app and select the devices to test on. At the last step you'll get the `Device selection id` which you'll need for the XTC step on Bitrise.
7. There are 3 things you have to specify in the XTC step settings:
  * `User email` - Your Xamarin user's email address
  * `API key` - Your Xamarin Test Cloud API key
  * `Device selection id` - The device id you would like to use
8. After you're done, save the workflow and start a new build.
9. Yay, your build passed! You should have received an email about it, too! You'll get an email about your XTC test results once they've been completed, too.
10. You'll notice on the build details page, that there are the actual apps generated for iOS and Android, which you can simply download and install on your device. That's where the bonus challenge comes in! 🏆

![T-shirt](/Evolve/minihack_tshirt_tease.jpg)

##Bonus challenge for t-shirt!

Install the Sample App on your phone by setting up Code Signing and tweet a selfie with our booth. 📸

###For iOS

>We know that setting up Code Signing for iOS is not your favorite pastime. That's why we created a [helpful tool and guide](http://devcenter.bitrise.io/docs/provprofile-cert-export#section-exporting-and-uploading-your-provisioning-profile) on our DevCenter, if you get stuck.

1. Go back to editing your workflow.
2. Select the *Code Signing & Files* menu on the left and upload a valid Provisioning Profile & Certificate so you can download the app to your iPhone.
3. You'll have received an email with the public install page's link. Open the link on your iPhone and press the Install button. *You can also find the Install button on the build details page.*
4. Go back to your home screen, open the app, go to the last slide and tweet a selfie with our booth, using the button there.
5. ???
6. Profit*

🤘 And while you are there, come say hi to us, too, if you feel like chatting about Bitrise or build automation.

###For Android

1. To install a `*-debug.apk` you have to enable `Unknown sources` / `Allow installation of apps from unknown sources` in your Android phone's Settings.
2. You'll have received an email with the public install page's link. Open the link on your phone and press the Install button. *You can also find the Install button on the build details page.*
3. Open the app, go to the last slide and tweet a selfie with our booth, using the button there.
4. ???
5. Profit*

🙌  And while you are there, come say hi to us, too, if you feel like chatting about Bitrise or build automation.


*Come by our booth, show us what you got and you'll receive a badass Bitrise t-shirt! Subject to availability, so hurry up!
