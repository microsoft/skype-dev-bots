## Overview
Samples walk you through functionalities of Skype bots developed using various services provided by Microsoft. Every sample has a link to add the bot as a Skype contact, so that you can experience the bot capabilities before diving into the code. To get started, clone this repository.

    git clone https://github.com/Microsoft/skype-dev-bots.git
    cd skype-dev-bots

## Fundamentals
Learn basics of Skype bot development.

Sample | Description | C# | Node
------------ | ------------- | :-----------: | :-----------:
Doctor Code | A bot that teaches how to implement basic functionalities in a Skype bot | [View Sample](/Samples/Csharp/Fundamentals/DoctorCode)[![Deploy to Azure][Deploy Button]][Deploy Csharp/Fundamentals/DoctorCode] | [View Sample](/Samples/Node/Fundamentals/DoctorCode)[![Deploy to Azure][Deploy Button]][Deploy Node/Fundamentals/DoctorCode]

## Storage
Learn how to store and retrieve information from Skype bot using cloud services.

Sample | Description | C# | Node
------------ | ------------- | :-----------: | :-----------:
Note | A bot that manages personal notes using MongoDB and Azure | [View Sample](/Samples/Csharp/Storage-MongoDB/Notes) | [View Sample](/Samples/Node/Storage-MongoDB/Notes)

## Realtime Media
Build powerful media bots using [Real-time Media Platform](https://github.com/Microsoft/BotBuilder-RealTimeMediaCalling).

Sample | Description | C#
------------ | ------------- | :-----------:
Text to Speech | A bot that generates synthesized speech on an Azure cloud services and streams the audio to the user in a Skype audio call. | [View Sample](/Samples/Csharp/RealtimeMedia/TextToSpeech)
Text to Video Speech | A bot that generates synthesized speech on an Azure cloud services and streams the video to the user in a Skype video call. | [View Sample](/Samples/Csharp/RealtimeMedia/TextToVideoSpeech)
Video Player | A bot that plays video and audio files stored as an Azure blob. | [View Sample](/Samples/Csharp/RealtimeMedia/VideoPlayer)

## Cognitive Services
Add intelligent features to your bots using [Microsoft Cognitive Services](https://azure.microsoft.com/en-us/services/cognitive-services/).  

Sample | Description | C# | Node
------------ | ------------- | :-----------: | :-----------:
Fridge | A bot that manages your refrigerator inventory using LUIS (Language Understanding Intelligent Service). | [View Sample](/Samples/Csharp/CognitiveServices-Language/Fridge) | [View Sample](/Samples/Node/CognitiveServices-Language/Fridge)[![Deploy to Azure][Deploy Button]][Deploy Node/CognitiveServices-Language/Fridge]
QnA | A bot that helps you get answers based on FAQs using QnAMaker service. | [View Sample](/Samples/Csharp/CognitiveServices-Knowledge/QnA) | [View Sample](/Samples/Node/CognitiveServices-Knowledge/QnA)[![Deploy to Azure][Deploy Button]][Deploy Node/CognitiveServices-Knowledge/QnA]

## Contributing
This project welcomes contributions and suggestions.  Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us the rights to use your contribution. For details, visit https://cla.microsoft.com.
When you submit a pull request, a CLA-bot will automatically determine whether you need to provide a CLA and decorate the PR appropriately (e.g., label,comment). Simply follow the instructions provided by the bot. You will only need to do this once across all repos using our CLA.
This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

[Deploy Button]: https://azuredeploy.net/deploybutton.png
[Deploy Node/Fundamentals/DoctorCode]: https://azuredeploy.net?repository=https://github.com/Microsoft/skype-dev-bots/tree/DeployToAzureButton/Samples/Node/Fundamentals/DoctorCode
[Deploy Csharp/Fundamentals/DoctorCode]: https://azuredeploy.net?repository=https://github.com/Microsoft/skype-dev-bots/tree/DeployToAzureButton/Samples/Csharp/Fundamentals/DoctorCode
[Deploy Node/CognitiveServices-Language/Fridge]: https://azuredeploy.net?repository=https://github.com/Microsoft/skype-dev-bots/tree/DeployToAzureButton/Samples/Node/CognitiveServices-Language/Fridge
[Deploy Node/CognitiveServices-Knowledge/QnA]: https://azuredeploy.net?repository=https://github.com/Microsoft/skype-dev-bots/tree/DeployToAzureButton/Samples/Node/CognitiveServices-Knowledge/QnA