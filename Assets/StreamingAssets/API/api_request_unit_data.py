import openai
import os
import json
import re

structure_file = open("structure_unit_data.json", "r")
units_file = open("units.json", "r")

structure_content = structure_file.read()
units_content = units_file.read()

with open('current_world.json', 'r') as file:
    current_world_data = json.load(file)
    
current_world_name = current_world_data["worldName"]
world_folder_path = os.path.join(os.pardir, "Worlds", current_world_name)

narrative_file = open(os.path.join(world_folder_path, "Narrative.json"), "r")
levels_file = open(os.path.join(world_folder_path, "Levels.json"), "r")
main_character_file = open(os.path.join(world_folder_path, "MainCharacter.json"), "r")
folder_path = os.path.join(os.pardir, "Worlds")


response = openai.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=0.7,
    max_tokens=4096,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content for the created fantasy game world. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "Your task is to generate units for a friendly character group and enemy groups. You should base your choice on existing world and levels description \n" + narrative_file.read() + "\n" + main_character_file.read() + "\n" + levels_file.read() + "\n" 
                       "You are restricted by the pool of available units you can take from this JSON file with units: \n" + units_content + "\n"
                       "Each unit consist of three attributes, first is defined by the outer list names, second by inner list names and the third by one of the strings in the list. Third attribute can be empty for some second attribute lists. \n"
                       "Units inside one group should preferably share the same first attribute. \n"
                       "Generate characteristic name that can consist of several words for each unit based on this unit only second and third attributes and an singular artistic name appropriate for this unit. \n"
                       "There are three levels. Each level contains 5 enemy groups. At least 3 enemy groups should contain 3 units. Generate an artistic name for each enemy group. \n"
                       "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                       "#Constraints: \n"
                       "1. Do not choose the same exact unit more than 2 times. \n"
                       "2. You cannot change given units attributes. \n"
                       "3. At least two units in each group should share the same firstAttribute. \n"
                       "4. artistic name and characteristic name of the same unit should not be similar. \n"
                       "#Structure: \n" + structure_content + "\n"
        },
        {
            "role": "user",
            "content": "Start generating."
        }
    ]
)

json_result = json.loads(response.choices[-1].message.content)

first_enemies = []

for level in json_result['levelsUnits']:
    for group in level['enemyGroups']:
        first_enemies.append(group['units'][0])
current_world_data["enemiesMain"] = first_enemies
    
with open('current_world.json', 'w') as f:
    json.dump(current_world_data, f, indent=2)

with open(os.path.join(world_folder_path, "UnitData.json"), 'w') as f:
    json.dump(json_result, f, indent=2)

structure_file.close()
narrative_file.close()
main_character_file.close()
levels_file.close()
print(response.choices[0].message.content)