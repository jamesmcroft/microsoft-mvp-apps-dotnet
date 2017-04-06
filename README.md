Microsoft MVP Applications
===========

This project is designed to create applications for UWP, Android and iOS (using Xamarin) that take advantage of the Microsoft MVP API.  

## Usage
This app has been made for the purpose of use by current Microsoft MVPs who have an active profile. You will sign in using your MSA associated with your MVP profile.

To submit a contribution via URI, the format is:

```
mvpapp://contribution?title={title}&date={date}&description={description}&quantity={quantity}&reach={reach}&url={url}
```

The query parameters are optional and calling without them will start a new blank contribution.

## Contributing
All MVPs are welcome to contribute towards these MVP applications. If you are an experienced native Xamarin developer, your expertise may be useful in fleshing out the iOS and Android applications!

You can take a look at the project backlogs here:

| Platform |
| ------ |
| [UWP](https://github.com/jamesmcroft/MS-MVP-Apps/projects/1) |
| [Android](https://github.com/jamesmcroft/MS-MVP-Apps/projects/2) |
| [iOS](https://github.com/jamesmcroft/MS-MVP-Apps/projects/3) |

### Getting started

To develop against this application, you will need to create an MSA Application (https://apps.dev.microsoft.com/). Both Converged and Live SDK are supported and you will define that in the API client settings. You will also need a subscription key for the Microsoft MVP API by registering on the Microsoft MVP API site. 

### Fixing issues

When fixing issues in what is part of the current live application, please make these changes against the master branch. This is where our live code resides and will make it easier for us to release a hotfix for. If the issue is part of newer 'in-development' code, please fix those against the develop branch.

## Issue tracking

If you find an issue in any variant of the applications, please could you raise it in the [issues](https://github.com/jamesmcroft/mvp-api-app/issues) section with appropriate tags and it would be fantastic if you could have a look at rectifying the issue and submitting a pull request too!

## App Packages

| Package | Version |
| ------ | ------ |
| UWP | [1.1.6](https://www.microsoft.com/en-us/store/p/mvp-community-app/9nm26mmrjbpf) |
| Android | Coming soon. |
| iOS | Coming soon. |

