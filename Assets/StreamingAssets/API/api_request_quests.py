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
    model="gpt-4o",
    temperature=0.8,
    max_tokens=4096,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content for the created fantasy game world. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "Your task is to generate quests for each level. You should base your choice on existing world, levels description and enemies on levels: \n" + narrative_file.read() + "\n" + json.dumps(units_data["levelsUnits"]) + "\n" + levels_file.read() + "\n" 
                       "On each level there should be a main quest. For two first levels reward for the main quest should be 'pass to the next level' and the type can only be 'kill or unite'. \n" 
                       "For the last level reward for the main quest should be 'win' and QuestObjective should always be the mainVillain unit artistic name, the type of this final quest should only be 'kill'. Description for the main quest should be connected to the main villain and the whole story.\n"
                       "Fill questList with 2 side quests for each level. Choose from one of the three available quest types: 'kill', 'unite', 'kill or unite'. QuestObjective should be chosen from all enemies on this level, use exactly artisticName for QuestObjective. Pick quest reward from two available: 'amulet of alliance', 'amulet of healing'. But for type 'unite' reward can only be 'amulet of healing' \n"
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