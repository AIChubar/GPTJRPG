import openai
import os
import json
import re

structure_file = open("structure_quests.json", "r")

current_world_file = open("current_world.json", "r")

units_file = open("units.json", "r")

structure_content = structure_file.read()
units_content = units_file.read()

current_world_data = json.load(current_world_file)

current_world_name = current_world_data["worldName"]
world_folder_path = os.path.join(os.pardir, "Worlds", current_world_name)

narrative_file = open(os.path.join(world_folder_path, "Narrative.json"), "r")
levels_file = open(os.path.join(world_folder_path, "Levels.json"), "r")
units_file = open(os.path.join(world_folder_path, "UnitData.json"), "r")

units_data = json.load(units_file)

folder_path = os.path.join(os.pardir, "Worlds")

response = openai.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=0.8,
    max_tokens=4096,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content for the created fantasy game world. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "Your task is to generate quests for each level. You should base your choice on existing world, levels description and enemies on levels: \n" + narrative_file.read() + "\n" + json.dumps(units_data["levelsUnits"]) + "\n" + levels_file.read() + "\n" 
                       "Fill questList with 2-3 quests for each level. Choose from one of the available quest types: kill. QuestObjective should be chosen from all enemies on this level, use exactly artisticName for QuestObjective. ObjectiveNum is always 1 for a kill questType. \n"
                       "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                       #"#Constraints: \n"
                       "#Structure: \n" + structure_content + "\n"
        },
        {
            "role": "user",
            "content": "Start generating."
        }
    ]
)

json_result = json.loads(response.choices[-1].message.content)



with open(os.path.join(world_folder_path, "QuestData.json"), 'w') as f:
    json.dump(json_result, f, indent=2)

structure_file.close()
current_world_file.close()
narrative_file.close()
levels_file.close()
print(response.choices[0].message.content)