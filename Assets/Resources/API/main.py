from openai import OpenAI
import os
import json

client = OpenAI()
structure = open("example2.json", "r")
monsters = open("Monsters.txt", "r")

response = client.chat.completions.create(
    model="gpt-3.5-turbo-0125",
    temperature=0.8,
    max_tokens=4000,
    response_format={"type": "json_object"},
    messages=[
        {"role": "system",
         "content": "You are an assistance for creating textual game content. The output should be in "
                    "JSON format, according to a structure I provide you later."},
        {"role": "user", "content": "Replace Placeholder with generated content. "
                                    "Replace \"...\" according to previous structure parts. "
                                    "Generate me 2 levels with at least 5 groups in each, there should be at most 3 "
                                    "enemies in each group. Try to have"
                                    "different monster in groups with the group name which can represent such a group "
                                    "of"
                                    "different units."},
        {"role": "user", "content": structure.read()},
        {"role": "assistant", "content": "How do I fill enemyName field?"},
        {"role": "user", "content": "Only by using names from the list:"},
        {"role": "user", "content": monsters.read().replace('\n', ' ')},
        {"role": "user", "content": "You can"}
    ]
)
json_result = json.loads(response.choices[0].message.content)
with open(os.path.join(os.pardir, "Worlds\\" + json_result["narrativeData"]["worldName"] + ".json"), 'w') as f:
    json.dump(json.loads(response.choices[0].message.content), f, indent=2)
structure.close()
monsters.close()

print(response.choices[0].message.content)
