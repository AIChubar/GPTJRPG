import openai
import os
import json
import re

structure_file = open("structure_levels.json", "r")

current_world_file = open("current_world.json", "r")
structure_content = structure_file.read()

current_world_data = json.load(current_world_file)

current_world_name = current_world_data["worldName"]
world_folder_path = os.path.join(os.pardir, "Worlds", current_world_name)
narrative_file = open(os.path.join(world_folder_path, "Narrative.json"), "r")

walkable_terrain = [
    "dark-dirt",
    "dark-grass",
    "dirt",
    "dirty-platform",
    "fractured-wall-platform",
    "grass",
    "grey-brick-platform",
    "lava-rock",
    "light-dirt",
    "pink-sand",
    "rusty-chain-platform",
    "sand",
    "snow",
    "tundra-sand"
]

obstacle_terrain = [
    "deep-dark-hole",
    "hole",
    "lava",
    "water"
]

folder_path = os.path.join(os.pardir, "Worlds")


response = openai.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=0.8,
    max_tokens=4000,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content for the created fantasy game world. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "Your task is to generate 3 levels for a fantasy world. \n"
                       "You need to choose a walkable terrain type for each level from this list: \n" + "\n".join(walkable_terrain) + "\n"
                       "You also need to choose a obstacle terrain type for each level from this list: \n" + "\n".join(obstacle_terrain) + "\n"
                       "You need to generate a name for each level and a small description. \n"
                       "Base your choice on the given fantasy world description : \n" + narrative_file.read() + "\n"
                       "Main villain is on the third level and it's description should tell about it. \n"
                       "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                       "#Constraints: \n"
                       "1. Do not choose the same walkable terrain for different levels. \n"
                       "#Structure: \n" + structure_content + "\n"
        },
        {
            "role": "user",
            "content": "Start generating."
        }
    ]
)

json_result = json.loads(response.choices[-1].message.content)



with open(os.path.join(world_folder_path, "Levels.json"), 'w') as f:
    json.dump(json_result, f, indent=2)

structure_file.close()
current_world_file.close()
narrative_file.close()

print(response.choices[0].message.content)