from openai import OpenAI
import os
import json

client = OpenAI()
structure = open("example2.json", "r")
monsters = open("Monsters.txt", "r")
existingWorlds = os.listdir(os.path.join(os.pardir, "Worlds\\"))
response = client.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=2.0,
    max_tokens=4000,
    response_format={"type": "json_object"},
    messages=[
        {"role": "system",
         "content": "You are an assistance for creating textual game content. The output should be in "
                    "JSON format, according to a structure I provide you later.\n"
                    "Replace Placeholder with generated content. "
                    "Replace \"...\" according to previous structure parts. "
                    "Generate me 2 levels with at least 5 groups in each, there should be at most 3 "
                    "and at least 2 enemies in each group. Try to have "
                    "different monster in groups with the group name which can represent such a group "
                    "of different units.\n" + structure.read() + "\n "},
        {"role": "assistant", "content": "How do I fill enemyName field?"},
        {"role": "user", "content": "Only by using names from the list:" + monsters.read().replace('\n', ' ')},
        {"role": "user", "content": "Iterate through the template twice to be sure that it doesn't contain any "
                                    "Placeholder strings and enemyName only contains names from the list of monsters "
                                    "I provided. Also do not use names from this list for the worldName "
                                    "and try to make names not similar to those:" +
                                    "\n".join(existingWorlds)}
    ]
)
json_result = json.loads(response.choices[0].message.content)
with open(os.path.join(os.pardir, "Worlds\\" + json_result["narrativeData"]["worldName"] + ".json"), 'w') as f:
    json.dump(json.loads(response.choices[0].message.content), f, indent=2)
structure.close()
monsters.close()

print(response.choices [0].message.content)
