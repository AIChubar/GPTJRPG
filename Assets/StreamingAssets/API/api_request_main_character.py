import openai
import os
import json
import re

with open('structure_main_character.json', 'r') as file:
    structure_content = file.read()

with open('units.json', 'r') as file:
    units_content = file.read()

with open('units_stats.json', 'r') as file:
    units_stats_content = file.read()

with open('current_world.json', 'r') as file:
    current_world_data = json.load(file)


current_world_name = current_world_data["worldName"]

worlds_folder_path = os.path.join(os.pardir, "Worlds")

world_folder_path = os.path.join(os.pardir, "Worlds", current_world_name)
with open(os.path.join(world_folder_path, "Narrative.json"), "r") as file:
    narrative_content = file.read()
main_character_contents = ""

for name in os.listdir(worlds_folder_path):
    json_file_path = os.path.join(worlds_folder_path, name, 'MainCharacter.json')
    if os.path.isdir(os.path.join(worlds_folder_path, name)) and os.path.exists(json_file_path):
        with open(json_file_path, 'r') as json_file:
            content = json.load(json_file)
            main_character_contents += json.dumps(content) + '\n'
            
response = openai.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=1.0,
    max_tokens=4096,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "You need to generate a central protagonist of the story main and only playable character within the fantasy world. Base it on the description of this fantasy world including already created main antagonist, which is the first unit of the antagonist group:  \n" + narrative_content + "\n"
                       "Generate race, class, occupation, name and backstory for this main protagonist. Base it on the storyType, which defines if the player will be playing for 'good', 'neutral' or 'evil' protagonist. \n"
                       "After you did it, generate a protagonist group which are allies that accompany the main protagonist. \n"
                       "Units should be generated by at first choosing the first attribute of each unit, which should be picked from outer objects names from 'units content' JSON file.\n"
                       "After that you should pick the second attribute that is defined by the name of the list inside first attribute object in 'units content' JSON. \n"
                       "The third one is the string in the corresponding to the second attribute list inside the corresponding to the first attribute object. The third attribute can be empty if the only string in the list is empty. \n"
                       "You can only take the second and third attributes from objects and lists corresponding to the previous attributes. \n"
                       "Here is the 'units content' JSON file: \n" + units_content + "\n"
                       "After that generate a characteristic name and base it on your interpretation of the second and the third attributes. It can consist of a few words. \n"
                       "After that generate an artistic name which should be an appropriate for this unit fantasy name. \n"
                       "Assign a power level for each unit based of your own knowledge about the creature, there are 3 different levels of power for protagonist group units: mighty, formidable, legendary. \n"
                       "You also should assign a class of the unit to it depending on what fits your knowledge about particular fantasy unit. You cannot assign 2 same unit classes for one group. Pick the class type from this 'unit characteristics' JSON file: \n" + units_stats_content + "\n"                                                                                                                                                                                                                                                                                           
                       "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                       "#Constraints: \n"
                       "1. Try to generate protagonist with characteristic not similar to these: \n" + main_character_contents + "\n"
                       "2. Make sure that each field in a given structure is filled. \n"
                       "3. Artistic name and characteristic name of the same unit should not be similar. \n"
                       "#Structure: \n" + structure_content + "\n"
        },
        {
            "role": "user",
            "content": "Start generating."
        }
    ]
)

json_result = json.loads(response.choices[-1].message.content)

with open(os.path.join(world_folder_path, "MainCharacter.json"), 'w') as f:
    json.dump(json_result, f, indent=2)

print(response.choices[0].message.content)