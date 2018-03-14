# Certification Checklist
This document captures the certification checklist used by Microsoft and Skype to validate Skype bots submitted for certification.

| Number | Requirement | Notes |
| ----------- | ------ | ------ |
| **100** | **Bot Profile Information** |
| 101 | Bot Name and Publisher Name | Your bot needs to have a clear and descriptive Bot Name and Publisher Name. Do not suffix ‘Bot’ to your bot name unless it is unavoidable and use spacing. E.g.: FooBot should ideally be Foo, or if unavoidable, Foo Bot.|
| 102 | Privacy Policy and Terms of Use documents published online and referenced by the bot | Your bot must have links to a valid Privacy Policy and Terms of Use. |
| 103 | Bot avatar image with non-black and non-white background color | Your bot must have an avatar image. You must not use the standard bot framework avatar. You must not infringe any copyrighted images. |
| 104 | Bot description explains the purpose of the bot | Your bot must have a clear and comprehensive description to help users understand its purpose. |
| **200** | **Bot Functionality** |
| 201 | Automatically display a Welcome message | When a user adds the bot as a contact, the bot must automatically display a welcome message. This increases user engagement and guides the user to success. |
| 202 | Display a Help screen when receiving unknown commands | Whenever a user sends any message / command to the bot that the bot doesn’t understand, the bot must respond with a help message, providing usage guidance. |
| 203 | Bot functionality works correctly |  Your bot must function correctly. Bots with obvious broken functionality will be rejected and / or removed from the directory. |
| 204 | Bot must be fast | The bot must respond to user commands without extensive delays.|
| 205 | Bot must be mobile-friendly | The bot must be designed in a way that enables users to be able to interact with it on a mobile device. For example, the bot should not send excessively long individual messages (instead it can send a sequence of several messages). To ensure the bot is mobile friendly, please test on a mobile device (Android or iOS) with the latest versions of Skype. |
| **300** | **Compliance** |
| 301 | Your Bot Submission must PASS Azure Online Service Terms| See [Azure Online Service Terms]( https://azure.microsoft.com/en-gb/support/legal/)|
| 302 | Your Bot Submission must PASS Microsoft Channel Publication Terms | See [Microsoft Channel Publication Terms](https://www.botframework.com/content/bot-service-channels-terms.htm) |
