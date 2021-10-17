# Microsoft Teams Presence MQTT Publisher

[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://www.gnu.org/licenses/lgpl-3.0)

## 1 Using the Microsoft Teams Presence Publisher
### 1.1 Installing the software
### 1.2 Configuring the software
### 1.3 Using your own Application Registration ID
The Microsoft Teams Presence Publisher needs an Azure Application Registration ID to identify itself with, when requesting the presence information on behalf of a user. In the source code there is a hardcoded registration number that I created on my personal domain. If you want to setup you own application registration, follow [this link](https://go.microsoft.com/fwlink/?linkid=2083908) and configure every step to match the following screenshots:

![Azure App Registration - API Permissions](https://github.com/Friendly0Fire/MicrosoftTeamsPresence/blob/master/img/AppRegistration-API-Permissions.png)

![Azure App Registration - Authentication](https://github.com/Friendly0Fire/MicrosoftTeamsPresence/blob/master/img/AppRegistration-Authentication.png)

## Credits
- Simple fork of [xs4free's MicrosoftTeamsPresenceLed](https://github.com/xs4free/MicrosoftTeamsPresenceLed) focused only on MQTT.
- This source is my rewrite of [PresenceLight](https://github.com/isaacrlevin/PresenceLight) by [Isaac Levin](https://github.com/isaacrlevin)
- [Greasemann](https://commons.wikimedia.org/wiki/File:Portrait_Placeholder.png) for the unknown user icon (under the [Creative Commons Attribution-Share Alike 4.0 International license](https://creativecommons.org/licenses/by-sa/4.0/deed.en))
