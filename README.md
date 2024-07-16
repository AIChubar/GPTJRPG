# Concept

This project was created for the master diploma thesis "Generating game content with generative language models" for Charles University in Prague. 
This includes the JRPG game made in Unity specifically for this thesis and the application that generates game content for it using the OpenAI API GPT model. 
The generated content, including story, quests, characters, and asset selection, is consolidated into a Game World concept.

# Installation Instructions

### GPT JRPG Game:

The game only supports the Windows OS and doesn't require any additional software installed. The following steps should be done in order to play the game:

1. Extract the compressed "GPT_JRPG_BUILD.zip" file locally.
2. Launch the "GPT JRPG.exe" executable that can be found in the extracted folder.
3. Choose one of the available Game Worlds and click the button "Start Game"

### OpenAI API Application:

In order to be able to generate a new Game World, you need to have an OpenAI account with enough credits on a balance (around $0.40 for the Game World generation with default settings, which is explained [here](#openai-api-settings)) and follow these steps to set up the environment:

1. Have at least Python 3.7.1 or newer installed. You can follow these official guidelines for beginners - [BeginnersGuide/Download | Python Wiki](https://wiki.python.org/moin/BeginnersGuide/Download)
2. Upgrade the PIP to the latest version:
   ```
   pip install --upgrade openai
   ```
3. Install the OpenAI API library by running the following command from the terminal:
   ```
   pip install --upgrade openai
   ```
4. Install the JSON python module:
   ```
   pip install --upgrade jsons
   ```
5. Set up your OpenAI API account - [OpenAI Platform](https://www.google.com]](https://platform.openai.com/auth/login) and add funds to a credit balance.
6. Set up an API key in your OpenAI API account profile - [User settings - OpenAI API](https://www.google.com]](https://platform.openai.com/settings/profile?tab=api-keys).
7. Add a new Windows Environment System Variable, with the Variable name 'OPENAI_API_KEY' and set a Variable value to your OpenAI API key.

After that, you will be able to generate a new Game World both from the game Main Menu or by running a python script 'run.py' from '...\GPT_JRPG_BUILD\GPT JRPG_Data\StreamingAssets\API' folder:
```
   python3 run.py
```
### OpenAI API Settings:
