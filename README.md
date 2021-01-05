# <img width="24px" src="./Logo/256.png" alt="Sonarr"></img> Sonarr 

Sonarr is a PVR for Usenet and BitTorrent users. It can monitor multiple RSS feeds for new episodes of your favorite shows and will grab, sort and rename them. It can also be configured to automatically upgrade the quality of files already downloaded when a better quality format becomes available.

*Important* Make sure you disable automatic updates to prevent installing the normal Sonarr versions!

Sonarr-AirDCPP adds airdcpp-web as both an indexer and download client. You can keep the original release dirs by using the symlink support added to this fork in combination with rar2fs. Setup a "Remove Path Mapping" on the Settings -> Download Clients page with your AirDCPP download dir (the same as you use in the Download Client config) as the Remote Path and the rar2fs mount that contains the extracted movies as your Local Path. This will create a symlink in the Sonarr dirs linking to the rar2fs episode file.

## Example config (screenshots are from Radarr, but Sonarr config is identical):

Default download directory in AirDCPP: /mnt/movies/

RAR2FS mount, containing the (virtually) extracted content of /mnt/movies: /mnt/plex/movies

First enable experimental symlink support under Media Management (advanced settings) (no automatic upgrades for now if you use this option):

![image](https://user-images.githubusercontent.com/1114597/102639917-00ba3f00-415a-11eb-8eb8-30670bb0ef46.png)

This option, in combination with a rar2fs mount, will allow you to keep your original release files/rars. It will import the movie to the Radarr movie dir and create a symlink linking to the rar2fs movie. With the example config, your movie would be downloaded to /mnt/movies/Sample.Movie.2018.1080p.Bluray-GROUP and the rar2fs mount /mnt/plex/movies/Sample.Movie.2018.1080p.Bluray-GROUP would show the extracted content. Radarr-AirDCPP will create a symlink to the movie file, not touching your original files.

AirDCPP indexer:

![image](https://user-images.githubusercontent.com/1114597/102640118-4aa32500-415a-11eb-83b7-e25eddf38993.png)

AirDCPP download client:

![image](https://user-images.githubusercontent.com/1114597/102640233-76bea600-415a-11eb-8aab-226440e5a69e.png)

Completed download handling settings:

![image](https://user-images.githubusercontent.com/1114597/102641323-3102dd00-415c-11eb-98b5-f4836be3caa6.png)

Remote path mapping for the AirDCPP download dir and the rar2fs mount:

![image](https://user-images.githubusercontent.com/1114597/102640513-dddc5a80-415a-11eb-80a4-d68be79dbc9d.png)

## Getting Started

- [Download](https://sonarr.tv/#download) (Linux, MacOS, Windows, Docker, etc.)
- [Installation](https://github.com/Sonarr/Sonarr/wiki/Installation)
- [FAQ](https://github.com/Sonarr/Sonarr/wiki/FAQ)
- [Wiki](https://github.com/Sonarr/Sonarr/wiki)
- [API Documentation](https://github.com/Sonarr/Sonarr/wiki/API)

## Support

- [Donate](https://sonarr.tv/donate)
- [Discord](https://discord.gg/M6BvZn5)
- [Reddit](https://www.reddit.com/r/sonarr)

## Features

### Current Features

- Support for major platforms: Windows, Linux, macOS, Raspberry Pi, etc.
- Automatically detects new episodes
- Can scan your existing library and download any missing episodes
- Can watch for better quality of the episodes you already have and do an automatic upgrade. *eg. from DVD to Blu-Ray*
- Automatic failed download handling will try another release if one fails
- Manual search so you can pick any release or to see why a release was not downloaded automatically
- Fully configurable episode renaming
- Full integration with SABnzbd and NZBGet
- Full integration with Kodi, Plex (notification, library update, metadata)
- Full support for specials and multi-episode releases
- And a beautiful UI

## Configuring Development Environment:

### Requirements

- [Visual Studio 2017](https://www.visualstudio.com/vs)
- [Git](https://git-scm.com/downloads)
- [NodeJS](https://nodejs.org/en/download)
- [Yarn](https://yarnpkg.com)

### Setup

- Make sure all the required software mentioned above are installed
- Clone the repository recursively to get Sonarr and it's submodules
    - You can do this by running `git clone --recursive https://github.com/Sonarr/Sonarr.git`
- Install the required Node Packages using `yarn`

### Backend Development

- Run `yarn build` to build the UI
- Open `Sonarr.sln` in Visual Studio
- Make sure `Sonarr.Console` is set as the startup project
- Build `Sonarr.Windows` and `Sonarr.Mono` projects
- Build Solution

### UI Development

- Run `yarn watch` to build UI and rebuild automatically when changes are detected
- Run Sonarr.Console.exe (or debug in Visual Studio)

### Licenses

- [GNU GPL v3](http://www.gnu.org/licenses/gpl.html)	
- Copyright 2010-2020

### Supporters

This project would not be possible without the support of our users and software providers. [**Become a sponsor or backer**](https://opencollective.com/sonarr) to help us out!

#### Sponsors

[![Sponsors](https://opencollective.com/sonarr/tiers/sponsor.svg)](https://opencollective.com/sonarr/contribute/sponsor-21443/checkout)

#### Flexible Sponsors

[![Flexible Sponsors](https://opencollective.com/sonarr/tiers/flexible-sponsor.svg?avatarHeight=54)](https://opencollective.com/sonarr/contribute/flexible-sponsor-21457/checkout)

#### Backers

[![Backers](https://opencollective.com/sonarr/tiers/backer.svg?avatarHeight=48)](https://opencollective.com/sonarr/contribute/backer-21442/checkout)

#### JetBrains

Thank you to [<img src="/Logo/Jetbrains/jetbrains.svg" alt="JetBrains" width="32"> JetBrains](http://www.jetbrains.com/) for providing us with free licenses to their great tools

* [<img src="/Logo/Jetbrains/teamcity.svg" alt="TeamCity" width="32"> TeamCity](http://www.jetbrains.com/teamcity/)
* [<img src="/Logo/Jetbrains/resharper.svg" alt="ReSharper" width="32"> ReSharper](http://www.jetbrains.com/resharper/)
* [<img src="/Logo/Jetbrains/dottrace.svg" alt="dotTrace" width="32"> dotTrace](http://www.jetbrains.com/dottrace/)

