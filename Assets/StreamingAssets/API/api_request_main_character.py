import openai
import os
import json
import re

structure_file = open("structure_main_character.json", "r")

current_world_file = open("current_world.json", "r")

current_world_data = json.load(current_world_file)

current_world_name = current_world_data["worldName"]

structure_content = structure_file.read()

world_folder_path = os.path.join(os.pardir, "Worlds", current_world_name)

narrative_file = open(os.path.join(world_folder_path, "Narrative.json"), "r")

response = openai.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=1.1,
    max_tokens=4000,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are a creative assistant for creating textual game content. You need to follow instructions while being creative and artistic. \n"
                       "#Instructions: \n "
                       "You need to generate a central protagonist of the story main and only playable character within the fantasy world. Base it on the description of this fantasy world:  \n" + narrative_file.read() + "\n"
                       "Generate race, class, occupation, name and backstory for this main protagonist. \n"
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

with open(os.path.join(world_folder_path, "MainCharacter.json"), 'w') as f:
    json.dump(json_result, f, indent=2)

structure_file.close()
current_world_file.close()
narrative_file.close()
print(response.choices[0].message.content)