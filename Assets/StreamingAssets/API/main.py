import openai
import os
import json

structure_file = open("example.json", "r")
units_file = open("enemies.txt", "r")
tile_palettes_file = open("palettes.txt", "r")
existingWorlds = os.listdir(os.path.join(os.pardir, "Worlds"))

structure_content = structure_file.read()
units_content = units_file.read()
palettes_content = tile_palettes_file.read()

response = openai.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=0.8,
    max_tokens=4000,
    response_format={"type": "json_object"},
    messages=[
        {
            "role": "system",
            "content": "#Setting: \n You are an assistant for creating textual game content. The output should be in "
                       "JSON format, according to a structure I provide you later.\n"
                       "#Instructions: \n Your main task is to replace placeholder values in JSON variables with "
                       "content generated by you, strictly following the constraints I provide.\n"
                       "Replace \"...\" according to the previous structure parts. \n"
                       "Generate Health and Damage as an integer value, balance it out so average ally units are about 70% stronger than average enemy units which base stats are around 50 hp and 10 damage.\n"
                       "#Structure: \n" + structure_content + "\n"
                       "#Constraints: \n"
                       "1. Fill the tilePalette variable in the resulting JSON only with one exact string including numerical id from the provided list: " + palettes_content.replace('\n', ' ') + "\n"
                       "First part of the tilePalette id represents walkable tiles description. Do not use the same walkable tile description for different levels" + "\n"
                       "2. Fill the unitID for enemies and ally groups variable in the resulting JSON only with exact strings including numerical id from the provided list: " + units_content.replace('\n', ' ') + "\n"
                       "3. Try to align chosen tilePalette theme with chosen enemies units."
                       "4. It is forbidden to change the numerical id or the id from the list itself. \n"
                       "5. unitName can be filled with the name that will be representing unitID and should be logically connected to it. Do not include new or old in unitName.\n"
                       "6. Do not use names from this list for the worldName, and try to make names dissimilar to those in the list of existing worlds: \n" + "\n".join(existingWorlds) + "\n"
                       "7. Do not modify lists provided. \n"
                       "8. Use at most two same unitID in each group. There should be at least two enemies in each enemyGroup. \n"
                       "9. Two levels. Five enemy groups in each level. \n"
                       "10. Do not use same ids for enemies and allies, try to use logically different units for enemies and allies. \n"
                       "11. Never include .json in worldName. WorldName can consist of several words. \n"
        },
        {
            "role": "user",
            "content": "Start generating."
        }
    ]
)

json_result = json.loads(response.choices[-1].message.content)
with open(os.path.join(os.pardir, "Worlds", json_result["narrativeData"]["worldName"] + ".json"), 'w') as f:
    json.dump(json_result, f, indent=2)

structure_file.close()
units_file.close()
tile_palettes_file.close()
print(response.choices[0].message.content)
