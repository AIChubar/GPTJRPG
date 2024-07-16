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

 ![Menu](./images/menu.png)

### OpenAI API Application:

In order to be able to generate a new Game World, you need to have an OpenAI account with enough credits on a balance (at least 1.0$ for the Game World generation with default settings, which is in more details explained [here](#openai-api-settings)) and follow these steps to set up the environment:

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

The folder '...\GPT_JRPG_BUILD\GPT JRPG_Data\StreamingAssets\API' contains the following scripts that make calls to OpenAI after you run the 'run.py' script:
1. "api_request_dialogue.py" - Generates dialogues with all enemies groups.
2. "api_request_levels.py" - Generates levels and chooses terrains for them.
3. "api_request_main_character.py" - Generates a protagonist, including name, class, backstory, and protagonist group.
4. "api_request_narrative.py" - Generates story, game messages and the main antagonist.
5. "api_request_quests.py" - Generates quests for each level.
6. "api_request_unit_data.py" - Generates enemy units.

You can modify the OpenAI API parameters here, such as the 'model' that will be used and the 'temperature', by changing the variables set at the beginning of the script, 'model_param' and 'temperature_param'.
We recommend using either GPT-4o (model_param = "gpt-4o") or GPT-3.5 turbo (model_param = "gpt-3.5-turbo-0125") models and temperature within the range 0.5 and 1.0.

The current model parameter for all scripts is set to use the model GPT-4o (model_param = "gpt-4o"). The Temperature is set to 0.6 for all scripts except the dialogue query, for which the temperature equals 1.0.
The cost of creating one world currently ranges between $0.6$ and $0.8$. The most costly part is the dialogue query, and lowering its model parameter to GPT-3.5 turbo (model_param = "gpt-3.5-turbo-0125") reduces costs to approximately $0.3.

Setting the temperature above the recommended value may cause a broken structure and can make this Game World unlaunchable.
