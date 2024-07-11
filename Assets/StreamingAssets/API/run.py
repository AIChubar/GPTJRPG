import subprocess

# running other file using run()
for i in range(3):
    subprocess.run(["python", "api_request_narrative.py"])
    subprocess.run(["python", "api_request_main_character.py"])
    subprocess.run(["python", "api_request_levels.py"])
    subprocess.run(["python", "api_request_unit_data.py"])
    subprocess.run(["python", "api_request_quests.py"])
    subprocess.run(["python", "api_request_dialogue.py"])
