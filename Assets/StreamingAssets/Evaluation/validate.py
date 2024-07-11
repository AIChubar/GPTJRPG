import json
import os

worlds_folder_path = os.path.join(os.pardir, "Worlds")

with open(os.path.join(os.pardir, "API", "units.json")) as f:
    units_atlas = json.load(f)


def is_valid_unit(unit, reference_json):
    first_attr = unit['firstAttribute']
    second_attr = unit['secondAttribute']
    third_attr = unit['thirdAttribute']

    if first_attr not in reference_json:
        return False

    if second_attr not in reference_json[first_attr]:
        return False
    if third_attr and third_attr not in reference_json[first_attr][second_attr]:
        return False

    return True


def is_valid_world(narrative, protagonist, units):
    is_true = True

    for unit in narrative['antagonistGroup']['units']:
        if is_valid_unit(unit, units_atlas):
            continue
        else:
            print(f"Unit {unit['characteristicName']} is invalid.")
            is_true = False

    for level in units['levelsUnits']:
        for group in level['enemyGroups']:
            for unit in group['units']:
                if is_valid_unit(unit, units_atlas):
                    continue
                else:
                    print(f"Unit {unit['characteristicName']} is invalid.")
                    is_true = False

    for unit in protagonist['protagonistGroup']['units']:
        if is_valid_unit(unit, units_atlas):
            continue
        else:
            print(f"Unit {unit['characteristicName']} is invalid.")
            is_true = False

    return is_true


for name in os.listdir(worlds_folder_path):
    if ".meta" in name:
        continue
    json_file_path = os.path.join(worlds_folder_path, name, 'UnitData.json')
    if os.path.isdir(os.path.join(worlds_folder_path, name)) and os.path.exists(json_file_path):
        with open(json_file_path, 'r') as json_file:
            units = json.load(json_file)

    json_file_path = os.path.join(worlds_folder_path, name, 'Narrative.json')
    if os.path.isdir(os.path.join(worlds_folder_path, name)) and os.path.exists(json_file_path):
        with open(json_file_path, 'r') as json_file:
            narrative = json.load(json_file)

    json_file_path = os.path.join(worlds_folder_path, name, 'MainCharacter.json')
    if os.path.isdir(os.path.join(worlds_folder_path, name)) and os.path.exists(json_file_path):
        with open(json_file_path, 'r') as json_file:
            protagonist = json.load(json_file)

    print(f"World {name} Start.")

    if is_valid_world(narrative, protagonist, units):
        print(f"World {name} is valid.")
    else:
        print(f"World {name} is invalid.")
