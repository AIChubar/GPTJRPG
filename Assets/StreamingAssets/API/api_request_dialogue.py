import openai
import os
import json
import re



with open('units.json', 'r') as file:
    units_content = file.read()

with open('current_world.json', 'r') as file:
    current_world_data = json.load(file)
    
with open('structure_dialogue.json', 'r') as file:
    structure_content = file.read()

current_world_name = current_world_data["worldName"]
world_folder_path = os.path.join(os.pardir, "Worlds", current_world_name)


with open(os.path.join(world_folder_path, "Narrative.json"), "r") as file:
    narrative_content = file.read()
with open(os.path.join(world_folder_path, "Levels.json"), "r") as file:
    levels_content = file.read()
with open(os.path.join(world_folder_path, "MainCharacter.json"), "r") as file:
    main_character_content = file.read()
with open(os.path.join(world_folder_path, "UnitData.json"), "r") as file:
    units_data_content = file.read()
with open(os.path.join(world_folder_path, "MainCharacter.json"), "r") as file:
    main_character_content = file.read()
units_data = json.loads(units_data_content)
folder_path = os.path.join(os.pardir, "Worlds")

for level in units_data["levelsUnits"]:
    for enemyGroup in level["enemyGroups"]:
        first_enemy = enemyGroup["units"][0]
        artistic_name = first_enemy["artisticName"].replace(" ", "")
        dialogue_folder_path = os.path.join(world_folder_path, "Dialogues")
        os.makedirs(dialogue_folder_path, exist_ok=True)
        dialogue_file_path = os.path.join(dialogue_folder_path, f"{artistic_name}.json")

        response = openai.chat.completions.create(
            model="gpt-3.5-turbo-0125",
            temperature=1.0,
            max_tokens=4096,
            response_format={"type": "json_object"},
            messages=[
                {
                    "role": "system",
                    "content": "#Setting: \n You are a creative assistant for creating textual game content for the created fantasy game world. You need to follow instructions while being creative and artistic. \n"
                               "#Instructions: \n "
                               "Your are given a fantasy world with its description, levels, units: \n"
                                + narrative_content + "\n" + main_character_content + "\n" + levels_content + "\n" + units_data_content + "\n" 
                               "The given unit will be a speaker that represents a group it is a part of, here is this unit data: \n" + json.dumps(first_enemy) + "\n"
                               "You should evaluate this unit behavioral metrics [aggressiveness, friendliness, intellect, strategicThinking, indifference, flexibility, communicationSkills, morality, braveness, chaotic] on 0 to 100 scale \n"
                               "I insist that you must do realistic evaluation of this unit in the given fantasy world assuming that it is cruel and creatures from here might not adore other creatures that are not alike. \n "
                               "Base unit's attitude on the fact that given unit will be communicating with the main player character that has it's own characteristics, story, etc. \n "
                               "After that you need to construct a dialogue tree that contains a phrases from the enemy unit and 3 different answer options from the main character. \n"
                               "For each option from the main character there is another phrase from the enemy unit and for that phrase you again have more options and etc. \n"
                               "Dialogue should be 3 choices deep and last choice is only two options. The last enemy answer that conclude the dialogue should represents certain \"outcome\" which can be on of the given variants: \n"
                               "\"attack\" for attacking a player, \"ally\" for becoming friends with a player without fighting, and \"ignore \" for not reacting to a player. \n"
                               "Ignoring is the situation when player character don't want to continue anymore and enemy decides to not attack or when after last phrase enemy decides to start ignoring or retreat. \n"
                               "There are some number of outcomes in the resulting tree and you should determine how many of each outcome should be there based on metrics and what answer options was generated. \n"
                               "i.e if the friendliness is close to 0, there will be only one ally outcome. \n"
                               "Last enemy answer before outcome should obviously state what is an outcome, there should not be any offers or engaging in further conversation \n"
                               "Metric chaotic is responsible for the non-obviousness of the answers but even if it is 100 enemy reaction should be somewhat logical. \n"
                               "Other metrics should affect outcome for each path in the tree, i.e. if the unit is very aggressive, being a bully should improve enemy attitude towards you. \n"
                               "Another example is that if morality is very low and strategicThinking is high and there is an option for a main character to retreat, enemy will attack you because it feels your weakness. \n "
                               "Last example is that if braveness is very low and friendliness is very low choosing an option to engage in fight will result in ignoring. \n"
                               "Given examples is not a rule but just a direction. Remember that world is fictional but realistic. \n"
                               "Be radical about changing enemy speaking style. Base it mainly on the most extremely low or high metric, don't hesitate to be very rude or very friendly, very thorough or very simple, etc. \n"
                               "Derive specific speaking artifacts, inherent sounds, literature accents, idioms or slang based on your knowledge about creatures in a fantasy world. \n"
                               "i.e. non humanoid creatures (intellect most animals or abominations should be low) should speak like they are dumb and if the unit is mage or priest it should show it's knowledge and wisdom. \n"
                               "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                               "#Constraints: \n"
                               "1. Do not include information who is speaking in the phrases strings (like \" unitName : unitSpeech\"). \n"
                               "2. Do not modify the given structure. \n"
                               "3. If the outcome is ignore the last phrase should not give any hope for the further interaction, it means that the player and the enemy will not interact in future. \n"
                               "#Structure: \n" + structure_content + "\n"
                },
                {
                    "role": "user",
                    "content": "Start generating."
                }
            ]
        )

        json_result = json.loads(response.choices[-1].message.content)



        with open(dialogue_file_path, 'w') as f:
                    json.dump(json_result, f, indent=2)

first_enemy = json.loads(narrative_content)["antagonist"]
artistic_name = first_enemy["artisticName"].replace(" ", "")
dialogue_folder_path = os.path.join(world_folder_path, "Dialogues")
os.makedirs(dialogue_folder_path, exist_ok=True)
dialogue_file_path = os.path.join(dialogue_folder_path, f"{artistic_name}.json")

response = openai.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=1.0,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content for the created fantasy game world. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "Your are given a fantasy world with its description, levels, units: \n"
                        + narrative_content + "\n" + main_character_content + "\n" + levels_content + "\n" + units_data_content + "\n" 
                       "The given unit is the main game villain: \n" + json.dumps(first_enemy) + "\n"
                       "You should evaluate this unit behavioral metrics [aggressiveness, friendliness, intellect, strategicThinking, indifference, flexibility, communicationSkills, morality, braveness, chaotic] on 0 to 100 scale \n"
                       "I insist that you must do realistic evaluation of this unit in the given fantasy world assuming that it is cruel and creatures from here might not adore other creatures that are not alike. \n "
                       "Base unit's attitude on the fact that given unit will be communicating with the main player character that has it's own characteristics, story, etc. \n "
                       "After that you need to construct a dialogue tree that contains a phrases from the enemy unit and three different answer options from the main character. \n"
                       "For each option from the main character there is another phrase from the enemy unit and for that phrase you again have more options and etc. \n"
                       "Dialogue should be 3 choices deep and last choice is only two options. The last enemy answer that conclude the dialogue should represents certain \"outcome\" which can only be 'attack' because the villain is an absolute enemy to the main character. \n"
                       "Last enemy answer before outcome should obviously state what is an outcome, there should not be any offers or engaging in further conversation \n"
                       "Metric chaotic is responsible for the non-obviousness of the answers but even if it is 100 enemy reaction should be somewhat logical. \n"
                       "Given examples is not a rule but just a direction. Remember that world is fictional but realistic. \n"
                       "Be radical about changing enemy speaking style. Base it mainly on the most extremely low or high metric, don't hesitate to be very rude or very friendly, very thorough or very simple, etc. \n"
                       "Derive specific speaking artifacts, inherent sounds, literature accents, idioms or slang based on your knowledge about creatures in a fantasy world. \n"
                       "i.e. non humanoid creatures (intellect most animals or abominations should be low) should speak like they are dumb and if the unit is mage or priest it should show it's knowledge and wisdom. \n"
                       "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                       "#Constraints: \n"
                       "1. Do not include information who is speaking in the phrases strings (like \" unitName : unitSpeech\"). \n"
                       "2. Do not modify the given structure. \n"
                       "#Structure: \n" + structure_content + "\n"
        },
        {
            "role": "user",
            "content": "Start generating."
        }
    ]
)

json_result = json.loads(response.choices[-1].message.content)



with open(dialogue_file_path, 'w') as f:
            json.dump(json_result, f, indent=2)

