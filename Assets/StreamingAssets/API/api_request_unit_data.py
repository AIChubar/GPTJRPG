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
    
with open('units_stats.json', 'r') as file:
    units_stats_text = file.read()
    
current_world_name = current_world_data["worldName"]
world_folder_path = os.path.join(os.pardir, "Worlds", current_world_name)

narrative_file = open(os.path.join(world_folder_path, "Narrative.json"), "r")
levels_file = open(os.path.join(world_folder_path, "Levels.json"), "r")
main_character_file = open(os.path.join(world_folder_path, "MainCharacter.json"), "r")
folder_path = os.path.join(os.pardir, "Worlds")


response = openai.chat.completions.create(
    model="gpt-4o",
    temperature=0.7,
    max_tokens=4096,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content for the created fantasy game world. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "Your task is to generate units for a friendly character group and enemy groups. "
                       "Also generate unit group for the main villain given, the first unit of the mainVillain group if the main villain itself, use villain name as an artisticName for the first unit. This villain unit should be legendary power level. \n"
                       "You should base your choice on existing world and levels description \n" + narrative_file.read() + "\n" + main_character_file.read() + "\n" + levels_file.read() + "\n" 
                       "You are restricted by the pool of available units you can take from this JSON file with units: \n" + units_content + "\n"
                       "Each unit consist of three attributes, first is defined by the outer list names, like 'horde' or 'relict' second by inner list names i.e. 'lab_rat' and the third by one of the strings in the list i.e. 'ice'. Third attribute can be empty for some second attribute lists. \n"
                       "Units inside one group should preferably share the same first attribute. \n"
                       "Assign a power level for each unit based of your own knowledge about the creature, there are 7 different levels of power: feeble, weak, moderate, strong, mighty, formidable, legendary. \n"
                       "You also should assign a type of the unit to it depending on what fits your knowledge about particular fantasy unit. You cannot assign 2 same unit types for one group. Pick the unit type from this list with characteristics: \n" + units_stats_text + "\n"
                       "There is an average group power (agp) that is the  between the power levels on 1 to 7 scale. Make player agp level 5.5-6.5. Enemy agp on 1st level - 2-3, on 2nd level - 4-5, on 3rd level - 5-6.\n"
                       "Try to use differently powered units to fill the group with set average power. Never use same three power levels for one group. \n"
                       "Generate characteristic name that can consist of several words for each unit based on this unit only second and third attributes and an singular artistic name appropriate for this unit. \n"
                       "There are 3 levels. Each level should contains four enemy groups. At least three enemy groups should contain 3 units. Generate an artistic name for each enemy group. \n"
                       "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                       "#Constraints: \n"
                       "1. Do not choose the same exact unit more than 2 times. \n"
                       "2. You cannot change given units attributes. \n"
                       "3. At least two units in each group should share the same firstAttribute. \n"
                       "4. Artistic name and characteristic name of the same unit should not be similar. \n"
                       "5. Pick units from different first attributes for different levels. Try to assign enemies that can fit level description. \n"
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