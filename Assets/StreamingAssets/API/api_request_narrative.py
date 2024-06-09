import openai
import os
import json
import re

structure_file = open("structure_narrative.json", "r")
structure_content = structure_file.read()

folder_path = os.path.join(os.pardir, "Worlds")
folder_names = [name for name in os.listdir(folder_path) if os.path.isdir(os.path.join(folder_path, name))]

def separate_words(name):
    return re.sub(r'([a-z])([A-Z])', r'\1\n\2', name)

# Separate words and join them with newline symbols
formatted_names = '\n'.join([separate_words(name) for name in folder_names])

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
                       "Generate a fantasy world setting describing specific features of this world. \n"
                       "Generate a main villain around which the story of this world revolves. \n"
                       "Generate a descriptive story of this world that tells main features and problems of this world. \n"
                       "Generate a name for this fantasy world that can consist of a few words. These words can resemble specific patterns of this world or a setting in which this world exists. \n"
                       "Generate a greeting game message for the unnamed main hero entering this world from the almighty observer. \n"
                       "The main point of the world narrative is the interaction with the villain, story that you provide is everything before the player and villain meet. \n"
                       "You should generate a game outcome message that will develop the story in case the main villain was defeated. \n"
                       "There is also an outcome when player was defeated during the game and it also have it's outcome that is likely bad for the game world. Generate this message as well. \n"
                       "Fill the generated content into the appropriate variables in the given JSON structure. \n"
                       "#Constraints: \n"
                       "1. Do not include specific symbols in the generated world name. Always separate words starting with capital letter with spaces.\n"
                       "2. Prefer not to use words for the world name that already exists in the already created worlds from this list: \n" + formatted_names + "\n"
                       "#Structure: \n" + structure_content + "\n"
        },
        {
            "role": "user",
            "content": "Start generating."
        }
    ]
)

json_result = json.loads(response.choices[-1].message.content)

folder_path = os.path.join(os.pardir, "Worlds", json_result["worldName"].replace(" ", ""))

os.makedirs(folder_path, exist_ok=True)

with open(os.path.join(folder_path, "Narrative.json"), 'w') as f:
    json.dump(json_result, f, indent=2)
    
with open('current_world.json', 'r+') as f:
    data = json.load(f)
    data['worldName'] = json_result["worldName"].replace(" ", "")
    f.seek(0)
    json.dump(data, f, indent=4)
    f.truncate()
structure_file.close()

print(response.choices[0].message.content)